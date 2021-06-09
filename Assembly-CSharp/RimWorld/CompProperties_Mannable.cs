using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F10 RID: 3856
	public class CompProperties_Mannable : CompProperties
	{
		// Token: 0x06005544 RID: 21828 RVA: 0x0003B242 File Offset: 0x00039442
		public CompProperties_Mannable()
		{
			this.compClass = typeof(CompMannable);
		}

		// Token: 0x04003663 RID: 13923
		public WorkTags manWorkType;
	}
}
