using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B95 RID: 2965
	public class QuestPart_ReplaceLostLeaderReferences : QuestPart
	{
		// Token: 0x0600454F RID: 17743 RVA: 0x0016FD54 File Offset: 0x0016DF54
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				Pawn arg = signal.args.GetArg<Pawn>("SUBJECT");
				Pawn arg2 = signal.args.GetArg<Pawn>("NEWFACTIONLEADER");
				if (arg != null && arg2 != null)
				{
					List<QuestPart> partsListForReading = this.quest.PartsListForReading;
					for (int i = 0; i < partsListForReading.Count; i++)
					{
						partsListForReading[i].ReplacePawnReferences(arg, arg2);
					}
					if (arg.questTags != null)
					{
						if (arg2.questTags == null)
						{
							arg2.questTags = new List<string>();
						}
						arg2.questTags.AddRange(arg.questTags);
					}
				}
			}
		}

		// Token: 0x06004550 RID: 17744 RVA: 0x0016FDFF File Offset: 0x0016DFFF
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
		}

		// Token: 0x06004551 RID: 17745 RVA: 0x0016FE19 File Offset: 0x0016E019
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
		}

		// Token: 0x04002A40 RID: 10816
		public string inSignal;
	}
}
