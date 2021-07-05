using System;
using System.Threading;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004C3 RID: 1219
	public static class RealTime
	{
		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x06002533 RID: 9523 RVA: 0x000E8532 File Offset: 0x000E6732
		public static float LastRealTime
		{
			get
			{
				return RealTime.lastRealTime;
			}
		}

		// Token: 0x06002534 RID: 9524 RVA: 0x000E853C File Offset: 0x000E673C
		public static void Update()
		{
			RealTime.frameCount = Time.frameCount;
			RealTime.deltaTime = Time.deltaTime;
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			RealTime.realDeltaTime = realtimeSinceStartup - RealTime.lastRealTime;
			RealTime.lastRealTime = realtimeSinceStartup;
			if (Current.ProgramState == ProgramState.Playing)
			{
				RealTime.moteList.MoteListUpdate();
			}
			else
			{
				RealTime.moteList.Clear();
			}
			if (DebugSettings.lowFPS && Time.deltaTime < 100f)
			{
				Thread.Sleep((int)(100f - Time.deltaTime));
			}
		}

		// Token: 0x0400172C RID: 5932
		public static float deltaTime;

		// Token: 0x0400172D RID: 5933
		public static float realDeltaTime;

		// Token: 0x0400172E RID: 5934
		public static RealtimeMoteList moteList = new RealtimeMoteList();

		// Token: 0x0400172F RID: 5935
		public static int frameCount;

		// Token: 0x04001730 RID: 5936
		private static float lastRealTime = 0f;
	}
}
