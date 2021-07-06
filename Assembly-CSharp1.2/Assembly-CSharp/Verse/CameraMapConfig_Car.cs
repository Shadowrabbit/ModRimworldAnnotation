using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000BE RID: 190
	public class CameraMapConfig_Car : CameraMapConfig
	{
		// Token: 0x060005E4 RID: 1508 RVA: 0x0000B078 File Offset: 0x00009278
		public CameraMapConfig_Car()
		{
			this.dollyRateKeys = 0f;
			this.dollyRateScreenEdge = 0f;
			this.camSpeedDecayFactor = 1f;
			this.moveSpeedScale = 1f;
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0008D514 File Offset: 0x0008B714
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

		// Token: 0x040002FD RID: 765
		private float targetAngle;

		// Token: 0x040002FE RID: 766
		private float angle;

		// Token: 0x040002FF RID: 767
		private float speed;

		// Token: 0x04000300 RID: 768
		private const float SpeedChangeSpeed = 1.2f;

		// Token: 0x04000301 RID: 769
		private const float AngleChangeSpeed = 0.72f;
	}
}
