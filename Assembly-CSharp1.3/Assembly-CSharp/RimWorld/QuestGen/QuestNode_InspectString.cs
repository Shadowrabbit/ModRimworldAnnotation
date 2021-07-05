using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016C8 RID: 5832
	public class QuestNode_InspectString : QuestNode
	{
		// Token: 0x06008714 RID: 34580 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06008715 RID: 34581 RVA: 0x00305C28 File Offset: 0x00303E28
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

		// Token: 0x04005508 RID: 21768
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04005509 RID: 21769
		public SlateRef<IEnumerable<ISelectable>> targets;

		// Token: 0x0400550A RID: 21770
		public SlateRef<string> inspectString;
	}
}
