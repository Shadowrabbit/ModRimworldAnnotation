using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F02 RID: 3842
	public class CompProperties_CausesGameCondition : CompProperties
	{
		// Token: 0x06005509 RID: 21769 RVA: 0x0003AF86 File Offset: 0x00039186
		public CompProperties_CausesGameCondition()
		{
			this.compClass = typeof(CompCauseGameCondition);
		}

		// Token: 0x0400360E RID: 13838
		public GameConditionDef conditionDef;

		// Token: 0x0400360F RID: 13839
		public int worldRange;

		// Token: 0x04003610 RID: 13840
		public bool preventConditionStacking = true;
	}
}
