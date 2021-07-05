using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A0E RID: 2574
	public class CompProperties_SnowExpand : CompProperties
	{
		// Token: 0x06003F07 RID: 16135 RVA: 0x00157ED6 File Offset: 0x001560D6
		public CompProperties_SnowExpand()
		{
			this.compClass = typeof(CompSnowExpand);
		}

		// Token: 0x040021FE RID: 8702
		public int expandInterval = 500;

		// Token: 0x040021FF RID: 8703
		public float addAmount = 0.12f;

		// Token: 0x04002200 RID: 8704
		public float maxRadius = 55f;
	}
}
