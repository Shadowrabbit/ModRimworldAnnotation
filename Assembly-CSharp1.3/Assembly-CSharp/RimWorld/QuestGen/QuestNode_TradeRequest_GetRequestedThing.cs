using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200170E RID: 5902
	public class QuestNode_TradeRequest_GetRequestedThing : QuestNode
	{
		// Token: 0x0600884E RID: 34894 RVA: 0x0030FBC8 File Offset: 0x0030DDC8
		private static int RandomRequestCount(ThingDef thingDef, Map map)
		{
			Rand.PushState(Find.TickManager.TicksGame ^ thingDef.GetHashCode() ^ 876093659);
			float num = (float)QuestNode_TradeRequest_GetRequestedThing.BaseValueWantedRange.RandomInRange;
			Rand.PopState();
			num *= QuestNode_TradeRequest_GetRequestedThing.ValueWantedFactorFromWealthCurve.Evaluate(map.wealthWatcher.WealthTotal);
			return ThingUtility.RoundedResourceStackCount(Mathf.Max(1, Mathf.RoundToInt(num / thingDef.BaseMarketValue)));
		}

		// Token: 0x0600884F RID: 34895 RVA: 0x0030FC38 File Offset: 0x0030DE38
		private static bool TryFindRandomRequestedThingDef(Map map, out ThingDef thingDef, out int count, List<ThingDef> dontRequest)
		{
			QuestNode_TradeRequest_GetRequestedThing.requestCountDict.Clear();
			Func<ThingDef, bool> globalValidator = delegate(ThingDef td)
			{
				if (td.BaseMarketValue / td.BaseMass < 5f)
				{
					return false;
				}
				if (!td.alwaysHaulable)
				{
					return false;
				}
				CompProperties_Rottable compProperties = td.GetCompProperties<CompProperties_Rottable>();
				if (compProperties != null && compProperties.daysToRotStart < 10f)
				{
					return false;
				}
				if (td.ingestible != null && td.ingestible.HumanEdible)
				{
					return false;
				}
				if (td == ThingDefOf.Silver)
				{
					return false;
				}
				if (!td.PlayerAcquirable)
				{
					return false;
				}
				int num = QuestNode_TradeRequest_GetRequestedThing.RandomRequestCount(td, map);
				QuestNode_TradeRequest_GetRequestedThing.requestCountDict.Add(td, num);
				return PlayerItemAccessibilityUtility.PossiblyAccessible(td, num, map) && PlayerItemAccessibilityUtility.PlayerCanMake(td, map) && (td.thingSetMakerTags == null || !td.thingSetMakerTags.Contains("RewardStandardHighFreq")) && (dontRequest.NullOrEmpty<ThingDef>() || !dontRequest.Contains(td));
			};
			if ((from td in ThingSetMakerUtility.allGeneratableItems
			where globalValidator(td)
			select td).TryRandomElement(out thingDef))
			{
				count = QuestNode_TradeRequest_GetRequestedThing.requestCountDict[thingDef];
				return true;
			}
			count = 0;
			return false;
		}

		// Token: 0x06008850 RID: 34896 RVA: 0x0030FCA8 File Offset: 0x0030DEA8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			ThingDef thingDef;
			int num;
			if (QuestNode_TradeRequest_GetRequestedThing.TryFindRandomRequestedThingDef(slate.Get<Map>("map", null, false), out thingDef, out num, this.dontRequest.GetValue(slate)))
			{
				slate.Set<ThingDef>(this.storeThingAs.GetValue(slate), thingDef, false);
				slate.Set<int>(this.storeThingCountAs.GetValue(slate), num, false);
				slate.Set<float>(this.storeMarketValueAs.GetValue(slate), thingDef.GetStatValueAbstract(StatDefOf.MarketValue, null) * (float)num, false);
				slate.Set<bool>(this.storeHasQualityAs.GetValue(slate), thingDef.HasComp(typeof(CompQuality)), false);
			}
		}

		// Token: 0x06008851 RID: 34897 RVA: 0x0030FD4C File Offset: 0x0030DF4C
		protected override bool TestRunInt(Slate slate)
		{
			ThingDef thingDef;
			int num;
			if (QuestNode_TradeRequest_GetRequestedThing.TryFindRandomRequestedThingDef(slate.Get<Map>("map", null, false), out thingDef, out num, this.dontRequest.GetValue(slate)))
			{
				slate.Set<ThingDef>(this.storeThingAs.GetValue(slate), thingDef, false);
				slate.Set<int>(this.storeThingCountAs.GetValue(slate), num, false);
				slate.Set<float>(this.storeMarketValueAs.GetValue(slate), thingDef.GetStatValueAbstract(StatDefOf.MarketValue, null) * (float)num, false);
				return true;
			}
			return false;
		}

		// Token: 0x04005633 RID: 22067
		private static readonly IntRange BaseValueWantedRange = new IntRange(500, 2500);

		// Token: 0x04005634 RID: 22068
		private static readonly SimpleCurve ValueWantedFactorFromWealthCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.3f),
				true
			},
			{
				new CurvePoint(50000f, 1f),
				true
			},
			{
				new CurvePoint(300000f, 2f),
				true
			}
		};

		// Token: 0x04005635 RID: 22069
		private static Dictionary<ThingDef, int> requestCountDict = new Dictionary<ThingDef, int>();

		// Token: 0x04005636 RID: 22070
		[NoTranslate]
		public SlateRef<string> storeThingAs;

		// Token: 0x04005637 RID: 22071
		[NoTranslate]
		public SlateRef<string> storeThingCountAs;

		// Token: 0x04005638 RID: 22072
		[NoTranslate]
		public SlateRef<string> storeMarketValueAs;

		// Token: 0x04005639 RID: 22073
		[NoTranslate]
		public SlateRef<string> storeHasQualityAs;

		// Token: 0x0400563A RID: 22074
		public SlateRef<List<ThingDef>> dontRequest;
	}
}
