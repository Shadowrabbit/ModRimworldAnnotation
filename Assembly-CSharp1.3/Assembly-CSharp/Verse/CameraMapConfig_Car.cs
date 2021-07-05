using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000070 RID: 112
	public class CameraMapConfig_Car : CameraMapConfig
	{
		// Token: 0x06000476 RID: 1142 RVA: 0x00017BD4 File Offset: 0x00015DD4
		public CameraMapConfig_Car()
		{
			this.dollyRateKeys = 0f;
			this.dollyRateScreenEdge = 0f;
			this.camSpeedDecayFactor = 1f;
			this.moveSpeedScale = 1f;
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00017C08 File Offset: 0x00015E08
		public override void ConfigFixedUpdate_60(ref Vector3 velocity)
		{
			base.ConfigFixedUpdate_60(ref velocity);
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
				this.speed += 1.2f * num;
			}
			if (KeyBindingDefOf.MapDolly_Down.IsDown)
			{
				this.speed -= 1.2f * num;
				if (this.speed < 0f)
				{
					this.speed = 0f;
				}
			}
			this.angle = Mathf.Lerp(this.angle, this.targetAngle, 0.02f);
			velocity.x = Mathf.Cos(this.angle) * this.speed;
			velocity.z = Mathf.Sin(this.angle) * this.speed;
		}

		// Token: 0x04000183 RID: 387
		private float targetAngle;

		// Token: 0x04000184 RID: 388
		private float angle;

		// Token: 0x04000185 RID: 389
		private float speed;

		// Token: 0x04000186 RID: 390
		private const float SpeedChangeSpeed = 1.2f;

		// Token: 0x04000187 RID: 391
		private const float AngleChangeSpeed = 0.72f;
	}
}
