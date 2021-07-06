using System;

namespace Verse
{
	// Token: 0x0200009D RID: 157
	public class MoteCounter
	{
		// Token: 0x170000EE RID: 238
		// (get) Token: 0x0600053C RID: 1340 RVA: 0x0000A7B1 File Offset: 0x000089B1
		public int MoteCount
		{
			get
			{
				return this.moteCount;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x0600053D RID: 1341 RVA: 0x0000A7B9 File Offset: 0x000089B9
		public float Saturation
		{
			get
			{
				return (float)this.moteCount / 250f;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x0600053E RID: 1342 RVA: 0x0000A7C8 File Offset: 0x000089C8
		public bool Saturated
		{
			get
			{
				return this.Saturation > 1f;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x0600053F RID: 1343 RVA: 0x0000A7D7 File Offset: 0x000089D7
		public bool SaturatedLowPriority
		{
			get
			{
				return this.Saturation > 0.8f;
			}
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0000A7E6 File Offset: 0x000089E6
		public void Notify_MoteSpawned()
		{
			this.moteCount++;
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0000A7F6 File Offset: 0x000089F6
		public void Notify_MoteDespawned()
		{
			this.moteCount--;
		}

		// Token: 0x04000295 RID: 661
		private int moteCount;

		// Token: 0x04000296 RID: 662
		private const int SaturatedCount = 250;
	}
}
