using System;

namespace Verse
{
	// Token: 0x020002AD RID: 685
	public class HediffCompProperties_ReactOnDamage : HediffCompProperties
	{
		// Token: 0x060012B9 RID: 4793 RVA: 0x0006B7BC File Offset: 0x000699BC
		public HediffCompProperties_ReactOnDamage()
		{
			this.compClass = typeof(HediffComp_ReactOnDamage);
		}

		// Token: 0x04000E24 RID: 3620
		public DamageDef damageDefIncoming;

		// Token: 0x04000E25 RID: 3621
		public BodyPartDef createHediffOn;

		// Token: 0x04000E26 RID: 3622
		public HediffDef createHediff;

		// Token: 0x04000E27 RID: 3623
		public bool vomit;
	}
}
