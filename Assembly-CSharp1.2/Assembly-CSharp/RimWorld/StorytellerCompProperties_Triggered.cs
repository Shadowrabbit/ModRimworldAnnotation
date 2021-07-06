using System;

namespace RimWorld
{
	// Token: 0x02001246 RID: 4678
	public class StorytellerCompProperties_Triggered : StorytellerCompProperties
	{
		// Token: 0x06006616 RID: 26134 RVA: 0x00045C77 File Offset: 0x00043E77
		public StorytellerCompProperties_Triggered()
		{
			this.compClass = typeof(StorytellerComp_Triggered);
		}

		// Token: 0x040043F8 RID: 17400
		public IncidentDef incident;

		// Token: 0x040043F9 RID: 17401
		public int delayTicks = 60;
	}
}
