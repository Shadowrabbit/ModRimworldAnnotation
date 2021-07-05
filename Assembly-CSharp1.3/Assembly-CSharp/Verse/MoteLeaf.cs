using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200036A RID: 874
	public class MoteLeaf : Mote
	{
		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x060018BE RID: 6334 RVA: 0x00092025 File Offset: 0x00090225
		protected override bool EndOfLife
		{
			get
			{
				return base.AgeSecs >= this.spawnDelay + this.FallTime + base.SolidTime + this.def.mote.fadeOutTime;
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x060018BF RID: 6335 RVA: 0x00092057 File Offset: 0x00090257
		private float FallTime
		{
			get
			{
				return this.startSpatialPosition.y / MoteLeaf.FallSpeed;
			}
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x060018C0 RID: 6336 RVA: 0x0009206C File Offset: 0x0009026C
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

		// Token: 0x060018C1 RID: 6337 RVA: 0x0009214E File Offset: 0x0009034E
		public void Initialize(Vector3 position, float spawnDelay, bool front, float treeHeight)
		{
			this.startSpatialPosition = position;
			this.spawnDelay = spawnDelay;
			this.front = front;
			this.treeHeight = treeHeight;
			this.TimeInterval(0f);
		}

		// Token: 0x060018C2 RID: 6338 RVA: 0x00092178 File Offset: 0x00090378
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

		// Token: 0x060018C3 RID: 6339 RVA: 0x00092224 File Offset: 0x00090424
		public override void Draw()
		{
			base.Draw(this.front ? (this.def.altitudeLayer.AltitudeFor() + 0.1f * GenMath.InverseLerp(0f, this.treeHeight, this.currentSpatialPosition.y) * 2f) : this.def.altitudeLayer.AltitudeFor());
		}

		// Token: 0x040010C5 RID: 4293
		private Vector3 startSpatialPosition;

		// Token: 0x040010C6 RID: 4294
		private Vector3 currentSpatialPosition;

		// Token: 0x040010C7 RID: 4295
		private float spawnDelay;

		// Token: 0x040010C8 RID: 4296
		private bool front;

		// Token: 0x040010C9 RID: 4297
		private float treeHeight;

		// Token: 0x040010CA RID: 4298
		[TweakValue("Graphics", 0f, 5f)]
		private static float FallSpeed = 0.5f;
	}
}
