using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001172 RID: 4466
	public class CompProperties_ProjectileInterceptor : CompProperties
	{
		// Token: 0x06006B3D RID: 27453 RVA: 0x0023FE9D File Offset: 0x0023E09D
		public CompProperties_ProjectileInterceptor()
		{
			this.compClass = typeof(CompProjectileInterceptor);
		}

		// Token: 0x04003B9F RID: 15263
		public float radius;

		// Token: 0x04003BA0 RID: 15264
		public int cooldownTicks;

		// Token: 0x04003BA1 RID: 15265
		public int disarmedByEmpForTicks;

		// Token: 0x04003BA2 RID: 15266
		public bool interceptGroundProjectiles;

		// Token: 0x04003BA3 RID: 15267
		public bool interceptAirProjectiles;

		// Token: 0x04003BA4 RID: 15268
		public bool interceptNonHostileProjectiles;

		// Token: 0x04003BA5 RID: 15269
		public bool interceptOutgoingProjectiles;

		// Token: 0x04003BA6 RID: 15270
		public int chargeIntervalTicks;

		// Token: 0x04003BA7 RID: 15271
		public int chargeDurationTicks;

		// Token: 0x04003BA8 RID: 15272
		public float minAlpha;

		// Token: 0x04003BA9 RID: 15273
		public float idlePulseSpeed = 0.7f;

		// Token: 0x04003BAA RID: 15274
		public float minIdleAlpha = -1.7f;

		// Token: 0x04003BAB RID: 15275
		public Color color = Color.white;

		// Token: 0x04003BAC RID: 15276
		public EffecterDef reactivateEffect;

		// Token: 0x04003BAD RID: 15277
		public EffecterDef interceptEffect;

		// Token: 0x04003BAE RID: 15278
		public SoundDef activeSound;
	}
}
