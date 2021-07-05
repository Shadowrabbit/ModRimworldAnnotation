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
	// Token: 0x020001EA RID: 490
	public abstract class Root : MonoBehaviour
	{
		// Token: 0x06000CBF RID: 3263 RVA: 0x000A554C File Offset: 0x000A374C
		public virtual void Start()
		{
			try
			{
				CultureInfoUtility.EnsureEnglish();
				Current.Notify_LoadedSceneChanged();
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
				Log.Error("Critical error in root Start(): " + arg, false);
			}
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x000A55E8 File Offset: 0x000A37E8
		private static void CheckGlobalInit()
		{
			if (Root.globalInitDone)
			{
				return;
			}
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			if (commandLineArgs != null && commandLineArgs.Length > 1)
			{
				Log.Message("Command line arguments: " + GenText.ToSpaceList(commandLineArgs.Skip(1)), false);
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

		// Token: 0x06000CC1 RID: 3265 RVA: 0x000A5684 File Offset: 0x000A3884
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
				Log.Error("Root level exception in Update(): " + arg, false);
			}
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x000A5738 File Offset: 0x000A3938
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
				Log.Error("Root level exception in OnGUI(): " + arg, false);
			}
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x000A5810 File Offset: 0x000A3A10
		public static void Shutdown()
		{
			try
			{
				SteamManager.ShutdownSteam();
			}
			catch (Exception arg)
			{
				Log.Error("Error in ShutdownSteam(): " + arg, false);
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
				Log.Error("Could not delete temporary files: " + arg2, false);
			}
			Application.Quit();
		}

		// Token: 0x04000AFE RID: 2814
		private static bool globalInitDone;

		// Token: 0x04000AFF RID: 2815
		private static bool prefsApplied;

		// Token: 0x04000B00 RID: 2816
		protected static bool checkedAutostartSaveFile;

		// Token: 0x04000B01 RID: 2817
		protected bool destroyed;

		// Token: 0x04000B02 RID: 2818
		public SoundRoot soundRoot;

		// Token: 0x04000B03 RID: 2819
		public UIRoot uiRoot;
	}
}
