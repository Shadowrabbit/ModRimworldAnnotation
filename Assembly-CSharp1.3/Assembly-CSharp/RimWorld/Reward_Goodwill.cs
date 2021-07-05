using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200149C RID: 5276
	public class Reward_Goodwill : Reward
	{
		// Token: 0x170015C5 RID: 5573
		// (get) Token: 0x06007E19 RID: 32281 RVA: 0x002CADFB File Offset: 0x002C8FFB
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield return QuestPartUtility.GetStandardRewardStackElement("Goodwill".Translate() + " " + this.amount.ToStringWithSign(), delegate(Rect r)
				{
					GUI.color = this.faction.Color;
					GUI.DrawTexture(r, this.faction.def.FactionIcon);
					GUI.color = Color.white;
				}, () => "GoodwillTip".Translate(this.faction, this.amount, -75, 75, this.faction.PlayerGoodwill, this.faction.PlayerRelationKind.GetLabelCap()), delegate()
				{
					Find.WindowStack.Add(new Dialog_InfoCard(this.faction));
				});
				yield break;
			}
		}

		// Token: 0x06007E1A RID: 32282 RVA: 0x002CAE0C File Offset: 0x002C900C
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			this.amount = GenMath.RoundRandom(RewardsGenerator.RewardValueToGoodwillCurve.Evaluate(rewardValue));
			this.amount = Mathf.Min(this.amount, 100 - parms.giverFaction.PlayerGoodwill);
			this.amount = Mathf.Max(this.amount, 1);
			valueActuallyUsed = RewardsGenerator.RewardValueToGoodwillCurve.EvaluateInverted((float)this.amount);
			if (parms.giverFaction.HostileTo(Faction.OfPlayer))
			{
				this.amount += Mathf.Clamp(-parms.giverFaction.PlayerGoodwill / 2, 0, this.amount);
				this.amount = Mathf.Min(this.amount, 100 - parms.giverFaction.PlayerGoodwill);
				if (this.amount < 1)
				{
					Log.Warning("Tried to use " + this.amount + " goodwill in Reward_Goodwill. A different reward type should have been chosen in this case.");
					this.amount = 1;
				}
			}
			this.faction = parms.giverFaction;
		}

		// Token: 0x06007E1B RID: 32283 RVA: 0x002CAF06 File Offset: 0x002C9106
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			yield return new QuestPart_FactionGoodwillChange
			{
				change = this.amount,
				faction = this.faction,
				inSignal = QuestGen.slate.Get<string>("inSignal", null, false)
			};
			yield break;
		}

		// Token: 0x06007E1C RID: 32284 RVA: 0x002CAF18 File Offset: 0x002C9118
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_Goodwill".Translate(this.faction, this.amount).Resolve();
		}

		// Token: 0x06007E1D RID: 32285 RVA: 0x002CAF50 File Offset: 0x002C9150
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				base.GetType().Name,
				" (faction=",
				this.faction.ToStringSafe<Faction>(),
				", amount=",
				this.amount,
				")"
			});
		}

		// Token: 0x06007E1E RID: 32286 RVA: 0x002CAFAA File Offset: 0x002C91AA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.amount, "amount", 0, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x04004E93 RID: 20115
		public int amount;

		// Token: 0x04004E94 RID: 20116
		public Faction faction;
	}
}
