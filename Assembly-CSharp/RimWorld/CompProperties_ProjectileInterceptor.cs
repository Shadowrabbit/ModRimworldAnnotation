using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001809 RID: 6153
	public class CompProperties_ProjectileInterceptor : CompProperties
	{
		// Token: 0x0600881C RID: 34844 RVA: 0x0005B5CA File Offset: 0x000597CA
		public CompProperties_ProjectileInterceptor()
		{
			this.compClass = typeof(CompProjectileInterceptor);
		}

		// Token: 0x04005750 RID: 22352
		public float radius;

		// Token: 0x04005751 RID: 22353
		public int cooldownTicks;

		// Token: 0x04005752 RID: 22354
		public int disarmedByEmpForTicks;

		// Token: 0x04005753 RID: 22355
		public bool interceptGroundProjectiles;

		// Token: 0x04005754 RID: 22356
		public bool interceptAirProjectiles;

		// Token: 0x04005755 RID: 22357
		public bool interceptNonHostileProjectiles;

		// Token: 0x04005756 RID: 22358
		public bool interceptOutgoingProjectiles;

		// Token: 0x04005757 RID: 22359
		public int chargeIntervalTicks;

		// Token: 0x04005758 RID: 22360
		public int chargeDurationTicks;

		// Token: 0x04005759 RID: 22361
		public float minAlpha;

		// Token: 0x0400575A RID: 22362
		public float idlePulseSpeed = 0.7f;

		// Token: 0x0400575B RID: 22363
		public float minIdleAlpha = -1.7f;

		// Token: 0x0400575C RID: 22364
		public Color color = Color.white;

		// Token: 0x0400575D RID: 22365
		public EffecterDef reactivateEffect;

		// Token: 0x0400575E RID: 22366
		public EffecterDef interceptEffect;

		// Token: 0x0400575F RID: 22367
		public SoundDef activeSound;
	}
}
