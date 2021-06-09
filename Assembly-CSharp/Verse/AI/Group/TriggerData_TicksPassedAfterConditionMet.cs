using System;

namespace Verse.AI.Group
{
	// Token: 0x02000AF7 RID: 2807
	public class TriggerData_TicksPassedAfterConditionMet : TriggerData_TicksPassed
	{
		// Token: 0x06004207 RID: 16903 RVA: 0x00031249 File Offset: 0x0002F449
		public override void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.conditionMet, "conditionMet", false, false);
		}

		// Token: 0x04002D54 RID: 11604
		public bool conditionMet;
	}
}
