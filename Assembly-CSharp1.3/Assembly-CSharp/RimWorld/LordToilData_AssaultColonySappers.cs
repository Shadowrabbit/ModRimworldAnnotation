using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008A8 RID: 2216
	public class LordToilData_AssaultColonySappers : LordToilData
	{
		// Token: 0x06003AAA RID: 15018 RVA: 0x00148454 File Offset: 0x00146654
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.sapperDest, "sapperDest", default(IntVec3), false);
		}

		// Token: 0x04002014 RID: 8212
		public IntVec3 sapperDest = IntVec3.Invalid;
	}
}
