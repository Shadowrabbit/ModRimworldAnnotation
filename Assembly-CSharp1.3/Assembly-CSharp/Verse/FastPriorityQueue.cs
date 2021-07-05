using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020004A8 RID: 1192
	public class FastPriorityQueue<T>
	{
		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06002410 RID: 9232 RVA: 0x000E065A File Offset: 0x000DE85A
		public int Count
		{
			get
			{
				return this.innerList.Count;
			}
		}

		// Token: 0x06002411 RID: 9233 RVA: 0x000E0667 File Offset: 0x000DE867
		public FastPriorityQueue()
		{
			this.comparer = Comparer<T>.Default;
		}

		// Token: 0x06002412 RID: 9234 RVA: 0x000E0685 File Offset: 0x000DE885
		public FastPriorityQueue(IComparer<T> comparer)
		{
			this.comparer = comparer;
		}

		// Token: 0x06002413 RID: 9235 RVA: 0x000E06A0 File Offset: 0x000DE8A0
		public void Push(T item)
		{
			int num = this.innerList.Count;
			this.innerList.Add(item);
			while (num != 0)
			{
				int num2 = (num - 1) / 2;
				if (this.CompareElements(num, num2) >= 0)
				{
					break;
				}
				this.SwapElements(num, num2);
				num = num2;
			}
		}

		// Token: 0x06002414 RID: 9236 RVA: 0x000E06E8 File Offset: 0x000DE8E8
		public T Pop()
		{
			T result = this.innerList[0];
			int num = 0;
			int count = this.innerList.Count;
			this.innerList[0] = this.innerList[count - 1];
			this.innerList.RemoveAt(count - 1);
			count = this.innerList.Count;
			for (;;)
			{
				int num2 = num;
				int num3 = 2 * num + 1;
				int num4 = num3 + 1;
				if (num3 < count && this.CompareElements(num, num3) > 0)
				{
					num = num3;
				}
				if (num4 < count && this.CompareElements(num, num4) > 0)
				{
					num = num4;
				}
				if (num == num2)
				{
					break;
				}
				this.SwapElements(num, num2);
			}
			return result;
		}

		// Token: 0x06002415 RID: 9237 RVA: 0x000E078A File Offset: 0x000DE98A
		public void Clear()
		{
			this.innerList.Clear();
		}

		// Token: 0x06002416 RID: 9238 RVA: 0x000E0798 File Offset: 0x000DE998
		protected void SwapElements(int i, int j)
		{
			T value = this.innerList[i];
			this.innerList[i] = this.innerList[j];
			this.innerList[j] = value;
		}

		// Token: 0x06002417 RID: 9239 RVA: 0x000E07D7 File Offset: 0x000DE9D7
		protected int CompareElements(int i, int j)
		{
			return this.comparer.Compare(this.innerList[i], this.innerList[j]);
		}

		// Token: 0x040016B9 RID: 5817
		protected List<T> innerList = new List<T>();

		// Token: 0x040016BA RID: 5818
		protected IComparer<T> comparer;
	}
}
