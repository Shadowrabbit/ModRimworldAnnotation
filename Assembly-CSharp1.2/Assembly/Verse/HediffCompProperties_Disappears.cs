using System;

namespace Verse
{
	// Token: 0x020003C9 RID: 969
	public class HediffCompProperties_Disappears : HediffCompProperties
	{
		// Token: 0x0600180B RID: 6155 RVA: 0x00016DEA File Offset: 0x00014FEA
		public HediffCompProperties_Disappears()
		{
			this.compClass = typeof(HediffComp_Disappears);
		}

		// Token: 0x04001245 RID: 4677
		public IntRange disappearsAfterTicks;

		// Token: 0x04001246 RID: 4678
		public bool showRemainingTime;
	}
}
