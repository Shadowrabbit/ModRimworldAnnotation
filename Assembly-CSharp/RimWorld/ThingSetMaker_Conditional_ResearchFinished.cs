using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001741 RID: 5953
	public class ThingSetMaker_Conditional_ResearchFinished : ThingSetMaker_Conditional
	{
		// Token: 0x06008348 RID: 33608 RVA: 0x0005828B File Offset: 0x0005648B
		protected override bool Condition(ThingSetMakerParams parms)
		{
			return this.researchProject.IsFinished;
		}

		// Token: 0x04005514 RID: 21780
		public ResearchProjectDef researchProject;
	}
}
