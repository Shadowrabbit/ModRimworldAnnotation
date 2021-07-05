using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000B8 RID: 184
	public class SkyfallerProperties
	{
		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060005A0 RID: 1440 RVA: 0x0001CF6D File Offset: 0x0001B16D
		public bool MakesShrapnel
		{
			get
			{
				return this.metalShrapnelCountRange.max > 0 || this.rubbleShrapnelCountRange.max > 0;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060005A1 RID: 1441 RVA: 0x0001CF8D File Offset: 0x0001B18D
		public bool CausesExplosion
		{
			get
			{
				return this.explosionDamage != null && this.explosionRadius > 0f;
			}
		}

		// Token: 0x0400036F RID: 879
		public bool hitRoof = true;

		// Token: 0x04000370 RID: 880
		public IntRange ticksToImpactRange = new IntRange(120, 200);

		// Token: 0x04000371 RID: 881
		public IntRange ticksToDiscardInReverse = IntRange.zero;

		// Token: 0x04000372 RID: 882
		public bool reversed;

		// Token: 0x04000373 RID: 883
		public float explosionRadius = 3f;

		// Token: 0x04000374 RID: 884
		public DamageDef explosionDamage;

		// Token: 0x04000375 RID: 885
		public bool damageSpawnedThings;

		// Token: 0x04000376 RID: 886
		public float explosionDamageFactor = 1f;

		// Token: 0x04000377 RID: 887
		public IntRange metalShrapnelCountRange = IntRange.zero;

		// Token: 0x04000378 RID: 888
		public IntRange rubbleShrapnelCountRange = IntRange.zero;

		// Token: 0x04000379 RID: 889
		public float shrapnelDistanceFactor = 1f;

		// Token: 0x0400037A RID: 890
		public SkyfallerMovementType movementType;

		// Token: 0x0400037B RID: 891
		public float speed = 1f;

		// Token: 0x0400037C RID: 892
		public string shadow = "Things/Skyfaller/SkyfallerShadowCircle";

		// Token: 0x0400037D RID: 893
		public Vector2 shadowSize = Vector2.one;

		// Token: 0x0400037E RID: 894
		public float cameraShake;

		// Token: 0x0400037F RID: 895
		public SoundDef impactSound;

		// Token: 0x04000380 RID: 896
		public bool rotateGraphicTowardsDirection;

		// Token: 0x04000381 RID: 897
		public SoundDef anticipationSound;

		// Token: 0x04000382 RID: 898
		public SoundDef floatingSound;

		// Token: 0x04000383 RID: 899
		public int anticipationSoundTicks = 100;

		// Token: 0x04000384 RID: 900
		public int motesPerCell = 3;

		// Token: 0x04000385 RID: 901
		public float moteSpawnTime = float.MinValue;

		// Token: 0x04000386 RID: 902
		public SimpleCurve xPositionCurve;

		// Token: 0x04000387 RID: 903
		public SimpleCurve zPositionCurve;

		// Token: 0x04000388 RID: 904
		public SimpleCurve angleCurve;

		// Token: 0x04000389 RID: 905
		public SimpleCurve rotationCurve;

		// Token: 0x0400038A RID: 906
		public SimpleCurve speedCurve;

		// Token: 0x0400038B RID: 907
		public int fadeInTicks;

		// Token: 0x0400038C RID: 908
		public int fadeOutTicks;
	}
}
