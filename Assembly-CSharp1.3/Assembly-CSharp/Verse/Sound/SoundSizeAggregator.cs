using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x02000573 RID: 1395
	public class SoundSizeAggregator
	{
		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x0600291D RID: 10525 RVA: 0x000F8D8C File Offset: 0x000F6F8C
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

		// Token: 0x0600291E RID: 10526 RVA: 0x000F8DFC File Offset: 0x000F6FFC
		public SoundSizeAggregator()
		{
			this.testSize = Rand.Value * 3f;
			this.testSize *= this.testSize;
		}

		// Token: 0x0600291F RID: 10527 RVA: 0x000F8E33 File Offset: 0x000F7033
		public void RegisterReporter(ISizeReporter newRep)
		{
			this.reporters.Add(newRep);
		}

		// Token: 0x06002920 RID: 10528 RVA: 0x000F8E41 File Offset: 0x000F7041
		public void RemoveReporter(ISizeReporter oldRep)
		{
			this.reporters.Remove(oldRep);
		}

		// Token: 0x04001978 RID: 6520
		private List<ISizeReporter> reporters = new List<ISizeReporter>();

		// Token: 0x04001979 RID: 6521
		private float testSize;
	}
}
