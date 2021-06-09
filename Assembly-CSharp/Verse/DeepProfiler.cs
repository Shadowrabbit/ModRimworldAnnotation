using System;
using System.Collections.Generic;
using System.Threading;

namespace Verse
{
	// Token: 0x02000803 RID: 2051
	public static class DeepProfiler
	{
		// Token: 0x060033B9 RID: 13241 RVA: 0x001506A0 File Offset: 0x0014E8A0
		public static ThreadLocalDeepProfiler Get()
		{
			object deepProfilersLock = DeepProfiler.DeepProfilersLock;
			ThreadLocalDeepProfiler result;
			lock (deepProfilersLock)
			{
				int managedThreadId = Thread.CurrentThread.ManagedThreadId;
				ThreadLocalDeepProfiler threadLocalDeepProfiler;
				if (!DeepProfiler.deepProfilers.TryGetValue(managedThreadId, out threadLocalDeepProfiler))
				{
					threadLocalDeepProfiler = new ThreadLocalDeepProfiler();
					DeepProfiler.deepProfilers.Add(managedThreadId, threadLocalDeepProfiler);
					result = threadLocalDeepProfiler;
				}
				else
				{
					result = threadLocalDeepProfiler;
				}
			}
			return result;
		}

		// Token: 0x060033BA RID: 13242 RVA: 0x00028914 File Offset: 0x00026B14
		public static void Start(string label = null)
		{
			if (!DeepProfiler.enabled || !Prefs.LogVerbose)
			{
				return;
			}
			DeepProfiler.Get().Start(label);
		}

		// Token: 0x060033BB RID: 13243 RVA: 0x00028932 File Offset: 0x00026B32
		public static void End()
		{
			if (!DeepProfiler.enabled || !Prefs.LogVerbose)
			{
				return;
			}
			DeepProfiler.Get().End();
		}

		// Token: 0x040023E1 RID: 9185
		public static volatile bool enabled = true;

		// Token: 0x040023E2 RID: 9186
		private static Dictionary<int, ThreadLocalDeepProfiler> deepProfilers = new Dictionary<int, ThreadLocalDeepProfiler>();

		// Token: 0x040023E3 RID: 9187
		private static readonly object DeepProfilersLock = new object();
	}
}
