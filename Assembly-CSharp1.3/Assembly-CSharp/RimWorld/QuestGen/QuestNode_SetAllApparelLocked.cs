using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016E0 RID: 5856
	public class QuestNode_SetAllApparelLocked : QuestNode
	{
		// Token: 0x0600875E RID: 34654 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600875F RID: 34655 RVA: 0x0030709C File Offset: 0x0030529C
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

		// Token: 0x04005575 RID: 21877
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04005576 RID: 21878
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
