using System;

namespace Verse.AI.Group
{
	// Token: 0x02000ADA RID: 2778
	public class LordToilData_Travel : LordToilData
	{
		// Token: 0x060041B3 RID: 16819 RVA: 0x001882A8 File Offset: 0x001864A8
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.dest, "dest", default(IntVec3), false);
		}

		// Token: 0x04002D35 RID: 11573
		public IntVec3 dest;
	}
}
