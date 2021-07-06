using System;
using System.Diagnostics;

namespace Verse
{
	// Token: 0x0200088F RID: 2191
	public static class ProfilerThreadCheck
	{
		// Token: 0x06003660 RID: 13920 RVA: 0x0002A399 File Offset: 0x00028599
		[Conditional("UNITY_EDITOR")]
		[Conditional("BUILD_AND_RUN")]
		public static void BeginSample(string name)
		{
			bool isInMainThread = UnityData.IsInMainThread;
		}

		// Token: 0x06003661 RID: 13921 RVA: 0x0002A399 File Offset: 0x00028599
		[Conditional("UNITY_EDITOR")]
		[Conditional("BUILD_AND_RUN")]
		public static void EndSample()
		{
			bool isInMainThread = UnityData.IsInMainThread;
		}
	}
}
