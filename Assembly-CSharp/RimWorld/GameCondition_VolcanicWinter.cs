using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200117B RID: 4475
	public class GameCondition_VolcanicWinter : GameCondition
	{
		// Token: 0x17000F78 RID: 3960
		// (get) Token: 0x0600629C RID: 25244 RVA: 0x00043DDB File Offset: 0x00041FDB
		public override int TransitionTicks
		{
			get
			{
				return 50000;
			}
		}

		// Token: 0x0600629D RID: 25245 RVA: 0x00043DE2 File Offset: 0x00041FE2
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 0.3f);
		}

		// Token: 0x0600629E RID: 25246 RVA: 0x00043DF6 File Offset: 0x00041FF6
		public override SkyTarget? SkyTarget(Map map)
		{
			return new SkyTarget?(new SkyTarget(0.55f, this.VolcanicWinterColors, 1f, 1f));
		}

		// Token: 0x0600629F RID: 25247 RVA: 0x00043E17 File Offset: 0x00042017
		public override float TemperatureOffset()
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, this.MaxTempOffset);
		}

		// Token: 0x060062A0 RID: 25248 RVA: 0x00043E2C File Offset: 0x0004202C
		public override float AnimalDensityFactor(Map map)
		{
			return 1f - GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 0.5f);
		}

		// Token: 0x060062A1 RID: 25249 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AllowEnjoyableOutsideNow(Map map)
		{
			return false;
		}

		// Token: 0x04004214 RID: 16916
		private float MaxTempOffset = -7f;

		// Token: 0x04004215 RID: 16917
		private const float AnimalDensityImpact = 0.5f;

		// Token: 0x04004216 RID: 16918
		private const float SkyGlow = 0.55f;

		// Token: 0x04004217 RID: 16919
		private const float MaxSkyLerpFactor = 0.3f;

		// Token: 0x04004218 RID: 16920
		private SkyColorSet VolcanicWinterColors = new SkyColorSet(new ColorInt(0, 0, 0).ToColor, Color.white, new Color(0.6f, 0.6f, 0.6f), 0.65f);
	}
}
