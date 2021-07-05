using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200164A RID: 5706
	public class QuestNode_RuntimeLog : QuestNode
	{
		// Token: 0x06008546 RID: 34118 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06008547 RID: 34119 RVA: 0x002FDC2C File Offset: 0x002FBE2C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Log questPart_Log = new QuestPart_Log();
			questPart_Log.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Log.message = this.message.GetValue(slate);
			QuestGen.quest.AddPart(questPart_Log);
		}

		// Token: 0x04005310 RID: 21264
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04005311 RID: 21265
		[NoTranslate]
		public SlateRef<string> message;
	}
}
