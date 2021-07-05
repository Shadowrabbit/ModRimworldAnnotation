using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B8D RID: 2957
	public class QuestPart_PawnsKilled : QuestPartActivable
	{
		// Token: 0x17000C18 RID: 3096
		// (get) Token: 0x06004524 RID: 17700 RVA: 0x0016EB44 File Offset: 0x0016CD44
		public override string DescriptionPart
		{
			get
			{
				return string.Concat(new object[]
				{
					"PawnsKilled".Translate(GenLabel.BestKindLabel(this.race.race.AnyPawnKind, Gender.None, true, -1)).CapitalizeFirst() + ": ",
					this.killed,
					" / ",
					this.count
				});
			}
		}

		// Token: 0x17000C19 RID: 3097
		// (get) Token: 0x06004525 RID: 17701 RVA: 0x0016EBC1 File Offset: 0x0016CDC1
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in base.InvolvedFactions)
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				if (this.requiredInstigatorFaction != null)
				{
					yield return this.requiredInstigatorFaction;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06004526 RID: 17702 RVA: 0x0016EBD1 File Offset: 0x0016CDD1
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.killed = 0;
		}

		// Token: 0x06004527 RID: 17703 RVA: 0x0016EBE4 File Offset: 0x0016CDE4
		public override void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
			base.Notify_PawnKilled(pawn, dinfo);
			if (base.State == QuestPartState.Enabled && pawn.def == this.race && (this.requiredInstigatorFaction == null || (dinfo != null && (dinfo.Value.Instigator == null || dinfo.Value.Instigator.Faction == this.requiredInstigatorFaction))))
			{
				this.killed++;
				Find.SignalManager.SendSignal(new Signal(this.outSignalPawnKilled));
				if (this.killed >= this.count)
				{
					base.Complete();
				}
			}
		}

		// Token: 0x06004528 RID: 17704 RVA: 0x0016EC88 File Offset: 0x0016CE88
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.race, "race");
			Scribe_References.Look<Faction>(ref this.requiredInstigatorFaction, "requiredInstigatorFaction", false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<int>(ref this.count, "count", 0, false);
			Scribe_Values.Look<int>(ref this.killed, "killed", 0, false);
			Scribe_Values.Look<string>(ref this.outSignalPawnKilled, "outSignalPawnKilled", null, false);
		}

		// Token: 0x06004529 RID: 17705 RVA: 0x0016ED03 File Offset: 0x0016CF03
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.race = ThingDefOf.Muffalo;
			this.requiredInstigatorFaction = Faction.OfPlayer;
			this.count = 10;
		}

		// Token: 0x04002A0A RID: 10762
		public ThingDef race;

		// Token: 0x04002A0B RID: 10763
		public Faction requiredInstigatorFaction;

		// Token: 0x04002A0C RID: 10764
		public int count;

		// Token: 0x04002A0D RID: 10765
		public MapParent mapParent;

		// Token: 0x04002A0E RID: 10766
		public string outSignalPawnKilled;

		// Token: 0x04002A0F RID: 10767
		private int killed;
	}
}
