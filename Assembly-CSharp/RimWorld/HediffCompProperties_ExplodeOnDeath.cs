using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DF6 RID: 7670
	public class HediffCompProperties_ExplodeOnDeath : HediffCompProperties
	{
		// Token: 0x0600A634 RID: 42548 RVA: 0x0006DF46 File Offset: 0x0006C146
		public HediffCompProperties_ExplodeOnDeath()
		{
			this.compClass = typeof(HediffComp_ExplodeOnDeath);
		}

		// Token: 0x040070AC RID: 28844
		public bool destroyGear;

		// Token: 0x040070AD RID: 28845
		public bool destroyBody;

		// Token: 0x040070AE RID: 28846
		public float explosionRadius;

		// Token: 0x040070AF RID: 28847
		public DamageDef damageDef;

		// Token: 0x040070B0 RID: 28848
		public int damageAmount = -1;
	}
}
