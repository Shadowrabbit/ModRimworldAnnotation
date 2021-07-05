using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D53 RID: 3411
	public class CompAbilityEffect_PutToSleep : CompAbilityEffect
	{
		// Token: 0x06004F80 RID: 20352 RVA: 0x001AA3AC File Offset: 0x001A85AC
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
