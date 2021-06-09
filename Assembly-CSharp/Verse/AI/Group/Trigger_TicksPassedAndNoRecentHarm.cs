using System;

namespace Verse.AI.Group
{
	// Token: 0x02000AF3 RID: 2803
	public class Trigger_TicksPassedAndNoRecentHarm : Trigger_TicksPassed
	{
		// Token: 0x060041FE RID: 16894 RVA: 0x00031186 File Offset: 0x0002F386
		public Trigger_TicksPassedAndNoRecentHarm(int tickLimit) : base(tickLimit)
		{
		}

		// Token: 0x060041FF RID: 16895 RVA: 0x0003118F File Offset: 0x0002F38F
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return base.ActivateOn(lord, signal) && Find.TickManager.TicksGame - lord.lastPawnHarmTick >= 300;
		}

		// Token: 0x04002D50 RID: 11600
		private const int MinTicksSinceDamage = 300;
	}
}
