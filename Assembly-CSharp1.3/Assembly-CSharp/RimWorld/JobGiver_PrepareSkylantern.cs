using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007A2 RID: 1954
	public class JobGiver_PrepareSkylantern : JobGiver_GotoAndStandSociallyActive
	{
		// Token: 0x06003549 RID: 13641 RVA: 0x0012D7E4 File Offset: 0x0012B9E4
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 dest = base.GetDest(pawn);
			Job job = JobMaker.MakeJob(JobDefOf.PrepareSkylantern, dest);
			job.locomotionUrgency = this.locomotionUrgency;
			job.expiryInterval = this.expiryInterval;
			job.checkOverrideOnExpire = true;
			job.thingDefToCarry = this.def;
			job.count = this.count;
			return job;
		}

		// Token: 0x0600354A RID: 13642 RVA: 0x0012D840 File Offset: 0x0012BA40
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_PrepareSkylantern jobGiver_PrepareSkylantern = (JobGiver_PrepareSkylantern)base.DeepCopy(resolve);
			jobGiver_PrepareSkylantern.def = this.def;
			jobGiver_PrepareSkylantern.count = this.count;
			return jobGiver_PrepareSkylantern;
		}

		// Token: 0x04001E88 RID: 7816
		public ThingDef def;

		// Token: 0x04001E89 RID: 7817
		public int count = 1;
	}
}
