using System;

namespace Verse
{
	// Token: 0x02000036 RID: 54
	public static class UnityDataInitializer
	{
		// Token: 0x0600032A RID: 810 RVA: 0x000113D8 File Offset: 0x0000F5D8
		public static void CopyUnityData()
		{
			UnityDataInitializer.initializing = true;
			try
			{
				UnityData.CopyUnityData();
			}
			finally
			{
				UnityDataInitializer.initializing = false;
			}
		}

		// Token: 0x0400009D RID: 157
		public static bool initializing;
	}
}
