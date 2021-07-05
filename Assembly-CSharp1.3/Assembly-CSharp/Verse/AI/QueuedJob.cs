using System;

namespace Verse.AI
{
	// Token: 0x020005BB RID: 1467
	public class QueuedJob : IExposable
	{
		// Token: 0x06002ACD RID: 10957 RVA: 0x000033AC File Offset: 0x000015AC
		public QueuedJob()
		{
		}

		// Token: 0x06002ACE RID: 10958 RVA: 0x00100D1B File Offset: 0x000FEF1B
		public QueuedJob(Job job, JobTag? tag)
		{
			this.job = job;
			this.tag = tag;
		}

		// Token: 0x06002ACF RID: 10959 RVA: 0x00100D34 File Offset: 0x000FEF34
		public void ExposeData()
		{
			Scribe_Deep.Look<Job>(ref this.job, "job", Array.Empty<object>());
			Scribe_Values.Look<JobTag?>(ref this.tag, "tag", null, false);
		}

		// Token: 0x04001A52 RID: 6738
		public Job job;

		// Token: 0x04001A53 RID: 6739
		public JobTag? tag;
	}
}
