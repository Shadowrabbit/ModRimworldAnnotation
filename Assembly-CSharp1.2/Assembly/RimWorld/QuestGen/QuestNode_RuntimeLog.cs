using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F06 RID: 7942
	public class QuestNode_RuntimeLog : QuestNode
	{
		// Token: 0x0600AA13 RID: 43539 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AA14 RID: 43540 RVA: 0x0031A94C File Offset: 0x00318B4C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Log questPart_Log = new QuestPart_Log();
			questPart_Log.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Log.message = this.message.GetValue(slate);
			QuestGen.quest.AddPart(questPart_Log);
		}

		// Token: 0x04007362 RID: 29538
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04007363 RID: 29539
		[NoTranslate]
		public SlateRef<string> message;
	}
}
