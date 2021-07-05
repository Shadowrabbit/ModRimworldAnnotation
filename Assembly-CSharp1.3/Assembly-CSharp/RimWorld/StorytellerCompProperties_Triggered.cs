using System;

namespace RimWorld
{
	// Token: 0x02000C55 RID: 3157
	public class StorytellerCompProperties_Triggered : StorytellerCompProperties
	{
		// Token: 0x060049C7 RID: 18887 RVA: 0x00185A4F File Offset: 0x00183C4F
		public StorytellerCompProperties_Triggered()
		{
			this.compClass = typeof(StorytellerComp_Triggered);
		}

		// Token: 0x04002CD4 RID: 11476
		public IncidentDef incident;

		// Token: 0x04002CD5 RID: 11477
		public int delayTicks = 60;
	}
}
