using System;

namespace Verse
{
	// Token: 0x020003E3 RID: 995
	public class HediffCompProperties_Infecter : HediffCompProperties
	{
		// Token: 0x06001867 RID: 6247 RVA: 0x00017295 File Offset: 0x00015495
		public HediffCompProperties_Infecter()
		{
			this.compClass = typeof(HediffComp_Infecter);
		}

		// Token: 0x0400127C RID: 4732
		public float infectionChance = 0.5f;
	}
}
