using System;

namespace Verse
{
	// Token: 0x020002C2 RID: 706
	public class FloodFillRangeQueue
	{
		// Token: 0x17000348 RID: 840
		// (get) Token: 0x060011DA RID: 4570 RVA: 0x00012E4F File Offset: 0x0001104F
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x060011DB RID: 4571 RVA: 0x00012E57 File Offset: 0x00011057
		public FloodFillRange First
		{
			get
			{
				return this.array[this.head];
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x060011DC RID: 4572 RVA: 0x000C3B5C File Offset: 0x000C1D5C
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

		// Token: 0x060011DD RID: 4573 RVA: 0x00012E6A File Offset: 0x0001106A
		public FloodFillRangeQueue(int initialSize)
		{
			this.array = new FloodFillRange[initialSize];
			this.head = 0;
			this.count = 0;
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x000C3BD8 File Offset: 0x000C1DD8
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

		// Token: 0x060011DF RID: 4575 RVA: 0x000C3C78 File Offset: 0x000C1E78
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

		// Token: 0x04000E69 RID: 3689
		private FloodFillRange[] array;

		// Token: 0x04000E6A RID: 3690
		private int count;

		// Token: 0x04000E6B RID: 3691
		private int head;

		// Token: 0x04000E6C RID: 3692
		private int debugNumTimesExpanded;

		// Token: 0x04000E6D RID: 3693
		private int debugMaxUsedSpace;
	}
}
