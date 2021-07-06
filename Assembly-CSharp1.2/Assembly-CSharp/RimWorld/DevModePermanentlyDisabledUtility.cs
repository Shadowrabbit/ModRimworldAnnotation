using System;
using System.IO;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CAA RID: 7338
	public static class DevModePermanentlyDisabledUtility
	{
		// Token: 0x170018CA RID: 6346
		// (get) Token: 0x06009FAD RID: 40877 RVA: 0x0006A7EA File Offset: 0x000689EA
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

		// Token: 0x06009FAE RID: 40878 RVA: 0x002EA8B8 File Offset: 0x002E8AB8
		public static void Disable()
		{
			try
			{
				File.Create(GenFilePaths.DevModePermanentlyDisabledFilePath).Dispose();
			}
			catch (Exception arg)
			{
				Log.Error("Could not permanently disable dev mode: " + arg, false);
				return;
			}
			DevModePermanentlyDisabledUtility.disabled = true;
			Prefs.DevMode = false;
		}

		// Token: 0x04006C79 RID: 27769
		private static bool initialized;

		// Token: 0x04006C7A RID: 27770
		private static bool disabled;
	}
}
