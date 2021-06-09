using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004FB RID: 1275
	public class MoteSplash : Mote
	{
		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x06001FBD RID: 8125 RVA: 0x0001BFA1 File Offset: 0x0001A1A1
		protected override bool EndOfLife
		{
			get
			{
				return base.AgeSecs >= this.targetSize / this.velocity;
			}
		}

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x06001FBE RID: 8126 RVA: 0x00100EC0 File Offset: 0x000FF0C0
		public override float Alpha
		{
			get
			{
				Mathf.Clamp01(base.AgeSecs * 10f);
				float num = 1f;
				float num2 = Mathf.Clamp01(1f - base.AgeSecs / (this.targetSize / this.velocity));
				return num * num2 * this.CalculatedIntensity();
			}
		}

		// Token: 0x06001FBF RID: 8127 RVA: 0x0001BFBB File Offset: 0x0001A1BB
		public void Initialize(Vector3 position, float size, float velocity)
		{
			this.exactPosition = position;
			this.targetSize = size;
			this.velocity = velocity;
			base.Scale = 0f;
		}

		// Token: 0x06001FC0 RID: 8128 RVA: 0x00100F10 File Offset: 0x000FF110
		protected override void TimeInterval(float deltaTime)
		{
			base.TimeInterval(deltaTime);
			if (base.Destroyed)
			{
				return;
			}
			float scale = base.AgeSecs * this.velocity;
			base.Scale = scale;
			this.exactPosition += base.Map.waterInfo.GetWaterMovement(this.exactPosition) * deltaTime;
		}

		// Token: 0x06001FC1 RID: 8129 RVA: 0x0001BFDD File Offset: 0x0001A1DD
		public float CalculatedIntensity()
		{
			return Mathf.Sqrt(this.targetSize) / 10f;
		}

		// Token: 0x06001FC2 RID: 8130 RVA: 0x0001BFF0 File Offset: 0x0001A1F0
		public float CalculatedShockwaveSpan()
		{
			return Mathf.Min(Mathf.Sqrt(this.targetSize) * 0.8f, this.exactScale.x) / this.exactScale.x;
		}

		// Token: 0x04001644 RID: 5700
		public const float VelocityFootstep = 1.5f;

		// Token: 0x04001645 RID: 5701
		public const float SizeFootstep = 2f;

		// Token: 0x04001646 RID: 5702
		public const float VelocityGunfire = 4f;

		// Token: 0x04001647 RID: 5703
		public const float SizeGunfire = 1f;

		// Token: 0x04001648 RID: 5704
		public const float VelocityExplosion = 20f;

		// Token: 0x04001649 RID: 5705
		public const float SizeExplosion = 6f;

		// Token: 0x0400164A RID: 5706
		private float targetSize;

		// Token: 0x0400164B RID: 5707
		private float velocity;
	}
}
