using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x02000689 RID: 1673
	public static class TriggerFilterExtension
	{
		// Token: 0x06002F14 RID: 12052 RVA: 0x001180EB File Offset: 0x001162EB
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
