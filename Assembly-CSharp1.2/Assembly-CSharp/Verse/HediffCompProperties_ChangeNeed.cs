using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003C4 RID: 964
	public class HediffCompProperties_ChangeNeed : HediffCompProperties
	{
		// Token: 0x060017F7 RID: 6135 RVA: 0x00016CDA File Offset: 0x00014EDA
		public HediffCompProperties_ChangeNeed()
		{
			this.compClass = typeof(HediffComp_ChangeNeed);
		}

		// Token: 0x04001239 RID: 4665
		public NeedDef needDef;

		// Token: 0x0400123A RID: 4666
		public float percentPerDay;
	}
}
