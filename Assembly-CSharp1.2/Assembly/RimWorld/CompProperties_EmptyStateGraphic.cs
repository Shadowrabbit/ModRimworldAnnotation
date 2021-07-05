using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017B7 RID: 6071
	public class CompProperties_EmptyStateGraphic : CompProperties
	{
		// Token: 0x06008638 RID: 34360 RVA: 0x0005A0FC File Offset: 0x000582FC
		public CompProperties_EmptyStateGraphic()
		{
			this.compClass = typeof(CompEmptyStateGraphic);
		}

		// Token: 0x04005678 RID: 22136
		public GraphicData graphicData;
	}
}
