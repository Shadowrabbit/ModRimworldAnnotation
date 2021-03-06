using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000517 RID: 1303
	public static class ThingUtility
	{
		// Token: 0x06002120 RID: 8480 RVA: 0x0001CE93 File Offset: 0x0001B093
		public static bool DestroyedOrNull(this Thing t)
		{
			return t == null || t.Destroyed;
		}

		// Token: 0x06002121 RID: 8481 RVA: 0x00105888 File Offset: 0x00103A88
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

		// Token: 0x06002122 RID: 8482 RVA: 0x001058C0 File Offset: 0x00103AC0
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

		// Token: 0x06002123 RID: 8483 RVA: 0x0001CEA0 File Offset: 0x0001B0A0
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

		// Token: 0x06002124 RID: 8484 RVA: 0x001058F8 File Offset: 0x00103AF8
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

		// Token: 0x06002125 RID: 8485 RVA: 0x0001CEC1 File Offset: 0x0001B0C1
		public static DamageDef PrimaryMeleeWeaponDamageType(ThingDef thing)
		{
			return ThingUtility.PrimaryMeleeWeaponDamageType(thing.tools);
		}

		// Token: 0x06002126 RID: 8486 RVA: 0x00105B0C File Offset: 0x00103D0C
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

		// Token: 0x06002127 RID: 8487 RVA: 0x00105B64 File Offset: 0x00103D64
		public static void CheckAutoRebuildOnDestroyed(Thing thing, DestroyMode mode, Map map, BuildableDef buildingDef)
		{
			if (Find.PlaySettings.autoRebuild && mode == DestroyMode.KillFinalize && thing.Faction == Faction.OfPlayer && buildingDef.blueprintDef != null && buildingDef.IsResearchFinished && map.areaManager.Home[thing.Position] && GenConstruct.CanPlaceBlueprintAt(buildingDef, thing.Position, thing.Rotation, map, false, null, null, thing.Stuff).Accepted)
			{
				GenConstruct.PlaceBlueprintForBuild(buildingDef, thing.Position, map, thing.Rotation, Faction.OfPlayer, thing.Stuff);
			}
		}

		// Token: 0x06002128 RID: 8488 RVA: 0x00105BFC File Offset: 0x00103DFC
		public static void CheckAutoRebuildTerrainOnDestroyed(TerrainDef terrainDef, IntVec3 pos, Map map)
		{
			if (Find.PlaySettings.autoRebuild && terrainDef.autoRebuildable && terrainDef.blueprintDef != null && terrainDef.IsResearchFinished && map.areaManager.Home[pos] && GenConstruct.CanPlaceBlueprintAt(terrainDef, pos, Rot4.South, map, false, null, null, null).Accepted)
			{
				GenConstruct.PlaceBlueprintForBuild(terrainDef, pos, map, Rot4.South, Faction.OfPlayer, null);
			}
		}

		// Token: 0x06002129 RID: 8489 RVA: 0x00105C70 File Offset: 0x00103E70
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

		// Token: 0x0600212A RID: 8490 RVA: 0x00105CB8 File Offset: 0x00103EB8
		public static TerrainAffordanceDef GetTerrainAffordanceNeed(this BuildableDef def, ThingDef stuffDef = null)
		{
			TerrainAffordanceDef terrainAffordanceNeeded = def.terrainAffordanceNeeded;
			if (stuffDef != null && def.useStuffTerrainAffordance && stuffDef.terrainAffordanceNeeded != null)
			{
				terrainAffordanceNeeded = stuffDef.terrainAffordanceNeeded;
			}
			return terrainAffordanceNeeded;
		}
	}
}
