using System;

namespace RimWorld
{
	// Token: 0x02000C52 RID: 3154
	public class StorytellerCompProperties_SingleOnceFixed : StorytellerCompProperties
	{
		// Token: 0x060049C2 RID: 18882 RVA: 0x001859FB File Offset: 0x00183BFB
		public StorytellerCompProperties_SingleOnceFixed()
		{
			this.compClass = typeof(StorytellerComp_SingleOnceFixed);
		}

		// Token: 0x04002CCE RID: 11470
		public IncidentDef incident;

		// Token: 0x04002CCF RID: 11471
		public int fireAfterDaysPassed;

		// Token: 0x04002CD0 RID: 11472
		public RoyalTitleDef skipIfColonistHasMinTitle;

		// Token: 0x04002CD1 RID: 11473
		public bool skipIfOnExtremeBiome;

		// Token: 0x04002CD2 RID: 11474
		public int minColonistCount;
	}
}
