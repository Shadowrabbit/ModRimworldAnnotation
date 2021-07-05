using System;

namespace RimWorld
{
	// Token: 0x02000A02 RID: 2562
	public class CompProperties_FireOverlayRitual : CompProperties_FireOverlay
	{
		// Token: 0x06003EEE RID: 16110 RVA: 0x00157C56 File Offset: 0x00155E56
		public CompProperties_FireOverlayRitual()
		{
			this.compClass = typeof(CompRitualFireOverlay);
		}

		// Token: 0x040021DB RID: 8667
		public float minRitualProgress;
	}
}
