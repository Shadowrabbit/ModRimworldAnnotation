using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A04 RID: 2564
	public class CompProperties_Launchable : CompProperties
	{
		// Token: 0x06003EF0 RID: 16112 RVA: 0x00157CC3 File Offset: 0x00155EC3
		public CompProperties_Launchable()
		{
			this.compClass = typeof(CompLaunchable);
		}

		// Token: 0x040021DF RID: 8671
		public bool requireFuel = true;

		// Token: 0x040021E0 RID: 8672
		public int fixedLaunchDistanceMax = -1;

		// Token: 0x040021E1 RID: 8673
		public ThingDef skyfallerLeaving;
	}
}
