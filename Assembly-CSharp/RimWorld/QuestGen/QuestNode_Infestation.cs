using System;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FF7 RID: 8183
	public class QuestNode_Infestation : QuestNode
	{
		// Token: 0x0600AD66 RID: 44390 RVA: 0x003278F0 File Offset: 0x00325AF0
		protected override bool TestRunInt(Slate slate)
		{
			if (!Find.Storyteller.difficultyValues.allowViolentQuests)
			{
				return false;
			}
			if (!slate.Exists("map", false))
			{
				return false;
			}
			Map map = slate.Get<Map>("map", null, false);
			IntVec3 intVec;
			return InfestationCellFinder.TryFindCell(out intVec, map);
		}

		// Token: 0x0600AD67 RID: 44391 RVA: 0x0032793C File Offset: 0x00325B3C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map = QuestGen.slate.Get<Map>("map", null, false);
			if (map == null)
			{
				return;
			}
			QuestPart_Infestation questPart = new QuestPart_Infestation();
			questPart.mapParent = map.Parent;
			questPart.hivesCount = this.hivesCount.GetValue(slate);
			questPart.tag = QuestGenUtility.HardcodedTargetQuestTagWithQuestID(this.tag.GetValue(slate));
			if (!this.customLetterLabel.GetValue(slate).NullOrEmpty() || this.customLetterLabelRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					questPart.customLetterLabel = x;
				}, QuestGenUtility.MergeRules(this.customLetterLabelRules.GetValue(slate), this.customLetterLabel.GetValue(slate), "root"));
			}
			if (!this.customLetterText.GetValue(slate).NullOrEmpty() || this.customLetterTextRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					questPart.customLetterText = x;
				}, QuestGenUtility.MergeRules(this.customLetterTextRules.GetValue(slate), this.customLetterText.GetValue(slate), "root"));
			}
			questPart.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart);
		}

		// Token: 0x040076EC RID: 30444
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040076ED RID: 30445
		public SlateRef<int> hivesCount;

		// Token: 0x040076EE RID: 30446
		public SlateRef<string> tag;

		// Token: 0x040076EF RID: 30447
		public SlateRef<string> customLetterLabel;

		// Token: 0x040076F0 RID: 30448
		public SlateRef<string> customLetterText;

		// Token: 0x040076F1 RID: 30449
		public SlateRef<RulePack> customLetterLabelRules;

		// Token: 0x040076F2 RID: 30450
		public SlateRef<RulePack> customLetterTextRules;

		// Token: 0x040076F3 RID: 30451
		private const string RootSymbol = "root";
	}
}
