using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200149B RID: 5275
	[StaticConstructorOnStartup]
	public class Reward_CampLoot : Reward
	{
		// Token: 0x170015C4 RID: 5572
		// (get) Token: 0x06007E12 RID: 32274 RVA: 0x002CAD8B File Offset: 0x002C8F8B
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield return QuestPartUtility.GetStandardRewardStackElement("Reward_CampLoot_Label".Translate(), Reward_CampLoot.Icon, () => this.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst() + ".", null);
				yield break;
			}
		}

		// Token: 0x06007E13 RID: 32275 RVA: 0x0002974C File Offset: 0x0002794C
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06007E14 RID: 32276 RVA: 0x0002974C File Offset: 0x0002794C
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06007E15 RID: 32277 RVA: 0x002CAD9C File Offset: 0x002C8F9C
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_CampLoot".Translate().Resolve();
		}

		// Token: 0x04004E92 RID: 20114
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Overlays/QuestionMark", true);
	}
}
