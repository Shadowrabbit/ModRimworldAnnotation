using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E01 RID: 3585
	public class LordToilData_HuntEnemies : LordToilData
	{
		// Token: 0x0600518B RID: 20875 RVA: 0x00039140 File Offset: 0x00037340
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.fallbackLocation, "fallbackLocation", IntVec3.Invalid, false);
		}

		// Token: 0x04003448 RID: 13384
		public IntVec3 fallbackLocation = IntVec3.Invalid;
	}
}
