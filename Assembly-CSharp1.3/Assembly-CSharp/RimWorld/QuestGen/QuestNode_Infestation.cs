using System;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001720 RID: 5920
	public class QuestNode_Infestation : QuestNode
	{
		// Token: 0x06008889 RID: 34953 RVA: 0x00310D24 File Offset: 0x0030EF24
		protected override bool TestRunInt(Slate slate)
		{
			if (!Find.Storyteller.difficulty.allowViolentQuests)
			{
				return false;
			}
			if (!slate.Exists("map", false))
			{
				return false;
			}
			if (Faction.OfInsects == null)
			{
				return false;
			}
			Map map = slate.Get<Map>("map", null, false);
			IntVec3 intVec;
			return InfestationCellFinder.TryFindCell(out intVec, map);
		}

		// Token: 0x0600888A RID: 34954 RVA: 0x00310D78 File Offset: 0x0030EF78
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

		// Token: 0x04005677 RID: 22135
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04005678 RID: 22136
		public SlateRef<int> hivesCount;

		// Token: 0x04005679 RID: 22137
		public SlateRef<string> tag;

		// Token: 0x0400567A RID: 22138
		public SlateRef<string> customLetterLabel;

		// Token: 0x0400567B RID: 22139
		public SlateRef<string> customLetterText;

		// Token: 0x0400567C RID: 22140
		public SlateRef<RulePack> customLetterLabelRules;

		// Token: 0x0400567D RID: 22141
		public SlateRef<RulePack> customLetterTextRules;

		// Token: 0x0400567E RID: 22142
		private const string RootSymbol = "root";
	}
}
