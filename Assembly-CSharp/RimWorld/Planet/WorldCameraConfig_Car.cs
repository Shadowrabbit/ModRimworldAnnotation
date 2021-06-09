using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x0200203B RID: 8251
	public class WorldCameraConfig_Car : WorldCameraConfig
	{
		// Token: 0x0600AEE6 RID: 44774 RVA: 0x00071DD8 File Offset: 0x0006FFD8
		public WorldCameraConfig_Car()
		{
			this.dollyRateKeys = 0f;
			this.dollyRateScreenEdge = 0f;
			this.camRotationDecayFactor = 1f;
			this.rotationSpeedScale = 0.15f;
		}

		// Token: 0x0600AEE7 RID: 44775 RVA: 0x0032D55C File Offset: 0x0032B75C
		public override void ConfigFixedUpdate_60(ref Vector2 rotationVelocity)
		{
			base.ConfigFixedUpdate_60(ref rotationVelocity);
			float num = 0.016666668f;
			if (KeyBindingDefOf.MapDolly_Left.IsDown)
			{
				this.targetAngle += 0.72f * num;
			}
			if (KeyBindingDefOf.MapDolly_Right.IsDown)
			{
				this.targetAngle -= 0.72f * num;
			}
			if (KeyBindingDefOf.MapDolly_Up.IsDown)
			{
				this.speed += 1.5f * num;
			}
			if (KeyBindingDefOf.MapDolly_Down.IsDown)
			{
				this.speed -= 1.5f * num;
				if (this.speed < 0f)
				{
					this.speed = 0f;
				}
			}
			this.angle = Mathf.Lerp(this.angle, this.targetAngle, 0.02f);
			rotationVelocity.x = Mathf.Cos(this.angle) * this.speed;
			rotationVelocity.y = Mathf.Sin(this.angle) * this.speed;
		}

		// Token: 0x0400781B RID: 30747
		private float targetAngle;

		// Token: 0x0400781C RID: 30748
		private float angle;

		// Token: 0x0400781D RID: 30749
		private float speed;

		// Token: 0x0400781E RID: 30750
		private const float SpeedChangeSpeed = 1.5f;

		// Token: 0x0400781F RID: 30751
		private const float AngleChangeSpeed = 0.72f;
	}
}
