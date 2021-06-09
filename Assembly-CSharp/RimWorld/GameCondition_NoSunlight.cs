using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001176 RID: 4470
	public class GameCondition_NoSunlight : GameCondition
	{
		// Token: 0x17000F71 RID: 3953
		// (get) Token: 0x0600627B RID: 25211 RVA: 0x000325BD File Offset: 0x000307BD
		public override int TransitionTicks
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x0600627C RID: 25212 RVA: 0x00043B23 File Offset: 0x00041D23
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 1f);
		}

		// Token: 0x0600627D RID: 25213 RVA: 0x00043C86 File Offset: 0x00041E86
		public override SkyTarget? SkyTarget(Map map)
		{
			return new SkyTarget?(new SkyTarget(0f, this.EclipseSkyColors, 1f, 0f));
		}

		// Token: 0x04004204 RID: 16900
		private SkyColorSet EclipseSkyColors = new SkyColorSet(new Color(0.482f, 0.603f, 0.682f), Color.white, new Color(0.6f, 0.6f, 0.6f), 1f);
	}
}
