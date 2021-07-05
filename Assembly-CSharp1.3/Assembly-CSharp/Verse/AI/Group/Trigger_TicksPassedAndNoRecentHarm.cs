using System;

namespace Verse.AI.Group
{
	// Token: 0x02000690 RID: 1680
	public class Trigger_TicksPassedAndNoRecentHarm : Trigger_TicksPassed
	{
		// Token: 0x06002F28 RID: 12072 RVA: 0x00118455 File Offset: 0x00116655
		public Trigger_TicksPassedAndNoRecentHarm(int tickLimit) : base(tickLimit)
		{
		}

		// Token: 0x06002F29 RID: 12073 RVA: 0x0011845E File Offset: 0x0011665E
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return base.ActivateOn(lord, signal) && Find.TickManager.TicksGame - lord.lastPawnHarmTick >= 300;
		}

		// Token: 0x04001CD9 RID: 7385
		private const int MinTicksSinceDamage = 300;
	}
}
