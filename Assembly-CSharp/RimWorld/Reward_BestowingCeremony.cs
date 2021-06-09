using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001CDF RID: 7391
	[StaticConstructorOnStartup]
	public class Reward_BestowingCeremony : Reward
	{
		// Token: 0x170018D6 RID: 6358
		// (get) Token: 0x0600A09F RID: 41119 RVA: 0x0006B00F File Offset: 0x0006920F
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				if (this.givePsylink)
				{
					yield return QuestPartUtility.GetStandardRewardStackElement("Reward_BestowingCeremony_Label".Translate(), Reward_BestowingCeremony.IconPsylink, () => this.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst() + ".", delegate()
					{
						Find.WindowStack.Add(new Dialog_InfoCard(HediffDefOf.PsychicAmplifier));
					});
				}
				yield return QuestPartUtility.GetStandardRewardStackElement("Reward_Title_Label".Translate(this.titleName.Named("TITLE")), this.awardingFaction.def.FactionIcon, () => "Reward_Title".Translate(this.targetPawnName.Named("PAWNNAME"), this.titleName.Named("TITLE"), this.awardingFaction.Named("FACTION")).Resolve() + ".", delegate()
				{
					if (this.royalTitle != null)
					{
						Find.WindowStack.Add(new Dialog_InfoCard(this.royalTitle, this.awardingFaction));
					}
				});
				yield break;
			}
		}

		// Token: 0x0600A0A0 RID: 41120 RVA: 0x0000FC33 File Offset: 0x0000DE33
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600A0A1 RID: 41121 RVA: 0x0000FC33 File Offset: 0x0000DE33
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600A0A2 RID: 41122 RVA: 0x002F0210 File Offset: 0x002EE410
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_BestowingCeremony".Translate(this.targetPawnName.Named("PAWNNAME")).Resolve();
		}

		// Token: 0x0600A0A3 RID: 41123 RVA: 0x002F0240 File Offset: 0x002EE440
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.targetPawnName, "targetPawnName", null, false);
			Scribe_Values.Look<string>(ref this.titleName, "titleName", null, false);
			Scribe_Values.Look<bool>(ref this.givePsylink, "givePsylink", false, false);
			Scribe_References.Look<Faction>(ref this.awardingFaction, "awardingFaction", false);
			Scribe_Defs.Look<RoyalTitleDef>(ref this.royalTitle, "royalTitle");
		}

		// Token: 0x04006D16 RID: 27926
		private static readonly Texture2D IconPsylink = ContentFinder<Texture2D>.Get("Things/Item/Special/PsylinkNeuroformer", true);

		// Token: 0x04006D17 RID: 27927
		public string targetPawnName;

		// Token: 0x04006D18 RID: 27928
		public string titleName;

		// Token: 0x04006D19 RID: 27929
		public RoyalTitleDef royalTitle;

		// Token: 0x04006D1A RID: 27930
		public bool givePsylink;

		// Token: 0x04006D1B RID: 27931
		public Faction awardingFaction;
	}
}
