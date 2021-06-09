using System;

namespace Verse.AI.Group
{
	// Token: 0x02000AD1 RID: 2769
	public class LordToilData_DefendPoint : LordToilData
	{
		// Token: 0x06004191 RID: 16785 RVA: 0x00187F04 File Offset: 0x00186104
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.defendPoint, "defendPoint", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.defendRadius, "defendRadius", 28f, false);
			Scribe_Values.Look<float?>(ref this.wanderRadius, "wanderRadius", null, false);
		}

		// Token: 0x04002D2A RID: 11562
		public IntVec3 defendPoint = IntVec3.Invalid;

		// Token: 0x04002D2B RID: 11563
		public float defendRadius = 28f;

		// Token: 0x04002D2C RID: 11564
		public float? wanderRadius;
	}
}
