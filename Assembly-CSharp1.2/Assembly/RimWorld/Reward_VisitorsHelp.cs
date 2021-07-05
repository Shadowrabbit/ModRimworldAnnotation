using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001CFC RID: 7420
	[StaticConstructorOnStartup]
	public class Reward_VisitorsHelp : Reward
	{
		// Token: 0x170018FC RID: 6396
		// (get) Token: 0x0600A167 RID: 41319 RVA: 0x0006B63D File Offset: 0x0006983D
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield return QuestPartUtility.GetStandardRewardStackElement("Reward_VisitorsHelp_Label".Translate(), Reward_VisitorsHelp.Icon, () => this.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst() + ".", null);
				yield break;
			}
		}

		// Token: 0x0600A168 RID: 41320 RVA: 0x0000FC33 File Offset: 0x0000DE33
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600A169 RID: 41321 RVA: 0x0000FC33 File Offset: 0x0000DE33
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600A16A RID: 41322 RVA: 0x002F1C54 File Offset: 0x002EFE54
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_VisitorsHelp".Translate().Resolve();
		}

		// Token: 0x04006D87 RID: 28039
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Icons/VisitorsHelp", true);
	}
}
