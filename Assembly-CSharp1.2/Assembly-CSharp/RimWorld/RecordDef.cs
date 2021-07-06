using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FCD RID: 4045
	public class RecordDef : Def
	{
		// Token: 0x17000DA8 RID: 3496
		// (get) Token: 0x06005873 RID: 22643 RVA: 0x0003D76A File Offset: 0x0003B96A
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

		// Token: 0x04003A87 RID: 14983
		public RecordType type;

		// Token: 0x04003A88 RID: 14984
		public Type workerClass = typeof(RecordWorker);

		// Token: 0x04003A89 RID: 14985
		public List<JobDef> measuredTimeJobs;

		// Token: 0x04003A8A RID: 14986
		[Unsaved(false)]
		private RecordWorker workerInt;
	}
}
