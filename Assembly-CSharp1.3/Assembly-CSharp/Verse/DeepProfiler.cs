using System;
using System.Collections.Generic;
using System.Threading;

namespace Verse
{
	// Token: 0x02000495 RID: 1173
	public static class DeepProfiler
	{
		// Token: 0x060023CB RID: 9163 RVA: 0x000DECF8 File Offset: 0x000DCEF8
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

		// Token: 0x060023CC RID: 9164 RVA: 0x000DED68 File Offset: 0x000DCF68
		public static void Start(string label = null)
		{
			if (!DeepProfiler.enabled || !Prefs.LogVerbose)
			{
				return;
			}
			DeepProfiler.Get().Start(label);
		}

		// Token: 0x060023CD RID: 9165 RVA: 0x000DED86 File Offset: 0x000DCF86
		public static void End()
		{
			if (!DeepProfiler.enabled || !Prefs.LogVerbose)
			{
				return;
			}
			DeepProfiler.Get().End();
		}

		// Token: 0x04001699 RID: 5785
		public static volatile bool enabled = true;

		// Token: 0x0400169A RID: 5786
		private static Dictionary<int, ThreadLocalDeepProfiler> deepProfilers = new Dictionary<int, ThreadLocalDeepProfiler>();

		// Token: 0x0400169B RID: 5787
		private static readonly object DeepProfilersLock = new object();
	}
}
