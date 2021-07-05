using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200004A RID: 74
	public static class SimplePool<T> where T : new()
	{
		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x00014B91 File Offset: 0x00012D91
		public static int FreeItemsCount
		{
			get
			{
				return SimplePool<T>.freeItems.Count;
			}
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x00014BA0 File Offset: 0x00012DA0
		public static T Get()
		{
			if (SimplePool<T>.freeItems.Count == 0)
			{
				return Activator.CreateInstance<T>();
			}
			int index = SimplePool<T>.freeItems.Count - 1;
			T result = SimplePool<T>.freeItems[index];
			SimplePool<T>.freeItems.RemoveAt(index);
			return result;
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x00014BE2 File Offset: 0x00012DE2
		public static void Return(T item)
		{
			SimplePool<T>.freeItems.Add(item);
		}

		// Token: 0x04000117 RID: 279
		private static List<T> freeItems = new List<T>();
	}
}
