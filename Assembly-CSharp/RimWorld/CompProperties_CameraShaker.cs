using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F01 RID: 3841
	public class CompProperties_CameraShaker : CompProperties
	{
		// Token: 0x06005508 RID: 21768 RVA: 0x0003AF63 File Offset: 0x00039163
		public CompProperties_CameraShaker()
		{
			this.compClass = typeof(CompCameraShaker);
		}

		// Token: 0x0400360D RID: 13837
		public float mag = 0.05f;
	}
}
