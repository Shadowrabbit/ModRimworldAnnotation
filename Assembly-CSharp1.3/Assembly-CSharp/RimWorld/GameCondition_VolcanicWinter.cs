using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BE4 RID: 3044
	public class GameCondition_VolcanicWinter : GameCondition
	{
		// Token: 0x17000C8F RID: 3215
		// (get) Token: 0x060047B0 RID: 18352 RVA: 0x0017B201 File Offset: 0x00179401
		public override int TransitionTicks
		{
			get
			{
				return 50000;
			}
		}

		// Token: 0x060047B1 RID: 18353 RVA: 0x0017B208 File Offset: 0x00179408
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 0.3f);
		}

		// Token: 0x060047B2 RID: 18354 RVA: 0x0017B21C File Offset: 0x0017941C
		public override SkyTarget? SkyTarget(Map map)
		{
			return new SkyTarget?(new SkyTarget(0.55f, this.VolcanicWinterColors, 1f, 1f));
		}

		// Token: 0x060047B3 RID: 18355 RVA: 0x0017B23D File Offset: 0x0017943D
		public override float TemperatureOffset()
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, this.MaxTempOffset);
		}

		// Token: 0x060047B4 RID: 18356 RVA: 0x0017B252 File Offset: 0x00179452
		public override float AnimalDensityFactor(Map map)
		{
			return 1f - GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 0.5f);
		}

		// Token: 0x060047B5 RID: 18357 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowEnjoyableOutsideNow(Map map)
		{
			return false;
		}

		// Token: 0x04002BFC RID: 11260
		private float MaxTempOffset = -7f;

		// Token: 0x04002BFD RID: 11261
		private const float AnimalDensityImpact = 0.5f;

		// Token: 0x04002BFE RID: 11262
		private const float SkyGlow = 0.55f;

		// Token: 0x04002BFF RID: 11263
		private const float MaxSkyLerpFactor = 0.3f;

		// Token: 0x04002C00 RID: 11264
		private SkyColorSet VolcanicWinterColors = new SkyColorSet(new ColorInt(0, 0, 0).ToColor, Color.white, new Color(0.6f, 0.6f, 0.6f), 0.65f);
	}
}
