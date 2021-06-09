using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001088 RID: 4232
	public class QuestPart_Log : QuestPart
	{
		// Token: 0x06005C3C RID: 23612 RVA: 0x0003FF49 File Offset: 0x0003E149
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				Log.Message(signal.args.GetFormattedText(this.message), false);
			}
		}

		// Token: 0x06005C3D RID: 23613 RVA: 0x0003FF87 File Offset: 0x0003E187
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.message, "message", null, false);
		}

		// Token: 0x06005C3E RID: 23614 RVA: 0x0003FFB3 File Offset: 0x0003E1B3
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.message = "Dev: Test";
		}

		// Token: 0x04003DC6 RID: 15814
		public string inSignal;

		// Token: 0x04003DC7 RID: 15815
		public string message;
	}
}
