using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F9E RID: 8094
	public class QuestNode_InspectString : QuestNode
	{
		// Token: 0x0600AC1A RID: 44058 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AC1B RID: 44059 RVA: 0x00320F5C File Offset: 0x0031F15C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.targets.GetValue(slate).EnumerableNullOrEmpty<ISelectable>())
			{
				return;
			}
			QuestPart_InspectString questPart_InspectString = new QuestPart_InspectString();
			questPart_InspectString.targets.AddRange(this.targets.GetValue(slate));
			questPart_InspectString.inspectString = this.inspectString.GetValue(slate);
			questPart_InspectString.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_InspectString);
		}

		// Token: 0x04007577 RID: 30071
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04007578 RID: 30072
		public SlateRef<IEnumerable<ISelectable>> targets;

		// Token: 0x04007579 RID: 30073
		public SlateRef<string> inspectString;
	}
}
