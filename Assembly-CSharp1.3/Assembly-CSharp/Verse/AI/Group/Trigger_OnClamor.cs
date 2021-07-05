using System;

namespace Verse.AI.Group
{
	// Token: 0x0200069C RID: 1692
	public class Trigger_OnClamor : Trigger
	{
		// Token: 0x06002F42 RID: 12098 RVA: 0x001186FA File Offset: 0x001168FA
		public Trigger_OnClamor(ClamorDef clamorType)
		{
			this.clamorType = clamorType;
		}

		// Token: 0x06002F43 RID: 12099 RVA: 0x00118709 File Offset: 0x00116909
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Clamor && signal.clamorType == this.clamorType;
		}

		// Token: 0x04001CE7 RID: 7399
		private ClamorDef clamorType;
	}
}
