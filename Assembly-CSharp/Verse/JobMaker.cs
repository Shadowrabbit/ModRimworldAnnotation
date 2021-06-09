using System;
using Verse.AI;

namespace Verse
{
	// Token: 0x020000AA RID: 170
	public static class JobMaker
	{
		// Token: 0x0600056B RID: 1387 RVA: 0x0000A8E7 File Offset: 0x00008AE7
		public static Job MakeJob()
		{
			Job job = SimplePool<Job>.Get();
			job.loadID = Find.UniqueIDsManager.GetNextJobID();
			return job;
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x0000A8FE File Offset: 0x00008AFE
		public static Job MakeJob(JobDef def)
		{
			Job job = JobMaker.MakeJob();
			job.def = def;
			return job;
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x0000A90C File Offset: 0x00008B0C
		public static Job MakeJob(JobDef def, LocalTargetInfo targetA)
		{
			Job job = JobMaker.MakeJob();
			job.def = def;
			job.targetA = targetA;
			return job;
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x0000A921 File Offset: 0x00008B21
		public static Job MakeJob(JobDef def, LocalTargetInfo targetA, LocalTargetInfo targetB)
		{
			Job job = JobMaker.MakeJob();
			job.def = def;
			job.targetA = targetA;
			job.targetB = targetB;
			return job;
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x0000A93D File Offset: 0x00008B3D
		public static Job MakeJob(JobDef def, LocalTargetInfo targetA, LocalTargetInfo targetB, LocalTargetInfo targetC)
		{
			Job job = JobMaker.MakeJob();
			job.def = def;
			job.targetA = targetA;
			job.targetB = targetB;
			job.targetC = targetC;
			return job;
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x0000A960 File Offset: 0x00008B60
		public static Job MakeJob(JobDef def, LocalTargetInfo targetA, int expiryInterval, bool checkOverrideOnExpiry = false)
		{
			Job job = JobMaker.MakeJob();
			job.def = def;
			job.targetA = targetA;
			job.expiryInterval = expiryInterval;
			job.checkOverrideOnExpire = checkOverrideOnExpiry;
			return job;
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x0000A983 File Offset: 0x00008B83
		public static Job MakeJob(JobDef def, int expiryInterval, bool checkOverrideOnExpiry = false)
		{
			Job job = JobMaker.MakeJob();
			job.def = def;
			job.expiryInterval = expiryInterval;
			job.checkOverrideOnExpire = checkOverrideOnExpiry;
			return job;
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0000A99F File Offset: 0x00008B9F
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

		// Token: 0x040002AD RID: 685
		private const int MaxJobPoolSize = 1000;
	}
}
