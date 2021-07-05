using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x02000685 RID: 1669
	public abstract class Trigger
	{
		// Token: 0x06002F0A RID: 12042
		public abstract bool ActivateOn(Lord lord, TriggerSignal signal);

		// Token: 0x06002F0B RID: 12043 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
		}

		// Token: 0x06002F0C RID: 12044 RVA: 0x00118001 File Offset: 0x00116201
		public override string ToString()
		{
			return base.GetType().ToString();
		}

		// Token: 0x04001CBB RID: 7355
		public TriggerData data;

		// Token: 0x04001CBC RID: 7356
		public List<TriggerFilter> filters;
	}
}
