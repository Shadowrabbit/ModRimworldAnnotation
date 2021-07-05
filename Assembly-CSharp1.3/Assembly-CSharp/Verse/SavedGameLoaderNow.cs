using System;
using System.Linq;

namespace Verse
{
	// Token: 0x020001F1 RID: 497
	public class SavedGameLoaderNow
	{
		// Token: 0x06000DF7 RID: 3575 RVA: 0x0004ED68 File Offset: 0x0004CF68
		public static void LoadGameFromSaveFileNow(string fileName)
		{
			string str = (from mod in LoadedModManager.RunningMods
			select mod.PackageIdPlayerFacing).ToLineList("  - ", false);
			Log.Message("Loading game from file " + fileName + " with mods:\n" + str);
			DeepProfiler.Start("Loading game from file " + fileName);
			Current.Game = new Game();
			DeepProfiler.Start("InitLoading (read file)");
			Scribe.loader.InitLoading(GenFilePaths.FilePathForSavedGame(fileName));
			DeepProfiler.End();
			try
			{
				ScribeMetaHeaderUtility.LoadGameDataHeader(ScribeMetaHeaderUtility.ScribeHeaderMode.Map, true);
				if (!Scribe.EnterNode("game"))
				{
					Log.Error("Could not find game XML node.");
					Scribe.ForceStop();
					return;
				}
				Current.Game = new Game();
				Current.Game.LoadGame();
			}
			catch (Exception)
			{
				Scribe.ForceStop();
				throw;
			}
			PermadeathModeUtility.CheckUpdatePermadeathModeUniqueNameOnGameLoad(fileName);
			DeepProfiler.End();
		}
	}
}
