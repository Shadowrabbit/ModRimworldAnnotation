using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008E5 RID: 2277
	public class TriggerData_PawnCycleInd : TriggerData
	{
		// Token: 0x06003BA9 RID: 15273 RVA: 0x0014C8CE File Offset: 0x0014AACE
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.pawnCycleInd, "pawnCycleInd", 0, false);
		}

		// Token: 0x04002074 RID: 8308
		public int pawnCycleInd;
	}
}
