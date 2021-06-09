using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EEC RID: 3820
	public static class TerrainDefGenerator_Stone
	{
		// Token: 0x0600548B RID: 21643 RVA: 0x0003AAC4 File Offset: 0x00038CC4
		public static IEnumerable<TerrainDef> ImpliedTerrainDefs()
		{
			int i = 0;
			foreach (ThingDef thingDef in from def in DefDatabase<ThingDef>.AllDefs
			where def.building != null && def.building.isNaturalRock && !def.building.isResourceRock
			select def)
			{
				TerrainDef terrainDef = new TerrainDef();
				TerrainDef hewn = new TerrainDef();
				TerrainDef smooth = new TerrainDef();
				terrainDef.texturePath = "Terrain/Surfaces/RoughStone";
				terrainDef.edgeType = TerrainDef.TerrainEdgeType.FadeRough;
				terrainDef.pathCost = 2;
				StatUtility.SetStatValueInList(ref terrainDef.statBases, StatDefOf.Beauty, -1f);
				terrainDef.scatterType = "Rocky";
				terrainDef.affordances = new List<TerrainAffordanceDef>();
				terrainDef.affordances.Add(TerrainAffordanceDefOf.Light);
				terrainDef.affordances.Add(TerrainAffordanceDefOf.Medium);
				terrainDef.affordances.Add(TerrainAffordanceDefOf.Heavy);
				terrainDef.affordances.Add(TerrainAffordanceDefOf.SmoothableStone);
				terrainDef.fertility = 0f;
				terrainDef.filthAcceptanceMask = (FilthSourceFlags.Terrain | FilthSourceFlags.Unnatural);
				terrainDef.modContentPack = thingDef.modContentPack;
				terrainDef.renderPrecedence = 190 + i;
				terrainDef.defName = thingDef.defName + "_Rough";
				terrainDef.label = "RoughStoneTerrainLabel".Translate(thingDef.label);
				terrainDef.description = "RoughStoneTerrainDesc".Translate(thingDef.label);
				terrainDef.color = thingDef.graphicData.color;
				thingDef.building.naturalTerrain = terrainDef;
				hewn.texturePath = "Terrain/Surfaces/RoughHewnRock";
				hewn.edgeType = TerrainDef.TerrainEdgeType.FadeRough;
				hewn.pathCost = 1;
				StatUtility.SetStatValueInList(ref hewn.statBases, StatDefOf.Beauty, -1f);
				hewn.scatterType = "Rocky";
				hewn.affordances = new List<TerrainAffordanceDef>();
				hewn.affordances.Add(TerrainAffordanceDefOf.Light);
				hewn.affordances.Add(TerrainAffordanceDefOf.Medium);
				hewn.affordances.Add(TerrainAffordanceDefOf.Heavy);
				hewn.affordances.Add(TerrainAffordanceDefOf.SmoothableStone);
				hewn.fertility = 0f;
				hewn.filthAcceptanceMask = FilthSourceFlags.Any;
				hewn.modContentPack = thingDef.modContentPack;
				hewn.renderPrecedence = 50 + i;
				hewn.defName = thingDef.defName + "_RoughHewn";
				hewn.label = "RoughHewnStoneTerrainLabel".Translate(thingDef.label);
				hewn.description = "RoughHewnStoneTerrainDesc".Translate(thingDef.label);
				hewn.color = thingDef.graphicData.color;
				thingDef.building.leaveTerrain = hewn;
				smooth.texturePath = "Terrain/Surfaces/SmoothStone";
				smooth.edgeType = TerrainDef.TerrainEdgeType.FadeRough;
				smooth.pathCost = 0;
				StatUtility.SetStatValueInList(ref smooth.statBases, StatDefOf.Beauty, 2f);
				StatUtility.SetStatValueInList(ref smooth.statBases, StatDefOf.MarketValue, 8f);
				smooth.scatterType = "Rocky";
				smooth.affordances = new List<TerrainAffordanceDef>();
				smooth.affordances.Add(TerrainAffordanceDefOf.Light);
				smooth.affordances.Add(TerrainAffordanceDefOf.Medium);
				smooth.affordances.Add(TerrainAffordanceDefOf.Heavy);
				smooth.fertility = 0f;
				smooth.filthAcceptanceMask = FilthSourceFlags.Any;
				smooth.modContentPack = thingDef.modContentPack;
				smooth.tags = new List<string>
				{
					"Floor"
				};
				smooth.renderPrecedence = 140 + i;
				smooth.defName = thingDef.defName + "_Smooth";
				smooth.label = "SmoothStoneTerrainLabel".Translate(thingDef.label);
				smooth.description = "SmoothStoneTerrainDesc".Translate(thingDef.label);
				smooth.color = thingDef.graphicData.color;
				terrainDef.smoothedTerrain = smooth;
				hewn.smoothedTerrain = smooth;
				yield return terrainDef;
				yield return hewn;
				yield return smooth;
				int num = i;
				i = num + 1;
				hewn = null;
				smooth = null;
			}
			IEnumerator<ThingDef> enumerator = null;
			yield break;
			yield break;
		}
	}
}
