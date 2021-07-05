using System;

namespace Verse.AI.Group
{
	// Token: 0x020006AC RID: 1708
	public class Trigger_Memo : Trigger
	{
		// Token: 0x06002F65 RID: 12133 RVA: 0x00118D45 File Offset: 0x00116F45
		public Trigger_Memo(string memo)
		{
			this.memo = memo;
		}

		// Token: 0x06002F66 RID: 12134 RVA: 0x00118D54 File Offset: 0x00116F54
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Memo && signal.memo == this.memo;
		}

		// Token: 0x04001CF3 RID: 7411
		private string memo;
	}
}
