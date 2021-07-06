using System;

namespace Verse.AI.Group
{
	// Token: 0x02000AEE RID: 2798
	public abstract class TriggerFilter
	{
		// Token: 0x060041F2 RID: 16882
		public abstract bool AllowActivation(Lord lord, TriggerSignal signal);
	}
}
