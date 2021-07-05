using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000029 RID: 41
	public static class GenList
	{
		// Token: 0x0600023C RID: 572 RVA: 0x0000B987 File Offset: 0x00009B87
		public static int CountAllowNull<T>(this IList<T> list)
		{
			if (list == null)
			{
				return 0;
			}
			return list.Count;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000B994 File Offset: 0x00009B94
		public static bool NullOrEmpty<T>(this IList<T> list)
		{
			return list == null || list.Count == 0;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000B9A4 File Offset: 0x00009BA4
		public static bool HasData<T>(this IList<T> list)
		{
			return list != null && list.Count >= 1;
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000B9B8 File Offset: 0x00009BB8
		public static List<T> ListFullCopy<T>(this List<T> source)
		{
			List<T> list = new List<T>(source.Count);
			for (int i = 0; i < source.Count; i++)
			{
				list.Add(source[i]);
			}
			return list;
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000B9F0 File Offset: 0x00009BF0
		public static List<T> ListFullCopyOrNull<T>(this List<T> source)
		{
			if (source == null)
			{
				return null;
			}
			return source.ListFullCopy<T>();
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000BA00 File Offset: 0x00009C00
		public static void RemoveDuplicates<T>(this List<T> list, Func<T, T, bool> comparer = null) where T : class
		{
			if (list.Count <= 1)
			{
				return;
			}
			for (int i = list.Count - 1; i >= 0; i--)
			{
				for (int j = 0; j < i; j++)
				{
					if ((comparer == null && list[i] == list[j]) || (comparer != null && comparer(list[i], list[j])))
					{
						list.RemoveAt(i);
						break;
					}
				}
			}
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000BA75 File Offset: 0x00009C75
		public static void TruncateToLength<T>(this List<T> list, int maxLength)
		{
			if (list.Count == 0 || list.Count <= maxLength)
			{
				return;
			}
			list.RemoveRange(maxLength, list.Count - maxLength);
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000BA98 File Offset: 0x00009C98
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

		// Token: 0x06000244 RID: 580 RVA: 0x0000BADC File Offset: 0x00009CDC
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
