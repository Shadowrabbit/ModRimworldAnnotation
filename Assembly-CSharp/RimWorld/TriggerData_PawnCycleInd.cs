using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E32 RID: 3634
	public class TriggerData_PawnCycleInd : TriggerData
	{
		// Token: 0x06005265 RID: 21093 RVA: 0x000399FC File Offset: 0x00037BFC
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.pawnCycleInd, "pawnCycleInd", 0, false);
		}

		// Token: 0x040034CC RID: 13516
		public int pawnCycleInd;
	}
}
