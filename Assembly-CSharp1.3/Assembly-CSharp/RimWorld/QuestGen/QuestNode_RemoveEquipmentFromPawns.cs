using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016D9 RID: 5849
	public class QuestNode_RemoveEquipmentFromPawns : QuestNode
	{
		// Token: 0x06008749 RID: 34633 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600874A RID: 34634 RVA: 0x00306D3C File Offset: 0x00304F3C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_RemoveEquipmentFromPawns questPart_RemoveEquipmentFromPawns = new QuestPart_RemoveEquipmentFromPawns();
			questPart_RemoveEquipmentFromPawns.pawns.AddRange(this.pawns.GetValue(slate));
			questPart_RemoveEquipmentFromPawns.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_RemoveEquipmentFromPawns);
		}

		// Token: 0x04005563 RID: 21859
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04005564 RID: 21860
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
