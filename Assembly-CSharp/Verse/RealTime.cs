using System;
using System.Threading;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000865 RID: 2149
	public static class RealTime
	{
		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x0600359F RID: 13727 RVA: 0x000299BB File Offset: 0x00027BBB
		public static float LastRealTime
		{
			get
			{
				return RealTime.lastRealTime;
			}
		}

		// Token: 0x060035A0 RID: 13728 RVA: 0x00159D9C File Offset: 0x00157F9C
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

		// Token: 0x0400254E RID: 9550
		public static float deltaTime;

		// Token: 0x0400254F RID: 9551
		public static float realDeltaTime;

		// Token: 0x04002550 RID: 9552
		public static RealtimeMoteList moteList = new RealtimeMoteList();

		// Token: 0x04002551 RID: 9553
		public static int frameCount;

		// Token: 0x04002552 RID: 9554
		private static float lastRealTime = 0f;
	}
}
