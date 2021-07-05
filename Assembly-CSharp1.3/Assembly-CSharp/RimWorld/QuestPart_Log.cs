using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B52 RID: 2898
	public class QuestPart_Log : QuestPart
	{
		// Token: 0x060043D6 RID: 17366 RVA: 0x00168EBD File Offset: 0x001670BD
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				Log.Message(signal.args.GetFormattedText(this.message));
			}
		}

		// Token: 0x060043D7 RID: 17367 RVA: 0x00168EFA File Offset: 0x001670FA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.message, "message", null, false);
		}

		// Token: 0x060043D8 RID: 17368 RVA: 0x00168F26 File Offset: 0x00167126
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.message = "Dev: Test";
		}

		// Token: 0x04002926 RID: 10534
		public string inSignal;

		// Token: 0x04002927 RID: 10535
		public string message;
	}
}
