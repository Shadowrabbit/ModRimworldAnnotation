using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020014A1 RID: 5281
	public class Reward_RelicInfo : Reward
	{
		// Token: 0x170015CC RID: 5580
		// (get) Token: 0x06007E44 RID: 32324 RVA: 0x002CB7FF File Offset: 0x002C99FF
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield return QuestPartUtility.GetStandardRewardStackElement("Reward_RelicInfoLabel".Translate(this.relic.Label), delegate(Rect rect)
				{
					this.relic.DrawIcon(rect);
				}, () => this.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst() + ".", delegate()
				{
					Quest quest = this.quest;
					if (((quest != null) ? quest.parent : null) != null)
					{
						Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
						((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(this.quest.parent);
					}
				});
				yield break;
			}
		}

		// Token: 0x06007E45 RID: 32325 RVA: 0x0002974C File Offset: 0x0002794C
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06007E46 RID: 32326 RVA: 0x0002974C File Offset: 0x0002794C
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06007E47 RID: 32327 RVA: 0x002CB80F File Offset: 0x002C9A0F
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_RelicInfo".Translate(this.relic.Label);
		}

		// Token: 0x06007E48 RID: 32328 RVA: 0x002CB830 File Offset: 0x002C9A30
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Precept_Relic>(ref this.relic, "relic", false);
			Scribe_References.Look<Quest>(ref this.quest, "quest", false);
		}

		// Token: 0x04004E9F RID: 20127
		public Precept_Relic relic;

		// Token: 0x04004EA0 RID: 20128
		public Quest quest;
	}
}
