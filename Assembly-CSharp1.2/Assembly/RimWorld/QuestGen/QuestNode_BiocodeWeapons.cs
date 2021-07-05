using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F83 RID: 8067
	public class QuestNode_BiocodeWeapons : QuestNode
	{
		// Token: 0x0600ABC1 RID: 43969 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600ABC2 RID: 43970 RVA: 0x0031FF14 File Offset: 0x0031E114
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_BiocodeWeapons questPart_BiocodeWeapons = new QuestPart_BiocodeWeapons();
			questPart_BiocodeWeapons.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_BiocodeWeapons.pawns.AddRange(this.pawns.GetValue(slate));
			QuestGen.quest.AddPart(questPart_BiocodeWeapons);
		}

		// Token: 0x04007509 RID: 29961
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400750A RID: 29962
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
