using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001CFA RID: 7418
	[StaticConstructorOnStartup]
	public class Reward_ShuttleLoot : Reward
	{
		// Token: 0x170018F9 RID: 6393
		// (get) Token: 0x0600A158 RID: 41304 RVA: 0x0006B5F1 File Offset: 0x000697F1
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield return QuestPartUtility.GetStandardRewardStackElement("Reward_ShuttleLoot_Label".Translate(), Reward_ShuttleLoot.Icon, () => this.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst() + ".", null);
				yield break;
			}
		}

		// Token: 0x0600A159 RID: 41305 RVA: 0x0000FC33 File Offset: 0x0000DE33
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600A15A RID: 41306 RVA: 0x0000FC33 File Offset: 0x0000DE33
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600A15B RID: 41307 RVA: 0x002F1B88 File Offset: 0x002EFD88
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_ShuttleLoot".Translate().Resolve();
		}

		// Token: 0x04006D82 RID: 28034
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Icons/CrashedShuttle", true);
	}
}
