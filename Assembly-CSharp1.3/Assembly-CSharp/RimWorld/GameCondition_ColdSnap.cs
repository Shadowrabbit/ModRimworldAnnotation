using System;

namespace RimWorld
{
	// Token: 0x02000BEA RID: 3050
	public class GameCondition_ColdSnap : GameCondition
	{
		// Token: 0x17000C94 RID: 3220
		// (get) Token: 0x060047CD RID: 18381 RVA: 0x0017B6C8 File Offset: 0x001798C8
		public override int TransitionTicks
		{
			get
			{
				return 12000;
			}
		}

		// Token: 0x060047CE RID: 18382 RVA: 0x0017B6E3 File Offset: 0x001798E3
		public override float TemperatureOffset()
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, -20f);
		}

		// Token: 0x04002C05 RID: 11269
		private const float MaxTempOffset = -20f;
	}
}
