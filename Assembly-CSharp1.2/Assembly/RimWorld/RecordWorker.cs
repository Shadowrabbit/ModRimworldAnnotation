using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000FCF RID: 4047
	public class RecordWorker
	{
		// Token: 0x06005875 RID: 22645 RVA: 0x001D0218 File Offset: 0x001CE418
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

		// Token: 0x04003A8F RID: 14991
		public RecordDef def;
	}
}
