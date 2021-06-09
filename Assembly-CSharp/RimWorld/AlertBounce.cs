using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02001933 RID: 6451
	internal class AlertBounce
	{
		// Token: 0x06008F00 RID: 36608 RVA: 0x0005FB68 File Offset: 0x0005DD68
		public void DoAlertStartEffect()
		{
			this.position = 300f;
			this.velocity = -200f;
			this.lastTime = Time.time;
			this.idle = false;
		}

		// Token: 0x06008F01 RID: 36609 RVA: 0x00293154 File Offset: 0x00291354
		public float CalculateHorizontalOffset()
		{
			if (this.idle)
			{
				return this.position;
			}
			float num = Mathf.Min(Time.time - this.lastTime, 0.05f);
			this.lastTime = Time.time;
			this.velocity -= 1200f * num;
			this.position += this.velocity * num;
			if (this.position < 0f)
			{
				this.position = 0f;
				this.velocity = Mathf.Max(-this.velocity / 3f - 1f, 0f);
			}
			if (Mathf.Abs(this.velocity) < 0.0001f && this.position < 1f)
			{
				this.velocity = 0f;
				this.position = 0f;
				this.idle = true;
			}
			return this.position;
		}

		// Token: 0x04005B36 RID: 23350
		private float position;

		// Token: 0x04005B37 RID: 23351
		private float velocity;

		// Token: 0x04005B38 RID: 23352
		private float lastTime = Time.time;

		// Token: 0x04005B39 RID: 23353
		private bool idle;

		// Token: 0x04005B3A RID: 23354
		private const float StartPosition = 300f;

		// Token: 0x04005B3B RID: 23355
		private const float StartVelocity = -200f;

		// Token: 0x04005B3C RID: 23356
		private const float Acceleration = 1200f;

		// Token: 0x04005B3D RID: 23357
		private const float DampingRatio = 3f;

		// Token: 0x04005B3E RID: 23358
		private const float DampingConstant = 1f;

		// Token: 0x04005B3F RID: 23359
		private const float MaxDelta = 0.05f;
	}
}
