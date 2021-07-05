using System;
using System.Linq;

namespace Verse
{
	// Token: 0x020002BB RID: 699
	public static class GameAndMapInitExceptionHandlers
	{
		// Token: 0x060011BE RID: 4542 RVA: 0x000C38E8 File Offset: 0x000C1AE8
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

		// Token: 0x060011BF RID: 4543 RVA: 0x00012DB7 File Offset: 0x00010FB7
		public static void ErrorWhileGeneratingMap(Exception e)
		{
			DelayedErrorWindowRequest.Add("ErrorWhileGeneratingMap".Translate(), "ErrorWhileGeneratingMapTitle".Translate());
			Scribe.ForceStop();
			GenScene.GoToMainMenu();
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x000C3998 File Offset: 0x000C1B98
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
