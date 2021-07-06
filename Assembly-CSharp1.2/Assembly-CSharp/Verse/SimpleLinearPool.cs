using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000093 RID: 147
	public class SimpleLinearPool<T> where T : new()
	{
		// Token: 0x0600051A RID: 1306 RVA: 0x0008AC64 File Offset: 0x00088E64
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

		// Token: 0x0600051B RID: 1307 RVA: 0x0000A6DC File Offset: 0x000088DC
		public void Clear()
		{
			this.readIndex = 0;
		}

		// Token: 0x04000281 RID: 641
		private List<T> items = new List<T>();

		// Token: 0x04000282 RID: 642
		private int readIndex;
	}
}
