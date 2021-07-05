using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008E9 RID: 2281
	public static class RestUtility
	{
		// Token: 0x17000AB6 RID: 2742
		// (get) Token: 0x06003BC6 RID: 15302 RVA: 0x0014D129 File Offset: 0x0014B329
		public static List<ThingDef> AllBedDefBestToWorst
		{
			get
			{
				return RestUtility.bedDefsBestToWorst_RestEffectiveness;
			}
		}

		// Token: 0x06003BC7 RID: 15303 RVA: 0x0014D130 File Offset: 0x0014B330
		public static void Reset()
		{
			RestUtility.bedDefsBestToWorst_RestEffectiveness = (from d in DefDatabase<ThingDef>.AllDefs
			where d.IsBed
			orderby d.building.bed_maxBodySize, d.GetStatValueAbstract(StatDefOf.BedRestEffectiveness, null) descending
			select d).ToList<ThingDef>();
			RestUtility.bedDefsBestToWorst_SlabBed_RestEffectiveness = (from d in DefDatabase<ThingDef>.AllDefs
			where d.IsBed
			select d).OrderBy(delegate(ThingDef d)
			{
				if (!d.building.bed_slabBed)
				{
					return 1;
				}
				return 0;
			}).ThenBy((ThingDef d) => d.building.bed_maxBodySize).ThenByDescending((ThingDef d) => d.GetStatValueAbstract(StatDefOf.BedRestEffectiveness, null)).ToList<ThingDef>();
			RestUtility.bedDefsBestToWorst_Medical = (from d in DefDatabase<ThingDef>.AllDefs
			where d.IsBed
			orderby d.building.bed_maxBodySize, d.GetStatValueAbstract(StatDefOf.MedicalTendQualityOffset, null) descending, d.GetStatValueAbstract(StatDefOf.BedRestEffectiveness, null) descending
			select d).ToList<ThingDef>();
			RestUtility.bedDefsBestToWorst_SlabBed_Medical = (from d in DefDatabase<ThingDef>.AllDefs
			where d.IsBed
			select d).OrderBy(delegate(ThingDef d)
			{
				if (!d.building.bed_slabBed)
				{
					return 1;
				}
				return 0;
			}).ThenBy((ThingDef d) => d.building.bed_maxBodySize).ThenByDescending((ThingDef d) => d.GetStatValueAbstract(StatDefOf.MedicalTendQualityOffset, null)).ThenByDescending((ThingDef d) => d.GetStatValueAbstract(StatDefOf.BedRestEffectiveness, null)).ToList<ThingDef>();
		}

		// Token: 0x06003BC8 RID: 15304 RVA: 0x0014D3BC File Offset: 0x0014B5BC
		public static bool IsValidBedFor(Thing bedThing, Pawn sleeper, Pawn traveler, bool checkSocialProperness, bool allowMedBedEvenIfSetToNoCare = false, bool ignoreOtherReservations = false, GuestStatus? guestStatus = null)
		{
			Building_Bed building_Bed = bedThing as Building_Bed;
			if (building_Bed == null)
			{
				return false;
			}
			if (!traveler.CanReserveAndReach(building_Bed, PathEndMode.OnCell, Danger.Some, building_Bed.SleepingSlotsCount, -1, null, ignoreOtherReservations))
			{
				return false;
			}
			if (traveler.HasReserved(building_Bed, new LocalTargetInfo?(sleeper), null, null))
			{
				return false;
			}
			if (!RestUtility.CanUseBedEver(sleeper, building_Bed.def))
			{
				return false;
			}
			if (!building_Bed.AnyUnoccupiedSleepingSlot && (!sleeper.InBed() || sleeper.CurrentBed() != building_Bed) && !building_Bed.CompAssignableToPawn.AssignedPawns.Contains(sleeper))
			{
				return false;
			}
			if (building_Bed.IsForbidden(traveler))
			{
				return false;
			}
			GuestStatus? guestStatus2 = guestStatus;
			GuestStatus guestStatus3 = GuestStatus.Prisoner;
			bool flag = guestStatus2.GetValueOrDefault() == guestStatus3 & guestStatus2 != null;
			guestStatus2 = guestStatus;
			guestStatus3 = GuestStatus.Slave;
			bool flag2 = guestStatus2.GetValueOrDefault() == guestStatus3 & guestStatus2 != null;
			if (checkSocialProperness && !building_Bed.IsSociallyProper(sleeper, flag, false))
			{
				return false;
			}
			if (building_Bed.CompAssignableToPawn.IdeoligionForbids(sleeper))
			{
				return false;
			}
			if (building_Bed.IsBurning())
			{
				return false;
			}
			if (flag)
			{
				if (!building_Bed.ForPrisoners)
				{
					return false;
				}
				if (!building_Bed.Position.IsInPrisonCell(building_Bed.Map))
				{
					return false;
				}
			}
			else if (flag2)
			{
				if (!building_Bed.ForSlaves)
				{
					return false;
				}
			}
			else
			{
				if (building_Bed.Faction != traveler.Faction && (traveler.HostFaction == null || building_Bed.Faction != traveler.HostFaction))
				{
					return false;
				}
				if (building_Bed.ForPrisoners || building_Bed.ForSlaves)
				{
					return false;
				}
			}
			if (building_Bed.Medical)
			{
				if (!allowMedBedEvenIfSetToNoCare && !HealthAIUtility.ShouldEverReceiveMedicalCareFromPlayer(sleeper))
				{
					return false;
				}
				if (!HealthAIUtility.ShouldSeekMedicalRest(sleeper))
				{
					return false;
				}
			}
			else if (building_Bed.OwnersForReading.Any<Pawn>() && !building_Bed.OwnersForReading.Contains(sleeper))
			{
				if (sleeper.IsPrisoner || flag || sleeper.IsSlave || flag2)
				{
					if (!building_Bed.AnyUnownedSleepingSlot)
					{
						return false;
					}
				}
				else
				{
					if (!RestUtility.IsAnyOwnerLovePartnerOf(building_Bed, sleeper))
					{
						return false;
					}
					if (!building_Bed.AnyUnownedSleepingSlot)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06003BC9 RID: 15305 RVA: 0x0014D5A0 File Offset: 0x0014B7A0
		private static bool IsAnyOwnerLovePartnerOf(Building_Bed bed, Pawn sleeper)
		{
			for (int i = 0; i < bed.OwnersForReading.Count; i++)
			{
				if (LovePartnerRelationUtility.LovePartnerRelationExists(sleeper, bed.OwnersForReading[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003BCA RID: 15306 RVA: 0x0014D5DA File Offset: 0x0014B7DA
		public static Building_Bed FindBedFor(Pawn p)
		{
			return RestUtility.FindBedFor(p, p, true, false, p.GuestStatus);
		}

		// Token: 0x06003BCB RID: 15307 RVA: 0x0014D5EC File Offset: 0x0014B7EC
		public static Building_Bed FindBedFor(Pawn sleeper, Pawn traveler, bool checkSocialProperness, bool ignoreOtherReservations = false, GuestStatus? guestStatus = null)
		{
			bool flag = false;
			if (sleeper.Ideo != null)
			{
				using (List<Precept>.Enumerator enumerator = sleeper.Ideo.PreceptsListForReading.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.def.prefersSlabBed)
						{
							flag = true;
							break;
						}
					}
				}
			}
			List<ThingDef> list = flag ? RestUtility.bedDefsBestToWorst_SlabBed_Medical : RestUtility.bedDefsBestToWorst_Medical;
			List<ThingDef> list2 = flag ? RestUtility.bedDefsBestToWorst_SlabBed_RestEffectiveness : RestUtility.bedDefsBestToWorst_RestEffectiveness;
			if (HealthAIUtility.ShouldSeekMedicalRest(sleeper))
			{
				if (sleeper.InBed() && sleeper.CurrentBed().Medical && RestUtility.IsValidBedFor(sleeper.CurrentBed(), sleeper, traveler, checkSocialProperness, false, ignoreOtherReservations, guestStatus))
				{
					return sleeper.CurrentBed();
				}
				for (int i = 0; i < list.Count; i++)
				{
					ThingDef thingDef = list[i];
					if (RestUtility.CanUseBedEver(sleeper, thingDef))
					{
						for (int j = 0; j < 2; j++)
						{
							Danger maxDanger = (j == 0) ? Danger.None : Danger.Deadly;
							Building_Bed building_Bed = (Building_Bed)GenClosest.ClosestThingReachable(sleeper.Position, sleeper.Map, ThingRequest.ForDef(thingDef), PathEndMode.OnCell, TraverseParms.For(traveler, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 9999f, (Thing b) => ((Building_Bed)b).Medical && b.Position.GetDangerFor(sleeper, sleeper.Map) <= maxDanger && RestUtility.IsValidBedFor(b, sleeper, traveler, checkSocialProperness, false, ignoreOtherReservations, guestStatus), null, 0, -1, false, RegionType.Set_Passable, false);
							if (building_Bed != null)
							{
								return building_Bed;
							}
						}
					}
				}
			}
			if (sleeper.ownership != null && sleeper.ownership.OwnedBed != null && RestUtility.IsValidBedFor(sleeper.ownership.OwnedBed, sleeper, traveler, checkSocialProperness, false, ignoreOtherReservations, guestStatus))
			{
				return sleeper.ownership.OwnedBed;
			}
			DirectPawnRelation directPawnRelation = LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(sleeper, false);
			if (directPawnRelation != null)
			{
				Building_Bed ownedBed = directPawnRelation.otherPawn.ownership.OwnedBed;
				if (ownedBed != null && RestUtility.IsValidBedFor(ownedBed, sleeper, traveler, checkSocialProperness, false, ignoreOtherReservations, guestStatus))
				{
					return ownedBed;
				}
			}
			for (int k = 0; k < 2; k++)
			{
				Danger maxDanger = (k == 0) ? Danger.None : Danger.Deadly;
				Predicate<Thing> <>9__1;
				for (int l = 0; l < list2.Count; l++)
				{
					ThingDef thingDef2 = list2[l];
					if (RestUtility.CanUseBedEver(sleeper, thingDef2))
					{
						IntVec3 position = sleeper.Position;
						Map map = sleeper.Map;
						ThingRequest thingReq = ThingRequest.ForDef(thingDef2);
						PathEndMode peMode = PathEndMode.OnCell;
						TraverseParms traverseParams = TraverseParms.For(traveler, Danger.Deadly, TraverseMode.ByPawn, false, false, false);
						float maxDistance = 9999f;
						Predicate<Thing> validator;
						if ((validator = <>9__1) == null)
						{
							validator = (<>9__1 = ((Thing b) => !((Building_Bed)b).Medical && b.Position.GetDangerFor(sleeper, sleeper.Map) <= maxDanger && RestUtility.IsValidBedFor(b, sleeper, traveler, checkSocialProperness, false, ignoreOtherReservations, guestStatus)));
						}
						Building_Bed building_Bed2 = (Building_Bed)GenClosest.ClosestThingReachable(position, map, thingReq, peMode, traverseParams, maxDistance, validator, null, 0, -1, false, RegionType.Set_Passable, false);
						if (building_Bed2 != null)
						{
							return building_Bed2;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06003BCC RID: 15308 RVA: 0x0014D9A0 File Offset: 0x0014BBA0
		public static Building_Bed FindPatientBedFor(Pawn pawn)
		{
			Predicate<Thing> medBedValidator = delegate(Thing t)
			{
				Building_Bed building_Bed2 = t as Building_Bed;
				return building_Bed2 != null && building_Bed2.Medical && RestUtility.IsValidBedFor(building_Bed2, pawn, pawn, false, true, false, pawn.GuestStatus);
			};
			if (pawn.InBed() && medBedValidator(pawn.CurrentBed()))
			{
				return pawn.CurrentBed();
			}
			for (int i = 0; i < 2; i++)
			{
				Danger maxDanger = (i == 0) ? Danger.None : Danger.Deadly;
				Building_Bed building_Bed = (Building_Bed)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Bed), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 9999f, (Thing b) => b.Position.GetDangerFor(pawn, pawn.Map) <= maxDanger && medBedValidator(b), null, 0, -1, false, RegionType.Set_Passable, false);
				if (building_Bed != null)
				{
					return building_Bed;
				}
			}
			return RestUtility.FindBedFor(pawn);
		}

		// Token: 0x06003BCD RID: 15309 RVA: 0x0014DA9C File Offset: 0x0014BC9C
		public static IntVec3 GetBedSleepingSlotPosFor(Pawn pawn, Building_Bed bed)
		{
			for (int i = 0; i < bed.OwnersForReading.Count; i++)
			{
				if (bed.OwnersForReading[i] == pawn)
				{
					return bed.GetSleepingSlotPos(i);
				}
			}
			for (int j = 0; j < bed.SleepingSlotsCount; j++)
			{
				Pawn curOccupant = bed.GetCurOccupant(j);
				if ((j >= bed.OwnersForReading.Count || bed.OwnersForReading[j] == null) && curOccupant == pawn)
				{
					return bed.GetSleepingSlotPos(j);
				}
			}
			for (int k = 0; k < bed.SleepingSlotsCount; k++)
			{
				Pawn curOccupant2 = bed.GetCurOccupant(k);
				if ((k >= bed.OwnersForReading.Count || bed.OwnersForReading[k] == null) && curOccupant2 == null)
				{
					return bed.GetSleepingSlotPos(k);
				}
			}
			Log.Error("Could not find good sleeping slot position for " + pawn + ". Perhaps AnyUnoccupiedSleepingSlot check is missing somewhere.");
			return bed.GetSleepingSlotPos(0);
		}

		// Token: 0x06003BCE RID: 15310 RVA: 0x0014DB75 File Offset: 0x0014BD75
		public static bool CanUseBedEver(Pawn p, ThingDef bedDef)
		{
			return p.BodySize <= bedDef.building.bed_maxBodySize && p.RaceProps.Humanlike == bedDef.building.bed_humanlike;
		}

		// Token: 0x06003BCF RID: 15311 RVA: 0x0014DBA7 File Offset: 0x0014BDA7
		public static bool TimetablePreventsLayDown(Pawn pawn)
		{
			return pawn.timetable != null && !pawn.timetable.CurrentAssignment.allowRest && pawn.needs.rest.CurLevel >= 0.2f;
		}

		// Token: 0x06003BD0 RID: 15312 RVA: 0x0014DBDD File Offset: 0x0014BDDD
		public static bool DisturbancePreventsLyingDown(Pawn pawn)
		{
			return !pawn.Downed && Find.TickManager.TicksGame - pawn.mindState.lastDisturbanceTick < 400;
		}

		// Token: 0x06003BD1 RID: 15313 RVA: 0x0014DC08 File Offset: 0x0014BE08
		public static bool Awake(this Pawn p)
		{
			return p.health.capacities.CanBeAwake && (!p.Spawned || p.CurJob == null || p.jobs.curDriver == null || !p.jobs.curDriver.asleep);
		}

		// Token: 0x06003BD2 RID: 15314 RVA: 0x0014DC60 File Offset: 0x0014BE60
		public static Building_Bed CurrentBed(this Pawn p)
		{
			if (!p.Spawned || p.CurJob == null || p.GetPosture() != PawnPosture.LayingInBed)
			{
				return null;
			}
			Building_Bed building_Bed = null;
			List<Thing> thingList = p.Position.GetThingList(p.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				building_Bed = (thingList[i] as Building_Bed);
				if (building_Bed != null)
				{
					break;
				}
			}
			if (building_Bed == null)
			{
				return null;
			}
			for (int j = 0; j < building_Bed.SleepingSlotsCount; j++)
			{
				if (building_Bed.GetCurOccupant(j) == p)
				{
					return building_Bed;
				}
			}
			return null;
		}

		// Token: 0x06003BD3 RID: 15315 RVA: 0x0014DCE0 File Offset: 0x0014BEE0
		public static bool InBed(this Pawn p)
		{
			return p.CurrentBed() != null;
		}

		// Token: 0x06003BD4 RID: 15316 RVA: 0x0014DCEC File Offset: 0x0014BEEC
		public static void WakeUp(Pawn p)
		{
			if (p.CurJob != null && (p.GetPosture().Laying() || p.CurJobDef == JobDefOf.LayDown) && !p.Downed)
			{
				p.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
			CompCanBeDormant comp = p.GetComp<CompCanBeDormant>();
			if (comp == null)
			{
				return;
			}
			comp.WakeUp();
		}

		// Token: 0x06003BD5 RID: 15317 RVA: 0x0014DD44 File Offset: 0x0014BF44
		public static float WakeThreshold(Pawn p)
		{
			Lord lord = p.GetLord();
			if (lord != null && lord.CurLordToil != null && lord.CurLordToil.CustomWakeThreshold != null)
			{
				return lord.CurLordToil.CustomWakeThreshold.Value;
			}
			return 1f;
		}

		// Token: 0x06003BD6 RID: 15318 RVA: 0x0014DD91 File Offset: 0x0014BF91
		public static float FallAsleepMaxLevel(Pawn p)
		{
			return Mathf.Min(0.75f, RestUtility.WakeThreshold(p) - 0.01f);
		}

		// Token: 0x04002082 RID: 8322
		private static List<ThingDef> bedDefsBestToWorst_RestEffectiveness;

		// Token: 0x04002083 RID: 8323
		private static List<ThingDef> bedDefsBestToWorst_Medical;

		// Token: 0x04002084 RID: 8324
		private static List<ThingDef> bedDefsBestToWorst_SlabBed_RestEffectiveness;

		// Token: 0x04002085 RID: 8325
		private static List<ThingDef> bedDefsBestToWorst_SlabBed_Medical;
	}
}
