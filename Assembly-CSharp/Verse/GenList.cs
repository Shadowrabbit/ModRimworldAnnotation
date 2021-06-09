using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000049 RID: 73
	public static class GenList
	{
		// Token: 0x0600030C RID: 780 RVA: 0x00008F52 File Offset: 0x00007152
		public static int CountAllowNull<T>(this IList<T> list)
		{
			if (list == null)
			{
				return 0;
			}
			return list.Count;
		}

		// Token: 0x0600030D RID: 781 RVA: 0x00008F5F File Offset: 0x0000715F
		public static bool NullOrEmpty<T>(this IList<T> list)
		{
			return list == null || list.Count == 0;
		}

		// Token: 0x0600030E RID: 782 RVA: 0x000821EC File Offset: 0x000803EC
		public static List<T> ListFullCopy<T>(this List<T> source)
		{
			List<T> list = new List<T>(source.Count);
			for (int i = 0; i < source.Count; i++)
			{
				list.Add(source[i]);
			}
			return list;
		}

		// Token: 0x0600030F RID: 783 RVA: 0x00008F6F File Offset: 0x0000716F
		public static List<T> ListFullCopyOrNull<T>(this List<T> source)
		{
			if (source == null)
			{
				return null;
			}
			return source.ListFullCopy<T>();
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00082224 File Offset: 0x00080424
		public static void RemoveDuplicates<T>(this List<T> list) where T : class
		{
			if (list.Count <= 1)
			{
				return;
			}
			for (int i = list.Count - 1; i >= 0; i--)
			{
				for (int j = 0; j < i; j++)
				{
					if (list[i] == list[j])
					{
						list.RemoveAt(i);
						break;
					}
				}
			}
		}

		// Token: 0x06000311 RID: 785 RVA: 0x00082280 File Offset: 0x00080480
		public static void Shuffle<T>(this IList<T> list)
		{
			int i = list.Count;
			while (i > 1)
			{
				i--;
				int index = Rand.RangeInclusive(0, i);
				T value = list[index];
				list[index] = list[i];
				list[i] = value;
			}
		}

		// Token: 0x06000312 RID: 786 RVA: 0x000822C4 File Offset: 0x000804C4
		public static void InsertionSort<T>(this IList<T> list, Comparison<T> comparison)
		{
			int count = list.Count;
			for (int i = 1; i < count; i++)
			{
				T t = list[i];
				int num = i - 1;
				while (num >= 0 && comparison(list[num], t) > 0)
				{
					list[num + 1] = list[num];
					num--;
				}
				list[num + 1] = t;
			}
		}
	}
}
