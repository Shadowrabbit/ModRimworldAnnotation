using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000283 RID: 643
	public class HediffCompProperties_ChangeNeed : HediffCompProperties
	{
		// Token: 0x06001233 RID: 4659 RVA: 0x000697B3 File Offset: 0x000679B3
		public HediffCompProperties_ChangeNeed()
		{
			this.compClass = typeof(HediffComp_ChangeNeed);
		}

		// Token: 0x04000DD5 RID: 3541
		public NeedDef needDef;

		// Token: 0x04000DD6 RID: 3542
		public float percentPerDay;
	}
}
