using System;

namespace Verse.AI.Group
{
	// Token: 0x02000B0E RID: 2830
	public class Trigger_Memo : Trigger
	{
		// Token: 0x06004237 RID: 16951 RVA: 0x00031579 File Offset: 0x0002F779
		public Trigger_Memo(string memo)
		{
			this.memo = memo;
		}

		// Token: 0x06004238 RID: 16952 RVA: 0x00031588 File Offset: 0x0002F788
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Memo && signal.memo == this.memo;
		}

		// Token: 0x04002D68 RID: 11624
		private string memo;
	}
}
