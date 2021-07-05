using System;

namespace Verse.AI.Group
{
	// Token: 0x02000696 RID: 1686
	public class TriggerData_Reason : TriggerData
	{
		// Token: 0x06002F35 RID: 12085 RVA: 0x001185B4 File Offset: 0x001167B4
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.reason, "reason", null, false);
		}

		// Token: 0x04001CDF RID: 7391
		public string reason;
	}
}
