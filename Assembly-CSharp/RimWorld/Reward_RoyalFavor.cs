using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001CF7 RID: 7415
	public class Reward_RoyalFavor : Reward
	{
		// Token: 0x170018F3 RID: 6387
		// (get) Token: 0x0600A13E RID: 41278 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool MakesUseOfChosenPawnSignal
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170018F4 RID: 6388
		// (get) Token: 0x0600A13F RID: 41279 RVA: 0x0006B534 File Offset: 0x00069734
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

		// Token: 0x0600A140 RID: 41280 RVA: 0x002F17D8 File Offset: 0x002EF9D8
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			this.amount = GenMath.RoundRandom(RewardsGenerator.RewardValueToRoyalFavorCurve.Evaluate(rewardValue));
			this.amount = Mathf.Clamp(this.amount, 1, 12);
			valueActuallyUsed = RewardsGenerator.RewardValueToRoyalFavorCurve.EvaluateInverted((float)this.amount);
			this.faction = parms.giverFaction;
		}

		// Token: 0x0600A141 RID: 41281 RVA: 0x0006B544 File Offset: 0x00069744
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

		// Token: 0x0600A142 RID: 41282 RVA: 0x002F1830 File Offset: 0x002EFA30
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			if (!parms.chosenPawnSignal.NullOrEmpty())
			{
				return "Reward_RoyalFavor_ChoosePawn".Translate(this.faction, this.amount, Faction.OfPlayer.def.pawnsPlural).Resolve();
			}
			return "Reward_RoyalFavor".Translate(this.faction, this.amount).Resolve();
		}

		// Token: 0x0600A143 RID: 41283 RVA: 0x002F18B0 File Offset: 0x002EFAB0
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

		// Token: 0x0600A144 RID: 41284 RVA: 0x0006B55B File Offset: 0x0006975B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.amount, "amount", 0, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x04006D75 RID: 28021
		public int amount;

		// Token: 0x04006D76 RID: 28022
		public Faction faction;
	}
}
