using System;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C79 RID: 3193
	public class JobGiver_Nuzzle : ThinkNode_JobGiver
	{
		// Token: 0x06004AB4 RID: 19124 RVA: 0x001A28F0 File Offset: 0x001A0AF0
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.RaceProps.nuzzleMtbHours <= 0f)
			{
				return null;
			}
			Pawn t;
			if (!(from p in pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction)
			where !p.NonHumanlikeOrWildMan() && p != pawn && p.Position.InHorDistOf(pawn.Position, 40f) && pawn.GetRoom(RegionType.Set_Passable) == p.GetRoom(RegionType.Set_Passable) && !p.Position.IsForbidden(pawn) && p.CanCasuallyInteractNow(false)
			select p).TryRandomElement(out t))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Nuzzle, t);
			job.locomotionUrgency = LocomotionUrgency.Walk;
			job.expiryInterval = 3000;
			return job;
		}

		// Token: 0x04003185 RID: 12677
		private const float MaxNuzzleDistance = 40f;
	}
}
