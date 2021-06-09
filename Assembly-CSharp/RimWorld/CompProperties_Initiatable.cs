using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018D0 RID: 6352
	public class CompProperties_Initiatable : CompProperties
	{
		// Token: 0x06008CC5 RID: 36037 RVA: 0x0005E59C File Offset: 0x0005C79C
		public CompProperties_Initiatable()
		{
			this.compClass = typeof(CompInitiatable);
		}

		// Token: 0x040059FE RID: 23038
		public int initiationDelayTicks;
	}
}
