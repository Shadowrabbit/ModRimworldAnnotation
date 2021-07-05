using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AB1 RID: 2737
	public class RecordWorker
	{
		// Token: 0x060040F9 RID: 16633 RVA: 0x0015E7A4 File Offset: 0x0015C9A4
		public virtual bool ShouldMeasureTimeNow(Pawn pawn)
		{
			if (this.def.measuredTimeJobs == null)
			{
				return false;
			}
			Job curJob = pawn.CurJob;
			if (curJob == null)
			{
				return false;
			}
			for (int i = 0; i < this.def.measuredTimeJobs.Count; i++)
			{
				if (curJob.def == this.def.measuredTimeJobs[i])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04002651 RID: 9809
		public RecordDef def;
	}
}
