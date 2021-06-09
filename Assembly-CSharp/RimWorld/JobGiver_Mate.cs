﻿using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C77 RID: 3191
	public class JobGiver_Mate : ThinkNode_JobGiver
	{
		// Token: 0x06004AB0 RID: 19120 RVA: 0x001A27E0 File Offset: 0x001A09E0
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.gender != Gender.Male || !pawn.ageTracker.CurLifeStage.reproductive)
			{
				return null;
			}
			Predicate<Thing> validator = delegate(Thing t)
			{
				Pawn pawn3 = t as Pawn;
				return !pawn3.Downed && pawn3.CanCasuallyInteractNow(false) && !pawn3.IsForbidden(pawn) && pawn3.Faction == pawn.Faction && PawnUtility.FertileMateTarget(pawn, pawn3);
			};
			Pawn pawn2 = (Pawn)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(pawn.def), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 30f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
			if (pawn2 == null)
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Mate, pawn2);
		}
	}
}
