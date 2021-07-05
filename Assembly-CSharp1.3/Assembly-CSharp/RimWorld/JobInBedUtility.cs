using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000756 RID: 1878
	public static class JobInBedUtility
	{
		// Token: 0x0600340D RID: 13325 RVA: 0x00127520 File Offset: 0x00125720
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

		// Token: 0x0600340E RID: 13326 RVA: 0x00127558 File Offset: 0x00125758
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
