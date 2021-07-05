using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200156D RID: 5485
	public class HediffCompProperties_ExplodeOnDeath : HediffCompProperties
	{
		// Token: 0x060081CC RID: 33228 RVA: 0x002DE1BD File Offset: 0x002DC3BD
		public HediffCompProperties_ExplodeOnDeath()
		{
			this.compClass = typeof(HediffComp_ExplodeOnDeath);
		}

		// Token: 0x040050C6 RID: 20678
		public bool destroyGear;

		// Token: 0x040050C7 RID: 20679
		public bool destroyBody;

		// Token: 0x040050C8 RID: 20680
		public float explosionRadius;

		// Token: 0x040050C9 RID: 20681
		public DamageDef damageDef;

		// Token: 0x040050CA RID: 20682
		public int damageAmount = -1;
	}
}
