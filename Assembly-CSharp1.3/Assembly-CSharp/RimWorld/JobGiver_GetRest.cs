using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007C2 RID: 1986
	public class JobGiver_GetRest : ThinkNode_JobGiver
	{
		// Token: 0x060035A5 RID: 13733 RVA: 0x0012F394 File Offset: 0x0012D594
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_GetRest jobGiver_GetRest = (JobGiver_GetRest)base.DeepCopy(resolve);
			jobGiver_GetRest.minCategory = this.minCategory;
			jobGiver_GetRest.maxLevelPercentage = this.maxLevelPercentage;
			return jobGiver_GetRest;
		}

		// Token: 0x060035A6 RID: 13734 RVA: 0x0012F3BC File Offset: 0x0012D5BC
		public override float GetPriority(Pawn pawn)
		{
			Need_Rest rest = pawn.needs.rest;
			if (rest == null)
			{
				return 0f;
			}
			if (rest.CurCategory < this.minCategory)
			{
				return 0f;
			}
			if (rest.CurLevelPercentage > this.maxLevelPercentage)
			{
				return 0f;
			}
			if (Find.TickManager.TicksGame < pawn.mindState.canSleepTick)
			{
				return 0f;
			}
			Lord lord = pawn.GetLord();
			if (lord != null && !lord.CurLordToil.AllowSatisfyLongNeeds)
			{
				return 0f;
			}
			TimeAssignmentDef timeAssignmentDef;
			if (pawn.RaceProps.Humanlike)
			{
				timeAssignmentDef = ((pawn.timetable == null) ? TimeAssignmentDefOf.Anything : pawn.timetable.CurrentAssignment);
			}
			else
			{
				int num = GenLocalDate.HourOfDay(pawn);
				if (num < 7 || num > 21)
				{
					timeAssignmentDef = TimeAssignmentDefOf.Sleep;
				}
				else
				{
					timeAssignmentDef = TimeAssignmentDefOf.Anything;
				}
			}
			float curLevel = rest.CurLevel;
			if (timeAssignmentDef == TimeAssignmentDefOf.Anything)
			{
				if (curLevel < 0.3f)
				{
					return 8f;
				}
				return 0f;
			}
			else
			{
				if (timeAssignmentDef == TimeAssignmentDefOf.Work)
				{
					return 0f;
				}
				if (timeAssignmentDef == TimeAssignmentDefOf.Meditate)
				{
					if (curLevel < 0.16f)
					{
						return 8f;
					}
					return 0f;
				}
				else if (timeAssignmentDef == TimeAssignmentDefOf.Joy)
				{
					if (curLevel < 0.3f)
					{
						return 8f;
					}
					return 0f;
				}
				else
				{
					if (timeAssignmentDef != TimeAssignmentDefOf.Sleep)
					{
						throw new NotImplementedException();
					}
					if (curLevel < RestUtility.FallAsleepMaxLevel(pawn))
					{
						return 8f;
					}
					return 0f;
				}
			}
		}

		// Token: 0x060035A7 RID: 13735 RVA: 0x0012F518 File Offset: 0x0012D718
		protected override Job TryGiveJob(Pawn pawn)
		{
			Need_Rest rest = pawn.needs.rest;
			if (rest == null || rest.CurCategory < this.minCategory || rest.CurLevelPercentage > this.maxLevelPercentage)
			{
				return null;
			}
			if (RestUtility.DisturbancePreventsLyingDown(pawn))
			{
				return null;
			}
			Lord lord = pawn.GetLord();
			Building_Bed building_Bed;
			if ((lord != null && lord.CurLordToil != null && !lord.CurLordToil.AllowRestingInBed) || pawn.IsWildMan() || (pawn.InMentalState && !pawn.MentalState.AllowRestingInBed))
			{
				building_Bed = null;
			}
			else
			{
				building_Bed = RestUtility.FindBedFor(pawn);
			}
			if (building_Bed != null)
			{
				return JobMaker.MakeJob(JobDefOf.LayDown, building_Bed);
			}
			return JobMaker.MakeJob(JobDefOf.LayDown, this.FindGroundSleepSpotFor(pawn));
		}

		// Token: 0x060035A8 RID: 13736 RVA: 0x0012F5D0 File Offset: 0x0012D7D0
		private IntVec3 FindGroundSleepSpotFor(Pawn pawn)
		{
			Map map = pawn.Map;
			Predicate<IntVec3> <>9__0;
			for (int i = 0; i < 2; i++)
			{
				int num = (i == 0) ? 4 : 12;
				IntVec3 position = pawn.Position;
				Map map2 = map;
				int radius = num;
				Predicate<IntVec3> extraValidator;
				if ((extraValidator = <>9__0) == null)
				{
					extraValidator = (<>9__0 = ((IntVec3 x) => !x.IsForbidden(pawn) && !x.GetTerrain(map).avoidWander));
				}
				IntVec3 result;
				if (CellFinder.TryRandomClosewalkCellNear(position, map2, radius, out result, extraValidator))
				{
					return result;
				}
			}
			return CellFinder.RandomClosewalkCellNearNotForbidden(pawn, 4);
		}

		// Token: 0x04001EAB RID: 7851
		private RestCategory minCategory;

		// Token: 0x04001EAC RID: 7852
		private float maxLevelPercentage = 1f;
	}
}
