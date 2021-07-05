using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008C1 RID: 2241
	public class LordToilData_Stage : LordToilData
	{
		// Token: 0x06003B1D RID: 15133 RVA: 0x0014A7E4 File Offset: 0x001489E4
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.stagingPoint, "stagingPoint", default(IntVec3), false);
		}

		// Token: 0x04002036 RID: 8246
		public IntVec3 stagingPoint;
	}
}
