using System;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006A8 RID: 1704
	public class DebugHistogram
	{
		// Token: 0x06002C64 RID: 11364 RVA: 0x0002352D File Offset: 0x0002172D
		public DebugHistogram(float[] buckets)
		{
			this.buckets = buckets.Concat(float.PositiveInfinity).ToArray<float>();
			this.counts = new int[this.buckets.Length];
		}

		// Token: 0x06002C65 RID: 11365 RVA: 0x0012EF94 File Offset: 0x0012D194
		public void Add(float val)
		{
			for (int i = 0; i < this.buckets.Length; i++)
			{
				if (this.buckets[i] > val)
				{
					this.counts[i]++;
					return;
				}
			}
		}

		// Token: 0x06002C66 RID: 11366 RVA: 0x0012EFD4 File Offset: 0x0012D1D4
		public void Display()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.Display(stringBuilder);
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x06002C67 RID: 11367 RVA: 0x0012EFFC File Offset: 0x0012D1FC
		public void Display(StringBuilder sb)
		{
			int num = Mathf.Max(this.counts.Max(), 1);
			int num2 = this.counts.Aggregate((int a, int b) => a + b);
			for (int i = 0; i < this.buckets.Length; i++)
			{
				sb.AppendLine(string.Format("{0}    {1}: {2} ({3:F2}%)", new object[]
				{
					new string('#', this.counts[i] * 40 / num),
					this.buckets[i],
					this.counts[i],
					(double)this.counts[i] * 100.0 / (double)num2
				}));
			}
		}

		// Token: 0x04001DFE RID: 7678
		private float[] buckets;

		// Token: 0x04001DFF RID: 7679
		private int[] counts;
	}
}
