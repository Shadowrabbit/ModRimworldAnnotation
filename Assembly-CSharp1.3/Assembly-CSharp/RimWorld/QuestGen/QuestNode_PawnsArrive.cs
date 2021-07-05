using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020016D4 RID: 5844
	public class QuestNode_PawnsArrive : QuestNode
	{
		// Token: 0x06008738 RID: 34616 RVA: 0x00306634 File Offset: 0x00304834
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Exists("map", false);
		}

		// Token: 0x06008739 RID: 34617 RVA: 0x00306648 File Offset: 0x00304848
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			PawnsArrivalModeDef pawnsArrivalModeDef = this.arrivalMode.GetValue(slate) ?? PawnsArrivalModeDefOf.EdgeWalkIn;
			QuestPart_PawnsArrive pawnsArrive = new QuestPart_PawnsArrive();
			pawnsArrive.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			pawnsArrive.pawns.AddRange(this.pawns.GetValue(slate));
			pawnsArrive.arrivalMode = pawnsArrivalModeDef;
			pawnsArrive.joinPlayer = this.joinPlayer.GetValue(slate);
			pawnsArrive.mapParent = QuestGen.slate.Get<Map>("map", null, false).Parent;
			pawnsArrive.customLetterDef = this.customLetterDef.GetValue(slate);
			if (pawnsArrivalModeDef.walkIn)
			{
				pawnsArrive.spawnNear = (this.walkInSpot.GetValue(slate) ?? (QuestGen.slate.Get<IntVec3?>("walkInSpot", null, false) ?? IntVec3.Invalid));
			}
			if (!this.customLetterLabel.GetValue(slate).NullOrEmpty() || this.customLetterLabelRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					pawnsArrive.customLetterLabel = x;
				}, QuestGenUtility.MergeRules(this.customLetterLabelRules.GetValue(slate), this.customLetterLabel.GetValue(slate), "root"));
			}
			if (!this.customLetterText.GetValue(slate).NullOrEmpty() || this.customLetterTextRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					pawnsArrive.customLetterText = x;
				}, QuestGenUtility.MergeRules(this.customLetterTextRules.GetValue(slate), this.customLetterText.GetValue(slate), "root"));
			}
			QuestGen.quest.AddPart(pawnsArrive);
			if (this.isSingleReward.GetValue(slate))
			{
				QuestPart_Choice questPart_Choice = new QuestPart_Choice();
				questPart_Choice.inSignalChoiceUsed = pawnsArrive.inSignal;
				QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
				choice.questParts.Add(pawnsArrive);
				foreach (Pawn pawn in pawnsArrive.pawns)
				{
					choice.rewards.Add(new Reward_Pawn
					{
						pawn = pawn,
						detailsHidden = this.rewardDetailsHidden.GetValue(slate)
					});
				}
				questPart_Choice.choices.Add(choice);
				QuestGen.quest.AddPart(questPart_Choice);
			}
		}

		// Token: 0x04005545 RID: 21829
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04005546 RID: 21830
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x04005547 RID: 21831
		public SlateRef<PawnsArrivalModeDef> arrivalMode;

		// Token: 0x04005548 RID: 21832
		public SlateRef<bool> joinPlayer;

		// Token: 0x04005549 RID: 21833
		public SlateRef<IntVec3?> walkInSpot;

		// Token: 0x0400554A RID: 21834
		public SlateRef<string> customLetterLabel;

		// Token: 0x0400554B RID: 21835
		public SlateRef<string> customLetterText;

		// Token: 0x0400554C RID: 21836
		public SlateRef<RulePack> customLetterLabelRules;

		// Token: 0x0400554D RID: 21837
		public SlateRef<RulePack> customLetterTextRules;

		// Token: 0x0400554E RID: 21838
		public SlateRef<LetterDef> customLetterDef;

		// Token: 0x0400554F RID: 21839
		public SlateRef<bool> isSingleReward;

		// Token: 0x04005550 RID: 21840
		public SlateRef<bool> rewardDetailsHidden;

		// Token: 0x04005551 RID: 21841
		private const string RootSymbol = "root";
	}
}
