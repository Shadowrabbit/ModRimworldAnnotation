using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017BC RID: 6076
	public class CompProperties_FadesInOut : CompProperties
	{
		// Token: 0x0600866B RID: 34411 RVA: 0x0005A287 File Offset: 0x00058487
		public CompProperties_FadesInOut()
		{
			this.compClass = typeof(CompFadesInOut);
		}

		// Token: 0x04005686 RID: 22150
		public float fadeInSecs;

		// Token: 0x04005687 RID: 22151
		public float fadeOutSecs;

		// Token: 0x04005688 RID: 22152
		public float solidTimeSecs;
	}
}
