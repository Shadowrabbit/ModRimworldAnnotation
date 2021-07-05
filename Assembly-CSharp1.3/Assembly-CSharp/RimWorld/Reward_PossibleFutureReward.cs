using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020014A0 RID: 5280
	[StaticConstructorOnStartup]
	public class Reward_PossibleFutureReward : Reward
	{
		// Token: 0x170015CB RID: 5579
		// (get) Token: 0x06007E3D RID: 32317 RVA: 0x002CB78F File Offset: 0x002C998F
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield return QuestPartUtility.GetStandardRewardStackElement("Reward_PossibleFutureReward_Label".Translate(), Reward_PossibleFutureReward.Icon, () => this.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst() + ".", null);
				yield break;
			}
		}

		// Token: 0x06007E3E RID: 32318 RVA: 0x0002974C File Offset: 0x0002794C
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06007E3F RID: 32319 RVA: 0x0002974C File Offset: 0x0002794C
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06007E40 RID: 32320 RVA: 0x002CB7A0 File Offset: 0x002C99A0
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_PossibleFutureReward".Translate().Resolve();
		}

		// Token: 0x04004E9E RID: 20126
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Icons/QuestionMark", true);
	}
}
