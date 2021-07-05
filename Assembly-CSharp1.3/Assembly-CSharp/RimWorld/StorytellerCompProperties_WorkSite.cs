using System;

namespace RimWorld
{
	// Token: 0x02000C57 RID: 3159
	public class StorytellerCompProperties_WorkSite : StorytellerCompProperties
	{
		// Token: 0x060049CC RID: 18892 RVA: 0x00185BA1 File Offset: 0x00183DA1
		public StorytellerCompProperties_WorkSite()
		{
			this.compClass = typeof(StorytellerComp_WorkSite);
		}

		// Token: 0x04002CD6 RID: 11478
		public IncidentDef incident;

		// Token: 0x04002CD7 RID: 11479
		public float baseMtbDays = 5f;
	}
}
