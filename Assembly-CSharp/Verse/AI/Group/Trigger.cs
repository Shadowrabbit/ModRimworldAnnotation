using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x02000AEC RID: 2796
	public abstract class Trigger
	{
		// Token: 0x060041EC RID: 16876
		public abstract bool ActivateOn(Lord lord, TriggerSignal signal);

		// Token: 0x060041ED RID: 16877 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
		}

		// Token: 0x060041EE RID: 16878 RVA: 0x000310CE File Offset: 0x0002F2CE
		public override string ToString()
		{
			return base.GetType().ToString();
		}

		// Token: 0x04002D4C RID: 11596
		public TriggerData data;

		// Token: 0x04002D4D RID: 11597
		public List<TriggerFilter> filters;
	}
}
