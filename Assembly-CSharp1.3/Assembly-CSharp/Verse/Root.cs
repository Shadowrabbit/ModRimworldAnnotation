using System;
using System.IO;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using UnityEngine.Analytics;
using Verse.AI;
using Verse.Sound;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000145 RID: 325
	public abstract class Root : MonoBehaviour
	{
		// Token: 0x06000900 RID: 2304 RVA: 0x00029928 File Offset: 0x00027B28
		public virtual void Start()
		{
			try
			{
				CultureInfoUtility.EnsureEnglish();
				Current.Notify_LoadedSceneChanged();
				GlobalTextureAtlasManager.FreeAllRuntimeAtlases();
				Root.CheckGlobalInit();
				Action action = delegate()
				{
					DeepProfiler.Start("Misc Init (InitializingInterface)");
					try
					{
						this.soundRoot = new SoundRoot();
						if (GenScene.InPlayScene)
						{
							this.uiRoot = new UIRoot_Play();
						}
						else if (GenScene.InEntryScene)
						{
							this.uiRoot = new UIRoot_Entry();
						}
						this.uiRoot.Init();
						Messages.Notify_LoadedLevelChanged();
						if (Current.SubcameraDriver != null)
						{
							Current.SubcameraDriver.Init();
						}
					}
					finally
					{
						DeepProfiler.End();
					}
				};
				if (!PlayDataLoader.Loaded)
				{
					Application.runInBackground = true;
					LongEventHandler.QueueLongEvent(delegate()
					{
						PlayDataLoader.LoadAllPlayData(false);
					}, null, true, null, true);
					LongEventHandler.QueueLongEvent(action, "InitializingInterface", false, null, true);
				}
				else
				{
					action();
				}
			}
			catch (Exception arg)
			{
				Log.Error("Critical error in root Start(): " + arg);
			}
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x000299C8 File Offset: 0x00027BC8
		private static void CheckGlobalInit()
		{
			if (Root.globalInitDone)
			{
				return;
			}
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			if (commandLineArgs != null && commandLineArgs.Length > 1)
			{
				Log.Message("Command line arguments: " + GenText.ToSpaceList(commandLineArgs.Skip(1)));
			}
			PerformanceReporting.enabled = false;
			Application.targetFrameRate = 60;
			UnityDataInitializer.CopyUnityData();
			SteamManager.InitIfNeeded();
			VersionControl.LogVersionNumber();
			Prefs.Init();
			Application.logMessageReceivedThreaded += Log.Notify_MessageReceivedThreadedInternal;
			if (Prefs.DevMode)
			{
				StaticConstructorOnStartupUtility.ReportProbablyMissingAttributes();
			}
			LongEventHandler.QueueLongEvent(new Action(StaticConstructorOnStartupUtility.CallAll), null, false, null, true);
			Root.globalInitDone = true;
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x00029A60 File Offset: 0x00027C60
		public virtual void Update()
		{
			try
			{
				ResolutionUtility.Update();
				RealTime.Update();
				bool flag;
				LongEventHandler.LongEventsUpdate(out flag);
				if (flag)
				{
					this.destroyed = true;
				}
				else if (!LongEventHandler.ShouldWaitForEvent)
				{
					Rand.EnsureStateStackEmpty();
					Widgets.EnsureMousePositionStackEmpty();
					SteamManager.Update();
					PortraitsCache.PortraitsCacheUpdate();
					AttackTargetsCache.AttackTargetsCacheStaticUpdate();
					Pawn_MeleeVerbs.PawnMeleeVerbsStaticUpdate();
					Storyteller.StorytellerStaticUpdate();
					CaravanInventoryUtility.CaravanInventoryUtilityStaticUpdate();
					this.uiRoot.UIRootUpdate();
					if (Time.frameCount > 3 && !Root.prefsApplied)
					{
						Root.prefsApplied = true;
						Prefs.Apply();
					}
					this.soundRoot.Update();
				}
			}
			catch (Exception arg)
			{
				Log.Error("Root level exception in Update(): " + arg);
			}
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x00029B10 File Offset: 0x00027D10
		public void OnGUI()
		{
			try
			{
				if (!this.destroyed)
				{
					GUI.depth = 50;
					UI.ApplyUIScale();
					LongEventHandler.LongEventsOnGUI();
					if (LongEventHandler.ShouldWaitForEvent)
					{
						ScreenFader.OverlayOnGUI(new Vector2((float)UI.screenWidth, (float)UI.screenHeight));
					}
					else
					{
						this.uiRoot.UIRootOnGUI();
						ScreenFader.OverlayOnGUI(new Vector2((float)UI.screenWidth, (float)UI.screenHeight));
						if (Find.CameraDriver != null && Find.CameraDriver.isActiveAndEnabled)
						{
							Find.CameraDriver.CameraDriverOnGUI();
						}
						if (Find.WorldCameraDriver != null && Find.WorldCameraDriver.isActiveAndEnabled)
						{
							Find.WorldCameraDriver.WorldCameraDriverOnGUI();
						}
					}
				}
			}
			catch (Exception arg)
			{
				Log.Error("Root level exception in OnGUI(): " + arg);
			}
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x00029BE8 File Offset: 0x00027DE8
		public static void Shutdown()
		{
			try
			{
				SteamManager.ShutdownSteam();
			}
			catch (Exception arg)
			{
				Log.Error("Error in ShutdownSteam(): " + arg);
			}
			try
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.TempFolderPath);
				FileInfo[] files = directoryInfo.GetFiles();
				for (int i = 0; i < files.Length; i++)
				{
					files[i].Delete();
				}
				DirectoryInfo[] directories = directoryInfo.GetDirectories();
				for (int i = 0; i < directories.Length; i++)
				{
					directories[i].Delete(true);
				}
			}
			catch (Exception arg2)
			{
				Log.Error("Could not delete temporary files: " + arg2);
			}
			Application.Quit();
		}

		// Token: 0x04000839 RID: 2105
		private static bool globalInitDone;

		// Token: 0x0400083A RID: 2106
		private static bool prefsApplied;

		// Token: 0x0400083B RID: 2107
		protected static bool checkedAutostartSaveFile;

		// Token: 0x0400083C RID: 2108
		protected bool destroyed;

		// Token: 0x0400083D RID: 2109
		public SoundRoot soundRoot;

		// Token: 0x0400083E RID: 2110
		public UIRoot uiRoot;
	}
}
