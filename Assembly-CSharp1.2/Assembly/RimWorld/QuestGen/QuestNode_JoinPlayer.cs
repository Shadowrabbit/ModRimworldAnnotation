using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F9F RID: 8095
	public class QuestNode_JoinPlayer : QuestNode
	{
		// Token: 0x0600AC1D RID: 44061 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AC1E RID: 44062 RVA: 0x00320FEC File Offset: 0x0031F1EC
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

		// Token: 0x0400757A RID: 30074
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400757B RID: 30075
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x0400757C RID: 30076
		public SlateRef<bool> joinPlayer;

		// Token: 0x0400757D RID: 30077
		public SlateRef<bool> makePrisoners;
	}
}
