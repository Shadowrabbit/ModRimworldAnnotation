using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200013F RID: 319
	public class TimeSlower
	{
		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x060008DC RID: 2268 RVA: 0x000296D1 File Offset: 0x000278D1
		public bool ForcedNormalSpeed
		{
			get
			{
				return !DebugViewSettings.neverForceNormalSpeed && Find.TickManager.TicksGame < this.forceNormalSpeedUntil;
			}
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x000296EE File Offset: 0x000278EE
		public void SignalForceNormalSpeed()
		{
			this.forceNormalSpeedUntil = Mathf.Max(new int[]
			{
				Find.TickManager.TicksGame + 800
			});
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x00029714 File Offset: 0x00027914
		public void SignalForceNormalSpeedShort()
		{
			this.forceNormalSpeedUntil = Mathf.Max(this.forceNormalSpeedUntil, Find.TickManager.TicksGame + 240);
		}

		// Token: 0x04000825 RID: 2085
		private int forceNormalSpeedUntil;

		// Token: 0x04000826 RID: 2086
		private const int ForceTicksStandard = 800;

		// Token: 0x04000827 RID: 2087
		private const int ForceTicksShort = 240;
	}
}
