using System;

namespace RimWorld
{
	// Token: 0x02001182 RID: 4482
	public class GameCondition_HeatWave : GameCondition
	{
		// Token: 0x17000F7F RID: 3967
		// (get) Token: 0x060062D3 RID: 25299 RVA: 0x00043FD9 File Offset: 0x000421D9
		public override int TransitionTicks
		{
			get
			{
				return 12000;
			}
		}

		// Token: 0x060062D4 RID: 25300 RVA: 0x00043FE0 File Offset: 0x000421E0
		public override float TemperatureOffset()
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 17f);
		}

		// Token: 0x04004221 RID: 16929
		private const float MaxTempOffset = 17f;
	}
}
