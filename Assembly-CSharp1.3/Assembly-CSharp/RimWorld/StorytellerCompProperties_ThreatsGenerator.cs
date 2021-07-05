using System;

namespace RimWorld
{
	// Token: 0x02000C53 RID: 3155
	public class StorytellerCompProperties_ThreatsGenerator : StorytellerCompProperties
	{
		// Token: 0x060049C3 RID: 18883 RVA: 0x00185A13 File Offset: 0x00183C13
		public StorytellerCompProperties_ThreatsGenerator()
		{
			this.compClass = typeof(StorytellerComp_ThreatsGenerator);
		}

		// Token: 0x04002CD3 RID: 11475
		public ThreatsGeneratorParams parms;
	}
}
