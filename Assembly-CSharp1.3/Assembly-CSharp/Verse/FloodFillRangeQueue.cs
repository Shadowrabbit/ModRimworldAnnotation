using System;

namespace Verse
{
	// Token: 0x020001F5 RID: 501
	public class FloodFillRangeQueue
	{
		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000E1C RID: 3612 RVA: 0x0004F591 File Offset: 0x0004D791
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000E1D RID: 3613 RVA: 0x0004F599 File Offset: 0x0004D799
		public FloodFillRange First
		{
			get
			{
				return this.array[this.head];
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000E1E RID: 3614 RVA: 0x0004F5AC File Offset: 0x0004D7AC
		public string PerfDebugString
		{
			get
			{
				return string.Concat(new object[]
				{
					"NumTimesExpanded: ",
					this.debugNumTimesExpanded,
					", MaxUsedSize= ",
					this.debugMaxUsedSpace,
					", ClaimedSize=",
					this.array.Length,
					", UnusedSpace=",
					this.array.Length - this.debugMaxUsedSpace
				});
			}
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x0004F627 File Offset: 0x0004D827
		public FloodFillRangeQueue(int initialSize)
		{
			this.array = new FloodFillRange[initialSize];
			this.head = 0;
			this.count = 0;
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x0004F64C File Offset: 0x0004D84C
		public void Enqueue(FloodFillRange r)
		{
			if (this.count + this.head == this.array.Length)
			{
				FloodFillRange[] destinationArray = new FloodFillRange[2 * this.array.Length];
				Array.Copy(this.array, this.head, destinationArray, 0, this.count);
				this.array = destinationArray;
				this.head = 0;
				this.debugNumTimesExpanded++;
			}
			FloodFillRange[] array = this.array;
			int num = this.head;
			int num2 = this.count;
			this.count = num2 + 1;
			array[num + num2] = r;
			this.debugMaxUsedSpace = this.count + this.head;
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x0004F6EC File Offset: 0x0004D8EC
		public FloodFillRange Dequeue()
		{
			FloodFillRange result = default(FloodFillRange);
			if (this.count > 0)
			{
				result = this.array[this.head];
				this.array[this.head] = default(FloodFillRange);
				this.head++;
				this.count--;
			}
			return result;
		}

		// Token: 0x04000B7A RID: 2938
		private FloodFillRange[] array;

		// Token: 0x04000B7B RID: 2939
		private int count;

		// Token: 0x04000B7C RID: 2940
		private int head;

		// Token: 0x04000B7D RID: 2941
		private int debugNumTimesExpanded;

		// Token: 0x04000B7E RID: 2942
		private int debugMaxUsedSpace;
	}
}
