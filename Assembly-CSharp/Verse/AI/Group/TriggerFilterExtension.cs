using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x02000AF0 RID: 2800
	public static class TriggerFilterExtension
	{
		// Token: 0x060041F6 RID: 16886 RVA: 0x000310E3 File Offset: 0x0002F2E3
		public static Trigger WithFilter(this Trigger t, TriggerFilter f)
		{
			if (t.filters == null)
			{
				t.filters = new List<TriggerFilter>();
			}
			t.filters.Add(f);
			return t;
		}
	}
}
