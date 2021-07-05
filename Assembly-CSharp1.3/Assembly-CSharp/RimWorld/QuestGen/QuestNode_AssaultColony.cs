using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001696 RID: 5782
	public class QuestNode_AssaultColony : QuestNode
	{
		// Token: 0x0600866A RID: 34410 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600866B RID: 34411 RVA: 0x00303794 File Offset: 0x00301994
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate).EnumerableNullOrEmpty<Pawn>())
			{
				return;
			}
			QuestPart_AssaultColony questPart_AssaultColony = new QuestPart_AssaultColony();
			questPart_AssaultColony.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_AssaultColony.pawns.AddRange(this.pawns.GetValue(slate));
			questPart_AssaultColony.mapParent = slate.Get<Map>("map", null, false).Parent;
			questPart_AssaultColony.faction = this.faction.GetValue(slate);
			questPart_AssaultColony.canTimeoutOrFlee = (this.canTimeoutOrFlee.GetValue(slate) ?? true);
			QuestGen.quest.AddPart(questPart_AssaultColony);
		}

		// Token: 0x04005446 RID: 21574
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04005447 RID: 21575
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x04005448 RID: 21576
		public SlateRef<Faction> faction;

		// Token: 0x04005449 RID: 21577
		public SlateRef<bool?> canTimeoutOrFlee;
	}
}
