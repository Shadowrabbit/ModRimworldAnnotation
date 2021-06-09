using System;
using System.Threading;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200006C RID: 108
	public static class UnityData
	{
		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000456 RID: 1110 RVA: 0x00009D7F File Offset: 0x00007F7F
		public static bool IsInMainThread
		{
			get
			{
				return UnityData.mainThreadId == Thread.CurrentThread.ManagedThreadId;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000457 RID: 1111 RVA: 0x00009D92 File Offset: 0x00007F92
		public static bool Is32BitBuild
		{
			get
			{
				return IntPtr.Size == 4;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000458 RID: 1112 RVA: 0x00009D9C File Offset: 0x00007F9C
		public static bool Is64BitBuild
		{
			get
			{
				return IntPtr.Size == 8;
			}
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x00009DA6 File Offset: 0x00007FA6
		static UnityData()
		{
			if (!UnityData.initialized && !UnityDataInitializer.initializing)
			{
				Log.Warning("Used UnityData before it's initialized.", false);
			}
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00009DC1 File Offset: 0x00007FC1
		public static void CopyUnityData()
		{
			UnityData.mainThreadId = Thread.CurrentThread.ManagedThreadId;
			UnityData.isEditor = Application.isEditor;
			UnityData.dataPath = Application.dataPath;
			UnityData.platform = Application.platform;
			UnityData.persistentDataPath = Application.persistentDataPath;
			UnityData.initialized = true;
		}

		// Token: 0x040001DA RID: 474
		private static bool initialized;

		// Token: 0x040001DB RID: 475
		public static bool isEditor;

		// Token: 0x040001DC RID: 476
		public static string dataPath;

		// Token: 0x040001DD RID: 477
		public static RuntimePlatform platform;

		// Token: 0x040001DE RID: 478
		public static string persistentDataPath;

		// Token: 0x040001DF RID: 479
		private static int mainThreadId;
	}
}
