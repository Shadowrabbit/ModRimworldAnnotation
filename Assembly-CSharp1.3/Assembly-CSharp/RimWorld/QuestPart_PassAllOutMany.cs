using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B19 RID: 2841
	public class QuestPart_PassAllOutMany : QuestPart
	{
		// Token: 0x17000BC3 RID: 3011
		// (get) Token: 0x060042CE RID: 17102 RVA: 0x001657BB File Offset: 0x001639BB
		private bool AllSignalsReceived
		{
			get
			{
				return PassAllQuestPartUtility.AllReceived(this.inSignals, this.signalsReceived);
			}
		}

		// Token: 0x060042CF RID: 17103 RVA: 0x001657D0 File Offset: 0x001639D0
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
						for (int i = 0; i < this.outSignals.Count; i++)
						{
							Find.SignalManager.SendSignal(new Signal(this.outSignals[i]));
						}
					}
				}
			}
		}

		// Token: 0x060042D0 RID: 17104 RVA: 0x00165860 File Offset: 0x00163A60
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.outSignals, "outSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<bool>(ref this.signalsReceived, "signalsReceived", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x060042D1 RID: 17105 RVA: 0x001658B8 File Offset: 0x00163AB8
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignals.Clear();
			for (int i = 0; i < 3; i++)
			{
				this.inSignals.Add("DebugSignal" + Rand.Int);
				this.outSignals.Add("DebugSignal" + Rand.Int);
			}
		}

		// Token: 0x040028AC RID: 10412
		public List<string> inSignals = new List<string>();

		// Token: 0x040028AD RID: 10413
		public List<string> outSignals = new List<string>();

		// Token: 0x040028AE RID: 10414
		private List<bool> signalsReceived = new List<bool>();
	}
}
