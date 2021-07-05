using System;

namespace Verse.AI.Group
{
	// Token: 0x02000694 RID: 1684
	public class TriggerData_TicksPassedAfterConditionMet : TriggerData_TicksPassed
	{
		// Token: 0x06002F31 RID: 12081 RVA: 0x0011857B File Offset: 0x0011677B
		public override void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.conditionMet, "conditionMet", false, false);
		}

		// Token: 0x04001CDD RID: 7389
		public bool conditionMet;
	}
}
