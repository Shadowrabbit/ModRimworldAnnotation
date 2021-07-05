using System;

namespace RimWorld
{
	// Token: 0x02001242 RID: 4674
	public class StorytellerCompProperties_SingleOnceFixed : StorytellerCompProperties
	{
		// Token: 0x06006608 RID: 26120 RVA: 0x00045BDD File Offset: 0x00043DDD
		public StorytellerCompProperties_SingleOnceFixed()
		{
			this.compClass = typeof(StorytellerComp_SingleOnceFixed);
		}

		// Token: 0x040043EB RID: 17387
		public IncidentDef incident;

		// Token: 0x040043EC RID: 17388
		public int fireAfterDaysPassed;

		// Token: 0x040043ED RID: 17389
		public RoyalTitleDef skipIfColonistHasMinTitle;

		// Token: 0x040043EE RID: 17390
		public bool skipIfOnExtremeBiome;

		// Token: 0x040043EF RID: 17391
		public int minColonistCount;
	}
}
