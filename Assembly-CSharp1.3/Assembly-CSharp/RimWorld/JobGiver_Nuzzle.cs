using System;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000766 RID: 1894
	public class JobGiver_Nuzzle : ThinkNode_JobGiver
	{
		// Token: 0x06003457 RID: 13399 RVA: 0x00128BAC File Offset: 0x00126DAC
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.RaceProps.nuzzleMtbHours <= 0f)
			{
				return null;
			}
			Pawn t;
			if (!(from p in pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction)
			where !p.NonHumanlikeOrWildMan() && p != pawn && p.Position.InHorDistOf(pawn.Position, 40f) && pawn.GetRoom(RegionType.Set_All) == p.GetRoom(RegionType.Set_All) && !p.Position.IsForbidden(pawn) && p.CanCasuallyInteractNow(false, false)
			select p).TryRandomElement(out t))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Nuzzle, t);
			job.locomotionUrgency = LocomotionUrgency.Walk;
			job.expiryInterval = 3000;
			return job;
		}

		// Token: 0x04001E44 RID: 7748
		private const float MaxNuzzleDistance = 40f;
	}
}
