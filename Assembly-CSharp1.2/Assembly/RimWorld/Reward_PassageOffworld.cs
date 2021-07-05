using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001CEE RID: 7406
	[StaticConstructorOnStartup]
	public class Reward_PassageOffworld : Reward
	{
		// Token: 0x170018E8 RID: 6376
		// (get) Token: 0x0600A105 RID: 41221 RVA: 0x0006B366 File Offset: 0x00069566
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield return QuestPartUtility.GetStandardRewardStackElement("Reward_PassageOffworld_Label".Translate(), Reward_PassageOffworld.Icon, () => this.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst() + ".", null);
				yield break;
			}
		}

		// Token: 0x0600A106 RID: 41222 RVA: 0x0000FC33 File Offset: 0x0000DE33
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600A107 RID: 41223 RVA: 0x0000FC33 File Offset: 0x0000DE33
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600A108 RID: 41224 RVA: 0x002F118C File Offset: 0x002EF38C
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_PassageOffworld".Translate().Resolve();
		}

		// Token: 0x04006D50 RID: 27984
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Icons/Stars", true);
	}
}
