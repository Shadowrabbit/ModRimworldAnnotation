using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007A7 RID: 1959
	public class JobGiver_StandAndBeSociallyActive : ThinkNode_JobGiver
	{
		// Token: 0x06003555 RID: 13653 RVA: 0x0012DB8F File Offset: 0x0012BD8F
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_StandAndBeSociallyActive jobGiver_StandAndBeSociallyActive = (JobGiver_StandAndBeSociallyActive)base.DeepCopy(resolve);
			jobGiver_StandAndBeSociallyActive.ticksRange = this.ticksRange;
			jobGiver_StandAndBeSociallyActive.lookDirection = this.lookDirection;
			return jobGiver_StandAndBeSociallyActive;
		}

		// Token: 0x06003556 RID: 13654 RVA: 0x0012DBB5 File Offset: 0x0012BDB5
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job job = JobMaker.MakeJob(JobDefOf.StandAndBeSociallyActive);
			job.expiryInterval = this.ticksRange.RandomInRange;
			job.lookDirection = this.lookDirection;
			return job;
		}

		// Token: 0x04001E8A RID: 7818
		public IntRange ticksRange = new IntRange(300, 600);

		// Token: 0x04001E8B RID: 7819
		public Direction8Way lookDirection;
	}
}
