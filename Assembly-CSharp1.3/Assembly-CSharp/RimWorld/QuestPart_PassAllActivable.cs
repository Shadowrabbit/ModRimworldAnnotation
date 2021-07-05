using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B18 RID: 2840
	public class QuestPart_PassAllActivable : QuestPartActivable
	{
		// Token: 0x17000BC0 RID: 3008
		// (get) Token: 0x060042C6 RID: 17094 RVA: 0x00165548 File Offset: 0x00163748
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

		// Token: 0x17000BC1 RID: 3009
		// (get) Token: 0x060042C7 RID: 17095 RVA: 0x00165596 File Offset: 0x00163796
		public int SignalsReceivedCount
		{
			get
			{
				return this.signalsReceived.Count((bool s) => s);
			}
		}

		// Token: 0x17000BC2 RID: 3010
		// (get) Token: 0x060042C8 RID: 17096 RVA: 0x001655C4 File Offset: 0x001637C4
		public override string ExpiryInfoPart
		{
			get
			{
				if (this.expiryInfoPartKey.NullOrEmpty())
				{
					return null;
				}
				return string.Concat(new object[]
				{
					this.expiryInfoPartKey.Translate() + " ",
					this.SignalsReceivedCount,
					" / ",
					this.inSignals.Count
				});
			}
		}

		// Token: 0x060042C9 RID: 17097 RVA: 0x00165631 File Offset: 0x00163831
		protected override void Enable(SignalArgs receivedArgs)
		{
			this.signalsReceived.Clear();
			base.Enable(receivedArgs);
		}

		// Token: 0x060042CA RID: 17098 RVA: 0x00165648 File Offset: 0x00163848
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
				if (!this.outSignalAny.NullOrEmpty())
				{
					SignalArgs args = signal.args;
					args.Add(this.SignalsReceivedCount.Named("COUNT"));
					Find.SignalManager.SendSignal(new Signal(this.outSignalAny, args));
				}
				if (this.AllSignalsReceived)
				{
					base.Complete();
				}
			}
		}

		// Token: 0x060042CB RID: 17099 RVA: 0x001656F0 File Offset: 0x001638F0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<bool>(ref this.signalsReceived, "signalsReceived", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.outSignalAny, "outSignalAny", null, false);
			Scribe_Values.Look<string>(ref this.expiryInfoPartKey, "expiryInfoPartKey", null, false);
		}

		// Token: 0x060042CC RID: 17100 RVA: 0x00165754 File Offset: 0x00163954
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignals.Clear();
			for (int i = 0; i < 3; i++)
			{
				this.inSignals.Add("DebugSignal" + Rand.Int);
			}
		}

		// Token: 0x040028A8 RID: 10408
		public List<string> inSignals = new List<string>();

		// Token: 0x040028A9 RID: 10409
		public string outSignalAny;

		// Token: 0x040028AA RID: 10410
		public string expiryInfoPartKey;

		// Token: 0x040028AB RID: 10411
		private List<bool> signalsReceived = new List<bool>();
	}
}
