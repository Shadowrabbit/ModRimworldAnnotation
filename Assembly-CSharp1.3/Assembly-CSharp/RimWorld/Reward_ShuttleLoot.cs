using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020014A3 RID: 5283
	[StaticConstructorOnStartup]
	public class Reward_ShuttleLoot : Reward
	{
		// Token: 0x170015CF RID: 5583
		// (get) Token: 0x06007E57 RID: 32343 RVA: 0x002CBAF7 File Offset: 0x002C9CF7
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield return QuestPartUtility.GetStandardRewardStackElement("Reward_ShuttleLoot_Label".Translate(), Reward_ShuttleLoot.Icon, () => this.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst() + ".", null);
				yield break;
			}
		}

		// Token: 0x06007E58 RID: 32344 RVA: 0x0002974C File Offset: 0x0002794C
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06007E59 RID: 32345 RVA: 0x0002974C File Offset: 0x0002794C
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06007E5A RID: 32346 RVA: 0x002CBB08 File Offset: 0x002C9D08
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_ShuttleLoot".Translate().Resolve();
		}

		// Token: 0x04004EA3 RID: 20131
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Icons/CrashedShuttle", true);
	}
}
