using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020016C4 RID: 5828
	public class QuestNode_GiveRewards : QuestNode
	{
		// Token: 0x06008700 RID: 34560 RVA: 0x003057DB File Offset: 0x003039DB
		protected override bool TestRunInt(Slate slate)
		{
			return this.nodeIfChosenPawnSignalUsed == null || this.nodeIfChosenPawnSignalUsed.TestRun(slate);
		}

		// Token: 0x06008701 RID: 34561 RVA: 0x003057F4 File Offset: 0x003039F4
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

		// Token: 0x06008702 RID: 34562 RVA: 0x00002688 File Offset: 0x00000888
		[Obsolete("Will be removed in the future")]
		private List<Reward> GenerateRewards(RewardsGeneratorParams parmsResolved, Slate slate, bool addDescriptionRules, ref bool chosenPawnSignalUsed, QuestPart_Choice.Choice choice, int variants)
		{
			return null;
		}

		// Token: 0x06008703 RID: 34563 RVA: 0x00002688 File Offset: 0x00000888
		[Obsolete("Will be removed in the future")]
		private List<Reward> TryGenerateRewards_SocialOnly(RewardsGeneratorParams parms, bool disallowRoyalFavor)
		{
			return null;
		}

		// Token: 0x06008704 RID: 34564 RVA: 0x00002688 File Offset: 0x00000888
		[Obsolete("Will be removed in the future")]
		private List<Reward> TryGenerateRewards_RoyalFavorOnly(RewardsGeneratorParams parms)
		{
			return null;
		}

		// Token: 0x06008705 RID: 34565 RVA: 0x00002688 File Offset: 0x00000888
		[Obsolete("Will be removed in the future")]
		private List<Reward> TryGenerateRewards_ThingsOnly(RewardsGeneratorParams parms)
		{
			return null;
		}

		// Token: 0x06008706 RID: 34566 RVA: 0x00002688 File Offset: 0x00000888
		[Obsolete("Will be removed in the future")]
		private List<Reward> TryGenerateNonRepeatingRewards(RewardsGeneratorParams parms)
		{
			return null;
		}

		// Token: 0x06008707 RID: 34567 RVA: 0x0001276E File Offset: 0x0001096E
		[Obsolete("Will be removed in the future")]
		private int GetDisplayPriority(QuestPart_Choice.Choice choice)
		{
			return 0;
		}

		// Token: 0x040054EB RID: 21739
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040054EC RID: 21740
		public SlateRef<RewardsGeneratorParams> parms;

		// Token: 0x040054ED RID: 21741
		public SlateRef<string> customLetterLabel;

		// Token: 0x040054EE RID: 21742
		public SlateRef<string> customLetterText;

		// Token: 0x040054EF RID: 21743
		public SlateRef<RulePack> customLetterLabelRules;

		// Token: 0x040054F0 RID: 21744
		public SlateRef<RulePack> customLetterTextRules;

		// Token: 0x040054F1 RID: 21745
		public SlateRef<bool?> useDifficultyFactor;

		// Token: 0x040054F2 RID: 21746
		public QuestNode nodeIfChosenPawnSignalUsed;

		// Token: 0x040054F3 RID: 21747
		public SlateRef<int?> variants;

		// Token: 0x040054F4 RID: 21748
		public SlateRef<bool> addCampLootReward;

		// Token: 0x040054F5 RID: 21749
		private List<List<Reward>> generatedRewards = new List<List<Reward>>();

		// Token: 0x040054F6 RID: 21750
		private const float MinRewardValue = 250f;

		// Token: 0x040054F7 RID: 21751
		private const int DefaultVariants = 3;
	}
}
