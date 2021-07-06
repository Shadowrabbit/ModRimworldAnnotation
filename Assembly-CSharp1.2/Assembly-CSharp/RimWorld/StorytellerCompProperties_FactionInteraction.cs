using System;

namespace RimWorld
{
	// Token: 0x02001228 RID: 4648
	public class StorytellerCompProperties_FactionInteraction : StorytellerCompProperties
	{
		// Token: 0x06006596 RID: 26006 RVA: 0x000457BF File Offset: 0x000439BF
		public StorytellerCompProperties_FactionInteraction()
		{
			this.compClass = typeof(StorytellerComp_FactionInteraction);
		}

		// Token: 0x0400439A RID: 17306
		public IncidentDef incident;

		// Token: 0x0400439B RID: 17307
		public float baseIncidentsPerYear;

		// Token: 0x0400439C RID: 17308
		public float minSpacingDays;

		// Token: 0x0400439D RID: 17309
		public StoryDanger minDanger;

		// Token: 0x0400439E RID: 17310
		public bool fullAlliesOnly;
	}
}
