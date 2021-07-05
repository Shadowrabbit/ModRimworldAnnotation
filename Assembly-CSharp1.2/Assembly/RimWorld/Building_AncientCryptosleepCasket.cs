using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200169F RID: 5791
	public class Building_AncientCryptosleepCasket : Building_CryptosleepCasket
	{
		// Token: 0x06007EA5 RID: 32421 RVA: 0x000551C4 File Offset: 0x000533C4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.groupID, "groupID", 0, false);
		}

		// Token: 0x06007EA6 RID: 32422 RVA: 0x0025A5B0 File Offset: 0x002587B0
		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			base.PreApplyDamage(ref dinfo, out absorbed);
			if (absorbed)
			{
				return;
			}
			if (!this.contentsKnown && this.innerContainer.Count > 0 && dinfo.Def.harmsHealth && dinfo.Instigator != null && dinfo.Instigator.Faction != null)
			{
				bool flag = false;
				using (IEnumerator<Thing> enumerator = ((IEnumerable<Thing>)this.innerContainer).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current is Pawn)
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					this.EjectContents();
				}
			}
			absorbed = false;
		}

		// Token: 0x06007EA7 RID: 32423 RVA: 0x0025A654 File Offset: 0x00258854
		public override void EjectContents()
		{
			bool contentsKnown = this.contentsKnown;
			List<Thing> list = null;
			if (!contentsKnown)
			{
				list = new List<Thing>();
				list.AddRange(this.innerContainer);
				list.AddRange(this.UnopenedCasketsInGroup().SelectMany((Building_AncientCryptosleepCasket c) => c.innerContainer));
				list.RemoveDuplicates<Thing>();
			}
			base.EjectContents();
			if (!contentsKnown)
			{
				ThingDef filth_Slime = ThingDefOf.Filth_Slime;
				FilthMaker.TryMakeFilth(base.Position, base.Map, filth_Slime, Rand.Range(8, 12), FilthSourceFlags.None);
				this.SetFaction(null, null);
				foreach (Building_AncientCryptosleepCasket building_AncientCryptosleepCasket in this.UnopenedCasketsInGroup())
				{
					building_AncientCryptosleepCasket.contentsKnown = true;
					building_AncientCryptosleepCasket.EjectContents();
				}
				IEnumerable<Pawn> enumerable = from p in list.OfType<Pawn>().ToList<Pawn>()
				where p.RaceProps.Humanlike && p.GetLord() == null && p.Faction == Faction.OfAncientsHostile
				select p;
				if (enumerable.Any<Pawn>())
				{
					LordMaker.MakeNewLord(Faction.OfAncientsHostile, new LordJob_AssaultColony(Faction.OfAncientsHostile, false, true, false, false, false), base.Map, enumerable);
				}
			}
		}

		// Token: 0x06007EA8 RID: 32424 RVA: 0x000551DE File Offset: 0x000533DE
		private IEnumerable<Building_AncientCryptosleepCasket> UnopenedCasketsInGroup()
		{
			yield return this;
			if (this.groupID != -1)
			{
				foreach (Thing thing in base.Map.listerThings.ThingsOfDef(ThingDefOf.AncientCryptosleepCasket))
				{
					Building_AncientCryptosleepCasket building_AncientCryptosleepCasket = thing as Building_AncientCryptosleepCasket;
					if (building_AncientCryptosleepCasket.groupID == this.groupID && !building_AncientCryptosleepCasket.contentsKnown)
					{
						yield return building_AncientCryptosleepCasket;
					}
				}
				List<Thing>.Enumerator enumerator = default(List<Thing>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x04005265 RID: 21093
		public int groupID = -1;
	}
}
