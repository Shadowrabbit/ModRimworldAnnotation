using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F0F RID: 3855
	public class CompProperties_Launchable : CompProperties
	{
		// Token: 0x06005543 RID: 21827 RVA: 0x0003B21C File Offset: 0x0003941C
		public CompProperties_Launchable()
		{
			this.compClass = typeof(CompLaunchable);
		}

		// Token: 0x04003660 RID: 13920
		public bool requireFuel = true;

		// Token: 0x04003661 RID: 13921
		public int fixedLaunchDistanceMax = -1;

		// Token: 0x04003662 RID: 13922
		public ThingDef skyfallerLeaving;
	}
}
