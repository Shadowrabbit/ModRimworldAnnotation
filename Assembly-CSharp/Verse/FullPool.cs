using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000092 RID: 146
	public static class FullPool<T> where T : IFullPoolable, new()
	{
		// Token: 0x06000517 RID: 1303 RVA: 0x0000A676 File Offset: 0x00008876
		public static T Get()
		{
			if (FullPool<T>.freeItems.Count == 0)
			{
				return Activator.CreateInstance<T>();
			}
			T result = FullPool<T>.freeItems[FullPool<T>.freeItems.Count - 1];
			FullPool<T>.freeItems.RemoveAt(FullPool<T>.freeItems.Count - 1);
			return result;
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x0000A6B6 File Offset: 0x000088B6
		public static void Return(T item)
		{
			item.Reset();
			FullPool<T>.freeItems.Add(item);
		}

		// Token: 0x04000280 RID: 640
		private static List<T> freeItems = new List<T>();
	}
}
