using System;

namespace RimWorld
{
	// Token: 0x02000BE9 RID: 3049
	public class GameCondition_HeatWave : GameCondition
	{
		// Token: 0x17000C93 RID: 3219
		// (get) Token: 0x060047CA RID: 18378 RVA: 0x0017B6C8 File Offset: 0x001798C8
		public override int TransitionTicks
		{
			get
			{
				return 12000;
			}
		}

		// Token: 0x060047CB RID: 18379 RVA: 0x0017B6CF File Offset: 0x001798CF
		public override float TemperatureOffset()
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 17f);
		}

		// Token: 0x04002C04 RID: 11268
		private const float MaxTempOffset = 17f;
	}
}
