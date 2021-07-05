using System;

namespace Verse.AI.Group
{
	// Token: 0x0200066C RID: 1644
	public class LordToilData_DefendPoint : LordToilData
	{
		// Token: 0x06002EB4 RID: 11956 RVA: 0x00116E24 File Offset: 0x00115024
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.defendPoint, "defendPoint", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.defendRadius, "defendRadius", 28f, false);
			Scribe_Values.Look<float?>(ref this.wanderRadius, "wanderRadius", null, false);
		}

		// Token: 0x04001C9B RID: 7323
		public IntVec3 defendPoint = IntVec3.Invalid;

		// Token: 0x04001C9C RID: 7324
		public float defendRadius = 28f;

		// Token: 0x04001C9D RID: 7325
		public float? wanderRadius;
	}
}
