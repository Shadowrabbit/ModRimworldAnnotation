using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200104B RID: 4171
	public class QuestPart_PassAllOutMany : QuestPart
	{
		// Token: 0x17000E19 RID: 3609
		// (get) Token: 0x06005AEC RID: 23276 RVA: 0x0003F0E4 File Offset: 0x0003D2E4
		private bool AllSignalsReceived
		{
			get
			{
				return PassAllQuestPartUtility.AllReceived(this.inSignals, this.signalsReceived);
			}
		}

		// Token: 0x06005AED RID: 23277 RVA: 0x001D6E04 File Offset: 0x001D5004
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

		// Token: 0x06005AEE RID: 23278 RVA: 0x001D6E94 File Offset: 0x001D5094
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.outSignals, "outSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<bool>(ref this.signalsReceived, "signalsReceived", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x06005AEF RID: 23279 RVA: 0x001D6EEC File Offset: 0x001D50EC
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

		// Token: 0x04003D16 RID: 15638
		public List<string> inSignals = new List<string>();

		// Token: 0x04003D17 RID: 15639
		public List<string> outSignals = new List<string>();

		// Token: 0x04003D18 RID: 15640
		private List<bool> signalsReceived = new List<bool>();
	}
}
