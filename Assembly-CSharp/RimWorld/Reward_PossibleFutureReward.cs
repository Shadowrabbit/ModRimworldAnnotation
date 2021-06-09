using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001CF5 RID: 7413
	[StaticConstructorOnStartup]
	public class Reward_PossibleFutureReward : Reward
	{
		// Token: 0x170018F0 RID: 6384
		// (get) Token: 0x0600A12F RID: 41263 RVA: 0x0006B4E8 File Offset: 0x000696E8
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield return QuestPartUtility.GetStandardRewardStackElement("Reward_PossibleFutureReward_Label".Translate(), Reward_PossibleFutureReward.Icon, () => this.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst() + ".", null);
				yield break;
			}
		}

		// Token: 0x0600A130 RID: 41264 RVA: 0x0000FC33 File Offset: 0x0000DE33
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600A131 RID: 41265 RVA: 0x0000FC33 File Offset: 0x0000DE33
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600A132 RID: 41266 RVA: 0x002F170C File Offset: 0x002EF90C
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_PossibleFutureReward".Translate().Resolve();
		}

		// Token: 0x04006D70 RID: 28016
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Icons/QuestionMark", true);
	}
}
