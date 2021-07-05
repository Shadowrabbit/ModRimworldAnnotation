using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008B7 RID: 2231
	public class LordToilData_HuntEnemies : LordToilData
	{
		// Token: 0x06003AE8 RID: 15080 RVA: 0x001494AC File Offset: 0x001476AC
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.fallbackLocation, "fallbackLocation", IntVec3.Invalid, false);
		}

		// Token: 0x04002022 RID: 8226
		public IntVec3 fallbackLocation = IntVec3.Invalid;
	}
}
