using System;
using System.Diagnostics;

namespace Verse
{
	// Token: 0x020004E3 RID: 1251
	public static class ProfilerThreadCheck
	{
		// Token: 0x060025D0 RID: 9680 RVA: 0x000EA714 File Offset: 0x000E8914
		[Conditional("UNITY_EDITOR")]
		[Conditional("BUILD_AND_RUN")]
		public static void BeginSample(string name)
		{
			bool isInMainThread = UnityData.IsInMainThread;
		}

		// Token: 0x060025D1 RID: 9681 RVA: 0x000EA714 File Offset: 0x000E8914
		[Conditional("UNITY_EDITOR")]
		[Conditional("BUILD_AND_RUN")]
		public static void EndSample()
		{
			bool isInMainThread = UnityData.IsInMainThread;
		}
	}
}
