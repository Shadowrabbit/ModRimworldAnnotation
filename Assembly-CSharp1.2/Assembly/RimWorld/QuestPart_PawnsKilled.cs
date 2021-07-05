using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010EF RID: 4335
	public class QuestPart_PawnsKilled : QuestPartActivable
	{
		// Token: 0x17000EB6 RID: 3766
		// (get) Token: 0x06005EB4 RID: 24244 RVA: 0x001E0450 File Offset: 0x001DE650
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

		// Token: 0x17000EB7 RID: 3767
		// (get) Token: 0x06005EB5 RID: 24245 RVA: 0x000418DE File Offset: 0x0003FADE
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

		// Token: 0x06005EB6 RID: 24246 RVA: 0x000418EE File Offset: 0x0003FAEE
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.killed = 0;
		}

		// Token: 0x06005EB7 RID: 24247 RVA: 0x001E04D0 File Offset: 0x001DE6D0
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

		// Token: 0x06005EB8 RID: 24248 RVA: 0x001E0574 File Offset: 0x001DE774
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

		// Token: 0x06005EB9 RID: 24249 RVA: 0x000418FE File Offset: 0x0003FAFE
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.race = ThingDefOf.Muffalo;
			this.requiredInstigatorFaction = Faction.OfPlayer;
			this.count = 10;
		}

		// Token: 0x04003F53 RID: 16211
		public ThingDef race;

		// Token: 0x04003F54 RID: 16212
		public Faction requiredInstigatorFaction;

		// Token: 0x04003F55 RID: 16213
		public int count;

		// Token: 0x04003F56 RID: 16214
		public MapParent mapParent;

		// Token: 0x04003F57 RID: 16215
		public string outSignalPawnKilled;

		// Token: 0x04003F58 RID: 16216
		private int killed;
	}
}
