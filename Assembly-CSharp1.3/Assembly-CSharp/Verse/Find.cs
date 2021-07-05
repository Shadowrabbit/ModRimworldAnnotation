using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020004A9 RID: 1193
	public static class Find
	{
		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06002418 RID: 9240 RVA: 0x000E07FC File Offset: 0x000DE9FC
		public static Root Root
		{
			get
			{
				return Current.Root;
			}
		}

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06002419 RID: 9241 RVA: 0x000E0803 File Offset: 0x000DEA03
		public static SoundRoot SoundRoot
		{
			get
			{
				return Current.Root.soundRoot;
			}
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x0600241A RID: 9242 RVA: 0x000E080F File Offset: 0x000DEA0F
		public static UIRoot UIRoot
		{
			get
			{
				if (!(Current.Root != null))
				{
					return null;
				}
				return Current.Root.uiRoot;
			}
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x0600241B RID: 9243 RVA: 0x000E082A File Offset: 0x000DEA2A
		public static MusicManagerEntry MusicManagerEntry
		{
			get
			{
				return ((Root_Entry)Current.Root).musicManagerEntry;
			}
		}

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x0600241C RID: 9244 RVA: 0x000E083B File Offset: 0x000DEA3B
		public static MusicManagerPlay MusicManagerPlay
		{
			get
			{
				return ((Root_Play)Current.Root).musicManagerPlay;
			}
		}

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x0600241D RID: 9245 RVA: 0x000E084C File Offset: 0x000DEA4C
		public static LanguageWorker ActiveLanguageWorker
		{
			get
			{
				return LanguageDatabase.activeLanguage.Worker;
			}
		}

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x0600241E RID: 9246 RVA: 0x000E0858 File Offset: 0x000DEA58
		public static Camera Camera
		{
			get
			{
				return Current.Camera;
			}
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x0600241F RID: 9247 RVA: 0x000E085F File Offset: 0x000DEA5F
		public static CameraDriver CameraDriver
		{
			get
			{
				return Current.CameraDriver;
			}
		}

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x06002420 RID: 9248 RVA: 0x000E0866 File Offset: 0x000DEA66
		public static ColorCorrectionCurves CameraColor
		{
			get
			{
				return Current.ColorCorrectionCurves;
			}
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x06002421 RID: 9249 RVA: 0x000E086D File Offset: 0x000DEA6D
		public static Camera PawnCacheCamera
		{
			get
			{
				return PawnCacheCameraManager.PawnCacheCamera;
			}
		}

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x06002422 RID: 9250 RVA: 0x000E0874 File Offset: 0x000DEA74
		public static PawnCacheRenderer PawnCacheRenderer
		{
			get
			{
				return PawnCacheCameraManager.PawnCacheRenderer;
			}
		}

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06002423 RID: 9251 RVA: 0x000E087B File Offset: 0x000DEA7B
		public static Camera WorldCamera
		{
			get
			{
				return WorldCameraManager.WorldCamera;
			}
		}

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x06002424 RID: 9252 RVA: 0x000E0882 File Offset: 0x000DEA82
		public static WorldCameraDriver WorldCameraDriver
		{
			get
			{
				return WorldCameraManager.WorldCameraDriver;
			}
		}

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x06002425 RID: 9253 RVA: 0x000E0889 File Offset: 0x000DEA89
		public static WindowStack WindowStack
		{
			get
			{
				if (Find.UIRoot == null)
				{
					return null;
				}
				return Find.UIRoot.windows;
			}
		}

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x06002426 RID: 9254 RVA: 0x000E089E File Offset: 0x000DEA9E
		public static ScreenshotModeHandler ScreenshotModeHandler
		{
			get
			{
				return Find.UIRoot.screenshotMode;
			}
		}

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x06002427 RID: 9255 RVA: 0x000E08AA File Offset: 0x000DEAAA
		public static MainButtonsRoot MainButtonsRoot
		{
			get
			{
				return ((UIRoot_Play)Find.UIRoot).mainButtonsRoot;
			}
		}

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06002428 RID: 9256 RVA: 0x000E08BB File Offset: 0x000DEABB
		public static MainTabsRoot MainTabsRoot
		{
			get
			{
				return Find.MainButtonsRoot.tabs;
			}
		}

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x06002429 RID: 9257 RVA: 0x000E08C7 File Offset: 0x000DEAC7
		public static MapInterface MapUI
		{
			get
			{
				return ((UIRoot_Play)Find.UIRoot).mapUI;
			}
		}

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x0600242A RID: 9258 RVA: 0x000E08D8 File Offset: 0x000DEAD8
		public static Selector Selector
		{
			get
			{
				return Find.MapUI.selector;
			}
		}

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x0600242B RID: 9259 RVA: 0x000E08E4 File Offset: 0x000DEAE4
		public static Targeter Targeter
		{
			get
			{
				return Find.MapUI.targeter;
			}
		}

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x0600242C RID: 9260 RVA: 0x000E08F0 File Offset: 0x000DEAF0
		public static ColonistBar ColonistBar
		{
			get
			{
				return Find.MapUI.colonistBar;
			}
		}

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x0600242D RID: 9261 RVA: 0x000E08FC File Offset: 0x000DEAFC
		public static DesignatorManager DesignatorManager
		{
			get
			{
				return Find.MapUI.designatorManager;
			}
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x0600242E RID: 9262 RVA: 0x000E0908 File Offset: 0x000DEB08
		public static ReverseDesignatorDatabase ReverseDesignatorDatabase
		{
			get
			{
				return Find.MapUI.reverseDesignatorDatabase;
			}
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x0600242F RID: 9263 RVA: 0x000E0914 File Offset: 0x000DEB14
		public static GameInitData GameInitData
		{
			get
			{
				if (Current.Game == null)
				{
					return null;
				}
				return Current.Game.InitData;
			}
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06002430 RID: 9264 RVA: 0x000E0929 File Offset: 0x000DEB29
		public static GameInfo GameInfo
		{
			get
			{
				return Current.Game.Info;
			}
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x06002431 RID: 9265 RVA: 0x000E0938 File Offset: 0x000DEB38
		public static Scenario Scenario
		{
			get
			{
				if (Current.Game != null && Current.Game.Scenario != null)
				{
					return Current.Game.Scenario;
				}
				if (ScenarioMaker.GeneratingScenario != null)
				{
					return ScenarioMaker.GeneratingScenario;
				}
				if (Find.UIRoot != null)
				{
					Page_ScenarioEditor page_ScenarioEditor = Find.WindowStack.WindowOfType<Page_ScenarioEditor>();
					if (page_ScenarioEditor != null)
					{
						return page_ScenarioEditor.EditingScenario;
					}
				}
				return null;
			}
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06002432 RID: 9266 RVA: 0x000E098D File Offset: 0x000DEB8D
		public static World World
		{
			get
			{
				if (Current.Game == null || Current.Game.World == null)
				{
					return Current.CreatingWorld;
				}
				return Current.Game.World;
			}
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x06002433 RID: 9267 RVA: 0x000E09B2 File Offset: 0x000DEBB2
		public static List<Map> Maps
		{
			get
			{
				if (Current.Game == null)
				{
					return null;
				}
				return Current.Game.Maps;
			}
		}

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06002434 RID: 9268 RVA: 0x000E09C7 File Offset: 0x000DEBC7
		public static Map CurrentMap
		{
			get
			{
				if (Current.Game == null)
				{
					return null;
				}
				return Current.Game.CurrentMap;
			}
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06002435 RID: 9269 RVA: 0x000E09DC File Offset: 0x000DEBDC
		public static Map AnyPlayerHomeMap
		{
			get
			{
				return Current.Game.AnyPlayerHomeMap;
			}
		}

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x06002436 RID: 9270 RVA: 0x000E09E8 File Offset: 0x000DEBE8
		public static Map RandomPlayerHomeMap
		{
			get
			{
				return Current.Game.RandomPlayerHomeMap;
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x06002437 RID: 9271 RVA: 0x000E09F4 File Offset: 0x000DEBF4
		public static StoryWatcher StoryWatcher
		{
			get
			{
				return Current.Game.storyWatcher;
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x06002438 RID: 9272 RVA: 0x000E0A00 File Offset: 0x000DEC00
		public static ResearchManager ResearchManager
		{
			get
			{
				return Current.Game.researchManager;
			}
		}

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x06002439 RID: 9273 RVA: 0x000E0A0C File Offset: 0x000DEC0C
		public static Storyteller Storyteller
		{
			get
			{
				if (Current.Game == null)
				{
					return null;
				}
				return Current.Game.storyteller;
			}
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x0600243A RID: 9274 RVA: 0x000E0A21 File Offset: 0x000DEC21
		public static GameEnder GameEnder
		{
			get
			{
				return Current.Game.gameEnder;
			}
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x0600243B RID: 9275 RVA: 0x000E0A2D File Offset: 0x000DEC2D
		public static LetterStack LetterStack
		{
			get
			{
				return Current.Game.letterStack;
			}
		}

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x0600243C RID: 9276 RVA: 0x000E0A39 File Offset: 0x000DEC39
		public static Archive Archive
		{
			get
			{
				if (Find.History == null)
				{
					return null;
				}
				return Find.History.archive;
			}
		}

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x0600243D RID: 9277 RVA: 0x000E0A4E File Offset: 0x000DEC4E
		public static PlaySettings PlaySettings
		{
			get
			{
				return Current.Game.playSettings;
			}
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x0600243E RID: 9278 RVA: 0x000E0A5A File Offset: 0x000DEC5A
		public static History History
		{
			get
			{
				if (Current.Game == null)
				{
					return null;
				}
				return Current.Game.history;
			}
		}

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x0600243F RID: 9279 RVA: 0x000E0A6F File Offset: 0x000DEC6F
		public static TaleManager TaleManager
		{
			get
			{
				return Current.Game.taleManager;
			}
		}

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x06002440 RID: 9280 RVA: 0x000E0A7B File Offset: 0x000DEC7B
		public static PlayLog PlayLog
		{
			get
			{
				return Current.Game.playLog;
			}
		}

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x06002441 RID: 9281 RVA: 0x000E0A87 File Offset: 0x000DEC87
		public static BattleLog BattleLog
		{
			get
			{
				return Current.Game.battleLog;
			}
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x06002442 RID: 9282 RVA: 0x000E0A93 File Offset: 0x000DEC93
		public static TickManager TickManager
		{
			get
			{
				return Current.Game.tickManager;
			}
		}

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x06002443 RID: 9283 RVA: 0x000E0A9F File Offset: 0x000DEC9F
		public static Tutor Tutor
		{
			get
			{
				if (Current.Game == null)
				{
					return null;
				}
				return Current.Game.tutor;
			}
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x06002444 RID: 9284 RVA: 0x000E0AB4 File Offset: 0x000DECB4
		public static TutorialState TutorialState
		{
			get
			{
				return Current.Game.tutor.tutorialState;
			}
		}

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x06002445 RID: 9285 RVA: 0x000E0AC5 File Offset: 0x000DECC5
		public static ActiveLessonHandler ActiveLesson
		{
			get
			{
				if (Current.Game == null)
				{
					return null;
				}
				return Current.Game.tutor.activeLesson;
			}
		}

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x06002446 RID: 9286 RVA: 0x000E0ADF File Offset: 0x000DECDF
		public static Autosaver Autosaver
		{
			get
			{
				return Current.Game.autosaver;
			}
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x06002447 RID: 9287 RVA: 0x000E0AEB File Offset: 0x000DECEB
		public static DateNotifier DateNotifier
		{
			get
			{
				return Current.Game.dateNotifier;
			}
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06002448 RID: 9288 RVA: 0x000E0AF7 File Offset: 0x000DECF7
		public static SignalManager SignalManager
		{
			get
			{
				return Current.Game.signalManager;
			}
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06002449 RID: 9289 RVA: 0x000E0B03 File Offset: 0x000DED03
		public static UniqueIDsManager UniqueIDsManager
		{
			get
			{
				if (Current.Game == null)
				{
					return null;
				}
				return Current.Game.uniqueIDsManager;
			}
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x0600244A RID: 9290 RVA: 0x000E0B18 File Offset: 0x000DED18
		public static QuestManager QuestManager
		{
			get
			{
				return Current.Game.questManager;
			}
		}

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x0600244B RID: 9291 RVA: 0x000E0B24 File Offset: 0x000DED24
		public static TransportShipManager TransportShipManager
		{
			get
			{
				return Current.Game.transportShipManager;
			}
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x0600244C RID: 9292 RVA: 0x000E0B30 File Offset: 0x000DED30
		public static FactionManager FactionManager
		{
			get
			{
				return Find.World.factionManager;
			}
		}

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x0600244D RID: 9293 RVA: 0x000E0B3C File Offset: 0x000DED3C
		public static IdeoManager IdeoManager
		{
			get
			{
				return Find.World.ideoManager;
			}
		}

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x0600244E RID: 9294 RVA: 0x000E0B48 File Offset: 0x000DED48
		public static WorldPawns WorldPawns
		{
			get
			{
				return Find.World.worldPawns;
			}
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x0600244F RID: 9295 RVA: 0x000E0B54 File Offset: 0x000DED54
		public static WorldObjectsHolder WorldObjects
		{
			get
			{
				return Find.World.worldObjects;
			}
		}

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x06002450 RID: 9296 RVA: 0x000E0B60 File Offset: 0x000DED60
		public static WorldGrid WorldGrid
		{
			get
			{
				return Find.World.grid;
			}
		}

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x06002451 RID: 9297 RVA: 0x000E0B6C File Offset: 0x000DED6C
		public static WorldDebugDrawer WorldDebugDrawer
		{
			get
			{
				return Find.World.debugDrawer;
			}
		}

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x06002452 RID: 9298 RVA: 0x000E0B78 File Offset: 0x000DED78
		public static WorldPathGrid WorldPathGrid
		{
			get
			{
				return Find.World.pathGrid;
			}
		}

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x06002453 RID: 9299 RVA: 0x000E0B84 File Offset: 0x000DED84
		public static WorldDynamicDrawManager WorldDynamicDrawManager
		{
			get
			{
				return Find.World.dynamicDrawManager;
			}
		}

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x06002454 RID: 9300 RVA: 0x000E0B90 File Offset: 0x000DED90
		public static WorldPathFinder WorldPathFinder
		{
			get
			{
				return Find.World.pathFinder;
			}
		}

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x06002455 RID: 9301 RVA: 0x000E0B9C File Offset: 0x000DED9C
		public static WorldPathPool WorldPathPool
		{
			get
			{
				return Find.World.pathPool;
			}
		}

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x06002456 RID: 9302 RVA: 0x000E0BA8 File Offset: 0x000DEDA8
		public static WorldReachability WorldReachability
		{
			get
			{
				return Find.World.reachability;
			}
		}

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x06002457 RID: 9303 RVA: 0x000E0BB4 File Offset: 0x000DEDB4
		public static WorldFloodFiller WorldFloodFiller
		{
			get
			{
				return Find.World.floodFiller;
			}
		}

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x06002458 RID: 9304 RVA: 0x000E0BC0 File Offset: 0x000DEDC0
		public static WorldFeatures WorldFeatures
		{
			get
			{
				return Find.World.features;
			}
		}

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x06002459 RID: 9305 RVA: 0x000E0BCC File Offset: 0x000DEDCC
		public static WorldInterface WorldInterface
		{
			get
			{
				return Find.World.UI;
			}
		}

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x0600245A RID: 9306 RVA: 0x000E0BD8 File Offset: 0x000DEDD8
		public static WorldSelector WorldSelector
		{
			get
			{
				return Find.WorldInterface.selector;
			}
		}

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x0600245B RID: 9307 RVA: 0x000E0BE4 File Offset: 0x000DEDE4
		public static WorldTargeter WorldTargeter
		{
			get
			{
				return Find.WorldInterface.targeter;
			}
		}

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x0600245C RID: 9308 RVA: 0x000E0BF0 File Offset: 0x000DEDF0
		public static WorldRoutePlanner WorldRoutePlanner
		{
			get
			{
				return Find.WorldInterface.routePlanner;
			}
		}

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x0600245D RID: 9309 RVA: 0x000E0BFC File Offset: 0x000DEDFC
		public static TilePicker TilePicker
		{
			get
			{
				return Find.WorldInterface.tilePicker;
			}
		}

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x0600245E RID: 9310 RVA: 0x000E0C08 File Offset: 0x000DEE08
		public static HistoryEventsManager HistoryEventsManager
		{
			get
			{
				return Find.History.historyEventsManager;
			}
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x0600245F RID: 9311 RVA: 0x000E0C14 File Offset: 0x000DEE14
		public static GoodwillSituationManager GoodwillSituationManager
		{
			get
			{
				return Find.FactionManager.goodwillSituationManager;
			}
		}
	}
}
