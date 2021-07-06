using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001CE7 RID: 7399
	public class Reward_Items : Reward
	{
		// Token: 0x170018E1 RID: 6369
		// (get) Token: 0x0600A0DD RID: 41181 RVA: 0x0006B1DE File Offset: 0x000693DE
		public List<Thing> ItemsListForReading
		{
			get
			{
				return this.items;
			}
		}

		// Token: 0x170018E2 RID: 6370
		// (get) Token: 0x0600A0DE RID: 41182 RVA: 0x0006B1E6 File Offset: 0x000693E6
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

		// Token: 0x170018E3 RID: 6371
		// (get) Token: 0x0600A0DF RID: 41183 RVA: 0x002F08B0 File Offset: 0x002EEAB0
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

		// Token: 0x0600A0E0 RID: 41184 RVA: 0x0006B1F6 File Offset: 0x000693F6
		public override void Notify_Used()
		{
			this.RememberItems();
			base.Notify_Used();
		}

		// Token: 0x0600A0E1 RID: 41185 RVA: 0x0006B204 File Offset: 0x00069404
		public override void Notify_PreCleanup()
		{
			this.RememberItems();
			base.Notify_PreCleanup();
		}

		// Token: 0x0600A0E2 RID: 41186 RVA: 0x002F0918 File Offset: 0x002EEB18
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

		// Token: 0x0600A0E3 RID: 41187 RVA: 0x002F09F0 File Offset: 0x002EEBF0
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			this.items.Clear();
			bool flag = true;
			float x2 = (Find.TickManager.TicksGame - Find.History.lastPsylinkAvailable).TicksToDays();
			if (Rand.Chance(QuestTuning.DaysSincePsylinkAvailableToGuaranteedNeuroformerChance.Evaluate(x2)) && ModsConfig.RoyaltyActive && (parms.disallowedThingDefs == null || !parms.disallowedThingDefs.Contains(ThingDefOf.PsychicAmplifier)) && rewardValue >= 600f && parms.giverFaction != Faction.Empire)
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

		// Token: 0x0600A0E4 RID: 41188 RVA: 0x0006B212 File Offset: 0x00069412
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

		// Token: 0x0600A0E5 RID: 41189 RVA: 0x002F0B48 File Offset: 0x002EED48
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			if (parms.giveToCaravan)
			{
				return "Reward_Items_Caravan".Translate(GenLabel.ThingsLabel(this.items, "  - "), this.TotalMarketValue.ToStringMoney(null));
			}
			return "Reward_Items".Translate(GenLabel.ThingsLabel(this.items, "  - "), this.TotalMarketValue.ToStringMoney(null));
		}

		// Token: 0x0600A0E6 RID: 41190 RVA: 0x002F0BC8 File Offset: 0x002EEDC8
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

		// Token: 0x0600A0E7 RID: 41191 RVA: 0x002F0C80 File Offset: 0x002EEE80
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

		// Token: 0x04006D31 RID: 27953
		public List<Thing> items = new List<Thing>();

		// Token: 0x04006D32 RID: 27954
		private List<Reward_Items.RememberedItem> itemDefs = new List<Reward_Items.RememberedItem>();

		// Token: 0x04006D33 RID: 27955
		private float lastTotalMarketValue;

		// Token: 0x04006D34 RID: 27956
		private const string RootSymbol = "root";

		// Token: 0x02001CE8 RID: 7400
		public struct RememberedItem : IExposable
		{
			// Token: 0x0600A0E9 RID: 41193 RVA: 0x0006B266 File Offset: 0x00069466
			public RememberedItem(ThingStuffPairWithQuality thing, int stackCount, string label)
			{
				this.thing = thing;
				this.stackCount = stackCount;
				this.label = label;
			}

			// Token: 0x0600A0EA RID: 41194 RVA: 0x0006B27D File Offset: 0x0006947D
			public void ExposeData()
			{
				Scribe_Deep.Look<ThingStuffPairWithQuality>(ref this.thing, "thing", Array.Empty<object>());
				Scribe_Values.Look<int>(ref this.stackCount, "stackCount", 0, false);
				Scribe_Values.Look<string>(ref this.label, "label", null, false);
			}

			// Token: 0x04006D35 RID: 27957
			public ThingStuffPairWithQuality thing;

			// Token: 0x04006D36 RID: 27958
			public int stackCount;

			// Token: 0x04006D37 RID: 27959
			public string label;
		}
	}
}
