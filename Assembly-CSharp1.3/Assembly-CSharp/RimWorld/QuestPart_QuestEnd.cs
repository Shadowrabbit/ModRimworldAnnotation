using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B23 RID: 2851
	public class QuestPart_QuestEnd : QuestPart
	{
		// Token: 0x060042F8 RID: 17144 RVA: 0x001661F8 File Offset: 0x001643F8
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

		// Token: 0x060042F9 RID: 17145 RVA: 0x00166264 File Offset: 0x00164464
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<QuestEndOutcome?>(ref this.outcome, "outcome", null, false);
			Scribe_Values.Look<bool>(ref this.sendLetter, "sendLetter", false, false);
		}

		// Token: 0x060042FA RID: 17146 RVA: 0x001662B5 File Offset: 0x001644B5
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
		}

		// Token: 0x040028C2 RID: 10434
		public string inSignal;

		// Token: 0x040028C3 RID: 10435
		public QuestEndOutcome? outcome;

		// Token: 0x040028C4 RID: 10436
		public bool sendLetter;
	}
}
