using System;

namespace Verse.AI.Group
{
	// Token: 0x02000675 RID: 1653
	public class LordToilData_Travel : LordToilData
	{
		// Token: 0x06002ED6 RID: 11990 RVA: 0x00117310 File Offset: 0x00115510
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.dest, "dest", default(IntVec3), false);
		}

		// Token: 0x04001CA6 RID: 7334
		public IntVec3 dest;
	}
}
