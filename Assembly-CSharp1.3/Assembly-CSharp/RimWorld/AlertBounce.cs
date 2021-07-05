using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x0200123C RID: 4668
	internal class AlertBounce
	{
		// Token: 0x06007012 RID: 28690 RVA: 0x0025577B File Offset: 0x0025397B
		public void DoAlertStartEffect()
		{
			this.position = 300f;
			this.velocity = -200f;
			this.lastTime = Time.time;
			this.idle = false;
		}

		// Token: 0x06007013 RID: 28691 RVA: 0x002557A8 File Offset: 0x002539A8
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

		// Token: 0x04003DDF RID: 15839
		private float position;

		// Token: 0x04003DE0 RID: 15840
		private float velocity;

		// Token: 0x04003DE1 RID: 15841
		private float lastTime = Time.time;

		// Token: 0x04003DE2 RID: 15842
		private bool idle;

		// Token: 0x04003DE3 RID: 15843
		private const float StartPosition = 300f;

		// Token: 0x04003DE4 RID: 15844
		private const float StartVelocity = -200f;

		// Token: 0x04003DE5 RID: 15845
		private const float Acceleration = 1200f;

		// Token: 0x04003DE6 RID: 15846
		private const float DampingRatio = 3f;

		// Token: 0x04003DE7 RID: 15847
		private const float DampingConstant = 1f;

		// Token: 0x04003DE8 RID: 15848
		private const float MaxDelta = 0.05f;
	}
}
