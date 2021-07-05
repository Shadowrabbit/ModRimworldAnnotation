using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004FA RID: 1274
	public class MoteLeaf : Mote
	{
		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x06001FB5 RID: 8117 RVA: 0x0001BF26 File Offset: 0x0001A126
		protected override bool EndOfLife
		{
			get
			{
				return base.AgeSecs >= this.spawnDelay + this.FallTime + base.SolidTime + this.def.mote.fadeOutTime;
			}
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06001FB6 RID: 8118 RVA: 0x0001BF58 File Offset: 0x0001A158
		private float FallTime
		{
			get
			{
				return this.startSpatialPosition.y / MoteLeaf.FallSpeed;
			}
		}

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06001FB7 RID: 8119 RVA: 0x00100CC8 File Offset: 0x000FEEC8
		public override float Alpha
		{
			get
			{
				float num = base.AgeSecs;
				if (num <= this.spawnDelay)
				{
					return 0f;
				}
				num -= this.spawnDelay;
				if (num <= this.def.mote.fadeInTime)
				{
					if (this.def.mote.fadeInTime > 0f)
					{
						return num / this.def.mote.fadeInTime;
					}
					return 1f;
				}
				else
				{
					if (num <= this.FallTime + base.SolidTime)
					{
						return 1f;
					}
					num -= this.FallTime + base.SolidTime;
					if (num <= this.def.mote.fadeOutTime)
					{
						return 1f - Mathf.InverseLerp(0f, this.def.mote.fadeOutTime, num);
					}
					num -= this.def.mote.fadeOutTime;
					return 0f;
				}
			}
		}

		// Token: 0x06001FB8 RID: 8120 RVA: 0x0001BF6B File Offset: 0x0001A16B
		public void Initialize(Vector3 position, float spawnDelay, bool front, float treeHeight)
		{
			this.startSpatialPosition = position;
			this.spawnDelay = spawnDelay;
			this.front = front;
			this.treeHeight = treeHeight;
			this.TimeInterval(0f);
		}

		// Token: 0x06001FB9 RID: 8121 RVA: 0x00100DAC File Offset: 0x000FEFAC
		protected override void TimeInterval(float deltaTime)
		{
			base.TimeInterval(deltaTime);
			if (base.Destroyed)
			{
				return;
			}
			float ageSecs = base.AgeSecs;
			this.exactPosition = this.startSpatialPosition;
			if (ageSecs > this.spawnDelay)
			{
				this.exactPosition.y = this.exactPosition.y - MoteLeaf.FallSpeed * (ageSecs - this.spawnDelay);
			}
			this.exactPosition.y = Mathf.Max(this.exactPosition.y, 0f);
			this.currentSpatialPosition = this.exactPosition;
			this.exactPosition.z = this.exactPosition.z + this.exactPosition.y;
			this.exactPosition.y = 0f;
		}

		// Token: 0x06001FBA RID: 8122 RVA: 0x00100E58 File Offset: 0x000FF058
		public override void Draw()
		{
			base.Draw(this.front ? (this.def.altitudeLayer.AltitudeFor() + 0.1f * GenMath.InverseLerp(0f, this.treeHeight, this.currentSpatialPosition.y) * 2f) : this.def.altitudeLayer.AltitudeFor());
		}

		// Token: 0x0400163E RID: 5694
		private Vector3 startSpatialPosition;

		// Token: 0x0400163F RID: 5695
		private Vector3 currentSpatialPosition;

		// Token: 0x04001640 RID: 5696
		private float spawnDelay;

		// Token: 0x04001641 RID: 5697
		private bool front;

		// Token: 0x04001642 RID: 5698
		private float treeHeight;

		// Token: 0x04001643 RID: 5699
		[TweakValue("Graphics", 0f, 5f)]
		private static float FallSpeed = 0.5f;
	}
}
