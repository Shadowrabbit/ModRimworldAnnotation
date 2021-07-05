using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020010E8 RID: 4328
	public class ThingSetMaker_ResourcePod : ThingSetMaker
	{
		// Token: 0x06006787 RID: 26503 RVA: 0x0022FDC8 File Offset: 0x0022DFC8
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			ThingDef thingDef = ThingSetMaker_ResourcePod.RandomPodContentsDef(false);
			float num = Rand.Range(150f, 600f);
			while (num > thingDef.BaseMarketValue)
			{
				ThingDef stuff = GenStuff.RandomStuffByCommonalityFor(thingDef, TechLevel.Undefined);
				Thing thing = ThingMaker.MakeThing(thingDef, stuff);
				int num2 = Rand.Range(20, 40);
				if (num2 > thing.def.stackLimit)
				{
					num2 = thing.def.stackLimit;
				}
				float statValue = thing.GetStatValue(StatDefOf.MarketValue, true);
				if ((float)num2 * statValue > num)
				{
					num2 = Mathf.FloorToInt(num / statValue);
				}
				if (num2 == 0)
				{
					num2 = 1;
				}
				thing.stackCount = num2;
				outThings.Add(thing);
				num -= (float)num2 * statValue;
				if (outThings.Count >= 7 || num <= statValue)
				{
					break;
				}
			}
		}

		// Token: 0x06006788 RID: 26504 RVA: 0x0022FE81 File Offset: 0x0022E081
		private static IEnumerable<ThingDef> PossiblePodContentsDefs()
		{
			return from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Item && d.tradeability.TraderCanSell() && d.equipmentType == EquipmentType.None && d.BaseMarketValue >= 1f && d.BaseMarketValue < 40f && !d.HasComp(typeof(CompHatcher))
			select d;
		}

		// Token: 0x06006789 RID: 26505 RVA: 0x0022FEAC File Offset: 0x0022E0AC
		public static ThingDef RandomPodContentsDef(bool mustBeResource = false)
		{
			IEnumerable<ThingDef> source = ThingSetMaker_ResourcePod.PossiblePodContentsDefs();
			if (mustBeResource)
			{
				source = from x in source
				where x.stackLimit > 1
				select x;
			}
			int numMeats = (from x in source
			where x.IsMeat
			select x).Count<ThingDef>();
			int numLeathers = (from x in source
			where x.IsLeather
			select x).Count<ThingDef>();
			return source.RandomElementByWeight((ThingDef d) => ThingSetMakerUtility.AdjustedBigCategoriesSelectionWeight(d, numMeats, numLeathers));
		}

		// Token: 0x0600678A RID: 26506 RVA: 0x0022FF60 File Offset: 0x0022E160
		[DebugOutput("Incidents", false)]
		private static void PodContentsPossibleDefs()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("ThingDefs that can go in the resource pod crash incident.");
			foreach (ThingDef thingDef in ThingSetMaker_ResourcePod.PossiblePodContentsDefs())
			{
				stringBuilder.AppendLine(thingDef.defName);
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x0600678B RID: 26507 RVA: 0x0022FFD0 File Offset: 0x0022E1D0
		[DebugOutput("Incidents", false)]
		private static void PodContentsTest()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < 100; i++)
			{
				stringBuilder.AppendLine(ThingSetMaker_ResourcePod.RandomPodContentsDef(false).LabelCap);
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x0600678C RID: 26508 RVA: 0x00230012 File Offset: 0x0022E212
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return ThingSetMaker_ResourcePod.PossiblePodContentsDefs();
		}

		// Token: 0x04003A5D RID: 14941
		private const int MaxStacks = 7;

		// Token: 0x04003A5E RID: 14942
		private const float MaxMarketValue = 40f;

		// Token: 0x04003A5F RID: 14943
		private const float MinMoney = 150f;

		// Token: 0x04003A60 RID: 14944
		private const float MaxMoney = 600f;
	}
}
