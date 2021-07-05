using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015AC RID: 5548
	public class SymbolResolver_BasePart_Outdoors_Leaf_PowerPlant : SymbolResolver
	{
		// Token: 0x060082E2 RID: 33506 RVA: 0x002E8A7C File Offset: 0x002E6C7C
		public override bool CanResolve(ResolveParams rp)
		{
			if (!base.CanResolve(rp))
			{
				return false;
			}
			if (BaseGen.globalSettings.basePart_buildingsResolved < BaseGen.globalSettings.minBuildings)
			{
				return false;
			}
			if (BaseGen.globalSettings.basePart_landingPadsResolved < BaseGen.globalSettings.minLandingPads && rp.rect.Width >= 9 && rp.rect.Height >= 9)
			{
				return false;
			}
			if (BaseGen.globalSettings.basePart_emptyNodesResolved < BaseGen.globalSettings.minEmptyNodes)
			{
				return false;
			}
			if (BaseGen.globalSettings.basePart_powerPlantsCoverage + (float)rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area >= 0.09f)
			{
				return false;
			}
			if (rp.faction != null && rp.faction.def.techLevel < TechLevel.Industrial)
			{
				return false;
			}
			if (rp.rect.Width > 13 || rp.rect.Height > 13)
			{
				return false;
			}
			this.CalculateAvailablePowerPlants(rp.rect);
			return SymbolResolver_BasePart_Outdoors_Leaf_PowerPlant.availablePowerPlants.Any<ThingDef>();
		}

		// Token: 0x060082E3 RID: 33507 RVA: 0x002E8B84 File Offset: 0x002E6D84
		public override void Resolve(ResolveParams rp)
		{
			this.CalculateAvailablePowerPlants(rp.rect);
			if (!SymbolResolver_BasePart_Outdoors_Leaf_PowerPlant.availablePowerPlants.Any<ThingDef>())
			{
				return;
			}
			BaseGen.symbolStack.Push("refuel", rp, null);
			ThingDef thingDef = SymbolResolver_BasePart_Outdoors_Leaf_PowerPlant.availablePowerPlants.RandomElement<ThingDef>();
			ResolveParams resolveParams = rp;
			resolveParams.singleThingDef = thingDef;
			resolveParams.fillWithThingsPadding = new int?(rp.fillWithThingsPadding ?? Mathf.Max(5 - thingDef.size.x, 1));
			BaseGen.symbolStack.Push("fillWithThings", resolveParams, null);
			BaseGen.globalSettings.basePart_powerPlantsCoverage += (float)rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area;
		}

		// Token: 0x060082E4 RID: 33508 RVA: 0x002E8C48 File Offset: 0x002E6E48
		private void CalculateAvailablePowerPlants(CellRect rect)
		{
			Map map = BaseGen.globalSettings.map;
			SymbolResolver_BasePart_Outdoors_Leaf_PowerPlant.availablePowerPlants.Clear();
			if (rect.Width >= ThingDefOf.SolarGenerator.size.x && rect.Height >= ThingDefOf.SolarGenerator.size.z)
			{
				int num = 0;
				using (CellRect.Enumerator enumerator = rect.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.Roofed(map))
						{
							num++;
						}
					}
				}
				if ((float)num / (float)rect.Area >= 0.5f)
				{
					SymbolResolver_BasePart_Outdoors_Leaf_PowerPlant.availablePowerPlants.Add(ThingDefOf.SolarGenerator);
				}
			}
			if (rect.Width >= ThingDefOf.WoodFiredGenerator.size.x && rect.Height >= ThingDefOf.WoodFiredGenerator.size.z)
			{
				SymbolResolver_BasePart_Outdoors_Leaf_PowerPlant.availablePowerPlants.Add(ThingDefOf.WoodFiredGenerator);
			}
		}

		// Token: 0x040051EB RID: 20971
		private static List<ThingDef> availablePowerPlants = new List<ThingDef>();

		// Token: 0x040051EC RID: 20972
		private const float MaxCoverage = 0.09f;
	}
}
