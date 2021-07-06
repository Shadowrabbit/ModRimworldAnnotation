using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FB8 RID: 8120
	public class QuestNode_SetAllApparelLocked : QuestNode
	{
		// Token: 0x0600AC6A RID: 44138 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AC6B RID: 44139 RVA: 0x003221A4 File Offset: 0x003203A4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_SetAllApparelLocked questPart_SetAllApparelLocked = new QuestPart_SetAllApparelLocked();
			questPart_SetAllApparelLocked.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_SetAllApparelLocked.pawns.AddRange(this.pawns.GetValue(slate));
			QuestGen.quest.AddPart(questPart_SetAllApparelLocked);
		}

		// Token: 0x040075E1 RID: 30177
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040075E2 RID: 30178
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
