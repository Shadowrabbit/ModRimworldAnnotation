using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020014A4 RID: 5284
	[StaticConstructorOnStartup]
	public class Reward_VisitorsHelp : Reward
	{
		// Token: 0x170015D0 RID: 5584
		// (get) Token: 0x06007E5E RID: 32350 RVA: 0x002CBB67 File Offset: 0x002C9D67
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield return QuestPartUtility.GetStandardRewardStackElement("Reward_VisitorsHelp_Label".Translate(), Reward_VisitorsHelp.Icon, () => this.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst() + ".", null);
				yield break;
			}
		}

		// Token: 0x06007E5F RID: 32351 RVA: 0x0002974C File Offset: 0x0002794C
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06007E60 RID: 32352 RVA: 0x0002974C File Offset: 0x0002794C
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06007E61 RID: 32353 RVA: 0x002CBB78 File Offset: 0x002C9D78
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_VisitorsHelp".Translate().Resolve();
		}

		// Token: 0x04004EA4 RID: 20132
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Icons/VisitorsHelp", true);
	}
}
