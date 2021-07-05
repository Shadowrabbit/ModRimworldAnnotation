using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B62 RID: 2914
	public class QuestPart_AddQuestRefugeeDelayedReward : QuestPart_AddQuest
	{
		// Token: 0x17000BF3 RID: 3059
		// (get) Token: 0x0600442A RID: 17450 RVA: 0x0016A156 File Offset: 0x00168356
		public override QuestScriptDef QuestDef
		{
			get
			{
				return QuestScriptDefOf.RefugeeDelayedReward;
			}
		}

		// Token: 0x0600442B RID: 17451 RVA: 0x0016A160 File Offset: 0x00168360
		public override Slate GetSlate()
		{
			Slate slate = new Slate();
			slate.Set<FloatRange>("marketValueRange", this.marketValueRange, false);
			slate.Set<Faction>("faction", this.faction, false);
			for (int i = 0; i < this.lodgers.Count; i++)
			{
				if (!this.lodgers[i].Dead && this.lodgers[i].Faction != Faction.OfPlayer && !this.lodgers[i].IsPrisoner)
				{
					slate.Set<Pawn>("rewardGiver", this.lodgers[i], false);
					break;
				}
			}
			return slate;
		}

		// Token: 0x0600442C RID: 17452 RVA: 0x0016A206 File Offset: 0x00168406
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.faction == f)
			{
				this.faction = null;
			}
		}

		// Token: 0x0600442D RID: 17453 RVA: 0x0016A218 File Offset: 0x00168418
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.lodgers.Contains(item))
			{
				this.lodgers.Remove(item);
			}
		}

		// Token: 0x0600442E RID: 17454 RVA: 0x0016A270 File Offset: 0x00168470
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Collections.Look<Pawn>(ref this.lodgers, "lodgers", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inSignalRemovePawn, "inSignalRemovePawn", null, false);
			Scribe_Values.Look<FloatRange>(ref this.marketValueRange, "marketValueRange", default(FloatRange), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.lodgers.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0400295A RID: 10586
		public List<Pawn> lodgers = new List<Pawn>();

		// Token: 0x0400295B RID: 10587
		public FloatRange marketValueRange;

		// Token: 0x0400295C RID: 10588
		public Faction faction;

		// Token: 0x0400295D RID: 10589
		public string inSignalRemovePawn;
	}
}
