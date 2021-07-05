using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200076C RID: 1900
	public abstract class JobGiver_Binge : ThinkNode_JobGiver
	{
		// Token: 0x06003469 RID: 13417 RVA: 0x0012949F File Offset: 0x0012769F
		protected bool IgnoreForbid(Pawn pawn)
		{
			return pawn.InMentalState;
		}

		// Token: 0x0600346A RID: 13418
		protected abstract int IngestInterval(Pawn pawn);

		// Token: 0x0600346B RID: 13419 RVA: 0x001294A8 File Offset: 0x001276A8
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

		// Token: 0x0600346C RID: 13420 RVA: 0x001294E4 File Offset: 0x001276E4
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

		// Token: 0x0600346D RID: 13421
		protected abstract Thing BestIngestTarget(Pawn pawn);
	}
}
