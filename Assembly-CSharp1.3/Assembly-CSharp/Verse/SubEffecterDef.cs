using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000C5 RID: 197
	public class SubEffecterDef
	{
		// Token: 0x060005E8 RID: 1512 RVA: 0x0001E2D0 File Offset: 0x0001C4D0
		public SubEffecter Spawn(Effecter parent)
		{
			return (SubEffecter)Activator.CreateInstance(this.subEffecterClass, new object[]
			{
				this,
				parent
			});
		}

		// Token: 0x040003FE RID: 1022
		public Type subEffecterClass;

		// Token: 0x040003FF RID: 1023
		public IntRange burstCount = new IntRange(1, 1);

		// Token: 0x04000400 RID: 1024
		public int ticksBetweenMotes = 40;

		// Token: 0x04000401 RID: 1025
		public int maxMoteCount = int.MaxValue;

		// Token: 0x04000402 RID: 1026
		public int initialDelayTicks;

		// Token: 0x04000403 RID: 1027
		public float chancePerTick = 0.1f;

		// Token: 0x04000404 RID: 1028
		public MoteSpawnLocType spawnLocType = MoteSpawnLocType.BetweenPositions;

		// Token: 0x04000405 RID: 1029
		public float positionLerpFactor = 0.5f;

		// Token: 0x04000406 RID: 1030
		public Vector3 positionOffset = Vector3.zero;

		// Token: 0x04000407 RID: 1031
		public float positionRadius;

		// Token: 0x04000408 RID: 1032
		public ThingDef moteDef;

		// Token: 0x04000409 RID: 1033
		public FleckDef fleckDef;

		// Token: 0x0400040A RID: 1034
		public Color color = Color.white;

		// Token: 0x0400040B RID: 1035
		public FloatRange angle = new FloatRange(0f, 360f);

		// Token: 0x0400040C RID: 1036
		public bool absoluteAngle;

		// Token: 0x0400040D RID: 1037
		public bool useTargetAInitialRotation;

		// Token: 0x0400040E RID: 1038
		public bool useTargetBInitialRotation;

		// Token: 0x0400040F RID: 1039
		public bool fleckUsesAngleForVelocity;

		// Token: 0x04000410 RID: 1040
		public FloatRange speed = new FloatRange(0f, 0f);

		// Token: 0x04000411 RID: 1041
		public FloatRange rotation = new FloatRange(0f, 360f);

		// Token: 0x04000412 RID: 1042
		public FloatRange rotationRate = new FloatRange(0f, 0f);

		// Token: 0x04000413 RID: 1043
		public FloatRange scale = new FloatRange(1f, 1f);

		// Token: 0x04000414 RID: 1044
		public FloatRange airTime = new FloatRange(999999f, 999999f);

		// Token: 0x04000415 RID: 1045
		public SoundDef soundDef;

		// Token: 0x04000416 RID: 1046
		public IntRange intermittentSoundInterval = new IntRange(300, 600);

		// Token: 0x04000417 RID: 1047
		public int ticksBeforeSustainerStart;
	}
}
