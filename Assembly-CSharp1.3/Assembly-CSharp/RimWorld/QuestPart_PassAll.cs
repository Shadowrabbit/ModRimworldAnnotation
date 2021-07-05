using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B16 RID: 2838
	public class QuestPart_PassAll : QuestPart
	{
		// Token: 0x17000BBF RID: 3007
		// (get) Token: 0x060042C0 RID: 17088 RVA: 0x001653AD File Offset: 0x001635AD
		private bool AllSignalsReceived
		{
			get
			{
				return PassAllQuestPartUtility.AllReceived(this.inSignals, this.signalsReceived);
			}
		}

		// Token: 0x060042C1 RID: 17089 RVA: 0x001653C0 File Offset: 0x001635C0
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (!this.AllSignalsReceived)
			{
				int num = this.inSignals.IndexOf(signal.tag);
				if (num >= 0)
				{
					while (this.signalsReceived.Count <= num)
					{
						this.signalsReceived.Add(false);
					}
					this.signalsReceived[num] = true;
					if (this.AllSignalsReceived)
					{
						Find.SignalManager.SendSignal(new Signal(this.outSignal));
					}
				}
			}
		}

		// Token: 0x060042C2 RID: 17090 RVA: 0x00165434 File Offset: 0x00163634
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			Scribe_Collections.Look<bool>(ref this.signalsReceived, "signalsReceived", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x060042C3 RID: 17091 RVA: 0x00165488 File Offset: 0x00163688
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

		// Token: 0x040028A5 RID: 10405
		public List<string> inSignals = new List<string>();

		// Token: 0x040028A6 RID: 10406
		public string outSignal;

		// Token: 0x040028A7 RID: 10407
		private List<bool> signalsReceived = new List<bool>();
	}
}
