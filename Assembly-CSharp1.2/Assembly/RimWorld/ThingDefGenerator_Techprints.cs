using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EF5 RID: 3829
	public static class ThingDefGenerator_Techprints
	{
		// Token: 0x060054BF RID: 21695 RVA: 0x0003AC49 File Offset: 0x00038E49
		public static IEnumerable<ThingDef> ImpliedTechprintDefs()
		{
			if (!ModLister.RoyaltyInstalled)
			{
				yield break;
			}
			foreach (ResearchProjectDef researchProjectDef in DefDatabase<ResearchProjectDef>.AllDefsListForReading)
			{
				if (researchProjectDef.TechprintCount > 0)
				{
					ThingDef thingDef = new ThingDef();
					thingDef.resourceReadoutPriority = ResourceCountPriority.Middle;
					thingDef.category = ThingCategory.Item;
					thingDef.thingClass = typeof(ThingWithComps);
					thingDef.thingCategories = new List<ThingCategoryDef>();
					thingDef.thingCategories.Add(ThingCategoryDefOf.Techprints);
					thingDef.graphicData = new GraphicData();
					thingDef.graphicData.graphicClass = typeof(Graphic_Single);
					thingDef.useHitPoints = true;
					thingDef.selectable = true;
					thingDef.thingSetMakerTags = new List<string>();
					thingDef.thingSetMakerTags.Add("Techprint");
					thingDef.SetStatBaseValue(StatDefOf.MaxHitPoints, 100f);
					thingDef.SetStatBaseValue(StatDefOf.Flammability, 1f);
					thingDef.SetStatBaseValue(StatDefOf.MarketValue, researchProjectDef.techprintMarketValue);
					thingDef.SetStatBaseValue(StatDefOf.Mass, 0.03f);
					thingDef.SetStatBaseValue(StatDefOf.SellPriceFactor, 0.1f);
					thingDef.altitudeLayer = AltitudeLayer.Item;
					thingDef.comps.Add(new CompProperties_Forbiddable());
					thingDef.comps.Add(new CompProperties_Techprint
					{
						project = researchProjectDef
					});
					thingDef.tickerType = TickerType.Never;
					thingDef.alwaysHaulable = true;
					thingDef.rotatable = false;
					thingDef.pathCost = DefGenerator.StandardItemPathCost;
					thingDef.drawGUIOverlay = true;
					thingDef.modContentPack = researchProjectDef.modContentPack;
					thingDef.tradeTags = new List<string>();
					thingDef.tradeTags.Add("Techprint");
					thingDef.category = ThingCategory.Item;
					thingDef.description = "TechprintDesc".Translate(researchProjectDef.Named("PROJECT")) + "\n\n" + researchProjectDef.LabelCap + "\n\n" + researchProjectDef.description;
					thingDef.useHitPoints = true;
					if (thingDef.thingCategories == null)
					{
						thingDef.thingCategories = new List<ThingCategoryDef>();
					}
					thingDef.graphicData.texPath = "Things/Item/Special/TechprintUltratech";
					thingDef.defName = "Techprint_" + researchProjectDef.defName;
					thingDef.label = "TechprintLabel".Translate(researchProjectDef.Named("PROJECT"));
					yield return thingDef;
				}
			}
			List<ResearchProjectDef>.Enumerator enumerator = default(List<ResearchProjectDef>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x04003573 RID: 13683
		public const string Tag = "Techprint";
	}
}
