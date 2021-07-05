using System;

namespace RimWorld
{
	// Token: 0x02000C40 RID: 3136
	public class StorytellerCompProperties_FactionInteraction : StorytellerCompProperties
	{
		// Token: 0x06004990 RID: 18832 RVA: 0x001853A4 File Offset: 0x001835A4
		public StorytellerCompProperties_FactionInteraction()
		{
			this.compClass = typeof(StorytellerComp_FactionInteraction);
		}

		// Token: 0x04002CB0 RID: 11440
		public IncidentDef incident;

		// Token: 0x04002CB1 RID: 11441
		public float baseIncidentsPerYear;

		// Token: 0x04002CB2 RID: 11442
		public float minSpacingDays;

		// Token: 0x04002CB3 RID: 11443
		public StoryDanger minDanger;

		// Token: 0x04002CB4 RID: 11444
		public bool fullAlliesOnly;

		// Token: 0x04002CB5 RID: 11445
		public float minWealth;
	}
}
