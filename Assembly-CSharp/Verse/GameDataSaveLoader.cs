using System;
using System.IO;
using RimWorld;
using Verse.Profile;

namespace Verse
{
	// Token: 0x0200049B RID: 1179
	public static class GameDataSaveLoader
	{
		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06001D86 RID: 7558 RVA: 0x0001A71D File Offset: 0x0001891D
		public static bool CurrentGameStateIsValuable
		{
			get
			{
				return Find.TickManager.TicksGame > GameDataSaveLoader.lastSaveTick + 60;
			}
		}

		// Token: 0x06001D87 RID: 7559 RVA: 0x000F6750 File Offset: 0x000F4950
		public static void SaveScenario(Scenario scen, string absFilePath)
		{
			try
			{
				scen.fileName = Path.GetFileNameWithoutExtension(absFilePath);
				SafeSaver.Save(absFilePath, "savedscenario", delegate
				{
					ScribeMetaHeaderUtility.WriteMetaHeader();
					Scribe_Deep.Look<Scenario>(ref scen, "scenario", Array.Empty<object>());
				}, false);
			}
			catch (Exception ex)
			{
				Log.Error("Exception while saving world: " + ex.ToString(), false);
			}
		}

		// Token: 0x06001D88 RID: 7560 RVA: 0x000F67C0 File Offset: 0x000F49C0
		public static bool TryLoadScenario(string absPath, ScenarioCategory category, out Scenario scen)
		{
			scen = null;
			try
			{
				Scribe.loader.InitLoading(absPath);
				try
				{
					ScribeMetaHeaderUtility.LoadGameDataHeader(ScribeMetaHeaderUtility.ScribeHeaderMode.Scenario, true);
					Scribe_Deep.Look<Scenario>(ref scen, "scenario", Array.Empty<object>());
					Scribe.loader.FinalizeLoading();
				}
				catch
				{
					Scribe.ForceStop();
					throw;
				}
				scen.fileName = Path.GetFileNameWithoutExtension(new FileInfo(absPath).Name);
				scen.Category = category;
			}
			catch (Exception ex)
			{
				Log.Error("Exception loading scenario: " + ex.ToString(), false);
				scen = null;
				Scribe.ForceStop();
			}
			return scen != null;
		}

		// Token: 0x06001D89 RID: 7561 RVA: 0x000F686C File Offset: 0x000F4A6C
		public static void SaveGame(string fileName)
		{
			try
			{
				SafeSaver.Save(GenFilePaths.FilePathForSavedGame(fileName), "savegame", delegate
				{
					ScribeMetaHeaderUtility.WriteMetaHeader();
					Game game = Current.Game;
					Scribe_Deep.Look<Game>(ref game, "game", Array.Empty<object>());
				}, Find.GameInfo.permadeathMode);
				GameDataSaveLoader.lastSaveTick = Find.TickManager.TicksGame;
			}
			catch (Exception arg)
			{
				Log.Error("Exception while saving game: " + arg, false);
			}
		}

		// Token: 0x06001D8A RID: 7562 RVA: 0x000F68E8 File Offset: 0x000F4AE8
		public static void CheckVersionAndLoadGame(string saveFileName)
		{
			PreLoadUtility.CheckVersionAndLoad(GenFilePaths.FilePathForSavedGame(saveFileName), ScribeMetaHeaderUtility.ScribeHeaderMode.Map, delegate
			{
				GameDataSaveLoader.LoadGame(saveFileName);
			});
		}

		// Token: 0x06001D8B RID: 7563 RVA: 0x0001A733 File Offset: 0x00018933
		public static void LoadGame(string saveFileName)
		{
			LongEventHandler.QueueLongEvent(delegate()
			{
				MemoryUtility.ClearAllMapsAndWorld();
				Current.Game = new Game();
				Current.Game.InitData = new GameInitData();
				Current.Game.InitData.gameToLoad = saveFileName;
			}, "Play", "LoadingLongEvent", true, null, true);
		}

		// Token: 0x06001D8C RID: 7564 RVA: 0x0001A75E File Offset: 0x0001895E
		public static void LoadGame(FileInfo saveFile)
		{
			GameDataSaveLoader.LoadGame(Path.GetFileNameWithoutExtension(saveFile.Name));
		}

		// Token: 0x0400151C RID: 5404
		private static int lastSaveTick = -9999;

		// Token: 0x0400151D RID: 5405
		public const string SavedScenarioParentNodeName = "savedscenario";

		// Token: 0x0400151E RID: 5406
		public const string SavedWorldParentNodeName = "savedworld";

		// Token: 0x0400151F RID: 5407
		public const string SavedGameParentNodeName = "savegame";

		// Token: 0x04001520 RID: 5408
		public const string GameNodeName = "game";

		// Token: 0x04001521 RID: 5409
		public const string WorldNodeName = "world";

		// Token: 0x04001522 RID: 5410
		public const string ScenarioNodeName = "scenario";

		// Token: 0x04001523 RID: 5411
		public const string AutosavePrefix = "Autosave";

		// Token: 0x04001524 RID: 5412
		public const string AutostartSaveName = "autostart";
	}
}
