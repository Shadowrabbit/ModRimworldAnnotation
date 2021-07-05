using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C84 RID: 3204
	public abstract class JobGiver_Binge : ThinkNode_JobGiver
	{
		// Token: 0x06004AD3 RID: 19155 RVA: 0x000357AF File Offset: 0x000339AF
		protected bool IgnoreForbid(Pawn pawn)
		{
			return pawn.InMentalState;
		}

		// Token: 0x06004AD4 RID: 19156
		protected abstract int IngestInterval(Pawn pawn);

		// Token: 0x06004AD5 RID: 19157 RVA: 0x001A33CC File Offset: 0x001A15CC
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (Find.TickManager.TicksGame - pawn.mindState.lastIngestTick > this.IngestInterval(pawn))
			{
				Job job = this.IngestJob(pawn);
				if (job != null)
				{
					return job;
				}
			}
			return null;
		}

		// Token: 0x06004AD6 RID: 19158 RVA: 0x001A3408 File Offset: 0x001A1608
		private Job IngestJob(Pawn pawn)
		{
			Thing thing = this.BestIngestTarget(pawn);
			if (thing == null)
			{
				return null;
			}
			ThingDef finalIngestibleDef = FoodUtility.GetFinalIngestibleDef(thing, false);
			Job job = JobMaker.MakeJob(JobDefOf.Ingest, thing);
			job.count = finalIngestibleDef.ingestible.maxNumToIngestAtOnce;
			job.ignoreForbidden = this.IgnoreForbid(pawn);
			job.overeat = true;
			return job;
		}

		// Token: 0x06004AD7 RID: 19159
		protected abstract Thing BestIngestTarget(Pawn pawn);
	}
}
