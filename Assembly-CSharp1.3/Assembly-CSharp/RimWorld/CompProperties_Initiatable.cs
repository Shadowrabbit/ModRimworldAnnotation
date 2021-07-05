using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011F8 RID: 4600
	public class CompProperties_Initiatable : CompProperties
	{
		// Token: 0x06006EAC RID: 28332 RVA: 0x00250B54 File Offset: 0x0024ED54
		public CompProperties_Initiatable()
		{
			this.compClass = typeof(CompInitiatable);
		}

		// Token: 0x04003D47 RID: 15687
		public int initiationDelayTicks;
	}
}
