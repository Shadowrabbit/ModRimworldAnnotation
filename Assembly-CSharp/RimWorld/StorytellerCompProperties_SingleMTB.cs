using System;

namespace RimWorld
{
	// Token: 0x0200123F RID: 4671
	public class StorytellerCompProperties_SingleMTB : StorytellerCompProperties
	{
		// Token: 0x060065FB RID: 26107 RVA: 0x00045B6C File Offset: 0x00043D6C
		public StorytellerCompProperties_SingleMTB()
		{
			this.compClass = typeof(StorytellerComp_SingleMTB);
		}

		// Token: 0x040043E3 RID: 17379
		public IncidentDef incident;

		// Token: 0x040043E4 RID: 17380
		public float mtbDays = 13f;
	}
}
