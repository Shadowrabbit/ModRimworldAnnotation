using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009F7 RID: 2551
	public class CompProperties_CausesGameCondition : CompProperties
	{
		// Token: 0x06003ECB RID: 16075 RVA: 0x00157492 File Offset: 0x00155692
		public CompProperties_CausesGameCondition()
		{
			this.compClass = typeof(CompCauseGameCondition);
		}

		// Token: 0x0400219B RID: 8603
		public GameConditionDef conditionDef;

		// Token: 0x0400219C RID: 8604
		public int worldRange;

		// Token: 0x0400219D RID: 8605
		public bool preventConditionStacking = true;
	}
}
