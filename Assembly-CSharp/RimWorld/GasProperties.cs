using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F31 RID: 3889
	public class GasProperties
	{
		// Token: 0x040036DC RID: 14044
		public bool blockTurretTracking;

		// Token: 0x040036DD RID: 14045
		public float accuracyPenalty;

		// Token: 0x040036DE RID: 14046
		public FloatRange expireSeconds = new FloatRange(30f, 30f);

		// Token: 0x040036DF RID: 14047
		public float rotationSpeed;
	}
}
