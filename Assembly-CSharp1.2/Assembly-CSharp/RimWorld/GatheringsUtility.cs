using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DAF RID: 3503
	public static class GatheringsUtility
	{
		// Token: 0x06004FD7 RID: 20439 RVA: 0x001B54E0 File Offset: 0x001B36E0
		public static bool ShouldGuestKeepAttendingGathering(Pawn p)
		{
			return !p.Downed && (p.needs == null || !p.needs.food.Starving) && p.health.hediffSet.BleedRateTotal <= 0f && (p.needs.rest == null || p.needs.rest.CurCategory < RestCategory.Exhausted) && !p.health.hediffSet.HasTendableNonInjuryNonMissingPartHediff(false) && p.Awake() && !p.InAggroMentalState && !p.IsPrisoner;
		}

		// Token: 0x06004FD8 RID: 20440 RVA: 0x001B5584 File Offset: 0x001B3784
		public static bool PawnCanStartOrContinueGathering(Pawn pawn)
		{
			if (pawn.Drafted)
			{
				return false;
			}
			if (pawn.health.hediffSet.BleedRateTotal > 0.3f)
			{
				return false;
			}
			if (pawn.IsPrisoner)
			{
				return false;
			}
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodLoss, false);
			return (firstHediffOfDef == null || firstHediffOfDef.Severity <= 0.2f) && !pawn.IsWildMan() && (pawn.Spawned && !pawn.Downed) && !pawn.InMentalState;
		}

		// Token: 0x06004FD9 RID: 20441 RVA: 0x001B560C File Offset: 0x001B380C
		public static bool AnyLordJobPreventsNewGatherings(Map map)
		{
			List<Lord> lords = map.lordManager.lords;
			for (int i = 0; i < lords.Count; i++)
			{
				if (!lords[i].LordJob.AllowStartNewGatherings)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004FDA RID: 20442 RVA: 0x001B564C File Offset: 0x001B384C
		public static bool AcceptableGameConditionsToStartGathering(Map map, GatheringDef gatheringDef)
		{
			if (!GatheringsUtility.AcceptableGameConditionsToContinueGathering(map))
			{
				return false;
			}
			if (GenLocalDate.HourInteger(map) < 4 || GenLocalDate.HourInteger(map) > 21)
			{
				return false;
			}
			if (GatheringsUtility.AnyLordJobPreventsNewGatherings(map))
			{
				return false;
			}
			if (map.dangerWatcher.DangerRating != StoryDanger.None)
			{
				return false;
			}
			int freeColonistsSpawnedCount = map.mapPawns.FreeColonistsSpawnedCount;
			if (freeColonistsSpawnedCount < 4)
			{
				return false;
			}
			int num = 0;
			foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
			{
				if (pawn.health.hediffSet.BleedRateTotal > 0f)
				{
					return false;
				}
				if (pawn.Drafted)
				{
					num++;
				}
			}
			return (float)num / (float)freeColonistsSpawnedCount < 0.5f && GatheringsUtility.EnoughPotentialGuestsToStartGathering(map, gatheringDef, null);
		}

		// Token: 0x06004FDB RID: 20443 RVA: 0x000380E8 File Offset: 0x000362E8
		public static bool AcceptableGameConditionsToContinueGathering(Map map)
		{
			return map.dangerWatcher.DangerRating != StoryDanger.High;
		}

		// Token: 0x06004FDC RID: 20444 RVA: 0x001B5738 File Offset: 0x001B3938
		public static bool ValidateGatheringSpot_NewTemp(IntVec3 cell, GatheringDef gatheringDef, Pawn organizer, bool enjoyableOutside, bool ignoreRequiredColonistCount)
		{
			Map map = organizer.Map;
			if (!cell.Standable(map))
			{
				return false;
			}
			if (cell.GetDangerFor(organizer, map) != Danger.None)
			{
				return false;
			}
			if (!enjoyableOutside && !cell.Roofed(map))
			{
				return false;
			}
			if (cell.IsForbidden(organizer))
			{
				return false;
			}
			if (!organizer.CanReserveAndReach(cell, PathEndMode.OnCell, Danger.None, 1, -1, null, false))
			{
				return false;
			}
			Room room = cell.GetRoom(map, RegionType.Set_Passable);
			bool flag = room != null && room.isPrisonCell;
			return organizer.IsPrisoner == flag && (ignoreRequiredColonistCount || GatheringsUtility.EnoughPotentialGuestsToStartGathering(map, gatheringDef, new IntVec3?(cell)));
		}

		// Token: 0x06004FDD RID: 20445 RVA: 0x000380FB File Offset: 0x000362FB
		[Obsolete("Will be removed in a future update")]
		public static bool ValidateGatheringSpot(IntVec3 cell, GatheringDef gatheringDef, Pawn organizer, bool enjoyableOutside)
		{
			return GatheringsUtility.ValidateGatheringSpot_NewTemp(cell, gatheringDef, organizer, enjoyableOutside, false);
		}

		// Token: 0x06004FDE RID: 20446 RVA: 0x001B57CC File Offset: 0x001B39CC
		public static bool EnoughPotentialGuestsToStartGathering(Map map, GatheringDef gatheringDef, IntVec3? gatherSpot = null)
		{
			int num = Mathf.RoundToInt((float)map.mapPawns.FreeColonistsSpawnedCount * 0.65f);
			num = Mathf.Clamp(num, 2, 10);
			int num2 = 0;
			foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
			{
				if (GatheringsUtility.ShouldPawnKeepGathering(pawn, gatheringDef) && (gatherSpot == null || !gatherSpot.Value.IsForbidden(pawn)) && (gatherSpot == null || pawn.CanReach(gatherSpot.Value, PathEndMode.Touch, Danger.Some, false, TraverseMode.ByPawn)))
				{
					num2++;
				}
			}
			return num2 >= num;
		}

		// Token: 0x06004FDF RID: 20447 RVA: 0x001B5890 File Offset: 0x001B3A90
		public static Pawn FindRandomGatheringOrganizer(Faction faction, Map map, GatheringDef gatheringDef)
		{
			Predicate<RoyalTitle> <>9__2;
			Predicate<Pawn> v = delegate(Pawn x)
			{
				if (!x.RaceProps.Humanlike || x.InBed() || x.InMentalState || x.GetLord() != null || !GatheringsUtility.ShouldPawnKeepGathering(x, gatheringDef) || x.Drafted)
				{
					return false;
				}
				if (gatheringDef.requiredTitleAny == null || gatheringDef.requiredTitleAny.Count == 0)
				{
					return true;
				}
				if (x.royalty != null)
				{
					List<RoyalTitle> allTitlesInEffectForReading = x.royalty.AllTitlesInEffectForReading;
					Predicate<RoyalTitle> predicate;
					if ((predicate = <>9__2) == null)
					{
						predicate = (<>9__2 = ((RoyalTitle t) => gatheringDef.requiredTitleAny.Contains(t.def)));
					}
					return allTitlesInEffectForReading.Any(predicate);
				}
				return false;
			};
			Pawn result;
			if ((from x in map.mapPawns.SpawnedPawnsInFaction(faction)
			where v(x)
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06004FE0 RID: 20448 RVA: 0x001B58E8 File Offset: 0x001B3AE8
		public static bool InGatheringArea(IntVec3 cell, IntVec3 partySpot, Map map)
		{
			if (GatheringsUtility.UseWholeRoomAsGatheringArea(partySpot, map) && cell.GetRoom(map, RegionType.Set_Passable) == partySpot.GetRoom(map, RegionType.Set_Passable))
			{
				return true;
			}
			if (!cell.InHorDistOf(partySpot, 10f))
			{
				return false;
			}
			Building edifice = cell.GetEdifice(map);
			TraverseParms traverseParams = TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.None, false);
			if (edifice != null)
			{
				return map.reachability.CanReach(partySpot, edifice, PathEndMode.ClosestTouch, traverseParams);
			}
			return map.reachability.CanReach(partySpot, cell, PathEndMode.ClosestTouch, traverseParams);
		}

		// Token: 0x06004FE1 RID: 20449 RVA: 0x001B5960 File Offset: 0x001B3B60
		public static bool TryFindRandomCellInGatheringArea(Pawn pawn, out IntVec3 result)
		{
			IntVec3 cell = pawn.mindState.duty.focus.Cell;
			Predicate<IntVec3> validator = (IntVec3 x) => x.Standable(pawn.Map) && !x.IsForbidden(pawn) && pawn.CanReserveAndReach(x, PathEndMode.OnCell, Danger.None, 1, -1, null, false);
			if (GatheringsUtility.UseWholeRoomAsGatheringArea(cell, pawn.Map))
			{
				return (from x in cell.GetRoom(pawn.Map, RegionType.Set_Passable).Cells
				where validator(x)
				select x).TryRandomElement(out result);
			}
			return CellFinder.TryFindRandomReachableCellNear(cell, pawn.Map, 10f, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), (IntVec3 x) => validator(x), null, out result, 10);
		}

		// Token: 0x06004FE2 RID: 20450 RVA: 0x001B5A18 File Offset: 0x001B3C18
		public static bool UseWholeRoomAsGatheringArea(IntVec3 partySpot, Map map)
		{
			Room room = partySpot.GetRoom(map, RegionType.Set_Passable);
			return room != null && !room.IsHuge && !room.PsychologicallyOutdoors && room.CellCount <= 324;
		}

		// Token: 0x06004FE3 RID: 20451 RVA: 0x00038107 File Offset: 0x00036307
		public static bool ShouldPawnKeepGathering(Pawn p, GatheringDef gatheringDef)
		{
			return (!gatheringDef.respectTimetable || p.timetable == null || p.timetable.CurrentAssignment.allowJoy) && GatheringsUtility.ShouldGuestKeepAttendingGathering(p);
		}

		// Token: 0x0400339B RID: 13211
		private const float GatherAreaRadiusIfNotWholeRoom = 10f;

		// Token: 0x0400339C RID: 13212
		private const int MaxRoomCellsCountToUseWholeRoom = 324;
	}
}
