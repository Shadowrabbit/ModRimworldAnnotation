using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FAB RID: 8107
	public class QuestNode_PawnsArrive : QuestNode
	{
		// Token: 0x0600AC40 RID: 44096 RVA: 0x0007086B File Offset: 0x0006EA6B
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Exists("map", false);
		}

		// Token: 0x0600AC41 RID: 44097 RVA: 0x0032184C File Offset: 0x0031FA4C
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

		// Token: 0x040075B0 RID: 30128
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040075B1 RID: 30129
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x040075B2 RID: 30130
		public SlateRef<PawnsArrivalModeDef> arrivalMode;

		// Token: 0x040075B3 RID: 30131
		public SlateRef<bool> joinPlayer;

		// Token: 0x040075B4 RID: 30132
		public SlateRef<IntVec3?> walkInSpot;

		// Token: 0x040075B5 RID: 30133
		public SlateRef<string> customLetterLabel;

		// Token: 0x040075B6 RID: 30134
		public SlateRef<string> customLetterText;

		// Token: 0x040075B7 RID: 30135
		public SlateRef<RulePack> customLetterLabelRules;

		// Token: 0x040075B8 RID: 30136
		public SlateRef<RulePack> customLetterTextRules;

		// Token: 0x040075B9 RID: 30137
		public SlateRef<bool> isSingleReward;

		// Token: 0x040075BA RID: 30138
		public SlateRef<bool> rewardDetailsHidden;

		// Token: 0x040075BB RID: 30139
		private const string RootSymbol = "root";
	}
}
