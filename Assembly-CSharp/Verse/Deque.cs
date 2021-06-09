using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020007DB RID: 2011
	internal class Deque<T>
	{
		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x06003281 RID: 12929 RVA: 0x00027924 File Offset: 0x00025B24
		public bool Empty
		{
			get
			{
				return this.count == 0;
			}
		}

		// Token: 0x06003282 RID: 12930 RVA: 0x0002792F File Offset: 0x00025B2F
		public Deque()
		{
			this.data = new T[8];
			this.first = 0;
			this.count = 0;
		}

		// Token: 0x06003283 RID: 12931 RVA: 0x0014D7A8 File Offset: 0x0014B9A8
		public void PushFront(T item)
		{
			this.PushPrep();
			this.first--;
			if (this.first < 0)
			{
				this.first += this.data.Length;
			}
			this.count++;
			this.data[this.first] = item;
		}

		// Token: 0x06003284 RID: 12932 RVA: 0x0014D808 File Offset: 0x0014BA08
		public void PushBack(T item)
		{
			this.PushPrep();
			T[] array = this.data;
			int num = this.first;
			int num2 = this.count;
			this.count = num2 + 1;
			array[(num + num2) % this.data.Length] = item;
		}

		// Token: 0x06003285 RID: 12933 RVA: 0x0014D848 File Offset: 0x0014BA48
		public T PopFront()
		{
			T result = this.data[this.first];
			this.data[this.first] = default(T);
			this.first = (this.first + 1) % this.data.Length;
			this.count--;
			return result;
		}

		// Token: 0x06003286 RID: 12934 RVA: 0x00027951 File Offset: 0x00025B51
		public void Clear()
		{
			this.first = 0;
			this.count = 0;
		}

		// Token: 0x06003287 RID: 12935 RVA: 0x0014D8A8 File Offset: 0x0014BAA8
		private void PushPrep()
		{
			if (this.count < this.data.Length)
			{
				return;
			}
			T[] destinationArray = new T[this.data.Length * 2];
			Array.Copy(this.data, this.first, destinationArray, 0, Mathf.Min(this.count, this.data.Length - this.first));
			if (this.first + this.count > this.data.Length)
			{
				Array.Copy(this.data, 0, destinationArray, this.data.Length - this.first, this.count - this.data.Length + this.first);
			}
			this.data = destinationArray;
			this.first = 0;
		}

		// Token: 0x04002300 RID: 8960
		private T[] data;

		// Token: 0x04002301 RID: 8961
		private int first;

		// Token: 0x04002302 RID: 8962
		private int count;
	}
}
