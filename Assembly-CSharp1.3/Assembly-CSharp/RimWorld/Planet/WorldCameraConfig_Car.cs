using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x0200174E RID: 5966
	public class WorldCameraConfig_Car : WorldCameraConfig
	{
		// Token: 0x060089BD RID: 35261 RVA: 0x00317583 File Offset: 0x00315783
		public WorldCameraConfig_Car()
		{
			this.dollyRateKeys = 0f;
			this.dollyRateScreenEdge = 0f;
			this.camRotationDecayFactor = 1f;
			this.rotationSpeedScale = 0.15f;
		}

		// Token: 0x060089BE RID: 35262 RVA: 0x003175B8 File Offset: 0x003157B8
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

		// Token: 0x0400576E RID: 22382
		private float targetAngle;

		// Token: 0x0400576F RID: 22383
		private float angle;

		// Token: 0x04005770 RID: 22384
		private float speed;

		// Token: 0x04005771 RID: 22385
		private const float SpeedChangeSpeed = 1.5f;

		// Token: 0x04005772 RID: 22386
		private const float AngleChangeSpeed = 0.72f;
	}
}
