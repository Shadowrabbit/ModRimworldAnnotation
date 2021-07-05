using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200004D RID: 77
	public class SimpleLinearPool<T> where T : new()
	{
		// Token: 0x060003C9 RID: 969 RVA: 0x00014C64 File Offset: 0x00012E64
		public T Get()
		{
			if (this.readIndex >= this.items.Count)
			{
				this.items.Add(Activator.CreateInstance<T>());
			}
			List<T> list = this.items;
			int num = this.readIndex;
			this.readIndex = num + 1;
			return list[num];
		}

		// Token: 0x060003CA RID: 970 RVA: 0x00014CB0 File Offset: 0x00012EB0
		public void Clear()
		{
			this.readIndex = 0;
		}

		// Token: 0x04000119 RID: 281
		private List<T> items = new List<T>();

		// Token: 0x0400011A RID: 282
		private int readIndex;
	}
}
