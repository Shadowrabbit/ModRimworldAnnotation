using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001047 RID: 4167
	public class QuestPart_PassActivable : QuestPartActivable
	{
		// Token: 0x06005ADC RID: 23260 RVA: 0x0003EFE9 File Offset: 0x0003D1E9
		protected override void ProcessQuestSignal(Signal signal)
		{
			base.ProcessQuestSignal(signal);
			if (signal.tag == this.inSignal)
			{
				this.Complete(signal.args);
			}
		}

		// Token: 0x06005ADD RID: 23261 RVA: 0x0003F011 File Offset: 0x0003D211
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
		}

		// Token: 0x06005ADE RID: 23262 RVA: 0x0003F02B File Offset: 0x0003D22B
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
		}

		// Token: 0x04003D10 RID: 15632
		public string inSignal;
	}
}
