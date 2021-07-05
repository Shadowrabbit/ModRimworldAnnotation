using System;

namespace RimWorld
{
	// Token: 0x02001183 RID: 4483
	public class GameCondition_ColdSnap : GameCondition
	{
		// Token: 0x17000F80 RID: 3968
		// (get) Token: 0x060062D6 RID: 25302 RVA: 0x00043FD9 File Offset: 0x000421D9
		public override int TransitionTicks
		{
			get
			{
				return 12000;
			}
		}

		// Token: 0x060062D7 RID: 25303 RVA: 0x00043FF4 File Offset: 0x000421F4
		public override float TemperatureOffset()
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, -20f);
		}

		// Token: 0x04004222 RID: 16930
		private const float MaxTempOffset = -20f;
	}
}
