using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000131 RID: 305
	public class SubEffecterDef
	{
		// Token: 0x06000827 RID: 2087 RVA: 0x0000C847 File Offset: 0x0000AA47
		public SubEffecter Spawn(Effecter parent)
		{
			return (SubEffecter)Activator.CreateInstance(this.subEffecterClass, new object[]
			{
				this,
				parent
			});
		}

		// Token: 0x040005EB RID: 1515
		public Type subEffecterClass;

		// Token: 0x040005EC RID: 1516
		public IntRange burstCount = new IntRange(1, 1);

		// Token: 0x040005ED RID: 1517
		public int ticksBetweenMotes = 40;

		// Token: 0x040005EE RID: 1518
		public int maxMoteCount = int.MaxValue;

		// Token: 0x040005EF RID: 1519
		public int initialDelayTicks;

		// Token: 0x040005F0 RID: 1520
		public float chancePerTick = 0.1f;

		// Token: 0x040005F1 RID: 1521
		public MoteSpawnLocType spawnLocType = MoteSpawnLocType.BetweenPositions;

		// Token: 0x040005F2 RID: 1522
		public float positionLerpFactor = 0.5f;

		// Token: 0x040005F3 RID: 1523
		public Vector3 positionOffset = Vector3.zero;

		// Token: 0x040005F4 RID: 1524
		public float positionRadius;

		// Token: 0x040005F5 RID: 1525
		public ThingDef moteDef;

		// Token: 0x040005F6 RID: 1526
		public Color color = Color.white;

		// Token: 0x040005F7 RID: 1527
		public FloatRange angle = new FloatRange(0f, 360f);

		// Token: 0x040005F8 RID: 1528
		public bool absoluteAngle;

		// Token: 0x040005F9 RID: 1529
		public FloatRange speed = new FloatRange(0f, 0f);

		// Token: 0x040005FA RID: 1530
		public FloatRange rotation = new FloatRange(0f, 360f);

		// Token: 0x040005FB RID: 1531
		public FloatRange rotationRate = new FloatRange(0f, 0f);

		// Token: 0x040005FC RID: 1532
		public FloatRange scale = new FloatRange(1f, 1f);

		// Token: 0x040005FD RID: 1533
		public FloatRange airTime = new FloatRange(999999f, 999999f);

		// Token: 0x040005FE RID: 1534
		public SoundDef soundDef;

		// Token: 0x040005FF RID: 1535
		public IntRange intermittentSoundInterval = new IntRange(300, 600);

		// Token: 0x04000600 RID: 1536
		public int ticksBeforeSustainerStart;
	}
}
