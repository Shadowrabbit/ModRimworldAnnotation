using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009E8 RID: 2536
	public static class ThingDefGenerator_Buildings
	{
		// Token: 0x06003E83 RID: 16003 RVA: 0x00155C92 File Offset: 0x00153E92
		public static IEnumerable<ThingDef> ImpliedBlueprintAndFrameDefs()
		{
			foreach (ThingDef def in DefDatabase<ThingDef>.AllDefs.ToList<ThingDef>())
			{
				ThingDef blueprint = null;
				if (def.BuildableByPlayer)
				{
					blueprint = ThingDefGenerator_Buildings.NewBlueprintDef_Thing(def, false, null);
					yield return blueprint;
					yield return ThingDefGenerator_Buildings.NewFrameDef_Thing(def);
				}
				if (def.Minifiable)
				{
					yield return ThingDefGenerator_Buildings.NewBlueprintDef_Thing(def, true, blueprint);
				}
				blueprint = null;
				def = null;
			}
			List<ThingDef>.Enumerator enumerator = default(List<ThingDef>.Enumerator);
			foreach (TerrainDef terrDef in DefDatabase<TerrainDef>.AllDefs)
			{
				if (terrDef.BuildableByPlayer)
				{
					yield return ThingDefGenerator_Buildings.NewBlueprintDef_Terrain(terrDef);
					yield return ThingDefGenerator_Buildings.NewFrameDef_Terrain(terrDef);
					terrDef = null;
				}
			}
			IEnumerator<TerrainDef> enumerator2 = null;
			yield break;
			yield break;
		}

		// Token: 0x06003E84 RID: 16004 RVA: 0x00155C9C File Offset: 0x00153E9C
		private static ThingDef BaseBlueprintDef()
		{
			return new ThingDef
			{
				category = ThingCategory.Ethereal,
				label = "Unspecified blueprint",
				altitudeLayer = AltitudeLayer.Blueprint,
				useHitPoints = false,
				selectable = true,
				seeThroughFog = true,
				comps = 
				{
					new CompProperties_Forbiddable(),
					new CompProperties_Styleable()
				},
				drawerType = DrawerType.MapMeshOnly
			};
		}

		// Token: 0x06003E85 RID: 16005 RVA: 0x00155D08 File Offset: 0x00153F08
		private static ThingDef BaseFrameDef()
		{
			return new ThingDef
			{
				isFrameInt = true,
				category = ThingCategory.Building,
				label = "Unspecified building frame",
				thingClass = typeof(Frame),
				altitudeLayer = AltitudeLayer.Building,
				useHitPoints = true,
				selectable = true,
				drawerType = DrawerType.RealtimeOnly,
				building = new BuildingProperties(),
				comps = 
				{
					new CompProperties_Forbiddable(),
					new CompProperties_Styleable()
				},
				scatterableOnMapGen = false,
				leaveResourcesWhenKilled = true
			};
		}

		// Token: 0x06003E86 RID: 16006 RVA: 0x00155D9C File Offset: 0x00153F9C
		private static ThingDef NewBlueprintDef_Thing(ThingDef def, bool isInstallBlueprint, ThingDef normalBlueprint = null)
		{
			ThingDef thingDef = ThingDefGenerator_Buildings.BaseBlueprintDef();
			thingDef.defName = ThingDefGenerator_Buildings.BlueprintDefNamePrefix + def.defName;
			thingDef.label = def.label + "BlueprintLabelExtra".Translate();
			thingDef.size = def.size;
			thingDef.clearBuildingArea = def.clearBuildingArea;
			thingDef.modContentPack = def.modContentPack;
			if (!isInstallBlueprint)
			{
				thingDef.constructionSkillPrerequisite = def.constructionSkillPrerequisite;
				thingDef.artisticSkillPrerequisite = def.artisticSkillPrerequisite;
			}
			thingDef.drawPlaceWorkersWhileSelected = def.drawPlaceWorkersWhileSelected;
			if (def.placeWorkers != null)
			{
				thingDef.placeWorkers = new List<Type>(def.placeWorkers);
			}
			if (isInstallBlueprint)
			{
				thingDef.defName = ThingDefGenerator_Buildings.BlueprintDefNamePrefix + ThingDefGenerator_Buildings.InstallBlueprintDefNamePrefix + def.defName;
			}
			if (isInstallBlueprint && normalBlueprint != null)
			{
				thingDef.graphicData = normalBlueprint.graphicData;
			}
			else
			{
				thingDef.graphicData = new GraphicData();
				if (def.building.blueprintGraphicData != null)
				{
					thingDef.graphicData.CopyFrom(def.building.blueprintGraphicData);
					if (thingDef.graphicData.graphicClass == null)
					{
						thingDef.graphicData.graphicClass = typeof(Graphic_Single);
					}
					if (thingDef.graphicData.shaderType == null)
					{
						thingDef.graphicData.shaderType = ShaderTypeDefOf.Transparent;
					}
					if (def.graphicData != null)
					{
						thingDef.graphicData.drawSize = def.graphicData.drawSize;
						thingDef.graphicData.linkFlags = def.graphicData.linkFlags;
						thingDef.graphicData.linkType = def.graphicData.linkType;
						thingDef.graphicData.asymmetricLink = def.graphicData.asymmetricLink;
					}
					thingDef.graphicData.color = ThingDefGenerator_Buildings.BlueprintColor;
				}
				else
				{
					thingDef.graphicData.CopyFrom(def.graphicData);
					thingDef.graphicData.shaderType = ShaderTypeDefOf.EdgeDetect;
					thingDef.graphicData.color = ThingDefGenerator_Buildings.BlueprintColor;
					thingDef.graphicData.colorTwo = Color.white;
					thingDef.graphicData.shadowData = null;
				}
			}
			if (thingDef.graphicData.shadowData != null)
			{
				Log.Error("Blueprint has shadow: " + def);
			}
			if (isInstallBlueprint)
			{
				thingDef.thingClass = typeof(Blueprint_Install);
			}
			else
			{
				thingDef.thingClass = def.building.blueprintClass;
			}
			if (def.thingClass == typeof(Building_Door))
			{
				thingDef.drawerType = DrawerType.RealtimeOnly;
			}
			else
			{
				thingDef.drawerType = def.drawerType;
			}
			thingDef.entityDefToBuild = def;
			if (isInstallBlueprint)
			{
				def.installBlueprintDef = thingDef;
			}
			else
			{
				def.blueprintDef = thingDef;
			}
			return thingDef;
		}

		// Token: 0x06003E87 RID: 16007 RVA: 0x00156040 File Offset: 0x00154240
		private static ThingDef NewFrameDef_Thing(ThingDef def)
		{
			ThingDef thingDef = ThingDefGenerator_Buildings.BaseFrameDef();
			thingDef.defName = ThingDefGenerator_Buildings.BuildingFrameDefNamePrefix + def.defName;
			thingDef.label = def.label + "FrameLabelExtra".Translate();
			thingDef.size = def.size;
			thingDef.SetStatBaseValue(StatDefOf.MaxHitPoints, (float)def.BaseMaxHitPoints * 0.25f);
			thingDef.SetStatBaseValue(StatDefOf.Beauty, -8f);
			thingDef.SetStatBaseValue(StatDefOf.Flammability, def.BaseFlammability);
			thingDef.fillPercent = 0.2f;
			thingDef.pathCost = DefGenerator.StandardItemPathCost;
			thingDef.description = def.description;
			thingDef.passability = def.passability;
			thingDef.altitudeLayer = def.altitudeLayer;
			if (thingDef.passability > Traversability.PassThroughOnly)
			{
				thingDef.passability = Traversability.PassThroughOnly;
			}
			thingDef.selectable = def.selectable;
			thingDef.constructEffect = def.constructEffect;
			thingDef.building.isEdifice = def.building.isEdifice;
			thingDef.building.watchBuildingInSameRoom = def.building.watchBuildingInSameRoom;
			thingDef.building.watchBuildingStandDistanceRange = def.building.watchBuildingStandDistanceRange;
			thingDef.building.watchBuildingStandRectWidth = def.building.watchBuildingStandRectWidth;
			thingDef.building.artificialForMeditationPurposes = def.building.artificialForMeditationPurposes;
			thingDef.constructionSkillPrerequisite = def.constructionSkillPrerequisite;
			thingDef.artisticSkillPrerequisite = def.artisticSkillPrerequisite;
			thingDef.clearBuildingArea = def.clearBuildingArea;
			thingDef.modContentPack = def.modContentPack;
			thingDef.blocksAltitudes = def.blocksAltitudes;
			thingDef.drawPlaceWorkersWhileSelected = def.drawPlaceWorkersWhileSelected;
			if (def.placeWorkers != null)
			{
				thingDef.placeWorkers = new List<Type>(def.placeWorkers);
			}
			if (def.BuildableByPlayer)
			{
				thingDef.stuffCategories = def.stuffCategories;
				thingDef.costListForDifficulty = def.costListForDifficulty;
			}
			thingDef.entityDefToBuild = def;
			def.frameDef = thingDef;
			return thingDef;
		}

		// Token: 0x06003E88 RID: 16008 RVA: 0x00156230 File Offset: 0x00154430
		private static ThingDef NewBlueprintDef_Terrain(TerrainDef terrDef)
		{
			ThingDef thingDef = ThingDefGenerator_Buildings.BaseBlueprintDef();
			thingDef.thingClass = typeof(Blueprint_Build);
			thingDef.defName = ThingDefGenerator_Buildings.BlueprintDefNamePrefix + terrDef.defName;
			thingDef.label = terrDef.label + "BlueprintLabelExtra".Translate();
			thingDef.entityDefToBuild = terrDef;
			thingDef.graphicData = new GraphicData();
			thingDef.graphicData.shaderType = ShaderTypeDefOf.MetaOverlay;
			thingDef.graphicData.texPath = ThingDefGenerator_Buildings.TerrainBlueprintGraphicPath;
			thingDef.graphicData.graphicClass = typeof(Graphic_Single);
			thingDef.constructionSkillPrerequisite = terrDef.constructionSkillPrerequisite;
			thingDef.artisticSkillPrerequisite = terrDef.artisticSkillPrerequisite;
			thingDef.clearBuildingArea = false;
			thingDef.modContentPack = terrDef.modContentPack;
			thingDef.entityDefToBuild = terrDef;
			terrDef.blueprintDef = thingDef;
			return thingDef;
		}

		// Token: 0x06003E89 RID: 16009 RVA: 0x0015630C File Offset: 0x0015450C
		private static ThingDef NewFrameDef_Terrain(TerrainDef terrDef)
		{
			ThingDef thingDef = ThingDefGenerator_Buildings.BaseFrameDef();
			thingDef.building.artificialForMeditationPurposes = false;
			thingDef.defName = ThingDefGenerator_Buildings.BuildingFrameDefNamePrefix + terrDef.defName;
			thingDef.label = terrDef.label + "FrameLabelExtra".Translate();
			thingDef.entityDefToBuild = terrDef;
			thingDef.useHitPoints = false;
			thingDef.fillPercent = 0f;
			thingDef.description = "Terrain building in progress.";
			thingDef.passability = Traversability.Standable;
			thingDef.selectable = true;
			thingDef.constructEffect = terrDef.constructEffect;
			thingDef.building.isEdifice = false;
			thingDef.constructionSkillPrerequisite = terrDef.constructionSkillPrerequisite;
			thingDef.artisticSkillPrerequisite = terrDef.artisticSkillPrerequisite;
			thingDef.clearBuildingArea = false;
			thingDef.modContentPack = terrDef.modContentPack;
			thingDef.category = ThingCategory.Ethereal;
			thingDef.entityDefToBuild = terrDef;
			terrDef.frameDef = thingDef;
			if (!thingDef.IsFrame)
			{
				Log.Error("Framedef is not frame: " + thingDef);
			}
			return thingDef;
		}

		// Token: 0x040020EF RID: 8431
		public static readonly string BlueprintDefNamePrefix = "Blueprint_";

		// Token: 0x040020F0 RID: 8432
		public static readonly string InstallBlueprintDefNamePrefix = "Install_";

		// Token: 0x040020F1 RID: 8433
		public static readonly string BuildingFrameDefNamePrefix = "Frame_";

		// Token: 0x040020F2 RID: 8434
		private static readonly string TerrainBlueprintGraphicPath = "Things/Special/TerrainBlueprint";

		// Token: 0x040020F3 RID: 8435
		private static Color BlueprintColor = new Color(0.8235294f, 0.92156863f, 1f, 0.6f);
	}
}
