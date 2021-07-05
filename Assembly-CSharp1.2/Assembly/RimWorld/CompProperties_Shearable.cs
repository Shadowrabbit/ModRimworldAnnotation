using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F1C RID: 3868
	public class CompProperties_Shearable : CompProperties
	{
		// Token: 0x06005585 RID: 21893 RVA: 0x0003B54F File Offset: 0x0003974F
		public CompProperties_Shearable()
		{
			this.compClass = typeof(CompShearable);
		}

		// Token: 0x0400369E RID: 13982
		public int shearIntervalDays;

		// Token: 0x0400369F RID: 13983
		public int woolAmount = 1;

		// Token: 0x040036A0 RID: 13984
		public ThingDef woolDef;
	}
}
