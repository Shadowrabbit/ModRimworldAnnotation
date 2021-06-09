using System;

namespace Verse
{
	// Token: 0x020003EA RID: 1002
	public class HediffCompProperties_ReactOnDamage : HediffCompProperties
	{
		// Token: 0x06001883 RID: 6275 RVA: 0x0001740D File Offset: 0x0001560D
		public HediffCompProperties_ReactOnDamage()
		{
			this.compClass = typeof(HediffComp_ReactOnDamage);
		}

		// Token: 0x0400128C RID: 4748
		public DamageDef damageDefIncoming;

		// Token: 0x0400128D RID: 4749
		public BodyPartDef createHediffOn;

		// Token: 0x0400128E RID: 4750
		public HediffDef createHediff;

		// Token: 0x0400128F RID: 4751
		public bool vomit;
	}
}
