using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010FA RID: 4346
	public class QuestPart_ReplaceLostLeaderReferences : QuestPart
	{
		// Token: 0x06005EF7 RID: 24311 RVA: 0x001E16C4 File Offset: 0x001DF8C4
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

		// Token: 0x06005EF8 RID: 24312 RVA: 0x00041B6E File Offset: 0x0003FD6E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
		}

		// Token: 0x06005EF9 RID: 24313 RVA: 0x00041B88 File Offset: 0x0003FD88
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
		}

		// Token: 0x04003F97 RID: 16279
		public string inSignal;
	}
}
