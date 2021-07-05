using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200149E RID: 5278
	[StaticConstructorOnStartup]
	public class Reward_PassageOffworld : Reward
	{
		// Token: 0x170015C9 RID: 5577
		// (get) Token: 0x06007E2F RID: 32303 RVA: 0x002CB56E File Offset: 0x002C976E
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield return QuestPartUtility.GetStandardRewardStackElement("Reward_PassageOffworld_Label".Translate(), Reward_PassageOffworld.Icon, () => this.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst() + ".", null);
				yield break;
			}
		}

		// Token: 0x06007E30 RID: 32304 RVA: 0x0002974C File Offset: 0x0002794C
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06007E31 RID: 32305 RVA: 0x0002974C File Offset: 0x0002794C
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06007E32 RID: 32306 RVA: 0x002CB580 File Offset: 0x002C9780
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_PassageOffworld".Translate().Resolve();
		}

		// Token: 0x04004E99 RID: 20121
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Icons/Stars", true);
	}
}
