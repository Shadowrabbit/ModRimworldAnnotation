using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E10 RID: 3600
	public class LordToilData_Stage : LordToilData
	{
		// Token: 0x060051D5 RID: 20949 RVA: 0x001BCE14 File Offset: 0x001BB014
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.stagingPoint, "stagingPoint", default(IntVec3), false);
		}

		// Token: 0x0400346E RID: 13422
		public IntVec3 stagingPoint;
	}
}
