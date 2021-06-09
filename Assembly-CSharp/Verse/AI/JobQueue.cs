using System;
using System.Collections;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A0E RID: 2574
	public class JobQueue : IExposable, IEnumerable<QueuedJob>, IEnumerable
	{
		// Token: 0x170009B0 RID: 2480
		// (get) Token: 0x06003D82 RID: 15746 RVA: 0x0002E53A File Offset: 0x0002C73A
		public int Count
		{
			get
			{
				return this.jobs.Count;
			}
		}

		// Token: 0x170009B1 RID: 2481
		public QueuedJob this[int index]
		{
			get
			{
				return this.jobs[index];
			}
		}

		// Token: 0x170009B2 RID: 2482
		// (get) Token: 0x06003D84 RID: 15748 RVA: 0x00175A10 File Offset: 0x00173C10
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

		// Token: 0x06003D85 RID: 15749 RVA: 0x0002E555 File Offset: 0x0002C755
		public void ExposeData()
		{
			Scribe_Collections.Look<QueuedJob>(ref this.jobs, "jobs", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x06003D86 RID: 15750 RVA: 0x0002E56D File Offset: 0x0002C76D
		public void EnqueueFirst(Job j, JobTag? tag = null)
		{
			this.jobs.Insert(0, new QueuedJob(j, tag));
		}

		// Token: 0x06003D87 RID: 15751 RVA: 0x0002E582 File Offset: 0x0002C782
		public void EnqueueLast(Job j, JobTag? tag = null)
		{
			this.jobs.Add(new QueuedJob(j, tag));
		}

		// Token: 0x06003D88 RID: 15752 RVA: 0x00175A50 File Offset: 0x00173C50
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

		// Token: 0x06003D89 RID: 15753 RVA: 0x00175A8C File Offset: 0x00173C8C
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

		// Token: 0x06003D8A RID: 15754 RVA: 0x0002E596 File Offset: 0x0002C796
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

		// Token: 0x06003D8B RID: 15755 RVA: 0x0002E5BF File Offset: 0x0002C7BF
		public QueuedJob Peek()
		{
			return this.jobs[0];
		}

		// Token: 0x06003D8C RID: 15756 RVA: 0x00175ADC File Offset: 0x00173CDC
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

		// Token: 0x06003D8D RID: 15757 RVA: 0x0002E5CD File Offset: 0x0002C7CD
		public IEnumerator<QueuedJob> GetEnumerator()
		{
			return this.jobs.GetEnumerator();
		}

		// Token: 0x06003D8E RID: 15758 RVA: 0x0002E5CD File Offset: 0x0002C7CD
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.jobs.GetEnumerator();
		}

		// Token: 0x06003D8F RID: 15759 RVA: 0x0002E5DF File Offset: 0x0002C7DF
		public JobQueue Capture()
		{
			JobQueue jobQueue = new JobQueue();
			jobQueue.jobs.AddRange(this.jobs);
			return jobQueue;
		}

		// Token: 0x04002AB0 RID: 10928
		private List<QueuedJob> jobs = new List<QueuedJob>();
	}
}
