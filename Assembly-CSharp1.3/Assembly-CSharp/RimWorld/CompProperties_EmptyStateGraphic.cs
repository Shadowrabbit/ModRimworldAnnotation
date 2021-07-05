using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001128 RID: 4392
	public class CompProperties_EmptyStateGraphic : CompProperties
	{
		// Token: 0x06006983 RID: 27011 RVA: 0x00238F75 File Offset: 0x00237175
		public CompProperties_EmptyStateGraphic()
		{
			this.compClass = typeof(CompEmptyStateGraphic);
		}

		// Token: 0x04003AFA RID: 15098
		public GraphicData graphicData;
	}
}
