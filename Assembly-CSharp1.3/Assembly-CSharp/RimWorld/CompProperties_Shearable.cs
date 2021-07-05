using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A0C RID: 2572
	public class CompProperties_Shearable : CompProperties
	{
		// Token: 0x06003F05 RID: 16133 RVA: 0x00157E9F File Offset: 0x0015609F
		public CompProperties_Shearable()
		{
			this.compClass = typeof(CompShearable);
		}

		// Token: 0x040021FB RID: 8699
		public int shearIntervalDays;

		// Token: 0x040021FC RID: 8700
		public int woolAmount = 1;

		// Token: 0x040021FD RID: 8701
		public ThingDef woolDef;
	}
}
