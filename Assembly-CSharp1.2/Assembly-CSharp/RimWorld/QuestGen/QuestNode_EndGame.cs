using System;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F93 RID: 8083
	public class QuestNode_EndGame : QuestNode
	{
		// Token: 0x0600ABF2 RID: 44018 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600ABF3 RID: 44019 RVA: 0x00320894 File Offset: 0x0031EA94
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_EndGame endGame = new QuestPart_EndGame();
			endGame.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			endGame.signalListenMode = (this.signalListenMode.GetValue(slate) ?? QuestPart.SignalListenMode.OngoingOnly);
			QuestGen.AddTextRequest("root", delegate(string x)
			{
				endGame.introText = x;
			}, QuestGenUtility.MergeRules(this.introTextRules.GetValue(slate), this.introText.GetValue(slate), "root"));
			QuestGen.AddTextRequest("root", delegate(string x)
			{
				endGame.endingText = x;
			}, QuestGenUtility.MergeRules(this.endingTextRules.GetValue(slate), this.endingText.GetValue(slate), "root"));
			QuestGen.quest.AddPart(endGame);
		}

		// Token: 0x04007542 RID: 30018
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04007543 RID: 30019
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;

		// Token: 0x04007544 RID: 30020
		public SlateRef<string> introText;

		// Token: 0x04007545 RID: 30021
		public SlateRef<string> endingText;

		// Token: 0x04007546 RID: 30022
		public SlateRef<RulePack> introTextRules;

		// Token: 0x04007547 RID: 30023
		public SlateRef<RulePack> endingTextRules;

		// Token: 0x04007548 RID: 30024
		private const string RootSymbol = "root";
	}
}
