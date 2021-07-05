using System;

namespace Verse
{
	// Token: 0x020002AB RID: 683
	public class HediffCompProperties_Link : HediffCompProperties
	{
		// Token: 0x060012B1 RID: 4785 RVA: 0x0006B572 File Offset: 0x00069772
		public HediffCompProperties_Link()
		{
			this.compClass = typeof(HediffComp_Link);
		}

		// Token: 0x04000E1D RID: 3613
		public bool showName = true;

		// Token: 0x04000E1E RID: 3614
		public float maxDistance = -1f;

		// Token: 0x04000E1F RID: 3615
		public bool requireLinkOnOtherPawn = true;

		// Token: 0x04000E20 RID: 3616
		public ThingDef customMote;
	}
}
