using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020007EC RID: 2028
	public static class ThingCountUtility
	{
		// Token: 0x06003349 RID: 13129 RVA: 0x0014F5DC File Offset: 0x0014D7DC
		public static int CountOf(List<ThingCount> list, Thing thing)
		{
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Thing == thing)
				{
					num += list[i].Count;
				}
			}
			return num;
		}

		// Token: 0x0600334A RID: 13130 RVA: 0x0014F624 File Offset: 0x0014D824
		public static void AddToList(List<ThingCount> list, Thing thing, int countToAdd)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Thing == thing)
				{
					list[i] = list[i].WithCount(list[i].Count + countToAdd);
					return;
				}
			}
			list.Add(new ThingCount(thing, countToAdd));
		}
	}
}
