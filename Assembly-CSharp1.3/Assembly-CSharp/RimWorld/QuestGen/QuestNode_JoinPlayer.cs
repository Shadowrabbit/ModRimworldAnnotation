using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016C9 RID: 5833
	public class QuestNode_JoinPlayer : QuestNode
	{
		// Token: 0x06008717 RID: 34583 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06008718 RID: 34584 RVA: 0x00305CB8 File Offset: 0x00303EB8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_JoinPlayer questPart_JoinPlayer = new QuestPart_JoinPlayer();
			questPart_JoinPlayer.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_JoinPlayer.joinPlayer = this.joinPlayer.GetValue(slate);
			questPart_JoinPlayer.makePrisoners = this.makePrisoners.GetValue(slate);
			questPart_JoinPlayer.mapParent = QuestGen.slate.Get<Map>("map", null, false).Parent;
			questPart_JoinPlayer.pawns.AddRange(this.pawns.GetValue(slate));
			QuestGen.quest.AddPart(questPart_JoinPlayer);
		}

		// Token: 0x0400550B RID: 21771
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400550C RID: 21772
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x0400550D RID: 21773
		public SlateRef<bool> joinPlayer;

		// Token: 0x0400550E RID: 21774
		public SlateRef<bool> makePrisoners;
	}
}
