using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016B0 RID: 5808
	public class QuestNode_BiocodeWeapons : QuestNode
	{
		// Token: 0x060086C2 RID: 34498 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060086C3 RID: 34499 RVA: 0x003046E4 File Offset: 0x003028E4
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

		// Token: 0x0400548A RID: 21642
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400548B RID: 21643
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
