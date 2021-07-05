using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200081C RID: 2076
	public static class Find
	{
		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x06003415 RID: 13333 RVA: 0x00028D1A File Offset: 0x00026F1A
		public static Root Root
		{
			get
			{
				return Current.Root;
			}
		}

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x06003416 RID: 13334 RVA: 0x00028D21 File Offset: 0x00026F21
		public static SoundRoot SoundRoot
		{
			get
			{
				return Current.Root.soundRoot;
			}
		}

		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x06003417 RID: 13335 RVA: 0x00028D2D File Offset: 0x00026F2D
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

		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x06003418 RID: 13336 RVA: 0x00028D48 File Offset: 0x00026F48
		public static MusicManagerEntry MusicManagerEntry
		{
			get
			{
				return ((Root_Entry)Current.Root).musicManagerEntry;
			}
		}

		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x06003419 RID: 13337 RVA: 0x00028D59 File Offset: 0x00026F59
		public static MusicManagerPlay MusicManagerPlay
		{
			get
			{
				return ((Root_Play)Current.Root).musicManagerPlay;
			}
		}

		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x0600341A RID: 13338 RVA: 0x00028D6A File Offset: 0x00026F6A
		public static LanguageWorker ActiveLanguageWorker
		{
			get
			{
				return LanguageDatabase.activeLanguage.Worker;
			}
		}

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x0600341B RID: 13339 RVA: 0x00028D76 File Offset: 0x00026F76
		public static Camera Camera
		{
			get
			{
				return Current.Camera;
			}
		}

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x0600341C RID: 13340 RVA: 0x00028D7D File Offset: 0x00026F7D
		public static CameraDriver CameraDriver
		{
			get
			{
				return Current.CameraDriver;
			}
		}

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x0600341D RID: 13341 RVA: 0x00028D84 File Offset: 0x00026F84
		public static ColorCorrectionCurves CameraColor
		{
			get
			{
				return Current.ColorCorrectionCurves;
			}
		}

		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x0600341E RID: 13342 RVA: 0x00028D8B File Offset: 0x00026F8B
		public static Camera PortraitCamera
		{
			get
			{
				return PortraitCameraManager.PortraitCamera;
			}
		}

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x0600341F RID: 13343 RVA: 0x00028D92 File Offset: 0x00026F92
		public static PortraitRenderer PortraitRenderer
		{
			get
			{
				return PortraitCameraManager.PortraitRenderer;
			}
		}

		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x06003420 RID: 13344 RVA: 0x00028D99 File Offset: 0x00026F99
		public static Camera WorldCamera
		{
			get
			{
				return WorldCameraManager.WorldCamera;
			}
		}

		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x06003421 RID: 13345 RVA: 0x00028DA0 File Offset: 0x00026FA0
		public static WorldCameraDriver WorldCameraDriver
		{
			get
			{
				return WorldCameraManager.WorldCameraDriver;
			}
		}

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x06003422 RID: 13346 RVA: 0x00028DA7 File Offset: 0x00026FA7
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

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x06003423 RID: 13347 RVA: 0x00028DBC File Offset: 0x00026FBC
		public static ScreenshotModeHandler ScreenshotModeHandler
		{
			get
			{
				return Find.UIRoot.screenshotMode;
			}
		}

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x06003424 RID: 13348 RVA: 0x00028DC8 File Offset: 0x00026FC8
		public static MainButtonsRoot MainButtonsRoot
		{
			get
			{
				return ((UIRoot_Play)Find.UIRoot).mainButtonsRoot;
			}
		}

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x06003425 RID: 13349 RVA: 0x00028DD9 File Offset: 0x00026FD9
		public static MainTabsRoot MainTabsRoot
		{
			get
			{
				return Find.MainButtonsRoot.tabs;
			}
		}

		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x06003426 RID: 13350 RVA: 0x00028DE5 File Offset: 0x00026FE5
		public static MapInterface MapUI
		{
			get
			{
				return ((UIRoot_Play)Find.UIRoot).mapUI;
			}
		}

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x06003427 RID: 13351 RVA: 0x00028DF6 File Offset: 0x00026FF6
		public static Selector Selector
		{
			get
			{
				return Find.MapUI.selector;
			}
		}

		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x06003428 RID: 13352 RVA: 0x00028E02 File Offset: 0x00027002
		public static Targeter Targeter
		{
			get
			{
				return Find.MapUI.targeter;
			}
		}

		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x06003429 RID: 13353 RVA: 0x00028E0E File Offset: 0x0002700E
		public static ColonistBar ColonistBar
		{
			get
			{
				return Find.MapUI.colonistBar;
			}
		}

		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x0600342A RID: 13354 RVA: 0x00028E1A File Offset: 0x0002701A
		public static DesignatorManager DesignatorManager
		{
			get
			{
				return Find.MapUI.designatorManager;
			}
		}

		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x0600342B RID: 13355 RVA: 0x00028E26 File Offset: 0x00027026
		public static ReverseDesignatorDatabase ReverseDesignatorDatabase
		{
			get
			{
				return Find.MapUI.reverseDesignatorDatabase;
			}
		}

		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x0600342C RID: 13356 RVA: 0x00028E32 File Offset: 0x00027032
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

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x0600342D RID: 13357 RVA: 0x00028E47 File Offset: 0x00027047
		public static GameInfo GameInfo
		{
			get
			{
				return Current.Game.Info;
			}
		}

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x0600342E RID: 13358 RVA: 0x00151CCC File Offset: 0x0014FECC
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

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x0600342F RID: 13359 RVA: 0x00028E53 File Offset: 0x00027053
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

		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x06003430 RID: 13360 RVA: 0x00028E78 File Offset: 0x00027078
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

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x06003431 RID: 13361 RVA: 0x00028E8D File Offset: 0x0002708D
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

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x06003432 RID: 13362 RVA: 0x00028EA2 File Offset: 0x000270A2
		public static Map AnyPlayerHomeMap
		{
			get
			{
				return Current.Game.AnyPlayerHomeMap;
			}
		}

		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x06003433 RID: 13363 RVA: 0x00028EAE File Offset: 0x000270AE
		public static Map RandomPlayerHomeMap
		{
			get
			{
				return Current.Game.RandomPlayerHomeMap;
			}
		}

		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x06003434 RID: 13364 RVA: 0x00028EBA File Offset: 0x000270BA
		public static StoryWatcher StoryWatcher
		{
			get
			{
				return Current.Game.storyWatcher;
			}
		}

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x06003435 RID: 13365 RVA: 0x00028EC6 File Offset: 0x000270C6
		public static ResearchManager ResearchManager
		{
			get
			{
				return Current.Game.researchManager;
			}
		}

		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x06003436 RID: 13366 RVA: 0x00028ED2 File Offset: 0x000270D2
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

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x06003437 RID: 13367 RVA: 0x00028EE7 File Offset: 0x000270E7
		public static GameEnder GameEnder
		{
			get
			{
				return Current.Game.gameEnder;
			}
		}

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x06003438 RID: 13368 RVA: 0x00028EF3 File Offset: 0x000270F3
		public static LetterStack LetterStack
		{
			get
			{
				return Current.Game.letterStack;
			}
		}

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x06003439 RID: 13369 RVA: 0x00028EFF File Offset: 0x000270FF
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

		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x0600343A RID: 13370 RVA: 0x00028F14 File Offset: 0x00027114
		public static PlaySettings PlaySettings
		{
			get
			{
				return Current.Game.playSettings;
			}
		}

		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x0600343B RID: 13371 RVA: 0x00028F20 File Offset: 0x00027120
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

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x0600343C RID: 13372 RVA: 0x00028F35 File Offset: 0x00027135
		public static TaleManager TaleManager
		{
			get
			{
				return Current.Game.taleManager;
			}
		}

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x0600343D RID: 13373 RVA: 0x00028F41 File Offset: 0x00027141
		public static PlayLog PlayLog
		{
			get
			{
				return Current.Game.playLog;
			}
		}

		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x0600343E RID: 13374 RVA: 0x00028F4D File Offset: 0x0002714D
		public static BattleLog BattleLog
		{
			get
			{
				return Current.Game.battleLog;
			}
		}

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x0600343F RID: 13375 RVA: 0x00028F59 File Offset: 0x00027159
		public static TickManager TickManager
		{
			get
			{
				return Current.Game.tickManager;
			}
		}

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x06003440 RID: 13376 RVA: 0x00028F65 File Offset: 0x00027165
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

		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x06003441 RID: 13377 RVA: 0x00028F7A File Offset: 0x0002717A
		public static TutorialState TutorialState
		{
			get
			{
				return Current.Game.tutor.tutorialState;
			}
		}

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x06003442 RID: 13378 RVA: 0x00028F8B File Offset: 0x0002718B
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

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x06003443 RID: 13379 RVA: 0x00028FA5 File Offset: 0x000271A5
		public static Autosaver Autosaver
		{
			get
			{
				return Current.Game.autosaver;
			}
		}

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x06003444 RID: 13380 RVA: 0x00028FB1 File Offset: 0x000271B1
		public static DateNotifier DateNotifier
		{
			get
			{
				return Current.Game.dateNotifier;
			}
		}

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x06003445 RID: 13381 RVA: 0x00028FBD File Offset: 0x000271BD
		public static SignalManager SignalManager
		{
			get
			{
				return Current.Game.signalManager;
			}
		}

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x06003446 RID: 13382 RVA: 0x00028FC9 File Offset: 0x000271C9
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

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x06003447 RID: 13383 RVA: 0x00028FDE File Offset: 0x000271DE
		public static QuestManager QuestManager
		{
			get
			{
				return Current.Game.questManager;
			}
		}

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x06003448 RID: 13384 RVA: 0x00028FEA File Offset: 0x000271EA
		public static FactionManager FactionManager
		{
			get
			{
				return Find.World.factionManager;
			}
		}

		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x06003449 RID: 13385 RVA: 0x00028FF6 File Offset: 0x000271F6
		public static WorldPawns WorldPawns
		{
			get
			{
				return Find.World.worldPawns;
			}
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x0600344A RID: 13386 RVA: 0x00029002 File Offset: 0x00027202
		public static WorldObjectsHolder WorldObjects
		{
			get
			{
				return Find.World.worldObjects;
			}
		}

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x0600344B RID: 13387 RVA: 0x0002900E File Offset: 0x0002720E
		public static WorldGrid WorldGrid
		{
			get
			{
				return Find.World.grid;
			}
		}

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x0600344C RID: 13388 RVA: 0x0002901A File Offset: 0x0002721A
		public static WorldDebugDrawer WorldDebugDrawer
		{
			get
			{
				return Find.World.debugDrawer;
			}
		}

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x0600344D RID: 13389 RVA: 0x00029026 File Offset: 0x00027226
		public static WorldPathGrid WorldPathGrid
		{
			get
			{
				return Find.World.pathGrid;
			}
		}

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x0600344E RID: 13390 RVA: 0x00029032 File Offset: 0x00027232
		public static WorldDynamicDrawManager WorldDynamicDrawManager
		{
			get
			{
				return Find.World.dynamicDrawManager;
			}
		}

		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x0600344F RID: 13391 RVA: 0x0002903E File Offset: 0x0002723E
		public static WorldPathFinder WorldPathFinder
		{
			get
			{
				return Find.World.pathFinder;
			}
		}

		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x06003450 RID: 13392 RVA: 0x0002904A File Offset: 0x0002724A
		public static WorldPathPool WorldPathPool
		{
			get
			{
				return Find.World.pathPool;
			}
		}

		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x06003451 RID: 13393 RVA: 0x00029056 File Offset: 0x00027256
		public static WorldReachability WorldReachability
		{
			get
			{
				return Find.World.reachability;
			}
		}

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x06003452 RID: 13394 RVA: 0x00029062 File Offset: 0x00027262
		public static WorldFloodFiller WorldFloodFiller
		{
			get
			{
				return Find.World.floodFiller;
			}
		}

		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x06003453 RID: 13395 RVA: 0x0002906E File Offset: 0x0002726E
		public static WorldFeatures WorldFeatures
		{
			get
			{
				return Find.World.features;
			}
		}

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x06003454 RID: 13396 RVA: 0x0002907A File Offset: 0x0002727A
		public static WorldInterface WorldInterface
		{
			get
			{
				return Find.World.UI;
			}
		}

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x06003455 RID: 13397 RVA: 0x00029086 File Offset: 0x00027286
		public static WorldSelector WorldSelector
		{
			get
			{
				return Find.WorldInterface.selector;
			}
		}

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x06003456 RID: 13398 RVA: 0x00029092 File Offset: 0x00027292
		public static WorldTargeter WorldTargeter
		{
			get
			{
				return Find.WorldInterface.targeter;
			}
		}

		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x06003457 RID: 13399 RVA: 0x0002909E File Offset: 0x0002729E
		public static WorldRoutePlanner WorldRoutePlanner
		{
			get
			{
				return Find.WorldInterface.routePlanner;
			}
		}
	}
}
