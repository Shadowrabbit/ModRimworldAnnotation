using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200081B RID: 2075
	public class FastPriorityQueue<T>
	{
		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x0600340D RID: 13325 RVA: 0x00028CA3 File Offset: 0x00026EA3
		public int Count
		{
			get
			{
				return this.innerList.Count;
			}
		}

		// Token: 0x0600340E RID: 13326 RVA: 0x00028CB0 File Offset: 0x00026EB0
		public FastPriorityQueue()
		{
			this.comparer = Comparer<T>.Default;
		}

		// Token: 0x0600340F RID: 13327 RVA: 0x00028CCE File Offset: 0x00026ECE
		public FastPriorityQueue(IComparer<T> comparer)
		{
			this.comparer = comparer;
		}

		// Token: 0x06003410 RID: 13328 RVA: 0x00151BA0 File Offset: 0x0014FDA0
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

		// Token: 0x06003411 RID: 13329 RVA: 0x00151BE8 File Offset: 0x0014FDE8
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

		// Token: 0x06003412 RID: 13330 RVA: 0x00028CE8 File Offset: 0x00026EE8
		public void Clear()
		{
			this.innerList.Clear();
		}

		// Token: 0x06003413 RID: 13331 RVA: 0x00151C8C File Offset: 0x0014FE8C
		protected void SwapElements(int i, int j)
		{
			T value = this.innerList[i];
			this.innerList[i] = this.innerList[j];
			this.innerList[j] = value;
		}

		// Token: 0x06003414 RID: 13332 RVA: 0x00028CF5 File Offset: 0x00026EF5
		protected int CompareElements(int i, int j)
		{
			return this.comparer.Compare(this.innerList[i], this.innerList[j]);
		}

		// Token: 0x0400240B RID: 9227
		protected List<T> innerList = new List<T>();

		// Token: 0x0400240C RID: 9228
		protected IComparer<T> comparer;
	}
}
