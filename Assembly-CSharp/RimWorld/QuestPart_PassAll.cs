using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001048 RID: 4168
	public class QuestPart_PassAll : QuestPart
	{
		// Token: 0x17000E17 RID: 3607
		// (get) Token: 0x06005AE0 RID: 23264 RVA: 0x0003F04D File Offset: 0x0003D24D
		private bool AllSignalsReceived
		{
			get
			{
				return PassAllQuestPartUtility.AllReceived(this.inSignals, this.signalsReceived);
			}
		}

		// Token: 0x06005AE1 RID: 23265 RVA: 0x001D6B9C File Offset: 0x001D4D9C
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

		// Token: 0x06005AE2 RID: 23266 RVA: 0x001D6C10 File Offset: 0x001D4E10
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			Scribe_Collections.Look<bool>(ref this.signalsReceived, "signalsReceived", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x06005AE3 RID: 23267 RVA: 0x001D6C64 File Offset: 0x001D4E64
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

		// Token: 0x04003D11 RID: 15633
		public List<string> inSignals = new List<string>();

		// Token: 0x04003D12 RID: 15634
		public string outSignal;

		// Token: 0x04003D13 RID: 15635
		private List<bool> signalsReceived = new List<bool>();
	}
}
