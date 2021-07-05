using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DED RID: 3565
	public class LordToilData_AssaultColonySappers : LordToilData
	{
		// Token: 0x0600513E RID: 20798 RVA: 0x001BAD54 File Offset: 0x001B8F54
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.sapperDest, "sapperDest", default(IntVec3), false);
		}

		// Token: 0x04003431 RID: 13361
		public IntVec3 sapperDest = IntVec3.Invalid;
	}
}
