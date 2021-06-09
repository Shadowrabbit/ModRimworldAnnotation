using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001132 RID: 4402
	public class QuestPart_ChangeHeir : QuestPart
	{
		// Token: 0x17000F11 RID: 3857
		// (get) Token: 0x0600605C RID: 24668 RVA: 0x000427F0 File Offset: 0x000409F0
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				yield return this.holder;
				yield return this.heir;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000F12 RID: 3858
		// (get) Token: 0x0600605D RID: 24669 RVA: 0x00042800 File Offset: 0x00040A00
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in base.InvolvedFactions)
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				if (this.faction != null)
				{
					yield return this.faction;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x0600605E RID: 24670 RVA: 0x001E4628 File Offset: 0x001E2828
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.faction != null)
			{
				this.holder.royalty.SetHeir(this.heir, this.faction);
				this.done = true;
			}
		}

		// Token: 0x0600605F RID: 24671 RVA: 0x00042810 File Offset: 0x00040A10
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.holder == replace)
			{
				this.holder = with;
			}
			if (this.heir == replace)
			{
				this.heir = with;
			}
		}

		// Token: 0x06006060 RID: 24672 RVA: 0x001E467C File Offset: 0x001E287C
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_References.Look<Pawn>(ref this.holder, "holder", false);
			Scribe_References.Look<Pawn>(ref this.heir, "heir", false);
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<bool>(ref this.done, "done", false, false);
		}

		// Token: 0x0400405E RID: 16478
		public Faction faction;

		// Token: 0x0400405F RID: 16479
		public Pawn holder;

		// Token: 0x04004060 RID: 16480
		public Pawn heir;

		// Token: 0x04004061 RID: 16481
		public string inSignal;

		// Token: 0x04004062 RID: 16482
		public bool done;
	}
}
