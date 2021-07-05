﻿using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000656 RID: 1622
	public static class GenAI
	{
		// Token: 0x06002DD6 RID: 11734 RVA: 0x00112BC8 File Offset: 0x00110DC8
		public static bool MachinesLike(Faction machineFaction, Pawn p)
		{
			return (p.Faction != null || !p.NonHumanlikeOrWildMan() || (p.HostFaction == machineFaction && !p.IsPrisoner)) && (!p.IsPrisoner || p.HostFaction != machineFaction) && (p.Faction == null || !p.Faction.HostileTo(machineFaction));
		}

		// Token: 0x06002DD7 RID: 11735 RVA: 0x00112C24 File Offset: 0x00110E24
		public static bool CanUseItemForWork(Pawn p, Thing item)
		{
			return !item.IsForbidden(p) && p.CanReserveAndReach(item, PathEndMode.ClosestTouch, p.NormalMaxDanger(), 1, -1, null, false);
		}

		// Token: 0x06002DD8 RID: 11736 RVA: 0x00112C50 File Offset: 0x00110E50
		public static bool CanBeArrestedBy(this Pawn pawn, Pawn arrester)
		{
			return pawn.RaceProps.Humanlike && (!pawn.InAggroMentalState || !pawn.HostileTo(arrester)) && !pawn.HostileTo(Faction.OfPlayer) && (!pawn.IsPrisonerOfColony || !pawn.Position.IsInPrisonCell(pawn.Map));
		}

		// Token: 0x06002DD9 RID: 11737 RVA: 0x00112CAC File Offset: 0x00110EAC
		public static bool InDangerousCombat(Pawn pawn)
		{
			Region root = pawn.GetRegion(RegionType.Set_Passable);
			bool found = false;
			Predicate<Thing> <>9__2;
			RegionTraverser.BreadthFirstTraverse(root, (Region r1, Region r2) => r2.Room == root.Room, delegate(Region r)
			{
				List<Thing> list = r.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn);
				Predicate<Thing> predicate;
				if ((predicate = <>9__2) == null)
				{
					predicate = (<>9__2 = delegate(Thing t)
					{
						Pawn pawn2 = t as Pawn;
						if (pawn2 != null && !pawn2.Downed && (float)(pawn.Position - pawn2.Position).LengthHorizontalSquared < 144f && pawn2.HostileTo(pawn.Faction))
						{
							found = true;
							return true;
						}
						return false;
					});
				}
				return list.Any(predicate);
			}, 9, RegionType.Set_Passable);
			return found;
		}

		// Token: 0x06002DDA RID: 11738 RVA: 0x00112D10 File Offset: 0x00110F10
		public static IntVec3 RandomRaidDest(IntVec3 raidSpawnLoc, Map map)
		{
			List<ThingDef> allBedDefBestToWorst = RestUtility.AllBedDefBestToWorst;
			List<Building> list = new List<Building>(map.mapPawns.FreeColonistsAndPrisonersSpawnedCount);
			for (int i = 0; i < allBedDefBestToWorst.Count; i++)
			{
				foreach (Building building in map.listerBuildings.AllBuildingsColonistOfDef(allBedDefBestToWorst[i]))
				{
					if (((Building_Bed)building).OwnersForReading.Any<Pawn>() && map.reachability.CanReach(raidSpawnLoc, building, PathEndMode.OnCell, TraverseMode.PassAllDestroyableThings, Danger.Deadly))
					{
						list.Add(building);
					}
				}
			}
			Building building2;
			if (list.TryRandomElement(out building2))
			{
				return building2.Position;
			}
			IEnumerable<Building> source = from b in map.listerBuildings.allBuildingsColonist
			where !b.def.building.ai_combatDangerous && !b.def.building.isInert && !b.def.building.ai_neverTrashThis
			select b;
			if (source.Any<Building>())
			{
				for (int j = 0; j < 500; j++)
				{
					IntVec3 intVec = source.RandomElement<Building>().RandomAdjacentCell8Way();
					if (intVec.Walkable(map) && map.reachability.CanReach(raidSpawnLoc, intVec, PathEndMode.OnCell, TraverseMode.PassAllDestroyableThings, Danger.Deadly))
					{
						return intVec;
					}
				}
			}
			Pawn pawn;
			if ((from x in map.mapPawns.FreeColonistsSpawned
			where map.reachability.CanReach(raidSpawnLoc, x, PathEndMode.OnCell, TraverseMode.PassAllDestroyableThings, Danger.Deadly)
			select x).TryRandomElement(out pawn))
			{
				return pawn.Position;
			}
			IntVec3 result;
			if (CellFinderLoose.TryGetRandomCellWith((IntVec3 x) => map.reachability.CanReach(raidSpawnLoc, x, PathEndMode.OnCell, TraverseMode.PassAllDestroyableThings, Danger.Deadly), map, 1000, out result))
			{
				return result;
			}
			return map.Center;
		}

		// Token: 0x06002DDB RID: 11739 RVA: 0x00112EFC File Offset: 0x001110FC
		public static bool EnemyIsNear(Pawn p, float radius)
		{
			if (!p.Spawned)
			{
				return false;
			}
			bool flag = p.Position.Fogged(p.Map);
			List<IAttackTarget> potentialTargetsFor = p.Map.attackTargetsCache.GetPotentialTargetsFor(p);
			for (int i = 0; i < potentialTargetsFor.Count; i++)
			{
				IAttackTarget attackTarget = potentialTargetsFor[i];
				if (!attackTarget.ThreatDisabled(p) && (flag || !attackTarget.Thing.Position.Fogged(attackTarget.Thing.Map)) && p.Position.InHorDistOf(((Thing)attackTarget).Position, radius))
				{
					return true;
				}
			}
			return false;
		}
	}
}
