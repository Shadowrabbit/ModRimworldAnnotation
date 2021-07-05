using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001764 RID: 5988
	public class ThingSetMaker_ResourcePod : ThingSetMaker
	{
		// Token: 0x0600840C RID: 33804 RVA: 0x0027181C File Offset: 0x0026FA1C
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			ThingDef thingDef = ThingSetMaker_ResourcePod.RandomPodContentsDef(false);
			float num = Rand.Range(150f, 600f);
			do
			{
				Thing thing = ThingMaker.MakeThing(thingDef, null);
				int num2 = Rand.Range(20, 40);
				if (num2 > thing.def.stackLimit)
				{
					num2 = thing.def.stackLimit;
				}
				if ((float)num2 * thing.def.BaseMarketValue > num)
				{
					num2 = Mathf.FloorToInt(num / thing.def.BaseMarketValue);
				}
				if (num2 == 0)
				{
					num2 = 1;
				}
				thing.stackCount = num2;
				outThings.Add(thing);
				num -= (float)num2 * thingDef.BaseMarketValue;
			}
			while (outThings.Count < 7 && num > thingDef.BaseMarketValue);
		}

		// Token: 0x0600840D RID: 33805 RVA: 0x00058893 File Offset: 0x00056A93
		private static IEnumerable<ThingDef> PossiblePodContentsDefs()
		{
			return from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Item && d.tradeability.TraderCanSell() && d.equipmentType == EquipmentType.None && d.BaseMarketValue >= 1f && d.BaseMarketValue < 40f && !d.HasComp(typeof(CompHatcher))
			select d;
		}

		// Token: 0x0600840E RID: 33806 RVA: 0x002718C4 File Offset: 0x0026FAC4
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

		// Token: 0x0600840F RID: 33807 RVA: 0x00271978 File Offset: 0x0026FB78
		[DebugOutput("Incidents", false)]
		private static void PodContentsPossibleDefs()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("ThingDefs that can go in the resource pod crash incident.");
			foreach (ThingDef thingDef in ThingSetMaker_ResourcePod.PossiblePodContentsDefs())
			{
				stringBuilder.AppendLine(thingDef.defName);
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x06008410 RID: 33808 RVA: 0x002719E8 File Offset: 0x0026FBE8
		[DebugOutput("Incidents", false)]
		private static void PodContentsTest()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < 100; i++)
			{
				stringBuilder.AppendLine(ThingSetMaker_ResourcePod.RandomPodContentsDef(false).LabelCap);
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x06008411 RID: 33809 RVA: 0x000588BE File Offset: 0x00056ABE
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return ThingSetMaker_ResourcePod.PossiblePodContentsDefs();
		}

		// Token: 0x04005594 RID: 21908
		private const int MaxStacks = 7;

		// Token: 0x04005595 RID: 21909
		private const float MaxMarketValue = 40f;

		// Token: 0x04005596 RID: 21910
		private const float MinMoney = 150f;

		// Token: 0x04005597 RID: 21911
		private const float MaxMoney = 600f;
	}
}
