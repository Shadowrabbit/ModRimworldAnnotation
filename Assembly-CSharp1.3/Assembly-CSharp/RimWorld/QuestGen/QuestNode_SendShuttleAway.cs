using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016DE RID: 5854
	public class QuestNode_SendShuttleAway : QuestNode
	{
		// Token: 0x06008758 RID: 34648 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06008759 RID: 34649 RVA: 0x00306FC0 File Offset: 0x003051C0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.shuttle.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_SendShuttleAway questPart_SendShuttleAway = new QuestPart_SendShuttleAway();
			questPart_SendShuttleAway.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_SendShuttleAway.shuttle = this.shuttle.GetValue(slate);
			questPart_SendShuttleAway.dropEverything = this.dropEverything.GetValue(slate);
			QuestGen.quest.AddPart(questPart_SendShuttleAway);
		}

		// Token: 0x04005570 RID: 21872
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04005571 RID: 21873
		public SlateRef<Thing> shuttle;

		// Token: 0x04005572 RID: 21874
		public SlateRef<bool> dropEverything;
	}
}
