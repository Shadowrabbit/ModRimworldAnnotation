using System;
using System.Collections;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x020005BC RID: 1468
	public class JobQueue : IExposable, IEnumerable<QueuedJob>, IEnumerable
	{
		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x06002AD0 RID: 10960 RVA: 0x00100D70 File Offset: 0x000FEF70
		public int Count
		{
			get
			{
				return this.jobs.Count;
			}
		}

		// Token: 0x1700084D RID: 2125
		public QueuedJob this[int index]
		{
			get
			{
				return this.jobs[index];
			}
		}

		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x06002AD2 RID: 10962 RVA: 0x00100D8C File Offset: 0x000FEF8C
		public bool AnyPlayerForced
		{
			get
			{
				for (int i = 0; i < this.jobs.Count; i++)
				{
					if (this.jobs[i].job.playerForced)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06002AD3 RID: 10963 RVA: 0x00100DCA File Offset: 0x000FEFCA
		public void ExposeData()
		{
			Scribe_Collections.Look<QueuedJob>(ref this.jobs, "jobs", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x06002AD4 RID: 10964 RVA: 0x00100DE2 File Offset: 0x000FEFE2
		public void EnqueueFirst(Job j, JobTag? tag = null)
		{
			this.jobs.Insert(0, new QueuedJob(j, tag));
		}

		// Token: 0x06002AD5 RID: 10965 RVA: 0x00100DF7 File Offset: 0x000FEFF7
		public void EnqueueLast(Job j, JobTag? tag = null)
		{
			this.jobs.Add(new QueuedJob(j, tag));
		}

		// Token: 0x06002AD6 RID: 10966 RVA: 0x00100E0C File Offset: 0x000FF00C
		public bool Contains(Job j)
		{
			for (int i = 0; i < this.jobs.Count; i++)
			{
				if (this.jobs[i].job == j)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002AD7 RID: 10967 RVA: 0x00100E48 File Offset: 0x000FF048
		public QueuedJob Extract(Job j)
		{
			int num = this.jobs.FindIndex((QueuedJob qj) => qj.job == j);
			if (num >= 0)
			{
				QueuedJob result = this.jobs[num];
				this.jobs.RemoveAt(num);
				return result;
			}
			return null;
		}

		// Token: 0x06002AD8 RID: 10968 RVA: 0x00100E98 File Offset: 0x000FF098
		public QueuedJob Dequeue()
		{
			if (this.jobs.NullOrEmpty<QueuedJob>())
			{
				return null;
			}
			QueuedJob result = this.jobs[0];
			this.jobs.RemoveAt(0);
			return result;
		}

		// Token: 0x06002AD9 RID: 10969 RVA: 0x00100EC1 File Offset: 0x000FF0C1
		public QueuedJob Peek()
		{
			return this.jobs[0];
		}

		// Token: 0x06002ADA RID: 10970 RVA: 0x00100ED0 File Offset: 0x000FF0D0
		public bool AnyCanBeginNow(Pawn pawn, bool whileLyingDown)
		{
			for (int i = 0; i < this.jobs.Count; i++)
			{
				if (this.jobs[i].job.CanBeginNow(pawn, whileLyingDown))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002ADB RID: 10971 RVA: 0x00100F10 File Offset: 0x000FF110
		public IEnumerator<QueuedJob> GetEnumerator()
		{
			return this.jobs.GetEnumerator();
		}

		// Token: 0x06002ADC RID: 10972 RVA: 0x00100F10 File Offset: 0x000FF110
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.jobs.GetEnumerator();
		}

		// Token: 0x06002ADD RID: 10973 RVA: 0x00100F22 File Offset: 0x000FF122
		public JobQueue Capture()
		{
			JobQueue jobQueue = new JobQueue();
			jobQueue.jobs.AddRange(this.jobs);
			return jobQueue;
		}

		// Token: 0x04001A54 RID: 6740
		private List<QueuedJob> jobs = new List<QueuedJob>();
	}
}
