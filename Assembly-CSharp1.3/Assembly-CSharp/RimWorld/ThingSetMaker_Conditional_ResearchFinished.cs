using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010DB RID: 4315
	public class ThingSetMaker_Conditional_ResearchFinished : ThingSetMaker_Conditional
	{
		// Token: 0x0600673C RID: 26428 RVA: 0x0022E0FB File Offset: 0x0022C2FB
		protected override bool Condition(ThingSetMakerParams parms)
		{
			return this.researchProject.IsFinished;
		}

		// Token: 0x04003A49 RID: 14921
		public ResearchProjectDef researchProject;
	}
}
