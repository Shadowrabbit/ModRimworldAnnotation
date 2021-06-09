using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F1E RID: 3870
	public class CompProperties_SnowExpand : CompProperties
	{
		// Token: 0x06005587 RID: 21895 RVA: 0x0003B586 File Offset: 0x00039786
		public CompProperties_SnowExpand()
		{
			this.compClass = typeof(CompSnowExpand);
		}

		// Token: 0x040036A1 RID: 13985
		public int expandInterval = 500;

		// Token: 0x040036A2 RID: 13986
		public float addAmount = 0.12f;

		// Token: 0x040036A3 RID: 13987
		public float maxRadius = 55f;
	}
}
