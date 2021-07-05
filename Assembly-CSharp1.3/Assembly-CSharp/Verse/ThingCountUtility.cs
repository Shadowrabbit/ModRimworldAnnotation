using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000489 RID: 1161
	public static class ThingCountUtility
	{
		// Token: 0x06002370 RID: 9072 RVA: 0x000DDABC File Offset: 0x000DBCBC
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

		// Token: 0x06002371 RID: 9073 RVA: 0x000DDB04 File Offset: 0x000DBD04
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
