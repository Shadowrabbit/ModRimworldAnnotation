using System;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020016BE RID: 5822
	public class QuestNode_EndGame : QuestNode
	{
		// Token: 0x060086ED RID: 34541 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060086EE RID: 34542 RVA: 0x00305154 File Offset: 0x00303354
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

		// Token: 0x040054C3 RID: 21699
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040054C4 RID: 21700
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;

		// Token: 0x040054C5 RID: 21701
		public SlateRef<string> introText;

		// Token: 0x040054C6 RID: 21702
		public SlateRef<string> endingText;

		// Token: 0x040054C7 RID: 21703
		public SlateRef<RulePack> introTextRules;

		// Token: 0x040054C8 RID: 21704
		public SlateRef<RulePack> endingTextRules;

		// Token: 0x040054C9 RID: 21705
		private const string RootSymbol = "root";
	}
}
