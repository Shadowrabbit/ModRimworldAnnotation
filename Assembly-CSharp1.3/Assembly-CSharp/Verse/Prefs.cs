using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020004DB RID: 1243
	public static class Prefs
	{
		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x06002584 RID: 9604 RVA: 0x000E9DF2 File Offset: 0x000E7FF2
		// (set) Token: 0x06002585 RID: 9605 RVA: 0x000E9DFE File Offset: 0x000E7FFE
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

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x06002586 RID: 9606 RVA: 0x000E9E1E File Offset: 0x000E801E
		// (set) Token: 0x06002587 RID: 9607 RVA: 0x000E9E2A File Offset: 0x000E802A
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

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x06002588 RID: 9608 RVA: 0x000E9E4A File Offset: 0x000E804A
		// (set) Token: 0x06002589 RID: 9609 RVA: 0x000E9E56 File Offset: 0x000E8056
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

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x0600258A RID: 9610 RVA: 0x000E9E76 File Offset: 0x000E8076
		// (set) Token: 0x0600258B RID: 9611 RVA: 0x000E9E82 File Offset: 0x000E8082
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

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x0600258C RID: 9612 RVA: 0x000E9EA2 File Offset: 0x000E80A2
		// (set) Token: 0x0600258D RID: 9613 RVA: 0x000E9EAE File Offset: 0x000E80AE
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

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x0600258E RID: 9614 RVA: 0x000E9ECE File Offset: 0x000E80CE
		// (set) Token: 0x0600258F RID: 9615 RVA: 0x000E9EDA File Offset: 0x000E80DA
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

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x06002590 RID: 9616 RVA: 0x000E9EFA File Offset: 0x000E80FA
		// (set) Token: 0x06002591 RID: 9617 RVA: 0x000E9F06 File Offset: 0x000E8106
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

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x06002592 RID: 9618 RVA: 0x000E9F26 File Offset: 0x000E8126
		// (set) Token: 0x06002593 RID: 9619 RVA: 0x000E9F32 File Offset: 0x000E8132
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

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x06002594 RID: 9620 RVA: 0x000E9F52 File Offset: 0x000E8152
		// (set) Token: 0x06002595 RID: 9621 RVA: 0x000E9F5E File Offset: 0x000E815E
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

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x06002596 RID: 9622 RVA: 0x000E9F7E File Offset: 0x000E817E
		// (set) Token: 0x06002597 RID: 9623 RVA: 0x000E9F8A File Offset: 0x000E818A
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

		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x06002598 RID: 9624 RVA: 0x000E9FAA File Offset: 0x000E81AA
		// (set) Token: 0x06002599 RID: 9625 RVA: 0x000E9FB6 File Offset: 0x000E81B6
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

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x0600259A RID: 9626 RVA: 0x000E9FD6 File Offset: 0x000E81D6
		// (set) Token: 0x0600259B RID: 9627 RVA: 0x000E9FEC File Offset: 0x000E81EC
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

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x0600259C RID: 9628 RVA: 0x000EA03F File Offset: 0x000E823F
		// (set) Token: 0x0600259D RID: 9629 RVA: 0x000EA054 File Offset: 0x000E8254
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

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x0600259E RID: 9630 RVA: 0x000EA074 File Offset: 0x000E8274
		// (set) Token: 0x0600259F RID: 9631 RVA: 0x000EA089 File Offset: 0x000E8289
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

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x060025A0 RID: 9632 RVA: 0x000EA0A9 File Offset: 0x000E82A9
		// (set) Token: 0x060025A1 RID: 9633 RVA: 0x000EA0BE File Offset: 0x000E82BE
		public static bool SimulateNotOwningIdology
		{
			get
			{
				return Prefs.data == null || Prefs.data.simulateNotOwningIdeology;
			}
			set
			{
				if (Prefs.data.simulateNotOwningIdeology == value)
				{
					return;
				}
				Prefs.data.simulateNotOwningIdeology = value;
				Prefs.Apply();
			}
		}

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x060025A2 RID: 9634 RVA: 0x000EA0DE File Offset: 0x000E82DE
		// (set) Token: 0x060025A3 RID: 9635 RVA: 0x000EA0EA File Offset: 0x000E82EA
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

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x060025A4 RID: 9636 RVA: 0x000EA10A File Offset: 0x000E830A
		// (set) Token: 0x060025A5 RID: 9637 RVA: 0x000EA116 File Offset: 0x000E8316
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

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x060025A6 RID: 9638 RVA: 0x000EA13B File Offset: 0x000E833B
		// (set) Token: 0x060025A7 RID: 9639 RVA: 0x000EA147 File Offset: 0x000E8347
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

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x060025A8 RID: 9640 RVA: 0x000EA167 File Offset: 0x000E8367
		// (set) Token: 0x060025A9 RID: 9641 RVA: 0x000EA17C File Offset: 0x000E837C
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

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x060025AA RID: 9642 RVA: 0x000EA189 File Offset: 0x000E8389
		// (set) Token: 0x060025AB RID: 9643 RVA: 0x000EA195 File Offset: 0x000E8395
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

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x060025AC RID: 9644 RVA: 0x000EA1A2 File Offset: 0x000E83A2
		// (set) Token: 0x060025AD RID: 9645 RVA: 0x000EA1AE File Offset: 0x000E83AE
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

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x060025AE RID: 9646 RVA: 0x000EA1BB File Offset: 0x000E83BB
		// (set) Token: 0x060025AF RID: 9647 RVA: 0x000EA1C7 File Offset: 0x000E83C7
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

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x060025B0 RID: 9648 RVA: 0x000EA1D4 File Offset: 0x000E83D4
		// (set) Token: 0x060025B1 RID: 9649 RVA: 0x000EA1E0 File Offset: 0x000E83E0
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

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x060025B2 RID: 9650 RVA: 0x000EA1ED File Offset: 0x000E83ED
		// (set) Token: 0x060025B3 RID: 9651 RVA: 0x000EA1F9 File Offset: 0x000E83F9
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

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x060025B4 RID: 9652 RVA: 0x000EA206 File Offset: 0x000E8406
		// (set) Token: 0x060025B5 RID: 9653 RVA: 0x000EA212 File Offset: 0x000E8412
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

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x060025B6 RID: 9654 RVA: 0x000EA21F File Offset: 0x000E841F
		// (set) Token: 0x060025B7 RID: 9655 RVA: 0x000EA22B File Offset: 0x000E842B
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

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x060025B8 RID: 9656 RVA: 0x000EA24B File Offset: 0x000E844B
		// (set) Token: 0x060025B9 RID: 9657 RVA: 0x000EA257 File Offset: 0x000E8457
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

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x060025BA RID: 9658 RVA: 0x000EA264 File Offset: 0x000E8464
		// (set) Token: 0x060025BB RID: 9659 RVA: 0x000EA270 File Offset: 0x000E8470
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

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x060025BC RID: 9660 RVA: 0x000EA27D File Offset: 0x000E847D
		// (set) Token: 0x060025BD RID: 9661 RVA: 0x000EA289 File Offset: 0x000E8489
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

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x060025BE RID: 9662 RVA: 0x000EA296 File Offset: 0x000E8496
		// (set) Token: 0x060025BF RID: 9663 RVA: 0x000EA2A2 File Offset: 0x000E84A2
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

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x060025C0 RID: 9664 RVA: 0x000EA2AF File Offset: 0x000E84AF
		// (set) Token: 0x060025C1 RID: 9665 RVA: 0x000EA2BB File Offset: 0x000E84BB
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

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x060025C2 RID: 9666 RVA: 0x000EA2DB File Offset: 0x000E84DB
		// (set) Token: 0x060025C3 RID: 9667 RVA: 0x000EA2E7 File Offset: 0x000E84E7
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

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x060025C4 RID: 9668 RVA: 0x000EA2F4 File Offset: 0x000E84F4
		// (set) Token: 0x060025C5 RID: 9669 RVA: 0x000EA345 File Offset: 0x000E8545
		public static ExpansionDef BackgroundImageExpansion
		{
			get
			{
				if (Prefs.data.backgroundExpansionId != null)
				{
					ExpansionDef expansionWithIdentifier = ModLister.GetExpansionWithIdentifier(Prefs.data.backgroundExpansionId);
					if (expansionWithIdentifier != null && expansionWithIdentifier.Status != ExpansionStatus.NotInstalled)
					{
						return expansionWithIdentifier;
					}
				}
				string lastInstalledExpansionId = ModsConfig.LastInstalledExpansionId;
				if (lastInstalledExpansionId != null)
				{
					ExpansionDef expansionWithIdentifier2 = ModLister.GetExpansionWithIdentifier(lastInstalledExpansionId);
					if (expansionWithIdentifier2 != null)
					{
						return expansionWithIdentifier2;
					}
				}
				return ExpansionDefOf.Core;
			}
			set
			{
				Prefs.data.backgroundExpansionId = ((value != null) ? value.linkedMod : null);
				((UI_BackgroundMain)UIMenuBackgroundManager.background).overrideBGImage = ((value != null) ? value.BackgroundImage : null);
			}
		}

		// Token: 0x060025C6 RID: 9670 RVA: 0x000EA378 File Offset: 0x000E8578
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

		// Token: 0x060025C7 RID: 9671 RVA: 0x000EA404 File Offset: 0x000E8604
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
				Log.Error("Exception saving prefs: " + ex);
			}
		}

		// Token: 0x060025C8 RID: 9672 RVA: 0x000EA48C File Offset: 0x000E868C
		public static void Apply()
		{
			Prefs.data.Apply();
		}

		// Token: 0x060025C9 RID: 9673 RVA: 0x000EA498 File Offset: 0x000E8698
		public static void Notify_NewExpansion()
		{
			Prefs.data.backgroundExpansionId = null;
		}

		// Token: 0x060025CA RID: 9674 RVA: 0x000EA4A8 File Offset: 0x000E86A8
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

		// Token: 0x04001779 RID: 6009
		private static PrefsData data;
	}
}
