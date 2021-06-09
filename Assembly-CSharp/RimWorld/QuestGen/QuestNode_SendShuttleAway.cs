using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FB6 RID: 8118
	public class QuestNode_SendShuttleAway : QuestNode
	{
		// Token: 0x0600AC64 RID: 44132 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AC65 RID: 44133 RVA: 0x003220C8 File Offset: 0x003202C8
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

		// Token: 0x040075DC RID: 30172
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040075DD RID: 30173
		public SlateRef<Thing> shuttle;

		// Token: 0x040075DE RID: 30174
		public SlateRef<bool> dropEverything;
	}
}
