using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200112E RID: 4398
	public class CompProperties_FadesInOut : CompProperties
	{
		// Token: 0x060069BD RID: 27069 RVA: 0x0023A24F File Offset: 0x0023844F
		public CompProperties_FadesInOut()
		{
			this.compClass = typeof(CompFadesInOut);
		}

		// Token: 0x04003B0A RID: 15114
		public float fadeInSecs;

		// Token: 0x04003B0B RID: 15115
		public float fadeOutSecs;

		// Token: 0x04003B0C RID: 15116
		public float solidTimeSecs;
	}
}
