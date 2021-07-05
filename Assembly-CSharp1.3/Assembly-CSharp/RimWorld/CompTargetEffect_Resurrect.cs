using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020011F1 RID: 4593
	public class CompTargetEffect_Resurrect : CompTargetEffect
	{
		// Token: 0x06006E8D RID: 28301 RVA: 0x002506BC File Offset: 0x0024E8BC
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
			user.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
		}
	}
}
