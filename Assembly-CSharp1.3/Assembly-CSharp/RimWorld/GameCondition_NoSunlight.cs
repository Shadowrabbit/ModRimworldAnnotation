using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BDF RID: 3039
	public class GameCondition_NoSunlight : GameCondition
	{
		// Token: 0x17000C88 RID: 3208
		// (get) Token: 0x0600478F RID: 18319 RVA: 0x0011EB3C File Offset: 0x0011CD3C
		public override int TransitionTicks
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x06004790 RID: 18320 RVA: 0x0017A3F6 File Offset: 0x001785F6
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 1f);
		}

		// Token: 0x06004791 RID: 18321 RVA: 0x0017AA64 File Offset: 0x00178C64
		public override SkyTarget? SkyTarget(Map map)
		{
			return new SkyTarget?(new SkyTarget(0f, this.EclipseSkyColors, 1f, 0f));
		}

		// Token: 0x04002BEC RID: 11244
		private SkyColorSet EclipseSkyColors = new SkyColorSet(new Color(0.482f, 0.603f, 0.682f), Color.white, new Color(0.6f, 0.6f, 0.6f), 1f);
	}
}
