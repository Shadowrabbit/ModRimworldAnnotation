using System;

namespace Verse
{
	// Token: 0x02000295 RID: 661
	public class HediffCompProperties_GetsPermanent : HediffCompProperties
	{
		// Token: 0x0600126B RID: 4715 RVA: 0x0006A398 File Offset: 0x00068598
		public HediffCompProperties_GetsPermanent()
		{
			this.compClass = typeof(HediffComp_GetsPermanent);
		}

		// Token: 0x04000DED RID: 3565
		public float becomePermanentChanceFactor = 1f;

		// Token: 0x04000DEE RID: 3566
		public string permanentLabel;

		// Token: 0x04000DEF RID: 3567
		public string instantlyPermanentLabel;
	}
}
