using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FC9 RID: 8137
	public class QuestNode_WorkDisabled : QuestNode
	{
		// Token: 0x0600ACA8 RID: 44200 RVA: 0x00322ED8 File Offset: 0x003210D8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_WorkDisabled questPart_WorkDisabled = new QuestPart_WorkDisabled();
			questPart_WorkDisabled.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_WorkDisabled.pawns.AddRange(this.pawns.GetValue(slate));
			questPart_WorkDisabled.disabledWorkTags = this.disabledWorkTags.GetValue(slate);
			QuestGen.quest.AddPart(questPart_WorkDisabled);
		}

		// Token: 0x0600ACA9 RID: 44201 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x04007628 RID: 30248
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04007629 RID: 30249
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x0400762A RID: 30250
		public SlateRef<WorkTags> disabledWorkTags;
	}
}
