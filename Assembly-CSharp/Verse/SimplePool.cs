using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000090 RID: 144
	public static class SimplePool<T> where T : new()
	{
		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000512 RID: 1298 RVA: 0x0000A651 File Offset: 0x00008851
		public static int FreeItemsCount
		{
			get
			{
				return SimplePool<T>.freeItems.Count;
			}
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x0008AC20 File Offset: 0x00088E20
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

		// Token: 0x06000514 RID: 1300 RVA: 0x0000A65D File Offset: 0x0000885D
		public static void Return(T item)
		{
			SimplePool<T>.freeItems.Add(item);
		}

		// Token: 0x0400027F RID: 639
		private static List<T> freeItems = new List<T>();
	}
}
