using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200047C RID: 1148
	internal class Deque<T>
	{
		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x060022BA RID: 8890 RVA: 0x000DB444 File Offset: 0x000D9644
		public bool Empty
		{
			get
			{
				return this.count == 0;
			}
		}

		// Token: 0x060022BB RID: 8891 RVA: 0x000DB44F File Offset: 0x000D964F
		public Deque()
		{
			this.data = new T[8];
			this.first = 0;
			this.count = 0;
		}

		// Token: 0x060022BC RID: 8892 RVA: 0x000DB474 File Offset: 0x000D9674
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

		// Token: 0x060022BD RID: 8893 RVA: 0x000DB4D4 File Offset: 0x000D96D4
		public void PushBack(T item)
		{
			this.PushPrep();
			T[] array = this.data;
			int num = this.first;
			int num2 = this.count;
			this.count = num2 + 1;
			array[(num + num2) % this.data.Length] = item;
		}

		// Token: 0x060022BE RID: 8894 RVA: 0x000DB514 File Offset: 0x000D9714
		public T PopFront()
		{
			T result = this.data[this.first];
			this.data[this.first] = default(T);
			this.first = (this.first + 1) % this.data.Length;
			this.count--;
			return result;
		}

		// Token: 0x060022BF RID: 8895 RVA: 0x000DB571 File Offset: 0x000D9771
		public void Clear()
		{
			this.first = 0;
			this.count = 0;
		}

		// Token: 0x060022C0 RID: 8896 RVA: 0x000DB584 File Offset: 0x000D9784
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

		// Token: 0x040015D8 RID: 5592
		private T[] data;

		// Token: 0x040015D9 RID: 5593
		private int first;

		// Token: 0x040015DA RID: 5594
		private int count;
	}
}
