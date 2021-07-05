using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F9A RID: 8090
	public class QuestNode_GiveRewards : QuestNode
	{
		// Token: 0x0600AC06 RID: 44038 RVA: 0x000707A9 File Offset: 0x0006E9A9
		protected override bool TestRunInt(Slate slate)
		{
			return this.nodeIfChosenPawnSignalUsed == null || this.nodeIfChosenPawnSignalUsed.TestRun(slate);
		}

		// Token: 0x0600AC07 RID: 44039 RVA: 0x00320C38 File Offset: 0x0031EE38
		protected override void RunInt()
		{
			QuestGen.quest.GiveRewards(this.parms.GetValue(QuestGen.slate), this.inSignal.GetValue(QuestGen.slate), this.customLetterLabel.GetValue(QuestGen.slate), this.customLetterText.GetValue(QuestGen.slate), this.customLetterLabelRules.GetValue(QuestGen.slate), this.customLetterTextRules.GetValue(QuestGen.slate), this.useDifficultyFactor.GetValue(QuestGen.slate), delegate
			{
				QuestNode questNode = this.nodeIfChosenPawnSignalUsed;
				if (questNode == null)
				{
					return;
				}
				questNode.Run();
			}, this.variants.GetValue(QuestGen.slate), this.addCampLootReward.GetValue(QuestGen.slate), QuestGen.slate.Get<Pawn>("asker", null, false), false, false);
		}

		// Token: 0x0600AC08 RID: 44040 RVA: 0x0000C32E File Offset: 0x0000A52E
		[Obsolete("Will be removed in the future")]
		private List<Reward> GenerateRewards(RewardsGeneratorParams parmsResolved, Slate slate, bool addDescriptionRules, ref bool chosenPawnSignalUsed, QuestPart_Choice.Choice choice, int variants)
		{
			return null;
		}

		// Token: 0x0600AC09 RID: 44041 RVA: 0x0000C32E File Offset: 0x0000A52E
		[Obsolete("Will be removed in the future")]
		private List<Reward> TryGenerateRewards_SocialOnly(RewardsGeneratorParams parms, bool disallowRoyalFavor)
		{
			return null;
		}

		// Token: 0x0600AC0A RID: 44042 RVA: 0x0000C32E File Offset: 0x0000A52E
		[Obsolete("Will be removed in the future")]
		private List<Reward> TryGenerateRewards_RoyalFavorOnly(RewardsGeneratorParams parms)
		{
			return null;
		}

		// Token: 0x0600AC0B RID: 44043 RVA: 0x0000C32E File Offset: 0x0000A52E
		[Obsolete("Will be removed in the future")]
		private List<Reward> TryGenerateRewards_ThingsOnly(RewardsGeneratorParams parms)
		{
			return null;
		}

		// Token: 0x0600AC0C RID: 44044 RVA: 0x0000C32E File Offset: 0x0000A52E
		[Obsolete("Will be removed in the future")]
		private List<Reward> TryGenerateNonRepeatingRewards(RewardsGeneratorParams parms)
		{
			return null;
		}

		// Token: 0x0600AC0D RID: 44045 RVA: 0x0000A2E4 File Offset: 0x000084E4
		[Obsolete("Will be removed in the future")]
		private int GetDisplayPriority(QuestPart_Choice.Choice choice)
		{
			return 0;
		}

		// Token: 0x0400755E RID: 30046
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400755F RID: 30047
		public SlateRef<RewardsGeneratorParams> parms;

		// Token: 0x04007560 RID: 30048
		public SlateRef<string> customLetterLabel;

		// Token: 0x04007561 RID: 30049
		public SlateRef<string> customLetterText;

		// Token: 0x04007562 RID: 30050
		public SlateRef<RulePack> customLetterLabelRules;

		// Token: 0x04007563 RID: 30051
		public SlateRef<RulePack> customLetterTextRules;

		// Token: 0x04007564 RID: 30052
		public SlateRef<bool?> useDifficultyFactor;

		// Token: 0x04007565 RID: 30053
		public QuestNode nodeIfChosenPawnSignalUsed;

		// Token: 0x04007566 RID: 30054
		public SlateRef<int?> variants;

		// Token: 0x04007567 RID: 30055
		public SlateRef<bool> addCampLootReward;

		// Token: 0x04007568 RID: 30056
		private List<List<Reward>> generatedRewards = new List<List<Reward>>();

		// Token: 0x04007569 RID: 30057
		private const float MinRewardValue = 250f;

		// Token: 0x0400756A RID: 30058
		private const int DefaultVariants = 3;
	}
}
