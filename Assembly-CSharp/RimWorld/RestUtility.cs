using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E37 RID: 3639
	public static class RestUtility
	{
		// Token: 0x17000CB8 RID: 3256
		// (get) Token: 0x06005286 RID: 21126 RVA: 0x00039B8D File Offset: 0x00037D8D
		public static List<ThingDef> AllBedDefBestToWorst
		{
			get
			{
				return RestUtility.bedDefsBestToWorst_RestEffectiveness;
			}
		}

		// Token: 0x06005287 RID: 21127 RVA: 0x001BEA80 File Offset: 0x001BCC80
		public static void Reset()
		{
			RestUtility.bedDefsBestToWorst_RestEffectiveness = (from d in DefDatabase<ThingDef>.AllDefs
			where d.IsBed
			orderby d.building.bed_maxBodySize, d.GetStatValueAbstract(StatDefOf.BedRestEffectiveness, null) descending
			select d).ToList<ThingDef>();
			RestUtility.bedDefsBestToWorst_Medical = (from d in DefDatabase<ThingDef>.AllDefs
			where d.IsBed
			orderby d.building.bed_maxBodySize, d.GetStatValueAbstract(StatDefOf.MedicalTendQualityOffset, null) descending, d.GetStatValueAbstract(StatDefOf.BedRestEffectiveness, null) descending
			select d).ToList<ThingDef>();
		}

		// Token: 0x06005288 RID: 21128 RVA: 0x001BEBA8 File Offset: 0x001BCDA8
		public static bool IsValidBedFor(Thing bedThing, Pawn sleeper, Pawn traveler, bool sleeperWillBePrisoner, bool checkSocialProperness, bool allowMedBedEvenIfSetToNoCare = false, bool ignoreOtherReservations = false)
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
			if (checkSocialProperness && !building_Bed.IsSociallyProper(sleeper, sleeperWillBePrisoner, false))
			{
				return false;
			}
			if (building_Bed.IsBurning())
			{
				return false;
			}
			if (sleeperWillBePrisoner)
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
			else
			{
				if (building_Bed.Faction != traveler.Faction && (traveler.HostFaction == null || building_Bed.Faction != traveler.HostFaction))
				{
					return false;
				}
				if (building_Bed.ForPrisoners)
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
				if (sleeper.IsPrisoner || sleeperWillBePrisoner)
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

		// Token: 0x06005289 RID: 21129 RVA: 0x001BED28 File Offset: 0x001BCF28
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

		// Token: 0x0600528A RID: 21130 RVA: 0x00039B94 File Offset: 0x00037D94
		public static Building_Bed FindBedFor(Pawn p)
		{
			return RestUtility.FindBedFor(p, p, p.IsPrisoner, true, false);
		}

		// Token: 0x0600528B RID: 21131 RVA: 0x001BED64 File Offset: 0x001BCF64
		public static Building_Bed FindBedFor(Pawn sleeper, Pawn traveler, bool sleeperWillBePrisoner, bool checkSocialProperness, bool ignoreOtherReservations = false)
		{
			if (HealthAIUtility.ShouldSeekMedicalRest(sleeper))
			{
				if (sleeper.InBed() && sleeper.CurrentBed().Medical && RestUtility.IsValidBedFor(sleeper.CurrentBed(), sleeper, traveler, sleeperWillBePrisoner, checkSocialProperness, false, ignoreOtherReservations))
				{
					return sleeper.CurrentBed();
				}
				for (int i = 0; i < RestUtility.bedDefsBestToWorst_Medical.Count; i++)
				{
					ThingDef thingDef = RestUtility.bedDefsBestToWorst_Medical[i];
					if (RestUtility.CanUseBedEver(sleeper, thingDef))
					{
						for (int j = 0; j < 2; j++)
						{
							Danger maxDanger = (j == 0) ? Danger.None : Danger.Deadly;
							Building_Bed building_Bed = (Building_Bed)GenClosest.ClosestThingReachable(sleeper.Position, sleeper.Map, ThingRequest.ForDef(thingDef), PathEndMode.OnCell, TraverseParms.For(traveler, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, (Thing b) => ((Building_Bed)b).Medical && b.Position.GetDangerFor(sleeper, sleeper.Map) <= maxDanger && RestUtility.IsValidBedFor(b, sleeper, traveler, sleeperWillBePrisoner, checkSocialProperness, false, ignoreOtherReservations), null, 0, -1, false, RegionType.Set_Passable, false);
							if (building_Bed != null)
							{
								return building_Bed;
							}
						}
					}
				}
			}
			if (sleeper.ownership != null && sleeper.ownership.OwnedBed != null && RestUtility.IsValidBedFor(sleeper.ownership.OwnedBed, sleeper, traveler, sleeperWillBePrisoner, checkSocialProperness, false, ignoreOtherReservations))
			{
				return sleeper.ownership.OwnedBed;
			}
			DirectPawnRelation directPawnRelation = LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(sleeper, false);
			if (directPawnRelation != null)
			{
				Building_Bed ownedBed = directPawnRelation.otherPawn.ownership.OwnedBed;
				if (ownedBed != null && RestUtility.IsValidBedFor(ownedBed, sleeper, traveler, sleeperWillBePrisoner, checkSocialProperness, false, ignoreOtherReservations))
				{
					return ownedBed;
				}
			}
			for (int k = 0; k < 2; k++)
			{
				Danger maxDanger = (k == 0) ? Danger.None : Danger.Deadly;
				Predicate<Thing> <>9__1;
				for (int l = 0; l < RestUtility.bedDefsBestToWorst_RestEffectiveness.Count; l++)
				{
					ThingDef thingDef2 = RestUtility.bedDefsBestToWorst_RestEffectiveness[l];
					if (RestUtility.CanUseBedEver(sleeper, thingDef2))
					{
						IntVec3 position = sleeper.Position;
						Map map = sleeper.Map;
						ThingRequest thingReq = ThingRequest.ForDef(thingDef2);
						PathEndMode peMode = PathEndMode.OnCell;
						TraverseParms traverseParams = TraverseParms.For(traveler, Danger.Deadly, TraverseMode.ByPawn, false);
						float maxDistance = 9999f;
						Predicate<Thing> validator;
						if ((validator = <>9__1) == null)
						{
							validator = (<>9__1 = ((Thing b) => !((Building_Bed)b).Medical && b.Position.GetDangerFor(sleeper, sleeper.Map) <= maxDanger && RestUtility.IsValidBedFor(b, sleeper, traveler, sleeperWillBePrisoner, checkSocialProperness, false, ignoreOtherReservations)));
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

		// Token: 0x0600528C RID: 21132 RVA: 0x001BF08C File Offset: 0x001BD28C
		public static Building_Bed FindPatientBedFor(Pawn pawn)
		{
			Predicate<Thing> medBedValidator = delegate(Thing t)
			{
				Building_Bed building_Bed2 = t as Building_Bed;
				return building_Bed2 != null && (building_Bed2.Medical || !building_Bed2.def.building.bed_humanlike) && RestUtility.IsValidBedFor(building_Bed2, pawn, pawn, pawn.IsPrisoner, false, true, false);
			};
			if (pawn.InBed() && medBedValidator(pawn.CurrentBed()))
			{
				return pawn.CurrentBed();
			}
			for (int i = 0; i < 2; i++)
			{
				Danger maxDanger = (i == 0) ? Danger.None : Danger.Deadly;
				Building_Bed building_Bed = (Building_Bed)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Bed), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, (Thing b) => b.Position.GetDangerFor(pawn, pawn.Map) <= maxDanger && medBedValidator(b), null, 0, -1, false, RegionType.Set_Passable, false);
				if (building_Bed != null)
				{
					return building_Bed;
				}
			}
			return RestUtility.FindBedFor(pawn);
		}

		// Token: 0x0600528D RID: 21133 RVA: 0x001BF184 File Offset: 0x001BD384
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
			Log.Error("Could not find good sleeping slot position for " + pawn + ". Perhaps AnyUnoccupiedSleepingSlot check is missing somewhere.", false);
			return bed.GetSleepingSlotPos(0);
		}

		// Token: 0x0600528E RID: 21134 RVA: 0x00039BA5 File Offset: 0x00037DA5
		public static bool CanUseBedEver(Pawn p, ThingDef bedDef)
		{
			return p.BodySize <= bedDef.building.bed_maxBodySize && p.RaceProps.Humanlike == bedDef.building.bed_humanlike;
		}

		// Token: 0x0600528F RID: 21135 RVA: 0x00039BD7 File Offset: 0x00037DD7
		public static bool TimetablePreventsLayDown(Pawn pawn)
		{
			return pawn.timetable != null && !pawn.timetable.CurrentAssignment.allowRest && pawn.needs.rest.CurLevel >= 0.2f;
		}

		// Token: 0x06005290 RID: 21136 RVA: 0x00039C0D File Offset: 0x00037E0D
		public static bool DisturbancePreventsLyingDown(Pawn pawn)
		{
			return !pawn.Downed && Find.TickManager.TicksGame - pawn.mindState.lastDisturbanceTick < 400;
		}

		// Token: 0x06005291 RID: 21137 RVA: 0x001BF260 File Offset: 0x001BD460
		public static bool Awake(this Pawn p)
		{
			return p.health.capacities.CanBeAwake && (!p.Spawned || p.CurJob == null || p.jobs.curDriver == null || !p.jobs.curDriver.asleep);
		}

		// Token: 0x06005292 RID: 21138 RVA: 0x001BF2B8 File Offset: 0x001BD4B8
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

		// Token: 0x06005293 RID: 21139 RVA: 0x00039C36 File Offset: 0x00037E36
		public static bool InBed(this Pawn p)
		{
			return p.CurrentBed() != null;
		}

		// Token: 0x06005294 RID: 21140 RVA: 0x001BF338 File Offset: 0x001BD538
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

		// Token: 0x06005295 RID: 21141 RVA: 0x001BF390 File Offset: 0x001BD590
		public static float WakeThreshold(Pawn p)
		{
			Lord lord = p.GetLord();
			if (lord != null && lord.CurLordToil != null && lord.CurLordToil.CustomWakeThreshold != null)
			{
				return lord.CurLordToil.CustomWakeThreshold.Value;
			}
			return 1f;
		}

		// Token: 0x06005296 RID: 21142 RVA: 0x00039C41 File Offset: 0x00037E41
		public static float FallAsleepMaxLevel(Pawn p)
		{
			return Mathf.Min(0.75f, RestUtility.WakeThreshold(p) - 0.01f);
		}

		// Token: 0x040034DD RID: 13533
		private static List<ThingDef> bedDefsBestToWorst_RestEffectiveness;

		// Token: 0x040034DE RID: 13534
		private static List<ThingDef> bedDefsBestToWorst_Medical;
	}
}
