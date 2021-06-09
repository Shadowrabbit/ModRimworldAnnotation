using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001CB6 RID: 7350
	public static class GenConstruct
	{
		// Token: 0x06009FDA RID: 40922 RVA: 0x0006A98C File Offset: 0x00068B8C
		public static void Reset()
		{
			GenConstruct.SkillTooLowTrans = "SkillTooLowForConstruction".Translate();
			GenConstruct.IncapableOfDeconstruction = "IncapableOfDeconstruction".Translate();
			GenConstruct.IncapableOfMining = "IncapableOfMining".Translate();
		}

		// Token: 0x06009FDB RID: 40923 RVA: 0x0006A9CA File Offset: 0x00068BCA
		public static Blueprint_Build PlaceBlueprintForBuild(BuildableDef sourceDef, IntVec3 center, Map map, Rot4 rotation, Faction faction, ThingDef stuff)
		{
			Blueprint_Build blueprint_Build = (Blueprint_Build)ThingMaker.MakeThing(sourceDef.blueprintDef, null);
			blueprint_Build.SetFactionDirect(faction);
			blueprint_Build.stuffToUse = stuff;
			GenSpawn.Spawn(blueprint_Build, center, map, rotation, WipeMode.Vanish, false);
			return blueprint_Build;
		}

		// Token: 0x06009FDC RID: 40924 RVA: 0x0006A9F9 File Offset: 0x00068BF9
		public static Blueprint_Install PlaceBlueprintForInstall(MinifiedThing itemToInstall, IntVec3 center, Map map, Rot4 rotation, Faction faction)
		{
			Blueprint_Install blueprint_Install = (Blueprint_Install)ThingMaker.MakeThing(itemToInstall.InnerThing.def.installBlueprintDef, null);
			blueprint_Install.SetThingToInstallFromMinified(itemToInstall);
			blueprint_Install.SetFactionDirect(faction);
			GenSpawn.Spawn(blueprint_Install, center, map, rotation, WipeMode.Vanish, false);
			return blueprint_Install;
		}

		// Token: 0x06009FDD RID: 40925 RVA: 0x0006AA31 File Offset: 0x00068C31
		public static Blueprint_Install PlaceBlueprintForReinstall(Building buildingToReinstall, IntVec3 center, Map map, Rot4 rotation, Faction faction)
		{
			Blueprint_Install blueprint_Install = (Blueprint_Install)ThingMaker.MakeThing(buildingToReinstall.def.installBlueprintDef, null);
			blueprint_Install.SetBuildingToReinstall(buildingToReinstall);
			blueprint_Install.SetFactionDirect(faction);
			GenSpawn.Spawn(blueprint_Install, center, map, rotation, WipeMode.Vanish, false);
			return blueprint_Install;
		}

		// Token: 0x06009FDE RID: 40926 RVA: 0x002EB308 File Offset: 0x002E9508
		public static bool CanBuildOnTerrain(BuildableDef entDef, IntVec3 c, Map map, Rot4 rot, Thing thingToIgnore = null, ThingDef stuffDef = null)
		{
			if (entDef is TerrainDef && !c.GetTerrain(map).changeable)
			{
				return false;
			}
			TerrainAffordanceDef terrainAffordanceNeed = entDef.GetTerrainAffordanceNeed(stuffDef);
			if (terrainAffordanceNeed != null)
			{
				CellRect cellRect = GenAdj.OccupiedRect(c, rot, entDef.Size);
				cellRect.ClipInsideMap(map);
				foreach (IntVec3 c2 in cellRect)
				{
					if (!map.terrainGrid.TerrainAt(c2).affordances.Contains(terrainAffordanceNeed))
					{
						return false;
					}
					List<Thing> thingList = c2.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						if (thingList[i] != thingToIgnore)
						{
							TerrainDef terrainDef = thingList[i].def.entityDefToBuild as TerrainDef;
							if (terrainDef != null && !terrainDef.affordances.Contains(terrainAffordanceNeed))
							{
								return false;
							}
						}
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x06009FDF RID: 40927 RVA: 0x002EB414 File Offset: 0x002E9614
		public static Thing MiniToInstallOrBuildingToReinstall(Blueprint b)
		{
			Blueprint_Install blueprint_Install = b as Blueprint_Install;
			if (blueprint_Install != null)
			{
				return blueprint_Install.MiniToInstallOrBuildingToReinstall;
			}
			return null;
		}

		// Token: 0x06009FE0 RID: 40928 RVA: 0x002EB434 File Offset: 0x002E9634
		public static bool CanConstruct(Thing t, Pawn p, bool checkSkills = true, bool forced = false)
		{
			if (GenConstruct.FirstBlockingThing(t, p) != null)
			{
				return false;
			}
			if (!p.CanReserveAndReach(t, PathEndMode.Touch, forced ? Danger.Deadly : p.NormalMaxDanger(), 1, -1, null, forced))
			{
				return false;
			}
			if (t.IsBurning())
			{
				return false;
			}
			if (checkSkills && p.skills.GetSkill(SkillDefOf.Construction).Level < t.def.constructionSkillPrerequisite)
			{
				JobFailReason.Is(GenConstruct.SkillTooLowTrans.Formatted(SkillDefOf.Construction.LabelCap), null);
				return false;
			}
			if (checkSkills && p.skills.GetSkill(SkillDefOf.Artistic).Level < t.def.artisticSkillPrerequisite)
			{
				JobFailReason.Is(GenConstruct.SkillTooLowTrans.Formatted(SkillDefOf.Artistic.LabelCap), null);
				return false;
			}
			return true;
		}

		// Token: 0x06009FE1 RID: 40929 RVA: 0x002EB510 File Offset: 0x002E9710
		public static int AmountNeededByOf(IConstructible c, ThingDef resDef)
		{
			foreach (ThingDefCountClass thingDefCountClass in c.MaterialsNeeded())
			{
				if (thingDefCountClass.thingDef == resDef)
				{
					return thingDefCountClass.count;
				}
			}
			return 0;
		}

		// Token: 0x06009FE2 RID: 40930 RVA: 0x002EB574 File Offset: 0x002E9774
		public static AcceptanceReport CanPlaceBlueprintAt(BuildableDef entDef, IntVec3 center, Rot4 rot, Map map, bool godMode = false, Thing thingToIgnore = null, Thing thing = null, ThingDef stuffDef = null)
		{
			CellRect cellRect = GenAdj.OccupiedRect(center, rot, entDef.Size);
			if (stuffDef == null && thing != null)
			{
				stuffDef = thing.Stuff;
			}
			foreach (IntVec3 c in cellRect)
			{
				if (!c.InBounds(map))
				{
					return new AcceptanceReport("OutOfBounds".Translate());
				}
				if (c.InNoBuildEdgeArea(map) && !godMode)
				{
					return "TooCloseToMapEdge".Translate();
				}
			}
			if (center.Fogged(map))
			{
				return "CannotPlaceInUndiscovered".Translate();
			}
			List<Thing> thingList = center.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing2 = thingList[i];
				if (thing2 != thingToIgnore && thing2.Position == center && thing2.Rotation == rot)
				{
					if (thing2.def == entDef)
					{
						return new AcceptanceReport("IdenticalThingExists".Translate());
					}
					if (thing2.def.entityDefToBuild == entDef)
					{
						if (thing2 is Blueprint)
						{
							return new AcceptanceReport("IdenticalBlueprintExists".Translate());
						}
						return new AcceptanceReport("IdenticalThingExists".Translate());
					}
				}
			}
			ThingDef thingDef = entDef as ThingDef;
			if (thingDef != null && thingDef.hasInteractionCell)
			{
				IntVec3 c2 = ThingUtility.InteractionCellWhenAt(thingDef, center, rot, map);
				if (!c2.InBounds(map))
				{
					return new AcceptanceReport("InteractionSpotOutOfBounds".Translate());
				}
				List<Thing> list = map.thingGrid.ThingsListAtFast(c2);
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j] != thingToIgnore)
					{
						if (list[j].def.passability == Traversability.Impassable || list[j].def == thingDef)
						{
							return new AcceptanceReport("InteractionSpotBlocked".Translate(list[j].LabelNoCount, list[j]).CapitalizeFirst());
						}
						BuildableDef entityDefToBuild = list[j].def.entityDefToBuild;
						if (entityDefToBuild != null && (entityDefToBuild.passability == Traversability.Impassable || entityDefToBuild == thingDef))
						{
							return new AcceptanceReport("InteractionSpotWillBeBlocked".Translate(list[j].LabelNoCount, list[j]).CapitalizeFirst());
						}
					}
				}
			}
			foreach (IntVec3 c3 in GenAdj.CellsAdjacentCardinal(center, rot, entDef.Size))
			{
				if (c3.InBounds(map))
				{
					thingList = c3.GetThingList(map);
					for (int k = 0; k < thingList.Count; k++)
					{
						Thing thing3 = thingList[k];
						if (thing3 != thingToIgnore)
						{
							Blueprint blueprint = thing3 as Blueprint;
							ThingDef thingDef3;
							if (blueprint != null)
							{
								ThingDef thingDef2 = blueprint.def.entityDefToBuild as ThingDef;
								if (thingDef2 == null)
								{
									goto IL_3AD;
								}
								thingDef3 = thingDef2;
							}
							else
							{
								thingDef3 = thing3.def;
							}
							if (thingDef3.hasInteractionCell && (entDef.passability == Traversability.Impassable || entDef == thingDef3) && cellRect.Contains(ThingUtility.InteractionCellWhenAt(thingDef3, thing3.Position, thing3.Rotation, thing3.Map)))
							{
								return new AcceptanceReport("WouldBlockInteractionSpot".Translate(entDef.label, thingDef3.label).CapitalizeFirst());
							}
						}
						IL_3AD:;
					}
				}
			}
			TerrainDef terrainDef = entDef as TerrainDef;
			if (terrainDef != null)
			{
				if (map.terrainGrid.TerrainAt(center) == terrainDef)
				{
					return new AcceptanceReport("TerrainIsAlready".Translate(terrainDef.label));
				}
				if (map.designationManager.DesignationAt(center, DesignationDefOf.SmoothFloor) != null)
				{
					return new AcceptanceReport("SpaceBeingSmoothed".Translate());
				}
			}
			if (GenConstruct.CanBuildOnTerrain(entDef, center, map, rot, thingToIgnore, stuffDef))
			{
				if (ModsConfig.RoyaltyActive)
				{
					List<Thing> list2 = map.listerThings.ThingsOfDef(ThingDefOf.MonumentMarker);
					for (int l = 0; l < list2.Count; l++)
					{
						MonumentMarker monumentMarker = (MonumentMarker)list2[l];
						if (!monumentMarker.complete && !monumentMarker.AllowsPlacingBlueprint(entDef, center, rot, stuffDef))
						{
							return new AcceptanceReport("BlueprintWouldCollideWithMonument".Translate());
						}
					}
				}
				if (!godMode)
				{
					foreach (IntVec3 c4 in cellRect)
					{
						thingList = c4.GetThingList(map);
						for (int m = 0; m < thingList.Count; m++)
						{
							Thing thing4 = thingList[m];
							if (thing4 != thingToIgnore && !GenConstruct.CanPlaceBlueprintOver(entDef, thing4.def))
							{
								return new AcceptanceReport("SpaceAlreadyOccupied".Translate());
							}
						}
					}
				}
				if (entDef.PlaceWorkers != null)
				{
					for (int n = 0; n < entDef.PlaceWorkers.Count; n++)
					{
						AcceptanceReport result = entDef.PlaceWorkers[n].AllowsPlacing(entDef, center, rot, map, thingToIgnore, thing);
						if (!result.Accepted)
						{
							return result;
						}
					}
				}
				return AcceptanceReport.WasAccepted;
			}
			if (entDef.GetTerrainAffordanceNeed(stuffDef) == null)
			{
				return new AcceptanceReport("TerrainCannotSupport".Translate(entDef).CapitalizeFirst());
			}
			if (entDef.useStuffTerrainAffordance && stuffDef != null)
			{
				return new AcceptanceReport("TerrainCannotSupport_TerrainAffordanceFromStuff".Translate(entDef, entDef.GetTerrainAffordanceNeed(stuffDef), stuffDef).CapitalizeFirst());
			}
			return new AcceptanceReport("TerrainCannotSupport_TerrainAffordance".Translate(entDef, entDef.GetTerrainAffordanceNeed(stuffDef)).CapitalizeFirst());
		}

		// Token: 0x06009FE3 RID: 40931 RVA: 0x0006AA64 File Offset: 0x00068C64
		public static BuildableDef BuiltDefOf(ThingDef def)
		{
			if (def.entityDefToBuild == null)
			{
				return def;
			}
			return def.entityDefToBuild;
		}

		// Token: 0x06009FE4 RID: 40932 RVA: 0x002EBBFC File Offset: 0x002E9DFC
		public static bool CanPlaceBlueprintOver(BuildableDef newDef, ThingDef oldDef)
		{
			if (oldDef.EverHaulable)
			{
				return true;
			}
			TerrainDef terrainDef = newDef as TerrainDef;
			if (terrainDef != null)
			{
				if (oldDef.IsBlueprint || oldDef.IsFrame)
				{
					if (!terrainDef.affordances.Contains(oldDef.entityDefToBuild.terrainAffordanceNeeded))
					{
						return false;
					}
				}
				else if (oldDef.category == ThingCategory.Building && !terrainDef.affordances.Contains(oldDef.terrainAffordanceNeeded))
				{
					return false;
				}
			}
			ThingDef thingDef = newDef as ThingDef;
			BuildableDef buildableDef = GenConstruct.BuiltDefOf(oldDef);
			ThingDef thingDef2 = buildableDef as ThingDef;
			if (newDef.ForceAllowPlaceOver(oldDef))
			{
				return true;
			}
			if (oldDef.category == ThingCategory.Plant && oldDef.passability == Traversability.Impassable && thingDef != null && thingDef.category == ThingCategory.Building && !thingDef.building.canPlaceOverImpassablePlant)
			{
				return false;
			}
			if (oldDef.category == ThingCategory.Building || oldDef.IsBlueprint || oldDef.IsFrame)
			{
				if (thingDef != null)
				{
					if (!thingDef.IsEdifice())
					{
						return (oldDef.building == null || oldDef.building.canBuildNonEdificesUnder) && (!thingDef.EverTransmitsPower || !oldDef.EverTransmitsPower);
					}
					if (thingDef.IsEdifice() && oldDef != null && oldDef.category == ThingCategory.Building && !oldDef.IsEdifice())
					{
						return thingDef.building == null || thingDef.building.canBuildNonEdificesUnder;
					}
					if (thingDef2 != null && (thingDef2 == ThingDefOf.Wall || thingDef2.IsSmoothed) && thingDef.building != null && thingDef.building.canPlaceOverWall)
					{
						return true;
					}
					if (newDef != ThingDefOf.PowerConduit && buildableDef == ThingDefOf.PowerConduit)
					{
						return true;
					}
				}
				return (newDef is TerrainDef && buildableDef is ThingDef && ((ThingDef)buildableDef).CoexistsWithFloors) || (buildableDef is TerrainDef && !(newDef is TerrainDef));
			}
			return true;
		}

		// Token: 0x06009FE5 RID: 40933 RVA: 0x002EBDB4 File Offset: 0x002E9FB4
		public static Thing FirstBlockingThing(Thing constructible, Pawn pawnToIgnore)
		{
			Blueprint blueprint = constructible as Blueprint;
			Thing thing;
			if (blueprint != null)
			{
				thing = GenConstruct.MiniToInstallOrBuildingToReinstall(blueprint);
			}
			else
			{
				thing = null;
			}
			foreach (IntVec3 c in constructible.OccupiedRect())
			{
				List<Thing> thingList = c.GetThingList(constructible.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Thing thing2 = thingList[i];
					if (GenConstruct.BlocksConstruction(constructible, thing2) && thing2 != pawnToIgnore && thing2 != thing)
					{
						return thing2;
					}
				}
			}
			return null;
		}

		// Token: 0x06009FE6 RID: 40934 RVA: 0x002EBE64 File Offset: 0x002EA064
		public static Job HandleBlockingThingJob(Thing constructible, Pawn worker, bool forced = false)
		{
			Thing thing = GenConstruct.FirstBlockingThing(constructible, worker);
			if (thing == null)
			{
				return null;
			}
			if (thing.def.category == ThingCategory.Plant)
			{
				if (worker.CanReserveAndReach(thing, PathEndMode.ClosestTouch, worker.NormalMaxDanger(), 1, -1, null, forced))
				{
					return JobMaker.MakeJob(JobDefOf.CutPlant, thing);
				}
			}
			else if (thing.def.category == ThingCategory.Item)
			{
				if (thing.def.EverHaulable)
				{
					return HaulAIUtility.HaulAsideJobFor(worker, thing);
				}
				Log.ErrorOnce(string.Concat(new object[]
				{
					"Never haulable ",
					thing,
					" blocking ",
					constructible.ToStringSafe<Thing>(),
					" at ",
					constructible.Position
				}), 6429262, false);
			}
			else if (thing.def.category == ThingCategory.Building)
			{
				if (((Building)thing).DeconstructibleBy(worker.Faction))
				{
					if (worker.WorkTypeIsDisabled(WorkTypeDefOf.Construction))
					{
						JobFailReason.Is(GenConstruct.IncapableOfDeconstruction, null);
						return null;
					}
					if (worker.CanReserveAndReach(thing, PathEndMode.Touch, worker.NormalMaxDanger(), 1, -1, null, forced))
					{
						Job job = JobMaker.MakeJob(JobDefOf.Deconstruct, thing);
						job.ignoreDesignations = true;
						return job;
					}
				}
				if (thing.def.mineable)
				{
					if (worker.WorkTypeIsDisabled(WorkTypeDefOf.Mining))
					{
						JobFailReason.Is(GenConstruct.IncapableOfMining, null);
						return null;
					}
					if (worker.CanReserveAndReach(thing, PathEndMode.Touch, worker.NormalMaxDanger(), 1, -1, null, forced))
					{
						Job job2 = JobMaker.MakeJob(JobDefOf.Mine, thing);
						job2.ignoreDesignations = true;
						return job2;
					}
				}
			}
			return null;
		}

		// Token: 0x06009FE7 RID: 40935 RVA: 0x002EBFF4 File Offset: 0x002EA1F4
		public static bool BlocksConstruction(Thing constructible, Thing t)
		{
			if (t == constructible)
			{
				return false;
			}
			ThingDef thingDef;
			if (constructible is Blueprint)
			{
				thingDef = constructible.def;
			}
			else if (constructible is Frame)
			{
				thingDef = constructible.def.entityDefToBuild.blueprintDef;
			}
			else
			{
				thingDef = constructible.def.blueprintDef;
			}
			if (t.def.category == ThingCategory.Building && GenSpawn.SpawningWipes(thingDef.entityDefToBuild, t.def))
			{
				return true;
			}
			if (t.def.category == ThingCategory.Plant)
			{
				return t.def.plant.harvestWork > ThingDefOf.Plant_Dandelion.plant.harvestWork && (!(thingDef.entityDefToBuild is TerrainDef) || !t.Spawned || !(t.Position.GetEdifice(t.Map) is IPlantToGrowSettable));
			}
			if (!thingDef.clearBuildingArea)
			{
				return false;
			}
			if (thingDef.entityDefToBuild.ForceAllowPlaceOver(t.def))
			{
				return false;
			}
			ThingDef thingDef2 = thingDef.entityDefToBuild as ThingDef;
			if (thingDef2 != null)
			{
				if (thingDef2.EverTransmitsPower && t.def == ThingDefOf.PowerConduit && thingDef2 != ThingDefOf.PowerConduit)
				{
					return false;
				}
				if (t.def == ThingDefOf.Wall && thingDef2.building != null && thingDef2.building.canPlaceOverWall)
				{
					return false;
				}
			}
			return (t.def.IsEdifice() && thingDef2.IsEdifice()) || (t.def.category == ThingCategory.Pawn || (t.def.category == ThingCategory.Item && thingDef.entityDefToBuild.passability == Traversability.Impassable)) || (t.def.Fillage >= FillCategory.Partial && (t.def.IsEdifice() || (t.def.entityDefToBuild != null && t.def.entityDefToBuild.IsEdifice())));
		}

		// Token: 0x06009FE8 RID: 40936 RVA: 0x002EC1BC File Offset: 0x002EA3BC
		public static bool TerrainCanSupport(CellRect rect, Map map, ThingDef thing)
		{
			using (CellRect.Enumerator enumerator = rect.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.SupportsStructureType(map, thing.terrainAffordanceNeeded))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x04006C9F RID: 27807
		public const float ConstructionSpeedGlobalFactor = 1.7f;

		// Token: 0x04006CA0 RID: 27808
		private static string SkillTooLowTrans;

		// Token: 0x04006CA1 RID: 27809
		private static string IncapableOfDeconstruction;

		// Token: 0x04006CA2 RID: 27810
		private static string IncapableOfMining;
	}
}
