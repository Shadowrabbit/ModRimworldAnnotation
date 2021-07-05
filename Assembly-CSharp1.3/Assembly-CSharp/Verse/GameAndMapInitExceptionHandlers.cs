using System;
using System.Linq;

namespace Verse
{
	// Token: 0x020001F0 RID: 496
	public static class GameAndMapInitExceptionHandlers
	{
		// Token: 0x06000DF4 RID: 3572 RVA: 0x0004EC18 File Offset: 0x0004CE18
		public static void ErrorWhileLoadingAssets(Exception e)
		{
			string text = "ErrorWhileLoadingAssets".Translate();
			if (!ModsConfig.ActiveModsInLoadOrder.Any((ModMetaData x) => !x.Official))
			{
				if (ModsConfig.ActiveModsInLoadOrder.Any((ModMetaData x) => x.IsCoreMod))
				{
					goto IL_86;
				}
			}
			text += "\n\n" + "ErrorWhileLoadingAssets_ModsInfo".Translate();
			IL_86:
			DelayedErrorWindowRequest.Add(text, "ErrorWhileLoadingAssetsTitle".Translate());
			GenScene.GoToMainMenu();
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x0004ECC5 File Offset: 0x0004CEC5
		public static void ErrorWhileGeneratingMap(Exception e)
		{
			DelayedErrorWindowRequest.Add("ErrorWhileGeneratingMap".Translate(), "ErrorWhileGeneratingMapTitle".Translate());
			Scribe.ForceStop();
			GenScene.GoToMainMenu();
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x0004ECF4 File Offset: 0x0004CEF4
		public static void ErrorWhileLoadingGame(Exception e)
		{
			string text = "ErrorWhileLoadingMap".Translate();
			string value;
			string value2;
			if (!ScribeMetaHeaderUtility.LoadedModsMatchesActiveMods(out value, out value2))
			{
				text += "\n\n" + "ModsMismatchWarningText".Translate(value, value2);
			}
			DelayedErrorWindowRequest.Add(text, "ErrorWhileLoadingMapTitle".Translate());
			Scribe.ForceStop();
			GenScene.GoToMainMenu();
		}
	}
}
