using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x0200109F RID: 4255
	public class QuestPart_AddQuestRefugeeDelayedReward : QuestPart_AddQuest
	{
		// Token: 0x06005CCA RID: 23754 RVA: 0x001DB46C File Offset: 0x001D966C
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

		// Token: 0x06005CCB RID: 23755 RVA: 0x00040639 File Offset: 0x0003E839
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.faction == f)
			{
				this.faction = null;
			}
		}

		// Token: 0x06005CCC RID: 23756 RVA: 0x001DB514 File Offset: 0x001D9714
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.lodgers.Contains(item))
			{
				this.lodgers.Remove(item);
			}
		}

		// Token: 0x06005CCD RID: 23757 RVA: 0x001DB56C File Offset: 0x001D976C
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

		// Token: 0x04003E20 RID: 15904
		public List<Pawn> lodgers = new List<Pawn>();

		// Token: 0x04003E21 RID: 15905
		public FloatRange marketValueRange;

		// Token: 0x04003E22 RID: 15906
		public Faction faction;

		// Token: 0x04003E23 RID: 15907
		public string inSignalRemovePawn;
	}
}
