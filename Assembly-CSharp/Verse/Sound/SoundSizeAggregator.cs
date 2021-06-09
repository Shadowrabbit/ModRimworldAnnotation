using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x0200094E RID: 2382
	public class SoundSizeAggregator
	{
		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x06003A73 RID: 14963 RVA: 0x00169EB0 File Offset: 0x001680B0
		public float AggregateSize
		{
			get
			{
				if (this.reporters.Count == 0)
				{
					return this.testSize;
				}
				float num = 0f;
				foreach (ISizeReporter sizeReporter in this.reporters)
				{
					num += sizeReporter.CurrentSize();
				}
				return num;
			}
		}

		// Token: 0x06003A74 RID: 14964 RVA: 0x0002D05C File Offset: 0x0002B25C
		public SoundSizeAggregator()
		{
			this.testSize = Rand.Value * 3f;
			this.testSize *= this.testSize;
		}

		// Token: 0x06003A75 RID: 14965 RVA: 0x0002D093 File Offset: 0x0002B293
		public void RegisterReporter(ISizeReporter newRep)
		{
			this.reporters.Add(newRep);
		}

		// Token: 0x06003A76 RID: 14966 RVA: 0x0002D0A1 File Offset: 0x0002B2A1
		public void RemoveReporter(ISizeReporter oldRep)
		{
			this.reporters.Remove(oldRep);
		}

		// Token: 0x04002880 RID: 10368
		private List<ISizeReporter> reporters = new List<ISizeReporter>();

		// Token: 0x04002881 RID: 10369
		private float testSize;
	}
}
