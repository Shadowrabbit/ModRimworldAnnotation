using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009F6 RID: 2550
	public class CompProperties_CameraShaker : CompProperties
	{
		// Token: 0x06003ECA RID: 16074 RVA: 0x0015746F File Offset: 0x0015566F
		public CompProperties_CameraShaker()
		{
			this.compClass = typeof(CompCameraShaker);
		}

		// Token: 0x0400219A RID: 8602
		public float mag = 0.05f;
	}
}
