using System;

namespace RimWorld
{
	// Token: 0x02000C50 RID: 3152
	public class StorytellerCompProperties_SingleMTB : StorytellerCompProperties
	{
		// Token: 0x060049BD RID: 18877 RVA: 0x001859B4 File Offset: 0x00183BB4
		public StorytellerCompProperties_SingleMTB()
		{
			this.compClass = typeof(StorytellerComp_SingleMTB);
		}

		// Token: 0x04002CCC RID: 11468
		public IncidentDef incident;

		// Token: 0x04002CCD RID: 11469
		public float mtbDays = 13f;
	}
}
