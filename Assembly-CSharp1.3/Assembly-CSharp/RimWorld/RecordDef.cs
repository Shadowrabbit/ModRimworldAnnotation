using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AAF RID: 2735
	public class RecordDef : Def
	{
		// Token: 0x17000B68 RID: 2920
		// (get) Token: 0x060040F7 RID: 16631 RVA: 0x0015E75A File Offset: 0x0015C95A
		public RecordWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (RecordWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x04002649 RID: 9801
		public RecordType type;

		// Token: 0x0400264A RID: 9802
		public Type workerClass = typeof(RecordWorker);

		// Token: 0x0400264B RID: 9803
		public List<JobDef> measuredTimeJobs;

		// Token: 0x0400264C RID: 9804
		[Unsaved(false)]
		private RecordWorker workerInt;
	}
}
