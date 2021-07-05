using System;
using System.IO;
using RimWorld;
using Verse.Profile;

namespace Verse
{
	// Token: 0x0200031C RID: 796
	public static class GameDataSaveLoader
	{
		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x060016C8 RID: 5832 RVA: 0x00086435 File Offset: 0x00084635
		public static bool CurrentGameStateIsValuable
		{
			get
			{
				return Find.TickManager.TicksGame > GameDataSaveLoader.lastSaveTick + 60;
			}
		}

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x060016C9 RID: 5833 RVA: 0x0008644B File Offset: 0x0008464B
		public static bool SavingIsTemporarilyDisabled
		{
			get
			{
				return Find.TilePicker.Active || Find.WindowStack.WindowsPreventSave;
			}
		}

		// Token: 0x060016CA RID: 5834 RVA: 0x00086468 File Offset: 0x00084668
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
				Log.Error("Exception while saving world: " + ex.ToString());
			}
		}

		// Token: 0x060016CB RID: 5835 RVA: 0x000864D8 File Offset: 0x000846D8
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
				Log.Error("Exception loading scenario: " + ex.ToString());
				scen = null;
				Scribe.ForceStop();
			}
			return scen != null;
		}

		// Token: 0x060016CC RID: 5836 RVA: 0x00086580 File Offset: 0x00084780
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
				Log.Error("Exception while saving game: " + arg);
			}
		}

		// Token: 0x060016CD RID: 5837 RVA: 0x000865FC File Offset: 0x000847FC
		public static void CheckVersionAndLoadGame(string saveFileName)
		{
			PreLoadUtility.CheckVersionAndLoad(GenFilePaths.FilePathForSavedGame(saveFileName), ScribeMetaHeaderUtility.ScribeHeaderMode.Map, delegate
			{
				GameDataSaveLoader.LoadGame(saveFileName);
			});
		}

		// Token: 0x060016CE RID: 5838 RVA: 0x00086633 File Offset: 0x00084833
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

		// Token: 0x060016CF RID: 5839 RVA: 0x0008665E File Offset: 0x0008485E
		public static void LoadGame(FileInfo saveFile)
		{
			GameDataSaveLoader.LoadGame(Path.GetFileNameWithoutExtension(saveFile.Name));
		}

		// Token: 0x04000FD7 RID: 4055
		private static int lastSaveTick = -9999;

		// Token: 0x04000FD8 RID: 4056
		public const string SavedScenarioParentNodeName = "savedscenario";

		// Token: 0x04000FD9 RID: 4057
		public const string SavedWorldParentNodeName = "savedworld";

		// Token: 0x04000FDA RID: 4058
		public const string SavedGameParentNodeName = "savegame";

		// Token: 0x04000FDB RID: 4059
		public const string GameNodeName = "game";

		// Token: 0x04000FDC RID: 4060
		public const string WorldNodeName = "world";

		// Token: 0x04000FDD RID: 4061
		public const string ScenarioNodeName = "scenario";

		// Token: 0x04000FDE RID: 4062
		public const string AutosavePrefix = "Autosave";

		// Token: 0x04000FDF RID: 4063
		public const string AutostartSaveName = "autostart";
	}
}
