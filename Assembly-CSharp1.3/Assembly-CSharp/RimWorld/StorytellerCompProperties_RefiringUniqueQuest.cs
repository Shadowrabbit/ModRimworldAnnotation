using System;

namespace RimWorld
{
	// Token: 0x02000C4C RID: 3148
	public class StorytellerCompProperties_RefiringUniqueQuest : StorytellerCompProperties
	{
		// Token: 0x060049B3 RID: 18867 RVA: 0x0018587B File Offset: 0x00183A7B
		public StorytellerCompProperties_RefiringUniqueQuest()
		{
			this.compClass = typeof(StorytellerComp_RefiringUniqueQuest);
		}

		// Token: 0x04002CC8 RID: 11464
		public IncidentDef incident;

		// Token: 0x04002CC9 RID: 11465
		public float refireEveryDays = -1f;

		// Token: 0x04002CCA RID: 11466
		public int minColonyWealth = -1;
	}
}
