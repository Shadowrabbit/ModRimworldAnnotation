using System;

namespace Verse
{
	// Token: 0x02000056 RID: 86
	public class MoteCounter
	{
		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060003E4 RID: 996 RVA: 0x00015340 File Offset: 0x00013540
		public int MoteCount
		{
			get
			{
				return this.moteCount;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060003E5 RID: 997 RVA: 0x00015348 File Offset: 0x00013548
		public float Saturation
		{
			get
			{
				return (float)this.moteCount / 250f;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060003E6 RID: 998 RVA: 0x00015357 File Offset: 0x00013557
		public bool Saturated
		{
			get
			{
				return this.Saturation > 1f;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060003E7 RID: 999 RVA: 0x00015366 File Offset: 0x00013566
		public bool SaturatedLowPriority
		{
			get
			{
				return this.Saturation > 0.8f;
			}
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x00015375 File Offset: 0x00013575
		public void Notify_MoteSpawned()
		{
			this.moteCount++;
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00015385 File Offset: 0x00013585
		public void Notify_MoteDespawned()
		{
			this.moteCount--;
		}

		// Token: 0x0400012A RID: 298
		private int moteCount;

		// Token: 0x0400012B RID: 299
		private const int SaturatedCount = 250;
	}
}
