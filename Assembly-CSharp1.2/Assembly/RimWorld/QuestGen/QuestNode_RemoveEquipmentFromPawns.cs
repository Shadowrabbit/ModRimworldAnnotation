using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FB0 RID: 8112
	public class QuestNode_RemoveEquipmentFromPawns : QuestNode
	{
		// Token: 0x0600AC51 RID: 44113 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AC52 RID: 44114 RVA: 0x00321E44 File Offset: 0x00320044
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

		// Token: 0x040075CC RID: 30156
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040075CD RID: 30157
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
