using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000120 RID: 288
	public class SkyfallerProperties
	{
		// Token: 0x1700016A RID: 362
		// (get) Token: 0x060007D5 RID: 2005 RVA: 0x0000C39E File Offset: 0x0000A59E
		public bool MakesShrapnel
		{
			get
			{
				return this.metalShrapnelCountRange.max > 0 || this.rubbleShrapnelCountRange.max > 0;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x060007D6 RID: 2006 RVA: 0x0000C3BE File Offset: 0x0000A5BE
		public bool CausesExplosion
		{
			get
			{
				return this.explosionDamage != null && this.explosionRadius > 0f;
			}
		}

		// Token: 0x04000557 RID: 1367
		public bool hitRoof = true;

		// Token: 0x04000558 RID: 1368
		public IntRange ticksToImpactRange = new IntRange(120, 200);

		// Token: 0x04000559 RID: 1369
		public bool reversed;

		// Token: 0x0400055A RID: 1370
		public float explosionRadius = 3f;

		// Token: 0x0400055B RID: 1371
		public DamageDef explosionDamage;

		// Token: 0x0400055C RID: 1372
		public bool damageSpawnedThings;

		// Token: 0x0400055D RID: 1373
		public float explosionDamageFactor = 1f;

		// Token: 0x0400055E RID: 1374
		public IntRange metalShrapnelCountRange = IntRange.zero;

		// Token: 0x0400055F RID: 1375
		public IntRange rubbleShrapnelCountRange = IntRange.zero;

		// Token: 0x04000560 RID: 1376
		public float shrapnelDistanceFactor = 1f;

		// Token: 0x04000561 RID: 1377
		public SkyfallerMovementType movementType;

		// Token: 0x04000562 RID: 1378
		public float speed = 1f;

		// Token: 0x04000563 RID: 1379
		public string shadow = "Things/Skyfaller/SkyfallerShadowCircle";

		// Token: 0x04000564 RID: 1380
		public Vector2 shadowSize = Vector2.one;

		// Token: 0x04000565 RID: 1381
		public float cameraShake;

		// Token: 0x04000566 RID: 1382
		public SoundDef impactSound;

		// Token: 0x04000567 RID: 1383
		public bool rotateGraphicTowardsDirection;

		// Token: 0x04000568 RID: 1384
		public SoundDef anticipationSound;

		// Token: 0x04000569 RID: 1385
		public int anticipationSoundTicks = 100;

		// Token: 0x0400056A RID: 1386
		public int motesPerCell = 3;

		// Token: 0x0400056B RID: 1387
		public float moteSpawnTime = float.MinValue;

		// Token: 0x0400056C RID: 1388
		public SimpleCurve xPositionCurve;

		// Token: 0x0400056D RID: 1389
		public SimpleCurve zPositionCurve;

		// Token: 0x0400056E RID: 1390
		public SimpleCurve angleCurve;

		// Token: 0x0400056F RID: 1391
		public SimpleCurve rotationCurve;

		// Token: 0x04000570 RID: 1392
		public SimpleCurve speedCurve;
	}
}
