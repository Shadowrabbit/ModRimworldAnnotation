using System;

namespace Verse.AI
{
	// Token: 0x02000A0D RID: 2573
	public class QueuedJob : IExposable
	{
		// Token: 0x06003D7F RID: 15743 RVA: 0x00006B8B File Offset: 0x00004D8B
		public QueuedJob()
		{
		}

		// Token: 0x06003D80 RID: 15744 RVA: 0x0002E524 File Offset: 0x0002C724
		public QueuedJob(Job job, JobTag? tag)
		{
			this.job = job;
			this.tag = tag;
		}

		// Token: 0x06003D81 RID: 15745 RVA: 0x001759D4 File Offset: 0x00173BD4
		public void ExposeData()
		{
			Scribe_Deep.Look<Job>(ref this.job, "job", Array.Empty<object>());
			Scribe_Values.Look<JobTag?>(ref this.tag, "tag", null, false);
		}

		// Token: 0x04002AAE RID: 10926
		public Job job;

		// Token: 0x04002AAF RID: 10927
		public JobTag? tag;
	}
}
