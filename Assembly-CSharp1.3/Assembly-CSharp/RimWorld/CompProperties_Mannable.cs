using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A05 RID: 2565
	public class CompProperties_Mannable : CompProperties
	{
		// Token: 0x06003EF1 RID: 16113 RVA: 0x00157CE9 File Offset: 0x00155EE9
		public CompProperties_Mannable()
		{
			this.compClass = typeof(CompMannable);
		}

		// Token: 0x040021E2 RID: 8674
		public WorkTags manWorkType;
	}
}
