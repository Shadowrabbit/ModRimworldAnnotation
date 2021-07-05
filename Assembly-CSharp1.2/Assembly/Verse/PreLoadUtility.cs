using System;

namespace Verse
{
	// Token: 0x020004A0 RID: 1184
	public static class PreLoadUtility
	{
		// Token: 0x06001D97 RID: 7575 RVA: 0x000F694C File Offset: 0x000F4B4C
		public static void CheckVersionAndLoad(string path, ScribeMetaHeaderUtility.ScribeHeaderMode mode, Action loadAct)
		{
			try
			{
				Scribe.loader.InitLoadingMetaHeaderOnly(path);
				ScribeMetaHeaderUtility.LoadGameDataHeader(mode, false);
				Scribe.loader.FinalizeLoading();
			}
			catch (Exception ex)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Exception loading ",
					path,
					": ",
					ex
				}), false);
				Scribe.ForceStop();
			}
			if (!ScribeMetaHeaderUtility.TryCreateDialogsForVersionMismatchWarnings(loadAct))
			{
				loadAct();
			}
		}
	}
}
