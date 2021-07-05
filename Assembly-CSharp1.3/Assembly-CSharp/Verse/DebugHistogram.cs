using System;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003B9 RID: 953
	public class DebugHistogram
	{
		// Token: 0x06001D7F RID: 7551 RVA: 0x000B807B File Offset: 0x000B627B
		public DebugHistogram(float[] buckets)
		{
			this.buckets = buckets.Concat(float.PositiveInfinity).ToArray<float>();
			this.counts = new int[this.buckets.Length];
		}

		// Token: 0x06001D80 RID: 7552 RVA: 0x000B80AC File Offset: 0x000B62AC
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

		// Token: 0x06001D81 RID: 7553 RVA: 0x000B80EC File Offset: 0x000B62EC
		public void Display()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.Display(stringBuilder);
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06001D82 RID: 7554 RVA: 0x000B8114 File Offset: 0x000B6314
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

		// Token: 0x040011AC RID: 4524
		private float[] buckets;

		// Token: 0x040011AD RID: 4525
		private int[] counts;
	}
}
