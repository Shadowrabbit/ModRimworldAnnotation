using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001484 RID: 5252
	public static class GenConstruct
	{
		// Token: 0x06007D92 RID: 32146 RVA: 0x002C5C39 File Offset: 0x002C3E39
		public static void Reset()
		{
			GenConstruct.SkillTooLowTrans = "SkillTooLowForConstruction".Translate();
			GenConstruct.IncapableOfDeconstruction = "IncapableOfDeconstruction".Translate();
			GenConstruct.IncapableOfMining = "IncapableOfMining".Translate();
		}

		// Token: 0x06007D93 RID: 32147 RVA: 0x002C5C78 File Offset: 0x002C3E78
		public static Blueprint_Build PlaceBlueprintForBuild(BuildableDef sourceDef, IntVec3 center, Map map, Rot4 rotation, Faction faction, ThingDef stuff)
		{
			Blueprint_Build blueprint_Build = (Blueprint_Build)ThingMaker.MakeThing(sourceDef.blueprintDef, null);
			blueprint_Build.SetFactionDirect(faction);
			blueprint_Build.stuffToUse = stuff;
			GenSpawn.Spawn(blueprint_Build, center, map, rotation, WipeMode.Vanish, false);
			if (faction != null)
			{
				QuestUtility.SendQuestTargetSignals(faction.questTags, "PlacedBlueprint", blueprint_Build.Named("SUBJECT"));
			}
			return blueprint_Build;
		}

		// Token: 0x06007D94 RID: 32148 RVA: 0x002C5CD4 File Offset: 0x002C3ED4
		public static Blueprint_Install PlaceBlueprintForInstall(MinifiedThing itemToInstall, IntVec3 center, Map map, Rot4 rotation, Faction faction)
		{
			Blueprint_Install blueprint_Install = (Blueprint_Install)ThingMaker.MakeThing(itemToInstall.InnerThing.def.installBlueprintDef, null);
			blueprint_Install.SetThingToInstallFromMinified(itemToInstall);
			blueprint_Install.SetFactionDirect(faction);
			GenSpawn.Spawn(blueprint_Install, center, map, rotation, WipeMode.Vanish, false);
			if (faction != null)
			{
				QuestUtility.SendQuestTargetSignals(faction.questTags, "PlacedBlueprint", blueprint_Install.Named("SUBJECT"));
			}
			return blueprint_Install;
		}

		// Token: 0x06007D95 RID: 32149 RVA: 0x002C5D3C File Offset: 0x002C3F3C
		public static Blueprint_Install PlaceBlueprintForReinstall(Building buildingToReinstall, IntVec3 center, Map map, Rot4 rotation, Faction faction)
		{
			Blueprint_Install blueprint_Install = (Blueprint_Install)ThingMaker.MakeThing(buildingToReinstall.def.installBlueprintDef, null);
			blueprint_Install.SetBuildingToReinstall(buildingToReinstall);
			blueprint_Install.SetFactionDirect(faction);
			GenSpawn.Spawn(blueprint_Install, center, map, rotation, WipeMode.Vanish, false);
			if (faction != null)
			{
				QuestUtility.SendQuestTargetSignals(faction.questTags, "PlacedBlueprint", blueprint_Install.Named("SUBJECT"));
			}
			return blueprint_Install;
		}

		// Token: 0x06007D96 RID: 32150 RVA: 0x002C5D9C File Offset: 0x002C3F9C
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

		// Token: 0x06007D97 RID: 32151 RVA: 0x002C5EA8 File Offset: 0x002C40A8
		public static Thing MiniToInstallOrBuildingToReinstall(Blueprint b)
		{
			Blueprint_Install blueprint_Install = b as Blueprint_Install;
			if (blueprint_Install != null)
			{
				return blueprint_Install.MiniToInstallOrBuildingToReinstall;
			}
			return null;
		}

		// Token: 0x06007D98 RID: 32152 RVA: 0x002C5EC8 File Offset: 0x002C40C8
		public static bool CanConstruct(Thing t, Pawn pawn, WorkTypeDef workType, bool forced = false)
		{
			if (!pawn.workSettings.WorkIsActive(workType))
			{
				JobFailReason.Is("NotAssignedToWorkType".Translate(workType.gerundLabel).CapitalizeFirst(), null);
				return false;
			}
			return GenConstruct.CanConstruct(t, pawn, workType == WorkTypeDefOf.Construction, forced);
		}

		// Token: 0x06007D99 RID: 32153 RVA: 0x002C5F20 File Offset: 0x002C4120
		public static bool CanConstruct(Thing t, Pawn p, bool checkSkills = true, bool forced = false)
		{
			GenConstruct.tmpIdeoMemberNames.Clear();
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
			if (p.Ideo != null && !p.Ideo.MembersCanBuild(t))
			{
				foreach (Ideo ideo in Find.IdeoManager.IdeosListForReading)
				{
					if (ideo.MembersCanBuild(t))
					{
						GenConstruct.tmpIdeoMemberNames.Add(ideo.memberName);
					}
				}
				if (GenConstruct.tmpIdeoMemberNames.Any<string>())
				{
					JobFailReason.Is("OnlyMembersCanBuild".Translate(GenConstruct.tmpIdeoMemberNames.ToCommaList(true, false)), null);
				}
				return false;
			}
			return t.def.building == null || !t.def.building.IsTurret || t.TryGetComp<CompMannable>() != null || new HistoryEvent(HistoryEventDefOf.BuiltAutomatedTurret, p.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job();
		}

		// Token: 0x06007D9A RID: 32154 RVA: 0x002C60F8 File Offset: 0x002C42F8
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

		// Token: 0x06007D9B RID: 32155 RVA: 0x002C615C File Offset: 0x002C435C
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
						if (list[j].def.passability != Traversability.Standable || list[j].def == thingDef)
						{
							return new AcceptanceReport("InteractionSpotBlocked".Translate(list[j].LabelNoCount, list[j]).CapitalizeFirst());
						}
						BuildableDef entityDefToBuild = list[j].def.entityDefToBuild;
						if (entityDefToBuild != null && (entityDefToBuild.passability != Traversability.Standable || entityDefToBuild == thingDef))
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
									goto IL_3AA;
								}
								thingDef3 = thingDef2;
							}
							else
							{
								thingDef3 = thing3.def;
							}
							if (thingDef3.hasInteractionCell && (entDef.passability != Traversability.Standable || entDef == thingDef3) && cellRect.Contains(ThingUtility.InteractionCellWhenAt(thingDef3, thing3.Position, thing3.Rotation, thing3.Map)))
							{
								return new AcceptanceReport("WouldBlockInteractionSpot".Translate(entDef.label, thingDef3.label).CapitalizeFirst());
							}
						}
						IL_3AA:;
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

		// Token: 0x06007D9C RID: 32156 RVA: 0x002C67E4 File Offset: 0x002C49E4
		public static BuildableDef BuiltDefOf(ThingDef def)
		{
			if (def.entityDefToBuild == null)
			{
				return def;
			}
			return def.entityDefToBuild;
		}

		// Token: 0x06007D9D RID: 32157 RVA: 0x002C67F8 File Offset: 0x002C49F8
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
					if (oldDef.entityDefToBuild.terrainAffordanceNeeded != null && !terrainDef.affordances.Contains(oldDef.entityDefToBuild.terrainAffordanceNeeded))
					{
						return false;
					}
				}
				else if (oldDef.category == ThingCategory.Building && oldDef.terrainAffordanceNeeded != null && !terrainDef.affordances.Contains(oldDef.terrainAffordanceNeeded))
				{
					return false;
				}
			}
			GenConstruct.<>c__DisplayClass16_0 CS$<>8__locals1;
			CS$<>8__locals1.newThingDef = (newDef as ThingDef);
			CS$<>8__locals1.oldThingDef = oldDef;
			CS$<>8__locals1.oldDefBuilt = GenConstruct.BuiltDefOf(oldDef);
			ThingDef thingDef = CS$<>8__locals1.oldDefBuilt as ThingDef;
			if (newDef.blocksAltitudes != null && newDef.blocksAltitudes.Contains(oldDef.altitudeLayer))
			{
				return false;
			}
			BuildableDef oldDefBuilt = CS$<>8__locals1.oldDefBuilt;
			if (((oldDefBuilt != null) ? oldDefBuilt.blocksAltitudes : null) != null && CS$<>8__locals1.oldDefBuilt.blocksAltitudes.Contains(newDef.altitudeLayer))
			{
				return false;
			}
			if (newDef.ForceAllowPlaceOver(oldDef))
			{
				return true;
			}
			if (oldDef.category == ThingCategory.Plant && oldDef.passability == Traversability.Impassable && CS$<>8__locals1.newThingDef != null && CS$<>8__locals1.newThingDef.category == ThingCategory.Building && !CS$<>8__locals1.newThingDef.building.canPlaceOverImpassablePlant)
			{
				return false;
			}
			if (oldDef.category == ThingCategory.Building || oldDef.IsBlueprint || oldDef.IsFrame)
			{
				if (CS$<>8__locals1.newThingDef != null)
				{
					if (!CS$<>8__locals1.newThingDef.IsEdifice())
					{
						return (oldDef.building == null || oldDef.building.canBuildNonEdificesUnder) && (!CS$<>8__locals1.newThingDef.EverTransmitsPower || !oldDef.EverTransmitsPower);
					}
					if (GenConstruct.<CanPlaceBlueprintOver>g__IsEdificeOverNonEdifice|16_0(ref CS$<>8__locals1))
					{
						return CS$<>8__locals1.newThingDef.building == null || CS$<>8__locals1.newThingDef.building.canBuildNonEdificesUnder;
					}
					if (((thingDef != null) ? thingDef.building : null) != null && (thingDef.building.isPlaceOverableWall || thingDef.IsSmoothed) && CS$<>8__locals1.newThingDef.building != null && CS$<>8__locals1.newThingDef.building.canPlaceOverWall)
					{
						return true;
					}
					if (newDef != ThingDefOf.PowerConduit && CS$<>8__locals1.oldDefBuilt == ThingDefOf.PowerConduit)
					{
						return true;
					}
				}
				return (newDef is TerrainDef && CS$<>8__locals1.oldDefBuilt is ThingDef && ((ThingDef)CS$<>8__locals1.oldDefBuilt).CoexistsWithFloors) || (CS$<>8__locals1.oldDefBuilt is TerrainDef && !(newDef is TerrainDef));
			}
			return true;
		}

		// Token: 0x06007D9E RID: 32158 RVA: 0x002C6A64 File Offset: 0x002C4C64
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

		// Token: 0x06007D9F RID: 32159 RVA: 0x002C6B14 File Offset: 0x002C4D14
		public static Job HandleBlockingThingJob(Thing constructible, Pawn worker, bool forced = false)
		{
			Thing thing = GenConstruct.FirstBlockingThing(constructible, worker);
			if (thing == null)
			{
				return null;
			}
			if (thing.def.category == ThingCategory.Plant)
			{
				if (!PlantUtility.PawnWillingToCutPlant_Job(thing, worker))
				{
					return null;
				}
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
				}), 6429262);
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

		// Token: 0x06007DA0 RID: 32160 RVA: 0x002C6CAC File Offset: 0x002C4EAC
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
				if ((t.def.category == ThingCategory.Pawn || t.def.category == ThingCategory.Item) && thingDef2.passability != Traversability.Standable && thingDef2.surfaceType == SurfaceType.None)
				{
					return true;
				}
			}
			if (t.def.IsEdifice() && thingDef2.IsEdifice())
			{
				return true;
			}
			if (t.def.category == ThingCategory.Pawn || (t.def.category == ThingCategory.Item && thingDef.entityDefToBuild.passability == Traversability.Impassable))
			{
				return true;
			}
			if (t.def.Fillage < FillCategory.Partial || (!t.def.IsEdifice() && (t.def.entityDefToBuild == null || !t.def.entityDefToBuild.IsEdifice())))
			{
				return false;
			}
			if (thingDef2.blocksAltitudes == null)
			{
				return true;
			}
			List<AltitudeLayer> list;
			if ((list = t.def.blocksAltitudes) == null)
			{
				BuildableDef entityDefToBuild = t.def.entityDefToBuild;
				list = ((entityDefToBuild != null) ? entityDefToBuild.blocksAltitudes : null);
			}
			List<AltitudeLayer> list2 = list;
			return list2 != null && list2.SharesElementWith(thingDef2.blocksAltitudes);
		}

		// Token: 0x06007DA1 RID: 32161 RVA: 0x002C6EE0 File Offset: 0x002C50E0
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

		// Token: 0x06007DA3 RID: 32163 RVA: 0x002C6F48 File Offset: 0x002C5148
		[CompilerGenerated]
		internal static bool <CanPlaceBlueprintOver>g__IsEdificeOverNonEdifice|16_0(ref GenConstruct.<>c__DisplayClass16_0 A_0)
		{
			ThingDef thingDef;
			return A_0.newThingDef.IsEdifice() && ((A_0.oldThingDef != null && A_0.oldThingDef.category == ThingCategory.Building && !A_0.oldThingDef.IsEdifice()) || (A_0.oldDefBuilt != null && (thingDef = (A_0.oldDefBuilt as ThingDef)) != null && thingDef.category == ThingCategory.Building && !thingDef.IsEdifice()));
		}

		// Token: 0x04004E57 RID: 20055
		public const float ConstructionSpeedGlobalFactor = 1.7f;

		// Token: 0x04004E58 RID: 20056
		private static string SkillTooLowTrans;

		// Token: 0x04004E59 RID: 20057
		private static string IncapableOfDeconstruction;

		// Token: 0x04004E5A RID: 20058
		private static string IncapableOfMining;

		// Token: 0x04004E5B RID: 20059
		private static List<string> tmpIdeoMemberNames = new List<string>();
	}
}
