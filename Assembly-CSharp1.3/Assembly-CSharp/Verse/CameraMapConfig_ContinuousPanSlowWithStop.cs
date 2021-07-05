using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000074 RID: 116
	public class CameraMapConfig_ContinuousPanSlowWithStop : CameraMapConfig
	{
		// Token: 0x0600047B RID: 1147 RVA: 0x00017D58 File Offset: 0x00015F58
		public CameraMapConfig_ContinuousPanSlowWithStop()
		{
			this.dollyRateKeys = 10f;
			this.dollyRateScreenEdge = 5f;
			this.camSpeedDecayFactor = 1f;
			this.moveSpeedScale = 0.5f;
			this.minSize = 8.2f;
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00017D97 File Offset: 0x00015F97
		public override void ConfigFixedUpdate_60(ref Vector3 velocity)
		{
			base.ConfigFixedUpdate_60(ref velocity);
			if (KeyBindingDefOf.Misc1.IsDown)
			{
				velocity = Vector3.zero;
			}
		}
	}
}
