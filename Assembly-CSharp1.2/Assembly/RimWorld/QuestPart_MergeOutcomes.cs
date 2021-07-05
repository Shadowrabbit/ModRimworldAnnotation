using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001075 RID: 4213
	public class QuestPart_MergeOutcomes : QuestPart
	{
		// Token: 0x06005BAF RID: 23471 RVA: 0x001D8B2C File Offset: 0x001D6D2C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			int num = this.inSignals.IndexOf(signal.tag);
			if (num >= 0)
			{
				while (this.signalsReceived.Count <= num)
				{
					this.signalsReceived.Add(null);
				}
				this.signalsReceived[num] = new QuestEndOutcome?(this.GetOutcome(signal.args));
				this.CheckEnd();
			}
		}

		// Token: 0x06005BB0 RID: 23472 RVA: 0x001D8BA0 File Offset: 0x001D6DA0
		private QuestEndOutcome GetOutcome(SignalArgs args)
		{
			QuestEndOutcome result;
			if (args.TryGetArg<QuestEndOutcome>("OUTCOME", out result))
			{
				return result;
			}
			return QuestEndOutcome.Unknown;
		}

		// Token: 0x06005BB1 RID: 23473 RVA: 0x001D8BC0 File Offset: 0x001D6DC0
		private void CheckEnd()
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = this.inSignals.Count == this.signalsReceived.Count;
			for (int i = 0; i < this.signalsReceived.Count; i++)
			{
				if (this.signalsReceived[i] == null)
				{
					flag3 = false;
				}
				else if (this.signalsReceived[i].Value == QuestEndOutcome.Success)
				{
					flag = true;
				}
				else if (this.signalsReceived[i].Value == QuestEndOutcome.Fail)
				{
					flag2 = true;
				}
			}
			if (flag2)
			{
				Find.SignalManager.SendSignal(new Signal(this.outSignal, QuestEndOutcome.Fail.Named("OUTCOME")));
				return;
			}
			if (flag3)
			{
				if (flag)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignal, QuestEndOutcome.Success.Named("OUTCOME")));
					return;
				}
				Find.SignalManager.SendSignal(new Signal(this.outSignal, QuestEndOutcome.Unknown.Named("OUTCOME")));
			}
		}

		// Token: 0x06005BB2 RID: 23474 RVA: 0x001D8CCC File Offset: 0x001D6ECC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			Scribe_Collections.Look<QuestEndOutcome?>(ref this.signalsReceived, "signalsReceived", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x06005BB3 RID: 23475 RVA: 0x001D8D20 File Offset: 0x001D6F20
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignals.Clear();
			for (int i = 0; i < 3; i++)
			{
				this.inSignals.Add("DebugSignal" + Rand.Int);
			}
			this.outSignal = "DebugSignal" + Rand.Int;
		}

		// Token: 0x04003D82 RID: 15746
		public List<string> inSignals = new List<string>();

		// Token: 0x04003D83 RID: 15747
		public string outSignal;

		// Token: 0x04003D84 RID: 15748
		private List<QuestEndOutcome?> signalsReceived = new List<QuestEndOutcome?>();
	}
}
