using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A20 RID: 2592
	public class GasProperties
	{
		// Token: 0x04002239 RID: 8761
		public bool blockTurretTracking;

		// Token: 0x0400223A RID: 8762
		public float accuracyPenalty;

		// Token: 0x0400223B RID: 8763
		public FloatRange expireSeconds = new FloatRange(30f, 30f);

		// Token: 0x0400223C RID: 8764
		public float rotationSpeed;
	}
}
