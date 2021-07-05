﻿using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007E1 RID: 2017
	public class JoyGiver_GoForWalk : JoyGiver
	{
		// Token: 0x0600361E RID: 13854 RVA: 0x00132488 File Offset: 0x00130688
		public override Job TryGiveJob(Pawn pawn)
		{
			if (!JoyUtility.EnjoyableOutsideNow(pawn, null))
			{
				return null;
			}
			if (PawnUtility.WillSoonHaveBasicNeed(pawn))
			{
				return null;
			}
			Predicate<IntVec3> cellValidator = (IntVec3 x) => !PawnUtility.KnownDangerAt(x, pawn.Map, pawn) && !x.GetTerrain(pawn.Map).avoidWander && x.Standable(pawn.Map) && !x.Roofed(pawn.Map);
			Predicate<Region> validator = delegate(Region x)
			{
				IntVec3 intVec;
				return x.Room.PsychologicallyOutdoors && !x.IsForbiddenEntirely(pawn) && x.TryFindRandomCellInRegionUnforbidden(pawn, cellValidator, out intVec);
			};
			Region reg;
			if (!CellFinder.TryFindClosestRegionWith(pawn.GetRegion(RegionType.Set_Passable), TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), validator, 100, out reg, RegionType.Set_Passable))
			{
				return null;
			}
			IntVec3 root;
			if (!reg.TryFindRandomCellInRegionUnforbidden(pawn, cellValidator, out root))
			{
				return null;
			}
			List<IntVec3> list;
			if (!WalkPathFinder.TryFindWalkPath(pawn, root, out list))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(this.def.jobDef, list[0]);
			job.targetQueueA = new List<LocalTargetInfo>();
			for (int i = 1; i < list.Count; i++)
			{
				job.targetQueueA.Add(list[i]);
			}
			job.locomotionUrgency = LocomotionUrgency.Walk;
			return job;
		}
	}
}
