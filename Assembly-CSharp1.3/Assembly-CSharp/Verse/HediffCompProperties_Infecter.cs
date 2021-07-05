using System;

namespace Verse
{
	// Token: 0x020002A6 RID: 678
	public class HediffCompProperties_Infecter : HediffCompProperties
	{
		// Token: 0x0600129C RID: 4764 RVA: 0x0006B04F File Offset: 0x0006924F
		public HediffCompProperties_Infecter()
		{
			this.compClass = typeof(HediffComp_Infecter);
		}

		// Token: 0x04000E12 RID: 3602
		public float infectionChance = 0.5f;
	}
}
