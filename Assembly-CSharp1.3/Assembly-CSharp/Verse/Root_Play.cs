using System;
using System.IO;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000147 RID: 327
	public class Root_Play : Root
	{
		// Token: 0x0600090A RID: 2314 RVA: 0x00029E04 File Offset: 0x00028004
		public override void Start()
		{
			base.Start();
			try
			{
				this.musicManagerPlay = new MusicManagerPlay();
				FileInfo autostart = Root.checkedAutostartSaveFile ? null : SaveGameFilesUtility.GetAutostartSaveFile();
				Root.checkedAutostartSaveFile = true;
				if (autostart != null)
				{
					LongEventHandler.QueueLongEvent(delegate()
					{
						SavedGameLoaderNow.LoadGameFromSaveFileNow(Path.GetFileNameWithoutExtension(autostart.Name));
					}, "LoadingLongEvent", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileLoadingGame), true);
				}
				else if (Find.GameInitData != null && !Find.GameInitData.gameToLoad.NullOrEmpty())
				{
					LongEventHandler.QueueLongEvent(delegate()
					{
						SavedGameLoaderNow.LoadGameFromSaveFileNow(Find.GameInitData.gameToLoad);
					}, "LoadingLongEvent", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileLoadingGame), true);
				}
				else
				{
					LongEventHandler.QueueLongEvent(delegate()
					{
						if (Current.Game == null)
						{
							Root_Play.SetupForQuickTestPlay();
						}
						Current.Game.InitNewGame();
					}, "GeneratingMap", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap), true);
				}
				LongEventHandler.QueueLongEvent(delegate()
				{
					ScreenFader.SetColor(Color.black);
					ScreenFader.StartFade(Color.clear, 0.5f);
				}, null, false, null, true);
			}
			catch (Exception arg)
			{
				Log.Error("Critical error in root Start(): " + arg);
			}
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x00029F58 File Offset: 0x00028158
		public override void Update()
		{
			base.Update();
			if (LongEventHandler.ShouldWaitForEvent || this.destroyed)
			{
				return;
			}
			try
			{
				ShipCountdown.ShipCountdownUpdate();
				if (ModsConfig.IdeologyActive)
				{
					ArchonexusCountdown.ArchonexusCountdownUpdate();
				}
				TargetHighlighter.TargetHighlighterUpdate();
				Current.Game.UpdatePlay();
				this.musicManagerPlay.MusicUpdate();
			}
			catch (Exception arg)
			{
				Log.Error("Root level exception in Update(): " + arg);
			}
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x00029FCC File Offset: 0x000281CC
		private static void SetupForQuickTestPlay()
		{
			Current.ProgramState = ProgramState.Entry;
			Current.Game = new Game();
			Current.Game.InitData = new GameInitData();
			Current.Game.Scenario = ScenarioDefOf.Crashlanded.scenario;
			Find.Scenario.PreConfigure();
			Current.Game.storyteller = new Storyteller(StorytellerDefOf.Cassandra, DifficultyDefOf.Rough);
			Current.Game.World = WorldGenerator.GenerateWorld(0.05f, GenText.RandomSeedString(), OverallRainfall.Normal, OverallTemperature.Normal, OverallPopulation.Normal, null);
			Find.GameInitData.ChooseRandomStartingTile();
			Find.GameInitData.mapSize = 150;
			Find.Scenario.PostIdeoChosen();
			Find.GameInitData.PrepForMapGen();
			Find.Scenario.PreMapGenerate();
		}

		// Token: 0x04000840 RID: 2112
		public MusicManagerPlay musicManagerPlay;
	}
}
