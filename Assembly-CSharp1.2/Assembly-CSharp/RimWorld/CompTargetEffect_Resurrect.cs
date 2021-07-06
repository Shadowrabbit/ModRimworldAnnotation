using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020018CF RID: 6351
	public class CompTargetEffect_Resurrect : CompTargetEffect
	{
		// Token: 0x06008CC3 RID: 36035 RVA: 0x0028DA0C File Offset: 0x0028BC0C
		public override void DoEffectOn(Pawn user, Thing target)
		{
			if (!user.IsColonistPlayerControlled)
			{
				return;
			}
			if (!user.CanReserveAndReach(target, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
			{
				return;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Resurrect, target, this.parent);
			job.count = 1;
			user.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}
	}
}
