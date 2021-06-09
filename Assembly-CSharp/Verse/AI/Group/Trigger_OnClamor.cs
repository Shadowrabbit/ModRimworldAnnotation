using System;

namespace Verse.AI.Group
{
	// Token: 0x02000AFE RID: 2814
	public class Trigger_OnClamor : Trigger
	{
		// Token: 0x06004215 RID: 16917 RVA: 0x000313A7 File Offset: 0x0002F5A7
		public Trigger_OnClamor(ClamorDef clamorType)
		{
			this.clamorType = clamorType;
		}

		// Token: 0x06004216 RID: 16918 RVA: 0x000313B6 File Offset: 0x0002F5B6
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Clamor && signal.clamorType == this.clamorType;
		}

		// Token: 0x04002D5D RID: 11613
		private ClamorDef clamorType;
	}
}
