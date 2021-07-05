using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020014A2 RID: 5282
	public class Reward_RoyalFavor : Reward
	{
		// Token: 0x170015CD RID: 5581
		// (get) Token: 0x06007E4D RID: 32333 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool MakesUseOfChosenPawnSignal
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170015CE RID: 5582
		// (get) Token: 0x06007E4E RID: 32334 RVA: 0x002CB8E4 File Offset: 0x002C9AE4
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield return QuestPartUtility.GetStandardRewardStackElement(this.faction.def.royalFavorLabel.CapitalizeFirst() + " " + this.amount.ToStringWithSign(), this.faction.def.RoyalFavorIcon, () => "RoyalFavorTip".Translate(Faction.OfPlayer.def.pawnsPlural, this.amount, this.faction.def.royalFavorLabel, this.faction) + "\n\n" + "ClickForMoreInfo".Translate(), delegate()
				{
					Find.WindowStack.Add(new Dialog_InfoCard(this.faction));
				});
				yield break;
			}
		}

		// Token: 0x06007E4F RID: 32335 RVA: 0x002CB8F4 File Offset: 0x002C9AF4
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			this.amount = GenMath.RoundRandom(RewardsGenerator.RewardValueToRoyalFavorCurve.Evaluate(rewardValue));
			this.amount = Mathf.Clamp(this.amount, 1, 12);
			valueActuallyUsed = RewardsGenerator.RewardValueToRoyalFavorCurve.EvaluateInverted((float)this.amount);
			this.faction = parms.giverFaction;
		}

		// Token: 0x06007E50 RID: 32336 RVA: 0x002CB94A File Offset: 0x002C9B4A
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			Slate slate = QuestGen.slate;
			QuestPart_GiveRoyalFavor questPart_GiveRoyalFavor = new QuestPart_GiveRoyalFavor();
			questPart_GiveRoyalFavor.faction = this.faction;
			questPart_GiveRoyalFavor.amount = this.amount;
			if (!parms.chosenPawnSignal.NullOrEmpty())
			{
				questPart_GiveRoyalFavor.inSignal = QuestGenUtility.HardcodedSignalWithQuestID(parms.chosenPawnSignal);
				questPart_GiveRoyalFavor.signalListenMode = QuestPart.SignalListenMode.Always;
			}
			else
			{
				questPart_GiveRoyalFavor.inSignal = slate.Get<string>("inSignal", null, false);
				questPart_GiveRoyalFavor.giveToAccepter = true;
			}
			yield return questPart_GiveRoyalFavor;
			slate.Set<int>("royalFavorReward_amount", this.amount, false);
			yield break;
		}

		// Token: 0x06007E51 RID: 32337 RVA: 0x002CB964 File Offset: 0x002C9B64
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			if (!parms.chosenPawnSignal.NullOrEmpty())
			{
				return "Reward_RoyalFavor_ChoosePawn".Translate(this.faction, this.amount, Faction.OfPlayer.def.pawnsPlural).Resolve();
			}
			return "Reward_RoyalFavor".Translate(this.faction, this.amount).Resolve();
		}

		// Token: 0x06007E52 RID: 32338 RVA: 0x002CB9E4 File Offset: 0x002C9BE4
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

		// Token: 0x06007E53 RID: 32339 RVA: 0x002CBA3E File Offset: 0x002C9C3E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.amount, "amount", 0, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x04004EA1 RID: 20129
		public int amount;

		// Token: 0x04004EA2 RID: 20130
		public Faction faction;
	}
}
