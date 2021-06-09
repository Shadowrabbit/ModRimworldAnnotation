using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CB7 RID: 3255
	public class JobGiver_StandAndBeSociallyActive : ThinkNode_JobGiver
	{
		// Token: 0x06004B7F RID: 19327 RVA: 0x00035CEB File Offset: 0x00033EEB
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_StandAndBeSociallyActive jobGiver_StandAndBeSociallyActive = (JobGiver_StandAndBeSociallyActive)base.DeepCopy(resolve);
			jobGiver_StandAndBeSociallyActive.ticksRange = this.ticksRange;
			return jobGiver_StandAndBeSociallyActive;
		}

		// Token: 0x06004B80 RID: 19328 RVA: 0x00035D05 File Offset: 0x00033F05
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job job = JobMaker.MakeJob(JobDefOf.StandAndBeSociallyActive);
			job.expiryInterval = this.ticksRange.RandomInRange;
			return job;
		}

		// Token: 0x040031D9 RID: 12761
		public IntRange ticksRange = new IntRange(300, 600);
	}
}
