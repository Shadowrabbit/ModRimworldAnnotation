using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200149D RID: 5277
	public class Reward_Items : Reward
	{
		// Token: 0x170015C6 RID: 5574
		// (get) Token: 0x06007E23 RID: 32291 RVA: 0x002CB084 File Offset: 0x002C9284
		public List<Thing> ItemsListForReading
		{
			get
			{
				return this.items;
			}
		}

		// Token: 0x170015C7 RID: 5575
		// (get) Token: 0x06007E24 RID: 32292 RVA: 0x002CB08C File Offset: 0x002C928C
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				if (this.usedOrCleanedUp)
				{
					foreach (GenUI.AnonymousStackElement anonymousStackElement in QuestPartUtility.GetRewardStackElementsForThings(this.itemDefs))
					{
						yield return anonymousStackElement;
					}
					IEnumerator<GenUI.AnonymousStackElement> enumerator = null;
				}
				else
				{
					foreach (GenUI.AnonymousStackElement anonymousStackElement2 in QuestPartUtility.GetRewardStackElementsForThings(this.items, false))
					{
						yield return anonymousStackElement2;
					}
					IEnumerator<GenUI.AnonymousStackElement> enumerator = null;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x170015C8 RID: 5576
		// (get) Token: 0x06007E25 RID: 32293 RVA: 0x002CB09C File Offset: 0x002C929C
		public override float TotalMarketValue
		{
			get
			{
				if (this.usedOrCleanedUp)
				{
					return this.lastTotalMarketValue;
				}
				float num = 0f;
				for (int i = 0; i < this.items.Count; i++)
				{
					Thing innerIfMinified = this.items[i].GetInnerIfMinified();
					num += innerIfMinified.MarketValue * (float)this.items[i].stackCount;
				}
				return num;
			}
		}

		// Token: 0x06007E26 RID: 32294 RVA: 0x002CB103 File Offset: 0x002C9303
		public override void Notify_Used()
		{
			this.RememberItems();
			base.Notify_Used();
		}

		// Token: 0x06007E27 RID: 32295 RVA: 0x002CB111 File Offset: 0x002C9311
		public override void Notify_PreCleanup()
		{
			this.RememberItems();
			base.Notify_PreCleanup();
		}

		// Token: 0x06007E28 RID: 32296 RVA: 0x002CB120 File Offset: 0x002C9320
		private void RememberItems()
		{
			if (this.usedOrCleanedUp)
			{
				return;
			}
			this.itemDefs.Clear();
			this.lastTotalMarketValue = 0f;
			for (int i = 0; i < this.items.Count; i++)
			{
				Thing innerIfMinified = this.items[i].GetInnerIfMinified();
				if (!innerIfMinified.Destroyed)
				{
					QualityCategory quality;
					if (!innerIfMinified.TryGetQuality(out quality))
					{
						quality = QualityCategory.Normal;
					}
					this.itemDefs.Add(new Reward_Items.RememberedItem(new ThingStuffPairWithQuality(innerIfMinified.def, innerIfMinified.Stuff, quality), this.items[i].stackCount, this.items[i].LabelNoCount));
					this.lastTotalMarketValue += innerIfMinified.MarketValue * (float)this.items[i].stackCount;
				}
			}
		}

		// Token: 0x06007E29 RID: 32297 RVA: 0x002CB1F8 File Offset: 0x002C93F8
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			this.items.Clear();
			bool flag = true;
			float x2 = (Find.TickManager.TicksGame - Find.History.lastPsylinkAvailable).TicksToDays();
			if (Rand.Chance(QuestTuning.DaysSincePsylinkAvailableToGuaranteedNeuroformerChance.Evaluate(x2)) && ModsConfig.RoyaltyActive && (parms.disallowedThingDefs == null || !parms.disallowedThingDefs.Contains(ThingDefOf.PsychicAmplifier)) && rewardValue >= 600f && (Faction.OfEmpire == null || parms.giverFaction != Faction.OfEmpire))
			{
				this.items.Add(ThingMaker.MakeThing(ThingDefOf.PsychicAmplifier, null));
				rewardValue -= this.items[0].MarketValue;
				if (rewardValue < 100f)
				{
					flag = false;
				}
			}
			if (flag)
			{
				FloatRange value = rewardValue * new FloatRange(0.7f, 1.3f);
				ThingSetMakerParams parms2 = default(ThingSetMakerParams);
				parms2.totalMarketValueRange = new FloatRange?(value);
				parms2.makingFaction = parms.giverFaction;
				if (!parms.disallowedThingDefs.NullOrEmpty<ThingDef>())
				{
					parms2.validator = ((ThingDef x) => !parms.disallowedThingDefs.Contains(x));
				}
				this.items.AddRange(ThingSetMakerDefOf.Reward_ItemsStandard.root.Generate(parms2));
			}
			valueActuallyUsed = this.TotalMarketValue;
		}

		// Token: 0x06007E2A RID: 32298 RVA: 0x002CB359 File Offset: 0x002C9559
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			Slate slate = QuestGen.slate;
			for (int i = 0; i < this.items.Count; i++)
			{
				Pawn pawn = this.items[i] as Pawn;
				if (pawn != null)
				{
					QuestGen.AddToGeneratedPawns(pawn);
					if (!pawn.IsWorldPawn())
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					}
				}
			}
			if (parms.giveToCaravan)
			{
				yield return new QuestPart_GiveToCaravan
				{
					inSignal = slate.Get<string>("inSignal", null, false),
					Things = this.items
				};
			}
			else
			{
				QuestPart_DropPods dropPods = new QuestPart_DropPods();
				dropPods.inSignal = slate.Get<string>("inSignal", null, false);
				if (!customLetterLabel.NullOrEmpty() || customLetterLabelRules != null)
				{
					QuestGen.AddTextRequest("root", delegate(string x)
					{
						dropPods.customLetterLabel = x;
					}, QuestGenUtility.MergeRules(customLetterLabelRules, customLetterLabel, "root"));
				}
				if (!customLetterText.NullOrEmpty() || customLetterTextRules != null)
				{
					QuestGen.AddTextRequest("root", delegate(string x)
					{
						dropPods.customLetterText = x;
					}, QuestGenUtility.MergeRules(customLetterTextRules, customLetterText, "root"));
				}
				dropPods.mapParent = slate.Get<Map>("map", null, false).Parent;
				dropPods.useTradeDropSpot = true;
				dropPods.Things = this.items;
				yield return dropPods;
			}
			slate.Set<List<Thing>>("itemsReward_items", this.items, false);
			slate.Set<float>("itemsReward_totalMarketValue", this.TotalMarketValue, false);
			yield break;
		}

		// Token: 0x06007E2B RID: 32299 RVA: 0x002CB390 File Offset: 0x002C9590
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			if (parms.giveToCaravan)
			{
				return "Reward_Items_Caravan".Translate(GenLabel.ThingsLabel(this.items, "  - "), this.TotalMarketValue.ToStringMoney(null));
			}
			return "Reward_Items".Translate(GenLabel.ThingsLabel(this.items, "  - "), this.TotalMarketValue.ToStringMoney(null));
		}

		// Token: 0x06007E2C RID: 32300 RVA: 0x002CB410 File Offset: 0x002C9610
		public override string ToString()
		{
			string text = base.GetType().Name;
			text = text + "(value " + this.TotalMarketValue.ToStringMoney(null) + ")";
			foreach (Thing thing in this.items)
			{
				text = string.Concat(new string[]
				{
					text,
					"\n  -",
					thing.LabelCap,
					" ",
					(thing.MarketValue * (float)thing.stackCount).ToStringMoney(null)
				});
			}
			return text;
		}

		// Token: 0x06007E2D RID: 32301 RVA: 0x002CB4C8 File Offset: 0x002C96C8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Thing>(ref this.items, "items", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Reward_Items.RememberedItem>(ref this.itemDefs, "itemDefs", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<float>(ref this.lastTotalMarketValue, "lastTotalMarketValue", 0f, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.items.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x04004E95 RID: 20117
		public List<Thing> items = new List<Thing>();

		// Token: 0x04004E96 RID: 20118
		private List<Reward_Items.RememberedItem> itemDefs = new List<Reward_Items.RememberedItem>();

		// Token: 0x04004E97 RID: 20119
		private float lastTotalMarketValue;

		// Token: 0x04004E98 RID: 20120
		private const string RootSymbol = "root";

		// Token: 0x0200281B RID: 10267
		public struct RememberedItem : IExposable
		{
			// Token: 0x0600DC29 RID: 56361 RVA: 0x004186CB File Offset: 0x004168CB
			public RememberedItem(ThingStuffPairWithQuality thing, int stackCount, string label)
			{
				this.thing = thing;
				this.stackCount = stackCount;
				this.label = label;
			}

			// Token: 0x0600DC2A RID: 56362 RVA: 0x004186E2 File Offset: 0x004168E2
			public void ExposeData()
			{
				Scribe_Deep.Look<ThingStuffPairWithQuality>(ref this.thing, "thing", Array.Empty<object>());
				Scribe_Values.Look<int>(ref this.stackCount, "stackCount", 0, false);
				Scribe_Values.Look<string>(ref this.label, "label", null, false);
			}

			// Token: 0x04009793 RID: 38803
			public ThingStuffPairWithQuality thing;

			// Token: 0x04009794 RID: 38804
			public int stackCount;

			// Token: 0x04009795 RID: 38805
			public string label;
		}
	}
}
