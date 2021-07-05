using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200086B RID: 2155
	public static class GatheringsUtility
	{
		// Token: 0x060038D7 RID: 14551 RVA: 0x0013DF68 File Offset: 0x0013C168
		public static bool ShouldGuestKeepAttendingGathering(Pawn p)
		{
			return !p.Downed && (p.needs == null || !p.needs.food.Starving) && p.health.hediffSet.BleedRateTotal <= 0f && (p.needs.rest == null || p.needs.rest.CurCategory < RestCategory.Exhausted) && !p.health.hediffSet.HasTendableNonInjuryNonMissingPartHediff(false) && p.Awake() && !p.InAggroMentalState && !p.IsPrisoner && !p.IsSlave;
		}

		// Token: 0x060038D8 RID: 14552 RVA: 0x0013E012 File Offset: 0x0013C212
		public static bool ShouldGuestKeepAttendingRitual(Pawn p, bool ignoreBleeding = false)
		{
			return !p.Downed && (ignoreBleeding || p.health.hediffSet.BleedRateTotal <= 0f) && !p.InAggroMentalState;
		}

		// Token: 0x060038D9 RID: 14553 RVA: 0x0013E048 File Offset: 0x0013C248
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
			if (pawn.IsPrisoner || pawn.IsSlave)
			{
				return false;
			}
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodLoss, false);
			return (firstHediffOfDef == null || firstHediffOfDef.Severity <= 0.2f) && !pawn.IsWildMan() && (pawn.Spawned && !pawn.Downed) && !pawn.InMentalState;
		}

		// Token: 0x060038DA RID: 14554 RVA: 0x0013E0D8 File Offset: 0x0013C2D8
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

		// Token: 0x060038DB RID: 14555 RVA: 0x0013E118 File Offset: 0x0013C318
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

		// Token: 0x060038DC RID: 14556 RVA: 0x0013E204 File Offset: 0x0013C404
		public static bool AcceptableGameConditionsToContinueGathering(Map map)
		{
			return map.dangerWatcher.DangerRating != StoryDanger.High;
		}

		// Token: 0x060038DD RID: 14557 RVA: 0x0013E218 File Offset: 0x0013C418
		public static bool ValidateGatheringSpot(IntVec3 cell, GatheringDef gatheringDef, Pawn organizer, bool enjoyableOutside, bool ignoreRequiredColonistCount)
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
			Room room = cell.GetRoom(map);
			bool flag = room != null && room.IsPrisonCell;
			return organizer.IsPrisoner == flag && (ignoreRequiredColonistCount || GatheringsUtility.EnoughPotentialGuestsToStartGathering(map, gatheringDef, new IntVec3?(cell)));
		}

		// Token: 0x060038DE RID: 14558 RVA: 0x0013E2AC File Offset: 0x0013C4AC
		public static bool EnoughPotentialGuestsToStartGathering(Map map, GatheringDef gatheringDef, IntVec3? gatherSpot = null)
		{
			int num = Mathf.RoundToInt((float)map.mapPawns.FreeColonistsSpawnedCount * 0.65f);
			num = Mathf.Clamp(num, 2, 10);
			int num2 = 0;
			foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
			{
				if (GatheringsUtility.ShouldPawnKeepGathering(pawn, gatheringDef) && (gatherSpot == null || !gatherSpot.Value.IsForbidden(pawn)) && (gatherSpot == null || pawn.CanReach(gatherSpot.Value, PathEndMode.Touch, Danger.Some, false, false, TraverseMode.ByPawn)))
				{
					num2++;
				}
			}
			return num2 >= num;
		}

		// Token: 0x060038DF RID: 14559 RVA: 0x0013E370 File Offset: 0x0013C570
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

		// Token: 0x060038E0 RID: 14560 RVA: 0x0013E3C8 File Offset: 0x0013C5C8
		public static bool InGatheringArea(IntVec3 cell, IntVec3 partySpot, Map map)
		{
			GatheringsUtility.<>c__DisplayClass12_0 CS$<>8__locals1;
			CS$<>8__locals1.cell = cell;
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.partySpot = partySpot;
			if (GatheringsUtility.UseWholeRoomAsGatheringArea(CS$<>8__locals1.partySpot, CS$<>8__locals1.map))
			{
				return CS$<>8__locals1.cell.GetRoom(CS$<>8__locals1.map) == CS$<>8__locals1.partySpot.GetRoom(CS$<>8__locals1.map);
			}
			if (!CS$<>8__locals1.cell.InHorDistOf(CS$<>8__locals1.partySpot, 18f))
			{
				return false;
			}
			Building edifice = CS$<>8__locals1.cell.GetEdifice(CS$<>8__locals1.map);
			TraverseParms traverseParams = TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.None, false, false, false);
			if (edifice != null)
			{
				return CS$<>8__locals1.map.reachability.CanReach(CS$<>8__locals1.partySpot, edifice, PathEndMode.ClosestTouch, traverseParams) && !GatheringsUtility.<InGatheringArea>g__NeedsToPassDoor|12_0(ref CS$<>8__locals1);
			}
			return CS$<>8__locals1.map.reachability.CanReach(CS$<>8__locals1.partySpot, CS$<>8__locals1.cell, PathEndMode.ClosestTouch, traverseParams) && !GatheringsUtility.<InGatheringArea>g__NeedsToPassDoor|12_0(ref CS$<>8__locals1);
		}

		// Token: 0x060038E1 RID: 14561 RVA: 0x0013E4C0 File Offset: 0x0013C6C0
		public static bool TryFindRandomCellInGatheringArea(Pawn pawn, Predicate<IntVec3> cellValidator, out IntVec3 result)
		{
			IntVec3 cell = pawn.mindState.duty.focus.Cell;
			Predicate<IntVec3> validator = (IntVec3 x) => x.Standable(pawn.Map) && !x.IsForbidden(pawn) && pawn.CanReserveAndReach(x, PathEndMode.OnCell, Danger.None, 1, -1, null, false) && (cellValidator == null || cellValidator(x));
			if (GatheringsUtility.UseWholeRoomAsGatheringArea(cell, pawn.Map))
			{
				return (from x in cell.GetRoom(pawn.Map).Cells
				where validator(x)
				select x).TryRandomElement(out result);
			}
			return CellFinder.TryFindRandomReachableCellNear(cell, pawn.Map, 8f, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false), (IntVec3 x) => validator(x), null, out result, 10);
		}

		// Token: 0x060038E2 RID: 14562 RVA: 0x0013E580 File Offset: 0x0013C780
		public static bool TryFindRandomCellInGatheringAreaWithRadius(Pawn pawn, float radius, Predicate<IntVec3> cellValidator, out IntVec3 result)
		{
			IntVec3 partySpot = pawn.mindState.duty.focus.Cell;
			Predicate<IntVec3> validator = (IntVec3 x) => x.Standable(pawn.Map) && !x.IsForbidden(pawn) && pawn.CanReserveAndReach(x, PathEndMode.OnCell, Danger.None, 1, -1, null, false) && (cellValidator == null || cellValidator(x));
			if (GatheringsUtility.UseWholeRoomAsGatheringArea(partySpot, pawn.Map))
			{
				return (from x in partySpot.GetRoom(pawn.Map).Cells
				where validator(x) && x.InHorDistOf(partySpot, radius)
				select x).TryRandomElement(out result);
			}
			return CellFinder.TryFindRandomReachableCellNear(partySpot, pawn.Map, radius, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false), (IntVec3 x) => validator(x), null, out result, 10);
		}

		// Token: 0x060038E3 RID: 14563 RVA: 0x0013E65C File Offset: 0x0013C85C
		public static bool TryFindRandomCellAroundTarget(Pawn pawn, Thing target, out IntVec3 result)
		{
			if (!(from c in target.OccupiedRect().ExpandedBy(1).EdgeCells
			where c.InBounds(pawn.Map) && c.Standable(pawn.Map) && !c.IsForbidden(pawn) && pawn.CanReserveAndReach(c, PathEndMode.OnCell, Danger.None, 1, -1, null, false)
			select c).TryRandomElement(out result))
			{
				result = IntVec3.Invalid;
				return false;
			}
			return true;
		}

		// Token: 0x060038E4 RID: 14564 RVA: 0x0013E6B4 File Offset: 0x0013C8B4
		public static bool UseWholeRoomAsGatheringArea(IntVec3 partySpot, Map map)
		{
			Room room = partySpot.GetRoom(map);
			return room != null && !room.IsHuge && room.CellCount <= 100 && !room.PsychologicallyOutdoors;
		}

		// Token: 0x060038E5 RID: 14565 RVA: 0x0013E6E9 File Offset: 0x0013C8E9
		public static bool ShouldPawnKeepGathering(Pawn p, GatheringDef gatheringDef)
		{
			return (gatheringDef == null || !gatheringDef.respectTimetable || p.timetable == null || p.timetable.CurrentAssignment.allowJoy) && GatheringsUtility.ShouldGuestKeepAttendingGathering(p);
		}

		// Token: 0x060038E6 RID: 14566 RVA: 0x0013E71D File Offset: 0x0013C91D
		public static bool ShouldPawnKeepAttendingRitual(Pawn p, Precept_Ritual ritual, bool ignoreBleeding = false)
		{
			return GatheringsUtility.ShouldGuestKeepAttendingRitual(p, ignoreBleeding);
		}

		// Token: 0x060038E7 RID: 14567 RVA: 0x0013E72B File Offset: 0x0013C92B
		[CompilerGenerated]
		internal static bool <InGatheringArea>g__NeedsToPassDoor|12_0(ref GatheringsUtility.<>c__DisplayClass12_0 A_0)
		{
			return A_0.cell.GetRoom(A_0.map) != A_0.partySpot.GetRoom(A_0.map);
		}

		// Token: 0x04001F35 RID: 7989
		public const float GatherAreaRadiusIfNotWholeRoom = 18f;

		// Token: 0x04001F36 RID: 7990
		public const float WanderAreaRadiusIfNotWholeRoom = 8f;

		// Token: 0x04001F37 RID: 7991
		private const int MaxRoomCellsCountToUseWholeRoom = 100;
	}
}
