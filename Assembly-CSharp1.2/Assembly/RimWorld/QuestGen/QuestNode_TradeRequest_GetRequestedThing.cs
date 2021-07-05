using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FE1 RID: 8161
	public class QuestNode_TradeRequest_GetRequestedThing : QuestNode
	{
		// Token: 0x0600AD1D RID: 44317 RVA: 0x00326778 File Offset: 0x00324978
		private static int RandomRequestCount(ThingDef thingDef, Map map)
		{
			Rand.PushState(Find.TickManager.TicksGame ^ thingDef.GetHashCode() ^ 876093659);
			float num = (float)QuestNode_TradeRequest_GetRequestedThing.BaseValueWantedRange.RandomInRange;
			Rand.PopState();
			num *= QuestNode_TradeRequest_GetRequestedThing.ValueWantedFactorFromWealthCurve.Evaluate(map.wealthWatcher.WealthTotal);
			return ThingUtility.RoundedResourceStackCount(Mathf.Max(1, Mathf.RoundToInt(num / thingDef.BaseMarketValue)));
		}

		// Token: 0x0600AD1E RID: 44318 RVA: 0x00070CFF File Offset: 0x0006EEFF
		[Obsolete("Only used for mod compatibility. Will be removed in a future version.")]
		private static bool TryFindRandomRequestedThingDef(Map map, out ThingDef thingDef, out int count)
		{
			return QuestNode_TradeRequest_GetRequestedThing.TryFindRandomRequestedThingDef_NewTmp(map, out thingDef, out count, null);
		}

		// Token: 0x0600AD1F RID: 44319 RVA: 0x003267E8 File Offset: 0x003249E8
		private static bool TryFindRandomRequestedThingDef_NewTmp(Map map, out ThingDef thingDef, out int count, List<ThingDef> dontRequest)
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

		// Token: 0x0600AD20 RID: 44320 RVA: 0x00326858 File Offset: 0x00324A58
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			ThingDef thingDef;
			int num;
			if (QuestNode_TradeRequest_GetRequestedThing.TryFindRandomRequestedThingDef_NewTmp(slate.Get<Map>("map", null, false), out thingDef, out num, this.dontRequest.GetValue(slate)))
			{
				slate.Set<ThingDef>(this.storeThingAs.GetValue(slate), thingDef, false);
				slate.Set<int>(this.storeThingCountAs.GetValue(slate), num, false);
				slate.Set<float>(this.storeMarketValueAs.GetValue(slate), thingDef.GetStatValueAbstract(StatDefOf.MarketValue, null) * (float)num, false);
				slate.Set<bool>(this.storeHasQualityAs.GetValue(slate), thingDef.HasComp(typeof(CompQuality)), false);
			}
		}

		// Token: 0x0600AD21 RID: 44321 RVA: 0x003268FC File Offset: 0x00324AFC
		protected override bool TestRunInt(Slate slate)
		{
			ThingDef thingDef;
			int num;
			if (QuestNode_TradeRequest_GetRequestedThing.TryFindRandomRequestedThingDef_NewTmp(slate.Get<Map>("map", null, false), out thingDef, out num, this.dontRequest.GetValue(slate)))
			{
				slate.Set<ThingDef>(this.storeThingAs.GetValue(slate), thingDef, false);
				slate.Set<int>(this.storeThingCountAs.GetValue(slate), num, false);
				slate.Set<float>(this.storeMarketValueAs.GetValue(slate), thingDef.GetStatValueAbstract(StatDefOf.MarketValue, null) * (float)num, false);
				return true;
			}
			return false;
		}

		// Token: 0x0400769D RID: 30365
		private static readonly IntRange BaseValueWantedRange = new IntRange(500, 2500);

		// Token: 0x0400769E RID: 30366
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

		// Token: 0x0400769F RID: 30367
		private static Dictionary<ThingDef, int> requestCountDict = new Dictionary<ThingDef, int>();

		// Token: 0x040076A0 RID: 30368
		[NoTranslate]
		public SlateRef<string> storeThingAs;

		// Token: 0x040076A1 RID: 30369
		[NoTranslate]
		public SlateRef<string> storeThingCountAs;

		// Token: 0x040076A2 RID: 30370
		[NoTranslate]
		public SlateRef<string> storeMarketValueAs;

		// Token: 0x040076A3 RID: 30371
		[NoTranslate]
		public SlateRef<string> storeHasQualityAs;

		// Token: 0x040076A4 RID: 30372
		public SlateRef<List<ThingDef>> dontRequest;
	}
}
