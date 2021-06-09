using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001CE2 RID: 7394
	[StaticConstructorOnStartup]
	public class Reward_CampLoot : Reward
	{
		// Token: 0x170018D9 RID: 6361
		// (get) Token: 0x0600A0B4 RID: 41140 RVA: 0x0006B0AA File Offset: 0x000692AA
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield return QuestPartUtility.GetStandardRewardStackElement("Reward_CampLoot_Label".Translate(), Reward_CampLoot.Icon, () => this.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst() + ".", null);
				yield break;
			}
		}

		// Token: 0x0600A0B5 RID: 41141 RVA: 0x0000FC33 File Offset: 0x0000DE33
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600A0B6 RID: 41142 RVA: 0x0000FC33 File Offset: 0x0000DE33
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600A0B7 RID: 41143 RVA: 0x002F0474 File Offset: 0x002EE674
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_CampLoot".Translate().Resolve();
		}

		// Token: 0x04006D22 RID: 27938
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Overlays/QuestionMark", true);
	}
}
