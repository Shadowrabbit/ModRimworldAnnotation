using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200104A RID: 4170
	public class QuestPart_PassAllActivable : QuestPartActivable
	{
		// Token: 0x17000E18 RID: 3608
		// (get) Token: 0x06005AE6 RID: 23270 RVA: 0x001D6D04 File Offset: 0x001D4F04
		private bool AllSignalsReceived
		{
			get
			{
				if (this.inSignals.Count != this.signalsReceived.Count)
				{
					return false;
				}
				for (int i = 0; i < this.signalsReceived.Count; i++)
				{
					if (!this.signalsReceived[i])
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x06005AE7 RID: 23271 RVA: 0x0003F07E File Offset: 0x0003D27E
		protected override void Enable(SignalArgs receivedArgs)
		{
			this.signalsReceived.Clear();
			base.Enable(receivedArgs);
		}

		// Token: 0x06005AE8 RID: 23272 RVA: 0x001D6D54 File Offset: 0x001D4F54
		protected override void ProcessQuestSignal(Signal signal)
		{
			base.ProcessQuestSignal(signal);
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
					base.Complete();
				}
			}
		}

		// Token: 0x06005AE9 RID: 23273 RVA: 0x0003F092 File Offset: 0x0003D292
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<bool>(ref this.signalsReceived, "signalsReceived", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x06005AEA RID: 23274 RVA: 0x001D6DB8 File Offset: 0x001D4FB8
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignals.Clear();
			for (int i = 0; i < 3; i++)
			{
				this.inSignals.Add("DebugSignal" + Rand.Int);
			}
		}

		// Token: 0x04003D14 RID: 15636
		public List<string> inSignals = new List<string>();

		// Token: 0x04003D15 RID: 15637
		private List<bool> signalsReceived = new List<bool>();
	}
}
