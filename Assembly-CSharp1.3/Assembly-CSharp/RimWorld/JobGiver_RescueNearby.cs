using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007CF RID: 1999
	public class JobGiver_RescueNearby : ThinkNode_JobGiver
	{
		// Token: 0x060035D2 RID: 13778 RVA: 0x00130E18 File Offset: 0x0012F018
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_RescueNearby jobGiver_RescueNearby = (JobGiver_RescueNearby)base.DeepCopy(resolve);
			jobGiver_RescueNearby.radius = this.radius;
			return jobGiver_RescueNearby;
		}

		// Token: 0x060035D3 RID: 13779 RVA: 0x00130E34 File Offset: 0x0012F034
		protected override Job TryGiveJob(Pawn pawn)
		{
			Predicate<Thing> validator = delegate(Thing t)
			{
				Pawn pawn3 = (Pawn)t;
				return pawn3.Downed && pawn3.Faction == pawn.Faction && !pawn3.InBed() && pawn.CanReserve(pawn3, 1, -1, null, false) && !pawn3.IsForbidden(pawn) && !GenAI.EnemyIsNear(pawn3, 25f) && !pawn.ShouldBeSlaughtered();
			};
			Pawn pawn2 = (Pawn)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), this.radius, validator, null, 0, -1, false, RegionType.Set_Passable, false);
			if (pawn2 == null)
			{
				return null;
			}
			Building_Bed building_Bed = RestUtility.FindBedFor(pawn2, pawn, false, false, pawn2.GuestStatus);
			if (building_Bed == null || !pawn2.CanReserve(building_Bed, 1, -1, null, false))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Rescue, pawn2, building_Bed);
			job.count = 1;
			return job;
		}

		// Token: 0x04001EC2 RID: 7874
		private float radius = 30f;

		// Token: 0x04001EC3 RID: 7875
		private const float MinDistFromEnemy = 25f;
	}
}
