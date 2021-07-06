using System;

namespace Verse
{
	// Token: 0x0200006D RID: 109
	public static class UnityDataInitializer
	{
		// Token: 0x0600045B RID: 1115 RVA: 0x0008782C File Offset: 0x00085A2C
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

		// Token: 0x040001E0 RID: 480
		public static bool initializing;
	}
}
