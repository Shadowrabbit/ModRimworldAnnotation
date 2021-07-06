using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000886 RID: 2182
	public static class Prefs
	{
		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x06003616 RID: 13846 RVA: 0x00029EFD File Offset: 0x000280FD
		// (set) Token: 0x06003617 RID: 13847 RVA: 0x00029F09 File Offset: 0x00028109
		public static float VolumeGame
		{
			get
			{
				return Prefs.data.volumeGame;
			}
			set
			{
				if (Prefs.data.volumeGame == value)
				{
					return;
				}
				Prefs.data.volumeGame = value;
				Prefs.Apply();
			}
		}

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x06003618 RID: 13848 RVA: 0x00029F29 File Offset: 0x00028129
		// (set) Token: 0x06003619 RID: 13849 RVA: 0x00029F35 File Offset: 0x00028135
		public static float VolumeMusic
		{
			get
			{
				return Prefs.data.volumeMusic;
			}
			set
			{
				if (Prefs.data.volumeMusic == value)
				{
					return;
				}
				Prefs.data.volumeMusic = value;
				Prefs.Apply();
			}
		}

		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x0600361A RID: 13850 RVA: 0x00029F55 File Offset: 0x00028155
		// (set) Token: 0x0600361B RID: 13851 RVA: 0x00029F61 File Offset: 0x00028161
		public static float VolumeAmbient
		{
			get
			{
				return Prefs.data.volumeAmbient;
			}
			set
			{
				if (Prefs.data.volumeAmbient == value)
				{
					return;
				}
				Prefs.data.volumeAmbient = value;
				Prefs.Apply();
			}
		}

		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x0600361C RID: 13852 RVA: 0x00029F81 File Offset: 0x00028181
		// (set) Token: 0x0600361D RID: 13853 RVA: 0x00029F8D File Offset: 0x0002818D
		[Obsolete]
		public static bool ExtremeDifficultyUnlocked
		{
			get
			{
				return Prefs.data.extremeDifficultyUnlocked;
			}
			set
			{
				if (Prefs.data.extremeDifficultyUnlocked == value)
				{
					return;
				}
				Prefs.data.extremeDifficultyUnlocked = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x0600361E RID: 13854 RVA: 0x00029FAD File Offset: 0x000281AD
		// (set) Token: 0x0600361F RID: 13855 RVA: 0x00029FB9 File Offset: 0x000281B9
		public static bool AdaptiveTrainingEnabled
		{
			get
			{
				return Prefs.data.adaptiveTrainingEnabled;
			}
			set
			{
				if (Prefs.data.adaptiveTrainingEnabled == value)
				{
					return;
				}
				Prefs.data.adaptiveTrainingEnabled = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x06003620 RID: 13856 RVA: 0x00029FD9 File Offset: 0x000281D9
		// (set) Token: 0x06003621 RID: 13857 RVA: 0x00029FE5 File Offset: 0x000281E5
		public static bool EdgeScreenScroll
		{
			get
			{
				return Prefs.data.edgeScreenScroll;
			}
			set
			{
				if (Prefs.data.edgeScreenScroll == value)
				{
					return;
				}
				Prefs.data.edgeScreenScroll = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x06003622 RID: 13858 RVA: 0x0002A005 File Offset: 0x00028205
		// (set) Token: 0x06003623 RID: 13859 RVA: 0x0002A011 File Offset: 0x00028211
		public static bool RunInBackground
		{
			get
			{
				return Prefs.data.runInBackground;
			}
			set
			{
				if (Prefs.data.runInBackground == value)
				{
					return;
				}
				Prefs.data.runInBackground = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x06003624 RID: 13860 RVA: 0x0002A031 File Offset: 0x00028231
		// (set) Token: 0x06003625 RID: 13861 RVA: 0x0002A03D File Offset: 0x0002823D
		public static TemperatureDisplayMode TemperatureMode
		{
			get
			{
				return Prefs.data.temperatureMode;
			}
			set
			{
				if (Prefs.data.temperatureMode == value)
				{
					return;
				}
				Prefs.data.temperatureMode = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x06003626 RID: 13862 RVA: 0x0002A05D File Offset: 0x0002825D
		// (set) Token: 0x06003627 RID: 13863 RVA: 0x0002A069 File Offset: 0x00028269
		public static float AutosaveIntervalDays
		{
			get
			{
				return Prefs.data.autosaveIntervalDays;
			}
			set
			{
				if (Prefs.data.autosaveIntervalDays == value)
				{
					return;
				}
				Prefs.data.autosaveIntervalDays = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x06003628 RID: 13864 RVA: 0x0002A089 File Offset: 0x00028289
		// (set) Token: 0x06003629 RID: 13865 RVA: 0x0002A095 File Offset: 0x00028295
		public static bool CustomCursorEnabled
		{
			get
			{
				return Prefs.data.customCursorEnabled;
			}
			set
			{
				if (Prefs.data.customCursorEnabled == value)
				{
					return;
				}
				Prefs.data.customCursorEnabled = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x0600362A RID: 13866 RVA: 0x0002A0B5 File Offset: 0x000282B5
		// (set) Token: 0x0600362B RID: 13867 RVA: 0x0002A0C1 File Offset: 0x000282C1
		public static AnimalNameDisplayMode AnimalNameMode
		{
			get
			{
				return Prefs.data.animalNameMode;
			}
			set
			{
				if (Prefs.data.animalNameMode == value)
				{
					return;
				}
				Prefs.data.animalNameMode = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x0600362C RID: 13868 RVA: 0x0002A0E1 File Offset: 0x000282E1
		// (set) Token: 0x0600362D RID: 13869 RVA: 0x0015B814 File Offset: 0x00159A14
		public static bool DevMode
		{
			get
			{
				return Prefs.data == null || Prefs.data.devMode;
			}
			set
			{
				if (Prefs.data.devMode == value)
				{
					return;
				}
				Prefs.data.devMode = value;
				if (!Prefs.data.devMode)
				{
					Prefs.data.logVerbose = false;
					Prefs.data.resetModsConfigOnCrash = true;
					DebugSettings.godMode = false;
				}
				Prefs.Apply();
			}
		}

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x0600362E RID: 13870 RVA: 0x0002A0F6 File Offset: 0x000282F6
		// (set) Token: 0x0600362F RID: 13871 RVA: 0x0002A10B File Offset: 0x0002830B
		public static bool ResetModsConfigOnCrash
		{
			get
			{
				return Prefs.data == null || Prefs.data.resetModsConfigOnCrash;
			}
			set
			{
				if (Prefs.data.resetModsConfigOnCrash == value)
				{
					return;
				}
				Prefs.data.resetModsConfigOnCrash = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x06003630 RID: 13872 RVA: 0x0002A12B File Offset: 0x0002832B
		// (set) Token: 0x06003631 RID: 13873 RVA: 0x0002A140 File Offset: 0x00028340
		public static bool SimulateNotOwningRoyalty
		{
			get
			{
				return Prefs.data == null || Prefs.data.simulateNotOwningRoyalty;
			}
			set
			{
				if (Prefs.data.simulateNotOwningRoyalty == value)
				{
					return;
				}
				Prefs.data.simulateNotOwningRoyalty = value;
				Prefs.Apply();
			}
		}

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x06003632 RID: 13874 RVA: 0x0002A160 File Offset: 0x00028360
		// (set) Token: 0x06003633 RID: 13875 RVA: 0x0002A16C File Offset: 0x0002836C
		public static List<string> PreferredNames
		{
			get
			{
				return Prefs.data.preferredNames;
			}
			set
			{
				if (Prefs.data.preferredNames == value)
				{
					return;
				}
				Prefs.data.preferredNames = value;
				Prefs.Apply();
			}
		}

		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x06003634 RID: 13876 RVA: 0x0002A18C File Offset: 0x0002838C
		// (set) Token: 0x06003635 RID: 13877 RVA: 0x0002A198 File Offset: 0x00028398
		public static string LangFolderName
		{
			get
			{
				return Prefs.data.langFolderName;
			}
			set
			{
				if (Prefs.data.langFolderName == value)
				{
					return;
				}
				Prefs.data.langFolderName = value;
				Prefs.Apply();
			}
		}

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x06003636 RID: 13878 RVA: 0x0002A1BD File Offset: 0x000283BD
		// (set) Token: 0x06003637 RID: 13879 RVA: 0x0002A1C9 File Offset: 0x000283C9
		public static bool LogVerbose
		{
			get
			{
				return Prefs.data.logVerbose;
			}
			set
			{
				if (Prefs.data.logVerbose == value)
				{
					return;
				}
				Prefs.data.logVerbose = value;
				Prefs.Apply();
			}
		}

		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x06003638 RID: 13880 RVA: 0x0002A1E9 File Offset: 0x000283E9
		// (set) Token: 0x06003639 RID: 13881 RVA: 0x0002A1FE File Offset: 0x000283FE
		public static bool PauseOnError
		{
			get
			{
				return Prefs.data != null && Prefs.data.pauseOnError;
			}
			set
			{
				Prefs.data.pauseOnError = value;
			}
		}

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x0600363A RID: 13882 RVA: 0x0002A20B File Offset: 0x0002840B
		// (set) Token: 0x0600363B RID: 13883 RVA: 0x0002A217 File Offset: 0x00028417
		public static bool PauseOnLoad
		{
			get
			{
				return Prefs.data.pauseOnLoad;
			}
			set
			{
				Prefs.data.pauseOnLoad = value;
			}
		}

		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x0600363C RID: 13884 RVA: 0x0002A224 File Offset: 0x00028424
		// (set) Token: 0x0600363D RID: 13885 RVA: 0x0002A230 File Offset: 0x00028430
		public static AutomaticPauseMode AutomaticPauseMode
		{
			get
			{
				return Prefs.data.automaticPauseMode;
			}
			set
			{
				Prefs.data.automaticPauseMode = value;
			}
		}

		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x0600363E RID: 13886 RVA: 0x0002A23D File Offset: 0x0002843D
		// (set) Token: 0x0600363F RID: 13887 RVA: 0x0002A249 File Offset: 0x00028449
		public static bool ShowRealtimeClock
		{
			get
			{
				return Prefs.data.showRealtimeClock;
			}
			set
			{
				Prefs.data.showRealtimeClock = value;
			}
		}

		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x06003640 RID: 13888 RVA: 0x0002A256 File Offset: 0x00028456
		// (set) Token: 0x06003641 RID: 13889 RVA: 0x0002A262 File Offset: 0x00028462
		public static bool TestMapSizes
		{
			get
			{
				return Prefs.data.testMapSizes;
			}
			set
			{
				Prefs.data.testMapSizes = value;
			}
		}

		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x06003642 RID: 13890 RVA: 0x0002A26F File Offset: 0x0002846F
		// (set) Token: 0x06003643 RID: 13891 RVA: 0x0002A27B File Offset: 0x0002847B
		public static int MaxNumberOfPlayerSettlements
		{
			get
			{
				return Prefs.data.maxNumberOfPlayerSettlements;
			}
			set
			{
				Prefs.data.maxNumberOfPlayerSettlements = value;
			}
		}

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x06003644 RID: 13892 RVA: 0x0002A288 File Offset: 0x00028488
		// (set) Token: 0x06003645 RID: 13893 RVA: 0x0002A294 File Offset: 0x00028494
		public static bool PlantWindSway
		{
			get
			{
				return Prefs.data.plantWindSway;
			}
			set
			{
				Prefs.data.plantWindSway = value;
			}
		}

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x06003646 RID: 13894 RVA: 0x0002A2A1 File Offset: 0x000284A1
		// (set) Token: 0x06003647 RID: 13895 RVA: 0x0002A2AD File Offset: 0x000284AD
		public static bool ResourceReadoutCategorized
		{
			get
			{
				return Prefs.data.resourceReadoutCategorized;
			}
			set
			{
				if (value == Prefs.data.resourceReadoutCategorized)
				{
					return;
				}
				Prefs.data.resourceReadoutCategorized = value;
				Prefs.Save();
			}
		}

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x06003648 RID: 13896 RVA: 0x0002A2CD File Offset: 0x000284CD
		// (set) Token: 0x06003649 RID: 13897 RVA: 0x0002A2D9 File Offset: 0x000284D9
		public static float UIScale
		{
			get
			{
				return Prefs.data.uiScale;
			}
			set
			{
				Prefs.data.uiScale = value;
			}
		}

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x0600364A RID: 13898 RVA: 0x0002A2E6 File Offset: 0x000284E6
		// (set) Token: 0x0600364B RID: 13899 RVA: 0x0002A2F2 File Offset: 0x000284F2
		public static int ScreenWidth
		{
			get
			{
				return Prefs.data.screenWidth;
			}
			set
			{
				Prefs.data.screenWidth = value;
			}
		}

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x0600364C RID: 13900 RVA: 0x0002A2FF File Offset: 0x000284FF
		// (set) Token: 0x0600364D RID: 13901 RVA: 0x0002A30B File Offset: 0x0002850B
		public static int ScreenHeight
		{
			get
			{
				return Prefs.data.screenHeight;
			}
			set
			{
				Prefs.data.screenHeight = value;
			}
		}

		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x0600364E RID: 13902 RVA: 0x0002A318 File Offset: 0x00028518
		// (set) Token: 0x0600364F RID: 13903 RVA: 0x0002A324 File Offset: 0x00028524
		public static bool FullScreen
		{
			get
			{
				return Prefs.data.fullscreen;
			}
			set
			{
				Prefs.data.fullscreen = value;
			}
		}

		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x06003650 RID: 13904 RVA: 0x0002A331 File Offset: 0x00028531
		// (set) Token: 0x06003651 RID: 13905 RVA: 0x0002A33D File Offset: 0x0002853D
		public static bool HatsOnlyOnMap
		{
			get
			{
				return Prefs.data.hatsOnlyOnMap;
			}
			set
			{
				if (Prefs.data.hatsOnlyOnMap == value)
				{
					return;
				}
				Prefs.data.hatsOnlyOnMap = value;
				Prefs.Apply();
			}
		}

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x06003652 RID: 13906 RVA: 0x0002A35D File Offset: 0x0002855D
		// (set) Token: 0x06003653 RID: 13907 RVA: 0x0002A369 File Offset: 0x00028569
		public static float MapDragSensitivity
		{
			get
			{
				return Prefs.data.mapDragSensitivity;
			}
			set
			{
				Prefs.data.mapDragSensitivity = value;
			}
		}

		// Token: 0x06003654 RID: 13908 RVA: 0x0015B868 File Offset: 0x00159A68
		public static void Init()
		{
			bool flag = !new FileInfo(GenFilePaths.PrefsFilePath).Exists;
			Prefs.data = new PrefsData();
			Prefs.data = DirectXmlLoader.ItemFromXmlFile<PrefsData>(GenFilePaths.PrefsFilePath, true);
			BackCompatibility.PrefsDataPostLoad(Prefs.data);
			if (flag)
			{
				Prefs.data.langFolderName = LanguageDatabase.SystemLanguageFolderName();
				Prefs.data.uiScale = ResolutionUtility.GetRecommendedUIScale(Prefs.data.screenWidth, Prefs.data.screenHeight);
			}
			if (DevModePermanentlyDisabledUtility.Disabled)
			{
				Prefs.DevMode = false;
			}
			Prefs.Apply();
		}

		// Token: 0x06003655 RID: 13909 RVA: 0x0015B8F4 File Offset: 0x00159AF4
		public static void Save()
		{
			try
			{
				XDocument xdocument = new XDocument();
				XElement content = DirectXmlSaver.XElementFromObject(Prefs.data, typeof(PrefsData));
				xdocument.Add(content);
				xdocument.Save(GenFilePaths.PrefsFilePath);
			}
			catch (Exception ex)
			{
				GenUI.ErrorDialog("ProblemSavingFile".Translate(GenFilePaths.PrefsFilePath, ex.ToString()));
				Log.Error("Exception saving prefs: " + ex, false);
			}
		}

		// Token: 0x06003656 RID: 13910 RVA: 0x0002A376 File Offset: 0x00028576
		public static void Apply()
		{
			Prefs.data.Apply();
		}

		// Token: 0x06003657 RID: 13911 RVA: 0x0015B97C File Offset: 0x00159B7C
		public static NameTriple RandomPreferredName()
		{
			string rawName;
			if ((from name in Prefs.PreferredNames
			where !name.NullOrEmpty()
			select name).TryRandomElement(out rawName))
			{
				return NameTriple.FromString(rawName);
			}
			return null;
		}

		// Token: 0x040025B5 RID: 9653
		private static PrefsData data;
	}
}
