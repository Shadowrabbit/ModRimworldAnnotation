using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B48 RID: 2888
	public class QuestPart_MergeOutcomes : QuestPart
	{
		// Token: 0x0600438E RID: 17294 RVA: 0x00167F74 File Offset: 0x00166174
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

		// Token: 0x0600438F RID: 17295 RVA: 0x00167FE8 File Offset: 0x001661E8
		private QuestEndOutcome GetOutcome(SignalArgs args)
		{
			QuestEndOutcome result;
			if (args.TryGetArg<QuestEndOutcome>("OUTCOME", out result))
			{
				return result;
			}
			return QuestEndOutcome.Unknown;
		}

		// Token: 0x06004390 RID: 17296 RVA: 0x00168008 File Offset: 0x00166208
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

		// Token: 0x06004391 RID: 17297 RVA: 0x00168114 File Offset: 0x00166314
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			Scribe_Collections.Look<QuestEndOutcome?>(ref this.signalsReceived, "signalsReceived", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x06004392 RID: 17298 RVA: 0x00168168 File Offset: 0x00166368
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

		// Token: 0x0400290A RID: 10506
		public List<string> inSignals = new List<string>();

		// Token: 0x0400290B RID: 10507
		public string outSignal;

		// Token: 0x0400290C RID: 10508
		private List<QuestEndOutcome?> signalsReceived = new List<QuestEndOutcome?>();
	}
}
