using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x0200037B RID: 891
	public static class ThingUtility
	{
		// Token: 0x060019E7 RID: 6631 RVA: 0x000979DF File Offset: 0x00095BDF
		public static bool DestroyedOrNull(this Thing t)
		{
			return t == null || t.Destroyed;
		}

		// Token: 0x060019E8 RID: 6632 RVA: 0x000979EC File Offset: 0x00095BEC
		public static void DestroyOrPassToWorld(this Thing t, DestroyMode mode = DestroyMode.Vanish)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				if (!Find.WorldPawns.Contains(pawn))
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					return;
				}
			}
			else
			{
				t.Destroy(mode);
			}
		}

		// Token: 0x060019E9 RID: 6633 RVA: 0x00097A24 File Offset: 0x00095C24
		public static int TryAbsorbStackNumToTake(Thing thing, Thing other, bool respectStackLimit)
		{
			int result;
			if (respectStackLimit)
			{
				result = Mathf.Min(other.stackCount, thing.def.stackLimit - thing.stackCount);
			}
			else
			{
				result = other.stackCount;
			}
			return result;
		}

		// Token: 0x060019EA RID: 6634 RVA: 0x00097A5C File Offset: 0x00095C5C
		public static int RoundedResourceStackCount(int stackCount)
		{
			if (stackCount > 200)
			{
				return GenMath.RoundTo(stackCount, 10);
			}
			if (stackCount > 100)
			{
				return GenMath.RoundTo(stackCount, 5);
			}
			return stackCount;
		}

		// Token: 0x060019EB RID: 6635 RVA: 0x00097A80 File Offset: 0x00095C80
		public static IntVec3 InteractionCellWhenAt(ThingDef def, IntVec3 center, Rot4 rot, Map map)
		{
			if (def.hasInteractionCell)
			{
				IntVec3 b = def.interactionCellOffset.RotatedBy(rot);
				return center + b;
			}
			if (def.Size.x == 1 && def.Size.z == 1)
			{
				for (int i = 0; i < 8; i++)
				{
					IntVec3 intVec = center + GenAdj.AdjacentCells[i];
					if (intVec.Standable(map) && intVec.GetDoor(map) == null && ReachabilityImmediate.CanReachImmediate(intVec, center, map, PathEndMode.Touch, null))
					{
						return intVec;
					}
				}
				for (int j = 0; j < 8; j++)
				{
					IntVec3 intVec2 = center + GenAdj.AdjacentCells[j];
					if (intVec2.Standable(map) && ReachabilityImmediate.CanReachImmediate(intVec2, center, map, PathEndMode.Touch, null))
					{
						return intVec2;
					}
				}
				for (int k = 0; k < 8; k++)
				{
					IntVec3 intVec3 = center + GenAdj.AdjacentCells[k];
					if (intVec3.Walkable(map) && ReachabilityImmediate.CanReachImmediate(intVec3, center, map, PathEndMode.Touch, null))
					{
						return intVec3;
					}
				}
				return center;
			}
			List<IntVec3> list = GenAdjFast.AdjacentCells8Way(center, rot, def.size);
			CellRect rect = GenAdj.OccupiedRect(center, rot, def.size);
			for (int l = 0; l < list.Count; l++)
			{
				if (list[l].Standable(map) && list[l].GetDoor(map) == null && ReachabilityImmediate.CanReachImmediate(list[l], rect, map, PathEndMode.Touch, null))
				{
					return list[l];
				}
			}
			for (int m = 0; m < list.Count; m++)
			{
				if (list[m].Standable(map) && ReachabilityImmediate.CanReachImmediate(list[m], rect, map, PathEndMode.Touch, null))
				{
					return list[m];
				}
			}
			for (int n = 0; n < list.Count; n++)
			{
				if (list[n].Walkable(map) && ReachabilityImmediate.CanReachImmediate(list[n], rect, map, PathEndMode.Touch, null))
				{
					return list[n];
				}
			}
			return center;
		}

		// Token: 0x060019EC RID: 6636 RVA: 0x00097C94 File Offset: 0x00095E94
		public static DamageDef PrimaryMeleeWeaponDamageType(ThingDef thing)
		{
			return ThingUtility.PrimaryMeleeWeaponDamageType(thing.tools);
		}

		// Token: 0x060019ED RID: 6637 RVA: 0x00097CA4 File Offset: 0x00095EA4
		public static DamageDef PrimaryMeleeWeaponDamageType(List<Tool> tools)
		{
			if (tools.NullOrEmpty<Tool>())
			{
				return null;
			}
			ManeuverDef maneuverDef = tools.MaxBy((Tool tool) => tool.power).Maneuvers.FirstOrDefault<ManeuverDef>();
			if (maneuverDef == null)
			{
				return null;
			}
			return maneuverDef.verb.meleeDamageDef;
		}

		// Token: 0x060019EE RID: 6638 RVA: 0x00097CFC File Offset: 0x00095EFC
		public static void CheckAutoRebuildOnDestroyed(Thing thing, DestroyMode mode, Map map, BuildableDef buildingDef)
		{
			if (Find.PlaySettings.autoRebuild && mode == DestroyMode.KillFinalize && thing.Faction == Faction.OfPlayer && buildingDef.blueprintDef != null && buildingDef.IsResearchFinished && map.areaManager.Home[thing.Position] && GenConstruct.CanPlaceBlueprintAt(buildingDef, thing.Position, thing.Rotation, map, false, null, null, thing.Stuff).Accepted)
			{
				GenConstruct.PlaceBlueprintForBuild(buildingDef, thing.Position, map, thing.Rotation, Faction.OfPlayer, thing.Stuff);
			}
		}

		// Token: 0x060019EF RID: 6639 RVA: 0x00097D94 File Offset: 0x00095F94
		public static void CheckAutoRebuildTerrainOnDestroyed(TerrainDef terrainDef, IntVec3 pos, Map map)
		{
			if (Find.PlaySettings.autoRebuild && terrainDef.autoRebuildable && terrainDef.blueprintDef != null && terrainDef.IsResearchFinished && map.areaManager.Home[pos] && GenConstruct.CanPlaceBlueprintAt(terrainDef, pos, Rot4.South, map, false, null, null, null).Accepted)
			{
				GenConstruct.PlaceBlueprintForBuild(terrainDef, pos, map, Rot4.South, Faction.OfPlayer, null);
			}
		}

		// Token: 0x060019F0 RID: 6640 RVA: 0x00097E08 File Offset: 0x00096008
		public static Pawn FindPawn(List<Thing> things)
		{
			for (int i = 0; i < things.Count; i++)
			{
				Pawn pawn = things[i] as Pawn;
				if (pawn != null)
				{
					return pawn;
				}
				Corpse corpse = things[i] as Corpse;
				if (corpse != null)
				{
					return corpse.InnerPawn;
				}
			}
			return null;
		}

		// Token: 0x060019F1 RID: 6641 RVA: 0x00097E50 File Offset: 0x00096050
		public static TerrainAffordanceDef GetTerrainAffordanceNeed(this BuildableDef def, ThingDef stuffDef = null)
		{
			TerrainAffordanceDef terrainAffordanceNeeded = def.terrainAffordanceNeeded;
			if (stuffDef != null && def.useStuffTerrainAffordance && stuffDef.terrainAffordanceNeeded != null)
			{
				terrainAffordanceNeeded = stuffDef.terrainAffordanceNeeded;
			}
			return terrainAffordanceNeeded;
		}

		// Token: 0x060019F2 RID: 6642 RVA: 0x00097E7F File Offset: 0x0009607F
		public static bool HasThingCategory(this Thing thing, ThingCategoryDef thingCategory)
		{
			return !thing.def.thingCategories.NullOrEmpty<ThingCategoryDef>() && thing.def.thingCategories.Contains(thingCategory);
		}
	}
}
