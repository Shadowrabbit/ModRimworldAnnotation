using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C55 RID: 3157
	public static class JobInBedUtility
	{
		// Token: 0x06004A2D RID: 18989 RVA: 0x0019FD7C File Offset: 0x0019DF7C
		public static void KeepLyingDown(this JobDriver driver, TargetIndex bedIndex)
		{
			driver.AddFinishAction(delegate
			{
				Pawn pawn = driver.pawn;
				if (!pawn.Drafted)
				{
					pawn.jobs.jobQueue.EnqueueFirst(JobMaker.MakeJob(JobDefOf.LayDown, pawn.CurJob.GetTarget(bedIndex)), null);
				}
			});
		}

		// Token: 0x06004A2E RID: 18990 RVA: 0x0019FDB4 File Offset: 0x0019DFB4
		public static bool InBedOrRestSpotNow(Pawn pawn, LocalTargetInfo bedOrRestSpot)
		{
			if (!bedOrRestSpot.IsValid || !pawn.Spawned)
			{
				return false;
			}
			if (bedOrRestSpot.HasThing)
			{
				return bedOrRestSpot.Thing.Map == pawn.Map && RestUtility.GetBedSleepingSlotPosFor(pawn, (Building_Bed)bedOrRestSpot.Thing) == pawn.Position;
			}
			return bedOrRestSpot.Cell == pawn.Position;
		}
	}
}
