using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001697 RID: 5783
	public class QuestNode_VisitColony : QuestNode
	{
		// Token: 0x0600866D RID: 34413 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600866E RID: 34414 RVA: 0x0030385C File Offset: 0x00301A5C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate).EnumerableNullOrEmpty<Pawn>())
			{
				return;
			}
			QuestPart_VisitColony questPart_VisitColony = new QuestPart_VisitColony();
			questPart_VisitColony.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_VisitColony.pawns.AddRange(this.pawns.GetValue(slate));
			questPart_VisitColony.mapParent = slate.Get<Map>("map", null, false).Parent;
			questPart_VisitColony.faction = this.faction.GetValue(slate);
			questPart_VisitColony.durationTicks = this.durationTicks.GetValue(slate);
			QuestGen.quest.AddPart(questPart_VisitColony);
		}

		// Token: 0x0400544A RID: 21578
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400544B RID: 21579
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x0400544C RID: 21580
		public SlateRef<Faction> faction;

		// Token: 0x0400544D RID: 21581
		public SlateRef<int?> durationTicks;
	}
}
