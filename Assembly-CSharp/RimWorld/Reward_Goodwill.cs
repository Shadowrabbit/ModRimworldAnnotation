using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001CE4 RID: 7396
	public class Reward_Goodwill : Reward
	{
		// Token: 0x170018DC RID: 6364
		// (get) Token: 0x0600A0C3 RID: 41155 RVA: 0x0006B0F6 File Offset: 0x000692F6
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield return QuestPartUtility.GetStandardRewardStackElement("Goodwill".Translate() + " " + this.amount.ToStringWithSign(), delegate(Rect r)
				{
					GUI.color = this.faction.Color;
					GUI.DrawTexture(r, this.faction.def.FactionIcon);
					GUI.color = Color.white;
				}, () => "GoodwillTip".Translate(this.faction, this.amount, -75, 75, this.faction.PlayerGoodwill, this.faction.PlayerRelationKind.GetLabel()), delegate()
				{
					Find.WindowStack.Add(new Dialog_InfoCard(this.faction));
				});
				yield break;
			}
		}

		// Token: 0x0600A0C4 RID: 41156 RVA: 0x002F0540 File Offset: 0x002EE740
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
				this.amount = Mathf.Max(this.amount, 1);
			}
			this.faction = parms.giverFaction;
		}

		// Token: 0x0600A0C5 RID: 41157 RVA: 0x0006B106 File Offset: 0x00069306
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

		// Token: 0x0600A0C6 RID: 41158 RVA: 0x002F0620 File Offset: 0x002EE820
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return "Reward_Goodwill".Translate(this.faction, this.amount).Resolve();
		}

		// Token: 0x0600A0C7 RID: 41159 RVA: 0x002F0658 File Offset: 0x002EE858
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

		// Token: 0x0600A0C8 RID: 41160 RVA: 0x0006B116 File Offset: 0x00069316
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.amount, "amount", 0, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x04006D27 RID: 27943
		public int amount;

		// Token: 0x04006D28 RID: 27944
		public Faction faction;
	}
}
