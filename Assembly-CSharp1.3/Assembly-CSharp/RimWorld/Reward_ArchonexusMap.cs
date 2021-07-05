using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001499 RID: 5273
	[StaticConstructorOnStartup]
	public class Reward_ArchonexusMap : Reward
	{
		// Token: 0x170015C2 RID: 5570
		// (get) Token: 0x06007E00 RID: 32256 RVA: 0x002CAB83 File Offset: 0x002C8D83
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield return QuestPartUtility.GetStandardRewardStackElement("Reward_ArchonexusMapPartLabel".Translate(this.currentPart, 3), Reward_ArchonexusMap.Icon, () => this.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst() + ".", null);
				yield break;
			}
		}

		// Token: 0x06007E01 RID: 32257 RVA: 0x0002974C File Offset: 0x0002794C
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06007E02 RID: 32258 RVA: 0x0002974C File Offset: 0x0002794C
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06007E03 RID: 32259 RVA: 0x002CAB93 File Offset: 0x002C8D93
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_ArchonexusMapPart".Translate(this.currentPart, 3);
		}

		// Token: 0x06007E04 RID: 32260 RVA: 0x002CABB5 File Offset: 0x002C8DB5
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.currentPart, "currentPart", 0, false);
		}

		// Token: 0x04004E89 RID: 20105
		public int currentPart = 1;

		// Token: 0x04004E8A RID: 20106
		private const int totalParts = 3;

		// Token: 0x04004E8B RID: 20107
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Icons/ArchonexusMapPart", true);
	}
}
