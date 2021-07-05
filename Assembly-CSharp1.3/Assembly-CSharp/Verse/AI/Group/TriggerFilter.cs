using System;

namespace Verse.AI.Group
{
	// Token: 0x02000687 RID: 1671
	public abstract class TriggerFilter
	{
		// Token: 0x06002F10 RID: 12048
		public abstract bool AllowActivation(Lord lord, TriggerSignal signal);
	}
}
