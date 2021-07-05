using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001A0C RID: 6668
	public class Dialog_Options : Window
	{
		// Token: 0x1700176D RID: 5997
		// (get) Token: 0x06009362 RID: 37730 RVA: 0x00062BFD File Offset: 0x00060DFD
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(900f, 740f);
			}
		}

		// Token: 0x06009363 RID: 37731 RVA: 0x00062C0E File Offset: 0x00060E0E
		public Dialog_Options()
		{
			this.doCloseButton = true;
			this.doCloseX = true;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06009364 RID: 37732 RVA: 0x00062C32 File Offset: 0x00060E32
		public override void PostOpen()
		{
			base.PostOpen();
			this.simulateNotOwningRoyaltyWhenOpened = Prefs.SimulateNotOwningRoyalty;
		}

		// Token: 0x06009365 RID: 37733 RVA: 0x002A725C File Offset: 0x002A545C
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = inRect.AtZero();
			rect.yMax -= 35f;
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.ColumnWidth = (rect.width - 34f) / 3f;
			listing_Standard.Begin(rect);
			Text.Font = GameFont.Medium;
			listing_Standard.Label("Audiovisuals".Translate(), -1f, null);
			Text.Font = GameFont.Small;
			listing_Standard.Gap(12f);
			listing_Standard.Gap(12f);
			listing_Standard.Label("GameVolume".Translate(), -1f, null);
			Prefs.VolumeGame = listing_Standard.Slider(Prefs.VolumeGame, 0f, 1f);
			listing_Standard.Label("MusicVolume".Translate(), -1f, null);
			Prefs.VolumeMusic = listing_Standard.Slider(Prefs.VolumeMusic, 0f, 1f);
			listing_Standard.Label("AmbientVolume".Translate(), -1f, null);
			Prefs.VolumeAmbient = listing_Standard.Slider(Prefs.VolumeAmbient, 0f, 1f);
			if (listing_Standard.ButtonTextLabeled("Resolution".Translate(), Dialog_Options.ResToString(Screen.width, Screen.height)))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				using (IEnumerator<Resolution> enumerator = (from x in UnityGUIBugsFixer.ScreenResolutionsWithoutDuplicates
				where x.width >= 1024 && x.height >= 768
				orderby x.width, x.height
				select x).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Resolution res = enumerator.Current;
						list.Add(new FloatMenuOption(Dialog_Options.ResToString(res.width, res.height), delegate()
						{
							if (!ResolutionUtility.UIScaleSafeWithResolution(Prefs.UIScale, res.width, res.height))
							{
								Messages.Message("MessageScreenResTooSmallForUIScale".Translate(), MessageTypeDefOf.RejectInput, false);
								return;
							}
							ResolutionUtility.SafeSetResolution(res);
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
				}
				if (!list.Any<FloatMenuOption>())
				{
					list.Add(new FloatMenuOption("NoneBrackets".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			if (listing_Standard.ButtonTextLabeled("UIScale".Translate(), Prefs.UIScale.ToString() + "x"))
			{
				List<FloatMenuOption> list2 = new List<FloatMenuOption>();
				for (int i = 0; i < Dialog_Options.UIScales.Length; i++)
				{
					float scale = Dialog_Options.UIScales[i];
					list2.Add(new FloatMenuOption(Dialog_Options.UIScales[i].ToString() + "x", delegate()
					{
						if (scale != 1f && !ResolutionUtility.UIScaleSafeWithResolution(scale, Screen.width, Screen.height))
						{
							Messages.Message("MessageScreenResTooSmallForUIScale".Translate(), MessageTypeDefOf.RejectInput, false);
							return;
						}
						ResolutionUtility.SafeSetUIScale(scale);
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list2));
			}
			bool customCursorEnabled = Prefs.CustomCursorEnabled;
			listing_Standard.CheckboxLabeled("CustomCursor".Translate(), ref customCursorEnabled, null);
			Prefs.CustomCursorEnabled = customCursorEnabled;
			bool fullScreen = Screen.fullScreen;
			bool flag = fullScreen;
			listing_Standard.CheckboxLabeled("Fullscreen".Translate(), ref fullScreen, null);
			if (fullScreen != flag)
			{
				ResolutionUtility.SafeSetFullscreen(fullScreen);
			}
			listing_Standard.Gap(12f);
			bool hatsOnlyOnMap = Prefs.HatsOnlyOnMap;
			listing_Standard.CheckboxLabeled("HatsShownOnlyOnMap".Translate(), ref hatsOnlyOnMap, null);
			if (hatsOnlyOnMap != Prefs.HatsOnlyOnMap)
			{
				PortraitsCache.Clear();
			}
			Prefs.HatsOnlyOnMap = hatsOnlyOnMap;
			bool plantWindSway = Prefs.PlantWindSway;
			listing_Standard.CheckboxLabeled("PlantWindSway".Translate(), ref plantWindSway, null);
			Prefs.PlantWindSway = plantWindSway;
			bool showRealtimeClock = Prefs.ShowRealtimeClock;
			listing_Standard.CheckboxLabeled("ShowRealtimeClock".Translate(), ref showRealtimeClock, null);
			Prefs.ShowRealtimeClock = showRealtimeClock;
			if (listing_Standard.ButtonTextLabeled("ShowAnimalNames".Translate(), Prefs.AnimalNameMode.ToStringHuman()))
			{
				List<FloatMenuOption> list3 = new List<FloatMenuOption>();
				foreach (object obj in Enum.GetValues(typeof(AnimalNameDisplayMode)))
				{
					AnimalNameDisplayMode localMode2 = (AnimalNameDisplayMode)obj;
					AnimalNameDisplayMode localMode = localMode2;
					list3.Add(new FloatMenuOption(localMode.ToStringHuman(), delegate()
					{
						Prefs.AnimalNameMode = localMode;
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list3));
			}
			listing_Standard.NewColumn();
			Text.Font = GameFont.Medium;
			listing_Standard.Label("Gameplay".Translate(), -1f, null);
			Text.Font = GameFont.Small;
			listing_Standard.Gap(12f);
			listing_Standard.Gap(12f);
			if (listing_Standard.ButtonText("KeyboardConfig".Translate(), null))
			{
				Find.WindowStack.Add(new Dialog_KeyBindings());
			}
			if (listing_Standard.ButtonText("ChooseLanguage".Translate(), null))
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					Messages.Message("ChangeLanguageFromMainMenu".Translate(), MessageTypeDefOf.RejectInput, false);
					SoundDefOf.Click.PlayOneShotOnCamera(null);
				}
				else
				{
					List<FloatMenuOption> list4 = new List<FloatMenuOption>();
					foreach (LoadedLanguage localLang2 in LanguageDatabase.AllLoadedLanguages)
					{
						LoadedLanguage localLang = localLang2;
						list4.Add(new FloatMenuOption(localLang.DisplayName, delegate()
						{
							LanguageDatabase.SelectLanguage(localLang);
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
					Find.WindowStack.Add(new FloatMenu(list4));
				}
			}
			if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
			{
				if (listing_Standard.ButtonText("OpenSaveGameDataFolder".Translate(), null))
				{
					Application.OpenURL(GenFilePaths.SaveDataFolderPath);
					SoundDefOf.Click.PlayOneShotOnCamera(null);
				}
				if (listing_Standard.ButtonText("OpenLogFileFolder".Translate(), null))
				{
					Application.OpenURL(Application.persistentDataPath);
					SoundDefOf.Click.PlayOneShotOnCamera(null);
				}
			}
			else
			{
				if (listing_Standard.ButtonText("ShowSaveGameDataLocation".Translate(), null))
				{
					Find.WindowStack.Add(new Dialog_MessageBox(Path.GetFullPath(GenFilePaths.SaveDataFolderPath), null, null, null, null, null, false, null, null));
				}
				if (listing_Standard.ButtonText("ShowLogFileLocation".Translate(), null))
				{
					Find.WindowStack.Add(new Dialog_MessageBox(Path.GetFullPath(Application.persistentDataPath), null, null, null, null, null, false, null, null));
				}
			}
			if (listing_Standard.ButtonText("ResetAdaptiveTutor".Translate(), null))
			{
				Messages.Message("AdaptiveTutorIsReset".Translate(), MessageTypeDefOf.TaskCompletion, false);
				PlayerKnowledgeDatabase.ResetPersistent();
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
			bool adaptiveTrainingEnabled = Prefs.AdaptiveTrainingEnabled;
			listing_Standard.CheckboxLabeled("LearningHelper".Translate(), ref adaptiveTrainingEnabled, null);
			Prefs.AdaptiveTrainingEnabled = adaptiveTrainingEnabled;
			bool runInBackground = Prefs.RunInBackground;
			listing_Standard.CheckboxLabeled("RunInBackground".Translate(), ref runInBackground, null);
			Prefs.RunInBackground = runInBackground;
			bool edgeScreenScroll = Prefs.EdgeScreenScroll;
			listing_Standard.CheckboxLabeled("EdgeScreenScroll".Translate(), ref edgeScreenScroll, null);
			Prefs.EdgeScreenScroll = edgeScreenScroll;
			float mapDragSensitivity = Prefs.MapDragSensitivity;
			listing_Standard.Label("MapDragSensitivity".Translate() + ": " + mapDragSensitivity.ToStringPercent("F0"), -1f, null);
			Prefs.MapDragSensitivity = (float)Math.Round((double)listing_Standard.Slider(mapDragSensitivity, 0.8f, 2.5f), 2);
			bool pauseOnLoad = Prefs.PauseOnLoad;
			listing_Standard.CheckboxLabeled("PauseOnLoad".Translate(), ref pauseOnLoad, null);
			Prefs.PauseOnLoad = pauseOnLoad;
			AutomaticPauseMode automaticPauseMode = Prefs.AutomaticPauseMode;
			if (listing_Standard.ButtonTextLabeled("AutomaticPauseModeSetting".Translate(), Prefs.AutomaticPauseMode.ToStringHuman()))
			{
				List<FloatMenuOption> list5 = new List<FloatMenuOption>();
				foreach (object obj2 in Enum.GetValues(typeof(AutomaticPauseMode)))
				{
					AutomaticPauseMode localPmode2 = (AutomaticPauseMode)obj2;
					AutomaticPauseMode localPmode = localPmode2;
					list5.Add(new FloatMenuOption(localPmode.ToStringHuman(), delegate()
					{
						Prefs.AutomaticPauseMode = localPmode;
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list5));
			}
			Prefs.AutomaticPauseMode = automaticPauseMode;
			int maxNumberOfPlayerSettlements = Prefs.MaxNumberOfPlayerSettlements;
			listing_Standard.Label("MaxNumberOfPlayerSettlements".Translate(maxNumberOfPlayerSettlements), -1f, null);
			int num = Mathf.RoundToInt(listing_Standard.Slider((float)maxNumberOfPlayerSettlements, 1f, 5f));
			Prefs.MaxNumberOfPlayerSettlements = num;
			if (maxNumberOfPlayerSettlements != num && num > 1)
			{
				TutorUtility.DoModalDialogIfNotKnown(ConceptDefOf.MaxNumberOfPlayerSettlements, Array.Empty<string>());
			}
			if (listing_Standard.ButtonTextLabeled("TemperatureMode".Translate(), Prefs.TemperatureMode.ToStringHuman()))
			{
				List<FloatMenuOption> list6 = new List<FloatMenuOption>();
				foreach (object obj3 in Enum.GetValues(typeof(TemperatureDisplayMode)))
				{
					TemperatureDisplayMode localTmode2 = (TemperatureDisplayMode)obj3;
					TemperatureDisplayMode localTmode = localTmode2;
					list6.Add(new FloatMenuOption(localTmode.ToStringHuman(), delegate()
					{
						Prefs.TemperatureMode = localTmode;
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list6));
			}
			float autosaveIntervalDays = Prefs.AutosaveIntervalDays;
			string text = "Days".Translate();
			string text2 = "Day".Translate().ToLower();
			if (listing_Standard.ButtonTextLabeled("AutosaveInterval".Translate(), autosaveIntervalDays + " " + ((autosaveIntervalDays == 1f) ? text2 : text)))
			{
				List<FloatMenuOption> list7 = new List<FloatMenuOption>();
				if (Prefs.DevMode)
				{
					list7.Add(new FloatMenuOption("0.125 " + text + "(debug)", delegate()
					{
						Prefs.AutosaveIntervalDays = 0.125f;
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
					list7.Add(new FloatMenuOption("0.25 " + text + "(debug)", delegate()
					{
						Prefs.AutosaveIntervalDays = 0.25f;
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				list7.Add(new FloatMenuOption(("0.5 " + text) ?? "", delegate()
				{
					Prefs.AutosaveIntervalDays = 0.5f;
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list7.Add(new FloatMenuOption(1.ToString() + " " + text2, delegate()
				{
					Prefs.AutosaveIntervalDays = 1f;
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list7.Add(new FloatMenuOption(3.ToString() + " " + text, delegate()
				{
					Prefs.AutosaveIntervalDays = 3f;
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list7.Add(new FloatMenuOption(7.ToString() + " " + text, delegate()
				{
					Prefs.AutosaveIntervalDays = 7f;
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				list7.Add(new FloatMenuOption(14.ToString() + " " + text, delegate()
				{
					Prefs.AutosaveIntervalDays = 14f;
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
				Find.WindowStack.Add(new FloatMenu(list7));
			}
			if (Current.ProgramState == ProgramState.Playing && Current.Game.Info.permadeathMode && Prefs.AutosaveIntervalDays > 1f)
			{
				GUI.color = Color.red;
				listing_Standard.Label("MaxPermadeathAutosaveIntervalInfo".Translate(1f), -1f, null);
				GUI.color = Color.white;
			}
			if (Current.ProgramState == ProgramState.Playing && listing_Standard.ButtonText("ChangeStoryteller".Translate(), "OptionsButton-ChooseStoryteller") && TutorSystem.AllowAction("ChooseStoryteller"))
			{
				Find.WindowStack.Add(new Page_SelectStorytellerInGame());
			}
			if (!DevModePermanentlyDisabledUtility.Disabled && listing_Standard.ButtonText("PermanentlyDisableDevMode".Translate(), null))
			{
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmPermanentlyDisableDevMode".Translate(), delegate
				{
					DevModePermanentlyDisabledUtility.Disable();
				}, true, null));
			}
			bool testMapSizes = Prefs.TestMapSizes;
			listing_Standard.CheckboxLabeled("EnableTestMapSizes".Translate(), ref testMapSizes, null);
			Prefs.TestMapSizes = testMapSizes;
			if (!DevModePermanentlyDisabledUtility.Disabled || Prefs.DevMode)
			{
				bool devMode = Prefs.DevMode;
				listing_Standard.CheckboxLabeled("DevelopmentMode".Translate(), ref devMode, null);
				Prefs.DevMode = devMode;
			}
			if (Prefs.DevMode)
			{
				bool resetModsConfigOnCrash = Prefs.ResetModsConfigOnCrash;
				listing_Standard.CheckboxLabeled("ResetModsConfigOnCrash".Translate(), ref resetModsConfigOnCrash, null);
				Prefs.ResetModsConfigOnCrash = resetModsConfigOnCrash;
				bool logVerbose = Prefs.LogVerbose;
				listing_Standard.CheckboxLabeled("LogVerbose".Translate(), ref logVerbose, null);
				Prefs.LogVerbose = logVerbose;
				if (Current.ProgramState != ProgramState.Playing)
				{
					bool simulateNotOwningRoyalty = Prefs.SimulateNotOwningRoyalty;
					listing_Standard.CheckboxLabeled("SimulateNotOwningRoyalty".Translate(), ref simulateNotOwningRoyalty, null);
					Prefs.SimulateNotOwningRoyalty = simulateNotOwningRoyalty;
				}
			}
			listing_Standard.NewColumn();
			Text.Font = GameFont.Medium;
			listing_Standard.Label("", -1f, null);
			Text.Font = GameFont.Small;
			listing_Standard.Gap(12f);
			listing_Standard.Gap(12f);
			if (listing_Standard.ButtonText("ModSettings".Translate(), null))
			{
				Find.WindowStack.Add(new Dialog_ModSettings());
			}
			listing_Standard.Label("", -1f, null);
			listing_Standard.Label("NamesYouWantToSee".Translate(), -1f, null);
			Prefs.PreferredNames.RemoveAll((string n) => n.NullOrEmpty());
			for (int j = 0; j < Prefs.PreferredNames.Count; j++)
			{
				string name = Prefs.PreferredNames[j];
				PawnBio pawnBio = (from b in SolidBioDatabase.allBios
				where b.name.ToString() == name
				select b).FirstOrDefault<PawnBio>();
				if (pawnBio == null)
				{
					name += " [N]";
				}
				else
				{
					PawnBioType bioType = pawnBio.BioType;
					if (bioType != PawnBioType.BackstoryInGame)
					{
						if (bioType == PawnBioType.PirateKing)
						{
							name += " [PK]";
						}
					}
					else
					{
						name += " [B]";
					}
				}
				Rect rect2 = listing_Standard.GetRect(24f);
				Widgets.Label(rect2, name);
				if (Widgets.ButtonImage(new Rect(rect2.xMax - 24f, rect2.y, 24f, 24f), TexButton.DeleteX, Color.white, GenUI.SubtleMouseoverColor, true))
				{
					Prefs.PreferredNames.RemoveAt(j);
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
			}
			if (Prefs.PreferredNames.Count < 6 && listing_Standard.ButtonText("AddName".Translate() + "...", null))
			{
				Find.WindowStack.Add(new Dialog_AddPreferredName());
			}
			listing_Standard.Label("", -1f, null);
			if (listing_Standard.ButtonText("RestoreToDefaultSettings".Translate(), null))
			{
				Find.WindowStack.Add(new Dialog_MessageBox("ResetAndRestartConfirmationDialog".Translate(), "Yes".Translate(), delegate()
				{
					this.RestoreToDefaultSettings();
				}, "No".Translate(), null, null, false, null, null));
			}
			listing_Standard.End();
		}

		// Token: 0x06009366 RID: 37734 RVA: 0x00062C45 File Offset: 0x00060E45
		public override void PreClose()
		{
			base.PreClose();
			Prefs.Save();
			if (Prefs.SimulateNotOwningRoyalty && !this.simulateNotOwningRoyaltyWhenOpened && ModsConfig.RoyaltyActive)
			{
				ModsConfig.SetActive(ModContentPack.RoyaltyModPackageId, false);
				ModsConfig.RestartFromChangedMods();
			}
		}

		// Token: 0x06009367 RID: 37735 RVA: 0x002A8394 File Offset: 0x002A6594
		public static string ResToString(int width, int height)
		{
			string text = width + "x" + height;
			if (width == 1280 && height == 720)
			{
				text += " (720p)";
			}
			if (width == 1920 && height == 1080)
			{
				text += " (1080p)";
			}
			return text;
		}

		// Token: 0x06009368 RID: 37736 RVA: 0x002A83F4 File Offset: 0x002A65F4
		public void RestoreToDefaultSettings()
		{
			foreach (FileInfo fileInfo in new DirectoryInfo(GenFilePaths.ConfigFolderPath).GetFiles("*.xml"))
			{
				try
				{
					fileInfo.Delete();
				}
				catch (SystemException)
				{
				}
			}
			Find.WindowStack.Add(new Dialog_MessageBox("ResetAndRestart".Translate(), null, delegate()
			{
				GenCommandLine.Restart();
			}, null, null, null, false, null, null));
		}

		// Token: 0x04005D60 RID: 23904
		private bool simulateNotOwningRoyaltyWhenOpened;

		// Token: 0x04005D61 RID: 23905
		private const float SubOptionTabWidth = 40f;

		// Token: 0x04005D62 RID: 23906
		public static readonly float[] UIScales = new float[]
		{
			1f,
			1.25f,
			1.5f,
			1.75f,
			2f,
			2.5f,
			3f,
			3.5f,
			4f
		};
	}
}
