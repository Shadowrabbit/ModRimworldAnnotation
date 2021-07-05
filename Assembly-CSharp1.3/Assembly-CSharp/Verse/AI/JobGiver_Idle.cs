using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000634 RID: 1588
	public class JobGiver_Idle : ThinkNode_JobGiver
	{
		// Token: 0x06002D60 RID: 11616 RVA: 0x0010FD14 File Offset: 0x0010DF14
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_Idle jobGiver_Idle = (JobGiver_Idle)base.DeepCopy(resolve);
			jobGiver_Idle.ticks = this.ticks;
			return jobGiver_Idle;
		}

		// Token: 0x06002D61 RID: 11617 RVA: 0x0010FD30 File Offset: 0x0010DF30
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job job = JobMaker.MakeJob(JobDefOf.Wait);
			job.expiryInterval = this.ticks;
			Pawn_MindState mindState = pawn.mindState;
			Rot4? rot;
			if (mindState == null)
			{
				rot = null;
			}
			else
			{
				PawnDuty duty = mindState.duty;
				rot = ((duty != null) ? new Rot4?(duty.overrideFacing) : null);
			}
			job.overrideFacing = (rot ?? Rot4.Invalid);
			return job;
		}

		// Token: 0x04001BD1 RID: 7121
		public int ticks = 50;
	}
}
