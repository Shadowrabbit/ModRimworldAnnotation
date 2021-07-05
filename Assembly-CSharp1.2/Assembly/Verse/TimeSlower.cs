using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001E3 RID: 483
	public class TimeSlower
	{
		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000C98 RID: 3224 RVA: 0x0000FBC1 File Offset: 0x0000DDC1
		public bool ForcedNormalSpeed
		{
			get
			{
				return Find.TickManager.TicksGame < this.forceNormalSpeedUntil;
			}
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x0000FBD5 File Offset: 0x0000DDD5
		public void SignalForceNormalSpeed()
		{
			this.forceNormalSpeedUntil = Mathf.Max(new int[]
			{
				Find.TickManager.TicksGame + 800
			});
		}

		// Token: 0x06000C9A RID: 3226 RVA: 0x0000FBFB File Offset: 0x0000DDFB
		public void SignalForceNormalSpeedShort()
		{
			this.forceNormalSpeedUntil = Mathf.Max(this.forceNormalSpeedUntil, Find.TickManager.TicksGame + 240);
		}

		// Token: 0x04000AE8 RID: 2792
		private int forceNormalSpeedUntil;

		// Token: 0x04000AE9 RID: 2793
		private const int ForceTicksStandard = 800;

		// Token: 0x04000AEA RID: 2794
		private const int ForceTicksShort = 240;
	}
}
