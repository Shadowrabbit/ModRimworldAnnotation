using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016EE RID: 5870
	public class QuestNode_WorkDisabled : QuestNode
	{
		// Token: 0x0600878B RID: 34699 RVA: 0x00307BCC File Offset: 0x00305DCC
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

		// Token: 0x0600878C RID: 34700 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x040055AB RID: 21931
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x040055AC RID: 21932
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x040055AD RID: 21933
		public SlateRef<WorkTags> disabledWorkTags;
	}
}
