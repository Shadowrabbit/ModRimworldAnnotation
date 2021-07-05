using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200004C RID: 76
	public static class FullPool<T> where T : IFullPoolable, new()
	{
		// Token: 0x060003C6 RID: 966 RVA: 0x00014BFB File Offset: 0x00012DFB
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

		// Token: 0x060003C7 RID: 967 RVA: 0x00014C3B File Offset: 0x00012E3B
		public static void Return(T item)
		{
			item.Reset();
			FullPool<T>.freeItems.Add(item);
		}

		// Token: 0x04000118 RID: 280
		private static List<T> freeItems = new List<T>();
	}
}
