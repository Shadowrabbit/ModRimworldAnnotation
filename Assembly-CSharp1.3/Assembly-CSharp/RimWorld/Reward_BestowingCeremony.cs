using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200149A RID: 5274
	[StaticConstructorOnStartup]
	public class Reward_BestowingCeremony : Reward
	{
		// Token: 0x170015C3 RID: 5571
		// (get) Token: 0x06007E08 RID: 32264 RVA: 0x002CAC1B File Offset: 0x002C8E1B
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				if (this.givePsylink)
				{
					yield return QuestPartUtility.GetStandardRewardStackElement("Reward_BestowingCeremony_Label".Translate(), Reward_BestowingCeremony.IconPsylink, () => this.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst() + ".", delegate()
					{
						Find.WindowStack.Add(new Dialog_InfoCard(HediffDefOf.PsychicAmplifier, null));
					});
				}
				yield return QuestPartUtility.GetStandardRewardStackElement("Reward_Title_Label".Translate(this.titleName.Named("TITLE")), this.awardingFaction.def.FactionIcon, () => "Reward_Title".Translate(this.targetPawnName.Named("PAWNNAME"), this.titleName.Named("TITLE"), this.awardingFaction.Named("FACTION")).Resolve() + ".", delegate()
				{
					if (this.royalTitle != null)
					{
						Find.WindowStack.Add(new Dialog_InfoCard(this.royalTitle, this.awardingFaction, null));
					}
				});
				yield break;
			}
		}

		// Token: 0x06007E09 RID: 32265 RVA: 0x0002974C File Offset: 0x0002794C
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06007E0A RID: 32266 RVA: 0x0002974C File Offset: 0x0002794C
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06007E0B RID: 32267 RVA: 0x002CAC2C File Offset: 0x002C8E2C
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_BestowingCeremony".Translate(this.targetPawnName.Named("PAWNNAME")).Resolve();
		}

		// Token: 0x06007E0C RID: 32268 RVA: 0x002CAC5C File Offset: 0x002C8E5C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.targetPawnName, "targetPawnName", null, false);
			Scribe_Values.Look<string>(ref this.titleName, "titleName", null, false);
			Scribe_Values.Look<bool>(ref this.givePsylink, "givePsylink", false, false);
			Scribe_References.Look<Faction>(ref this.awardingFaction, "awardingFaction", false);
			Scribe_Defs.Look<RoyalTitleDef>(ref this.royalTitle, "royalTitle");
		}

		// Token: 0x04004E8C RID: 20108
		private static readonly Texture2D IconPsylink = ContentFinder<Texture2D>.Get("Things/Item/Special/PsylinkNeuroformer", true);

		// Token: 0x04004E8D RID: 20109
		public string targetPawnName;

		// Token: 0x04004E8E RID: 20110
		public string titleName;

		// Token: 0x04004E8F RID: 20111
		public RoyalTitleDef royalTitle;

		// Token: 0x04004E90 RID: 20112
		public bool givePsylink;

		// Token: 0x04004E91 RID: 20113
		public Faction awardingFaction;
	}
}
