using System;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000062 RID: 98
	public static class JobMaker
	{
		// Token: 0x06000412 RID: 1042 RVA: 0x00015C3B File Offset: 0x00013E3B
		public static Job MakeJob()
		{
			Job job = SimplePool<Job>.Get();
			job.loadID = Find.UniqueIDsManager.GetNextJobID();
			return job;
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00015C52 File Offset: 0x00013E52
		public static Job MakeJob(JobDef def)
		{
			Job job = JobMaker.MakeJob();
			job.def = def;
			return job;
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x00015C60 File Offset: 0x00013E60
		public static Job MakeJob(JobDef def, LocalTargetInfo targetA)
		{
			Job job = JobMaker.MakeJob();
			job.def = def;
			job.targetA = targetA;
			return job;
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x00015C75 File Offset: 0x00013E75
		public static Job MakeJob(JobDef def, LocalTargetInfo targetA, LocalTargetInfo targetB)
		{
			Job job = JobMaker.MakeJob();
			job.def = def;
			job.targetA = targetA;
			job.targetB = targetB;
			return job;
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00015C91 File Offset: 0x00013E91
		public static Job MakeJob(JobDef def, LocalTargetInfo targetA, LocalTargetInfo targetB, LocalTargetInfo targetC)
		{
			Job job = JobMaker.MakeJob();
			job.def = def;
			job.targetA = targetA;
			job.targetB = targetB;
			job.targetC = targetC;
			return job;
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x00015CB4 File Offset: 0x00013EB4
		public static Job MakeJob(JobDef def, LocalTargetInfo targetA, int expiryInterval, bool checkOverrideOnExpiry = false)
		{
			Job job = JobMaker.MakeJob();
			job.def = def;
			job.targetA = targetA;
			job.expiryInterval = expiryInterval;
			job.checkOverrideOnExpire = checkOverrideOnExpiry;
			return job;
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00015CD7 File Offset: 0x00013ED7
		public static Job MakeJob(JobDef def, int expiryInterval, bool checkOverrideOnExpiry = false)
		{
			Job job = JobMaker.MakeJob();
			job.def = def;
			job.expiryInterval = expiryInterval;
			job.checkOverrideOnExpire = checkOverrideOnExpiry;
			return job;
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x00015CF3 File Offset: 0x00013EF3
		public static void ReturnToPool(Job job)
		{
			if (job == null)
			{
				return;
			}
			if (SimplePool<Job>.FreeItemsCount >= 1000)
			{
				return;
			}
			job.Clear();
			SimplePool<Job>.Return(job);
		}

		// Token: 0x04000140 RID: 320
		private const int MaxJobPoolSize = 1000;
	}
}
