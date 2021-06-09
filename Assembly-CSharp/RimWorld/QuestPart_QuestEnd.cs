using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001053 RID: 4179
	public class QuestPart_QuestEnd : QuestPart
	{
		// Token: 0x06005B0E RID: 23310 RVA: 0x001D73CC File Offset: 0x001D55CC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				QuestEndOutcome questEndOutcome;
				if (this.outcome != null)
				{
					questEndOutcome = this.outcome.Value;
				}
				else if (!signal.args.TryGetArg<QuestEndOutcome>("OUTCOME", out questEndOutcome))
				{
					questEndOutcome = QuestEndOutcome.Unknown;
				}
				this.quest.End(questEndOutcome, this.sendLetter);
			}
		}

		// Token: 0x06005B0F RID: 23311 RVA: 0x001D7438 File Offset: 0x001D5638
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<QuestEndOutcome?>(ref this.outcome, "outcome", null, false);
			Scribe_Values.Look<bool>(ref this.sendLetter, "sendLetter", false, false);
		}

		// Token: 0x06005B10 RID: 23312 RVA: 0x0003F32C File Offset: 0x0003D52C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
		}

		// Token: 0x04003D26 RID: 15654
		public string inSignal;

		// Token: 0x04003D27 RID: 15655
		public QuestEndOutcome? outcome;

		// Token: 0x04003D28 RID: 15656
		public bool sendLetter;
	}
}
