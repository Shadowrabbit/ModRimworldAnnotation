using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001388 RID: 5000
	public class CompAbilityEffect_PutToSleep : CompAbilityEffect
	{
		// Token: 0x06006C95 RID: 27797 RVA: 0x00215B50 File Offset: 0x00213D50
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				pawn.needs.rest.CurLevel = 0f;
				Job job = JobMaker.MakeJob(JobDefOf.LayDown, pawn.Position);
				job.forceSleep = true;
				pawn.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false);
			}
		}
	}
}
