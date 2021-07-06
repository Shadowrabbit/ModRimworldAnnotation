using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F11 RID: 3857
	public class CompProperties_Milkable : CompProperties
	{
		// Token: 0x06005545 RID: 21829 RVA: 0x0003B25A File Offset: 0x0003945A
		public CompProperties_Milkable()
		{
			this.compClass = typeof(CompMilkable);
		}

		// Token: 0x04003664 RID: 13924
		public int milkIntervalDays;

		// Token: 0x04003665 RID: 13925
		public int milkAmount = 1;

		// Token: 0x04003666 RID: 13926
		public ThingDef milkDef;

		// Token: 0x04003667 RID: 13927
		public bool milkFemaleOnly = true;
	}
}
