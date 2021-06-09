using System;

namespace Verse
{
	// Token: 0x020003D8 RID: 984
	public class HediffCompProperties_GetsPermanent : HediffCompProperties
	{
		// Token: 0x06001841 RID: 6209 RVA: 0x000170EB File Offset: 0x000152EB
		public HediffCompProperties_GetsPermanent()
		{
			this.compClass = typeof(HediffComp_GetsPermanent);
		}

		// Token: 0x0400125E RID: 4702
		public float becomePermanentChanceFactor = 1f;

		// Token: 0x0400125F RID: 4703
		public string permanentLabel;

		// Token: 0x04001260 RID: 4704
		public string instantlyPermanentLabel;
	}
}
