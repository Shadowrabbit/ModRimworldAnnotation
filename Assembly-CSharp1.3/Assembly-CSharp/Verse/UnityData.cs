using System;
using System.Threading;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000035 RID: 53
	public static class UnityData
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000325 RID: 805 RVA: 0x00011355 File Offset: 0x0000F555
		public static bool IsInMainThread
		{
			get
			{
				return UnityData.mainThreadId == Thread.CurrentThread.ManagedThreadId;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000326 RID: 806 RVA: 0x00011368 File Offset: 0x0000F568
		public static bool Is32BitBuild
		{
			get
			{
				return IntPtr.Size == 4;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000327 RID: 807 RVA: 0x00011372 File Offset: 0x0000F572
		public static bool Is64BitBuild
		{
			get
			{
				return IntPtr.Size == 8;
			}
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0001137C File Offset: 0x0000F57C
		static UnityData()
		{
			if (!UnityData.initialized && !UnityDataInitializer.initializing)
			{
				Log.Warning("Used UnityData before it's initialized.");
			}
		}

		// Token: 0x06000329 RID: 809 RVA: 0x00011396 File Offset: 0x0000F596
		public static void CopyUnityData()
		{
			UnityData.mainThreadId = Thread.CurrentThread.ManagedThreadId;
			UnityData.isEditor = Application.isEditor;
			UnityData.dataPath = Application.dataPath;
			UnityData.platform = Application.platform;
			UnityData.persistentDataPath = Application.persistentDataPath;
			UnityData.initialized = true;
		}

		// Token: 0x04000097 RID: 151
		private static bool initialized;

		// Token: 0x04000098 RID: 152
		public static bool isEditor;

		// Token: 0x04000099 RID: 153
		public static string dataPath;

		// Token: 0x0400009A RID: 154
		public static RuntimePlatform platform;

		// Token: 0x0400009B RID: 155
		public static string persistentDataPath;

		// Token: 0x0400009C RID: 156
		private static int mainThreadId;
	}
}
