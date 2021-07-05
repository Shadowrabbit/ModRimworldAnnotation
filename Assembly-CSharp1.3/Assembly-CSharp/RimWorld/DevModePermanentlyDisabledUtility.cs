using System;
using System.IO;
using Verse;

namespace RimWorld
{
	// Token: 0x0200147C RID: 5244
	public static class DevModePermanentlyDisabledUtility
	{
		// Token: 0x170015BE RID: 5566
		// (get) Token: 0x06007D6E RID: 32110 RVA: 0x002C4D6A File Offset: 0x002C2F6A
		public static bool Disabled
		{
			get
			{
				if (!DevModePermanentlyDisabledUtility.initialized)
				{
					DevModePermanentlyDisabledUtility.initialized = true;
					DevModePermanentlyDisabledUtility.disabled = File.Exists(GenFilePaths.DevModePermanentlyDisabledFilePath);
				}
				return DevModePermanentlyDisabledUtility.disabled;
			}
		}

		// Token: 0x06007D6F RID: 32111 RVA: 0x002C4D90 File Offset: 0x002C2F90
		public static void Disable()
		{
			try
			{
				File.Create(GenFilePaths.DevModePermanentlyDisabledFilePath).Dispose();
			}
			catch (Exception arg)
			{
				Log.Error("Could not permanently disable dev mode: " + arg);
				return;
			}
			DevModePermanentlyDisabledUtility.disabled = true;
			Prefs.DevMode = false;
		}

		// Token: 0x04004E3A RID: 20026
		private static bool initialized;

		// Token: 0x04004E3B RID: 20027
		private static bool disabled;
	}
}
