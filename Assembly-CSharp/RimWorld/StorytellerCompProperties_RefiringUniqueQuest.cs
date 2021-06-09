using System;

namespace RimWorld
{
	// Token: 0x02001239 RID: 4665
	public class StorytellerCompProperties_RefiringUniqueQuest : StorytellerCompProperties
	{
		// Token: 0x060065E1 RID: 26081 RVA: 0x00045A85 File Offset: 0x00043C85
		public StorytellerCompProperties_RefiringUniqueQuest()
		{
			this.compClass = typeof(StorytellerComp_RefiringUniqueQuest);
		}

		// Token: 0x040043D4 RID: 17364
		public IncidentDef incident;

		// Token: 0x040043D5 RID: 17365
		public float refireEveryDays = -1f;
	}
}
