using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using Verse.Profile;

namespace Verse
{
	// Token: 0x0200012F RID: 303
	public class Game : IExposable
	{
		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x0600083C RID: 2108 RVA: 0x0002655C File Offset: 0x0002475C
		// (set) Token: 0x0600083D RID: 2109 RVA: 0x00026564 File Offset: 0x00024764
		public Scenario Scenario
		{
			get
			{
				return this.scenarioInt;
			}
			set
			{
				this.scenarioInt = value;
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x0600083E RID: 2110 RVA: 0x0002656D File Offset: 0x0002476D
		// (set) Token: 0x0600083F RID: 2111 RVA: 0x00026575 File Offset: 0x00024775
		public World World
		{
			get
			{
				return this.worldInt;
			}
			set
			{
				if (this.worldInt == value)
				{
					return;
				}
				this.worldInt = value;
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000840 RID: 2112 RVA: 0x00026588 File Offset: 0x00024788
		// (set) Token: 0x06000841 RID: 2113 RVA: 0x000265A8 File Offset: 0x000247A8
		public Map CurrentMap
		{
			get
			{
				if (this.currentMapIndex < 0)
				{
					return null;
				}
				return this.maps[(int)this.currentMapIndex];
			}
			set
			{
				int num;
				if (value == null)
				{
					num = -1;
				}
				else
				{
					num = this.maps.IndexOf(value);
					if (num < 0)
					{
						Log.Error("Could not set current map because it does not exist.");
						return;
					}
				}
				if ((int)this.currentMapIndex != num)
				{
					this.currentMapIndex = (sbyte)num;
					Find.MapUI.Notify_SwitchedMap();
					AmbientSoundManager.Notify_SwitchedMap();
				}
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000842 RID: 2114 RVA: 0x000265F8 File Offset: 0x000247F8
		public Map AnyPlayerHomeMap
		{
			get
			{
				if (Faction.OfPlayerSilentFail == null)
				{
					return null;
				}
				for (int i = 0; i < this.maps.Count; i++)
				{
					Map map = this.maps[i];
					if (map.IsPlayerHome)
					{
						return map;
					}
				}
				return null;
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000843 RID: 2115 RVA: 0x0002663C File Offset: 0x0002483C
		public Map RandomPlayerHomeMap
		{
			get
			{
				if (Faction.OfPlayerSilentFail == null)
				{
					return null;
				}
				Game.tmpPlayerHomeMaps.Clear();
				for (int i = 0; i < this.maps.Count; i++)
				{
					Map map = this.maps[i];
					if (map.IsPlayerHome)
					{
						Game.tmpPlayerHomeMaps.Add(map);
					}
				}
				if (Game.tmpPlayerHomeMaps.Any<Map>())
				{
					Map result = Game.tmpPlayerHomeMaps.RandomElement<Map>();
					Game.tmpPlayerHomeMaps.Clear();
					return result;
				}
				return null;
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000844 RID: 2116 RVA: 0x000266B4 File Offset: 0x000248B4
		public List<Map> Maps
		{
			get
			{
				return this.maps;
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000845 RID: 2117 RVA: 0x000266BC File Offset: 0x000248BC
		// (set) Token: 0x06000846 RID: 2118 RVA: 0x000266C4 File Offset: 0x000248C4
		public GameInitData InitData
		{
			get
			{
				return this.initData;
			}
			set
			{
				this.initData = value;
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000847 RID: 2119 RVA: 0x000266CD File Offset: 0x000248CD
		public GameInfo Info
		{
			get
			{
				return this.info;
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000848 RID: 2120 RVA: 0x000266D5 File Offset: 0x000248D5
		public GameRules Rules
		{
			get
			{
				return this.rules;
			}
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x000266E0 File Offset: 0x000248E0
		public Game()
		{
			this.FillComponents();
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x00026814 File Offset: 0x00024A14
		public void AddMap(Map map)
		{
			if (map == null)
			{
				Log.Error("Tried to add null map.");
				return;
			}
			if (this.maps.Contains(map))
			{
				Log.Error("Tried to add map but it's already here.");
				return;
			}
			if (this.maps.Count > 127)
			{
				Log.Error("Can't add map. Reached maps count limit (" + sbyte.MaxValue + ").");
				return;
			}
			this.maps.Add(map);
			Find.ColonistBar.MarkColonistsDirty();
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x0002688C File Offset: 0x00024A8C
		public Map FindMap(MapParent mapParent)
		{
			for (int i = 0; i < this.maps.Count; i++)
			{
				if (this.maps[i].info.parent == mapParent)
				{
					return this.maps[i];
				}
			}
			return null;
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x000268D8 File Offset: 0x00024AD8
		public Map FindMap(int tile)
		{
			for (int i = 0; i < this.maps.Count; i++)
			{
				if (this.maps[i].Tile == tile)
				{
					return this.maps[i];
				}
			}
			return null;
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x00026920 File Offset: 0x00024B20
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				Log.Error("You must use special LoadData method to load Game.");
				return;
			}
			Scribe_Values.Look<sbyte>(ref this.currentMapIndex, "currentMapIndex", -1, false);
			this.ExposeSmallComponents();
			Scribe_Deep.Look<World>(ref this.worldInt, "world", Array.Empty<object>());
			Scribe_Collections.Look<Map>(ref this.maps, "maps", LookMode.Deep, Array.Empty<object>());
			Find.CameraDriver.Expose();
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x00026990 File Offset: 0x00024B90
		private void ExposeSmallComponents()
		{
			Scribe_Deep.Look<GameInfo>(ref this.info, "info", Array.Empty<object>());
			Scribe_Deep.Look<GameRules>(ref this.rules, "rules", Array.Empty<object>());
			Scribe_Deep.Look<Scenario>(ref this.scenarioInt, "scenario", Array.Empty<object>());
			Scribe_Deep.Look<TickManager>(ref this.tickManager, "tickManager", Array.Empty<object>());
			Scribe_Deep.Look<PlaySettings>(ref this.playSettings, "playSettings", Array.Empty<object>());
			Scribe_Deep.Look<StoryWatcher>(ref this.storyWatcher, "storyWatcher", Array.Empty<object>());
			Scribe_Deep.Look<GameEnder>(ref this.gameEnder, "gameEnder", Array.Empty<object>());
			Scribe_Deep.Look<LetterStack>(ref this.letterStack, "letterStack", Array.Empty<object>());
			Scribe_Deep.Look<ResearchManager>(ref this.researchManager, "researchManager", Array.Empty<object>());
			Scribe_Deep.Look<Storyteller>(ref this.storyteller, "storyteller", Array.Empty<object>());
			Scribe_Deep.Look<History>(ref this.history, "history", Array.Empty<object>());
			Scribe_Deep.Look<TaleManager>(ref this.taleManager, "taleManager", Array.Empty<object>());
			Scribe_Deep.Look<PlayLog>(ref this.playLog, "playLog", Array.Empty<object>());
			Scribe_Deep.Look<BattleLog>(ref this.battleLog, "battleLog", Array.Empty<object>());
			Scribe_Deep.Look<OutfitDatabase>(ref this.outfitDatabase, "outfitDatabase", Array.Empty<object>());
			Scribe_Deep.Look<DrugPolicyDatabase>(ref this.drugPolicyDatabase, "drugPolicyDatabase", Array.Empty<object>());
			Scribe_Deep.Look<FoodRestrictionDatabase>(ref this.foodRestrictionDatabase, "foodRestrictionDatabase", Array.Empty<object>());
			Scribe_Deep.Look<Tutor>(ref this.tutor, "tutor", Array.Empty<object>());
			Scribe_Deep.Look<DateNotifier>(ref this.dateNotifier, "dateNotifier", Array.Empty<object>());
			Scribe_Deep.Look<UniqueIDsManager>(ref this.uniqueIDsManager, "uniqueIDsManager", Array.Empty<object>());
			Scribe_Deep.Look<QuestManager>(ref this.questManager, "questManager", Array.Empty<object>());
			Scribe_Deep.Look<TransportShipManager>(ref this.transportShipManager, "transportShipManager", Array.Empty<object>());
			Scribe_Collections.Look<GameComponent>(ref this.components, "components", LookMode.Deep, new object[]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.FillComponents();
				if (this.rules == null)
				{
					Log.Warning("Save game was missing rules. Replacing with a blank GameRules.");
					this.rules = new GameRules();
				}
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x00026BB8 File Offset: 0x00024DB8
		private void FillComponents()
		{
			this.components.RemoveAll((GameComponent component) => component == null);
			foreach (Type type in typeof(GameComponent).AllSubclassesNonAbstract())
			{
				if (this.GetComponent(type) == null)
				{
					try
					{
						GameComponent item = (GameComponent)Activator.CreateInstance(type, new object[]
						{
							this
						});
						this.components.Add(item);
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not instantiate a GameComponent of type ",
							type,
							": ",
							ex
						}));
					}
				}
			}
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x00026C9C File Offset: 0x00024E9C
		public void InitNewGame()
		{
			string str = (from mod in LoadedModManager.RunningMods
			select mod.PackageIdPlayerFacing).ToLineList("  - ", false);
			Log.Message("Initializing new game with mods:\n" + str);
			if (this.maps.Any<Map>())
			{
				Log.Error("Called InitNewGame() but there already is a map. There should be 0 maps...");
				return;
			}
			if (this.initData == null)
			{
				Log.Error("Called InitNewGame() but init data is null. Create it first.");
				return;
			}
			MemoryUtility.UnloadUnusedUnityAssets();
			DeepProfiler.Start("InitNewGame");
			try
			{
				Current.ProgramState = ProgramState.MapInitializing;
				IntVec3 intVec = new IntVec3(this.initData.mapSize, 1, this.initData.mapSize);
				Settlement settlement = null;
				List<Settlement> settlements = Find.WorldObjects.Settlements;
				for (int i = 0; i < settlements.Count; i++)
				{
					if (settlements[i].Faction == Faction.OfPlayer)
					{
						settlement = settlements[i];
						break;
					}
				}
				if (settlement == null)
				{
					Log.Error("Could not generate starting map because there is no any player faction base.");
				}
				this.tickManager.gameStartAbsTick = GenTicks.ConfiguredTicksAbsAtGameStart;
				Map currentMap = MapGenerator.GenerateMap(intVec, settlement, settlement.MapGeneratorDef, settlement.ExtraGenStepDefs, null);
				this.worldInt.info.initialMapSize = intVec;
				if (this.initData.permadeath)
				{
					this.info.permadeathMode = true;
					this.info.permadeathModeUniqueName = PermadeathModeUtility.GeneratePermadeathSaveName();
				}
				PawnUtility.GiveAllStartingPlayerPawnsThought(ThoughtDefOf.NewColonyOptimism);
				this.FinalizeInit();
				Current.Game.CurrentMap = currentMap;
				Find.CameraDriver.JumpToCurrentMapLoc(MapGenerator.PlayerStartSpot);
				Find.CameraDriver.ResetSize();
				if (Prefs.PauseOnLoad && this.initData.startedFromEntry)
				{
					LongEventHandler.ExecuteWhenFinished(delegate
					{
						this.tickManager.DoSingleTick();
						this.tickManager.CurTimeSpeed = TimeSpeed.Paused;
					});
				}
				Find.Scenario.PostGameStart();
				this.history.FinalizeInit();
				ResearchUtility.ApplyPlayerStartingResearch();
				GameComponentUtility.StartedNewGame();
				this.initData = null;
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x00026EA0 File Offset: 0x000250A0
		public void LoadGame()
		{
			if (this.maps.Any<Map>())
			{
				Log.Error("Called LoadGame() but there already is a map. There should be 0 maps...");
				return;
			}
			MemoryUtility.UnloadUnusedUnityAssets();
			BackCompatibility.PreLoadSavegame(ScribeMetaHeaderUtility.loadedGameVersion);
			Current.ProgramState = ProgramState.MapInitializing;
			this.ExposeSmallComponents();
			LongEventHandler.SetCurrentEventText("LoadingWorld".Translate());
			if (Scribe.EnterNode("world"))
			{
				try
				{
					this.World = new World();
					this.World.ExposeData();
					goto IL_7C;
				}
				finally
				{
					Scribe.ExitNode();
				}
				goto IL_71;
				IL_7C:
				this.World.FinalizeInit();
				LongEventHandler.SetCurrentEventText("LoadingMap".Translate());
				Scribe_Collections.Look<Map>(ref this.maps, "maps", LookMode.Deep, Array.Empty<object>());
				if (this.maps.RemoveAll((Map x) => x == null) != 0)
				{
					Log.Warning("Some maps were null after loading.");
				}
				int num = -1;
				Scribe_Values.Look<int>(ref num, "currentMapIndex", -1, false);
				if (num < 0 && this.maps.Any<Map>())
				{
					Log.Error("Current map is null after loading but there are maps available. Setting current map to [0].");
					num = 0;
				}
				if (num >= this.maps.Count)
				{
					Log.Error("Current map index out of bounds after loading.");
					if (this.maps.Any<Map>())
					{
						num = 0;
					}
					else
					{
						num = -1;
					}
				}
				this.currentMapIndex = sbyte.MinValue;
				this.CurrentMap = ((num >= 0) ? this.maps[num] : null);
				LongEventHandler.SetCurrentEventText("InitializingGame".Translate());
				Find.CameraDriver.Expose();
				DeepProfiler.Start("FinalizeLoading");
				Scribe.loader.FinalizeLoading();
				DeepProfiler.End();
				LongEventHandler.SetCurrentEventText("SpawningAllThings".Translate());
				for (int i = 0; i < this.maps.Count; i++)
				{
					try
					{
						this.maps[i].FinalizeLoading();
					}
					catch (Exception arg)
					{
						Log.Error("Error in Map.FinalizeLoading(): " + arg);
					}
					try
					{
						this.maps[i].Parent.FinalizeLoading();
					}
					catch (Exception arg2)
					{
						Log.Error("Error in MapParent.FinalizeLoading(): " + arg2);
					}
				}
				this.FinalizeInit();
				if (Prefs.PauseOnLoad)
				{
					LongEventHandler.ExecuteWhenFinished(delegate
					{
						Find.TickManager.DoSingleTick();
						Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
					});
				}
				GameComponentUtility.LoadedGame();
				BackCompatibility.PostLoadSavegame(ScribeMetaHeaderUtility.loadedGameVersion);
				return;
			}
			IL_71:
			Log.Error("Could not find world XML node.");
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x0002712C File Offset: 0x0002532C
		public void UpdateEntry()
		{
			GameComponentUtility.GameComponentUpdate();
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x00027134 File Offset: 0x00025334
		public void UpdatePlay()
		{
			this.tickManager.TickManagerUpdate();
			this.letterStack.LetterStackUpdate();
			this.World.WorldUpdate();
			for (int i = 0; i < this.maps.Count; i++)
			{
				this.maps[i].MapUpdate();
			}
			this.Info.GameInfoUpdate();
			GameComponentUtility.GameComponentUpdate();
			this.signalManager.SignalManagerUpdate();
			GlobalTextureAtlasManager.GlobalTextureAtlasManagerUpdate();
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x000271AC File Offset: 0x000253AC
		public T GetComponent<T>() where T : GameComponent
		{
			for (int i = 0; i < this.components.Count; i++)
			{
				T t = this.components[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x000271FC File Offset: 0x000253FC
		public GameComponent GetComponent(Type type)
		{
			for (int i = 0; i < this.components.Count; i++)
			{
				if (type.IsAssignableFrom(this.components[i].GetType()))
				{
					return this.components[i];
				}
			}
			return null;
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x00027246 File Offset: 0x00025446
		public void FinalizeInit()
		{
			LogSimple.FlushToFileAndOpen();
			this.researchManager.ReapplyAllMods();
			MessagesRepeatAvoider.Reset();
			GameComponentUtility.FinalizeInit();
			Current.ProgramState = ProgramState.Playing;
			Current.Game.World.ideoManager.Notify_GameStarted();
			RecipeDefGenerator.ResetRecipeIngredientsForDifficulty();
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x00027284 File Offset: 0x00025484
		public void DeinitAndRemoveMap(Map map)
		{
			if (map == null)
			{
				Log.Error("Tried to remove null map.");
				return;
			}
			if (!this.maps.Contains(map))
			{
				Log.Error("Tried to remove map " + map + " but it's not here.");
				return;
			}
			if (map.Parent != null)
			{
				map.Parent.Notify_MyMapAboutToBeRemoved();
			}
			Map currentMap = this.CurrentMap;
			MapDeiniter.Deinit(map);
			this.maps.Remove(map);
			if (currentMap != null)
			{
				sbyte b = (sbyte)this.maps.IndexOf(currentMap);
				if (b < 0)
				{
					if (this.maps.Any<Map>())
					{
						this.CurrentMap = this.maps[0];
					}
					else
					{
						this.CurrentMap = null;
					}
					Find.World.renderer.wantedMode = WorldRenderMode.Planet;
				}
				else
				{
					this.currentMapIndex = b;
				}
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
			MapComponentUtility.MapRemoved(map);
			if (map.Parent != null)
			{
				map.Parent.Notify_MyMapRemoved(map);
			}
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x00027374 File Offset: 0x00025574
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Game debug data:");
			stringBuilder.AppendLine("initData:");
			if (this.initData == null)
			{
				stringBuilder.AppendLine("   null");
			}
			else
			{
				stringBuilder.AppendLine(this.initData.ToString());
			}
			stringBuilder.AppendLine("Scenario:");
			if (this.scenarioInt == null)
			{
				stringBuilder.AppendLine("   null");
			}
			else
			{
				stringBuilder.AppendLine("   " + this.scenarioInt.ToString());
			}
			stringBuilder.AppendLine("World:");
			if (this.worldInt == null)
			{
				stringBuilder.AppendLine("   null");
			}
			else
			{
				stringBuilder.AppendLine("   name: " + this.worldInt.info.name);
			}
			stringBuilder.AppendLine("Maps count: " + this.maps.Count);
			for (int i = 0; i < this.maps.Count; i++)
			{
				stringBuilder.AppendLine("   Map " + this.maps[i].Index + ":");
				stringBuilder.AppendLine("      tile: " + this.maps[i].TileInfo);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040007C3 RID: 1987
		private GameInitData initData;

		// Token: 0x040007C4 RID: 1988
		public sbyte currentMapIndex = -1;

		// Token: 0x040007C5 RID: 1989
		private GameInfo info = new GameInfo();

		// Token: 0x040007C6 RID: 1990
		public List<GameComponent> components = new List<GameComponent>();

		// Token: 0x040007C7 RID: 1991
		private GameRules rules = new GameRules();

		// Token: 0x040007C8 RID: 1992
		private Scenario scenarioInt;

		// Token: 0x040007C9 RID: 1993
		private World worldInt;

		// Token: 0x040007CA RID: 1994
		private List<Map> maps = new List<Map>();

		// Token: 0x040007CB RID: 1995
		public PlaySettings playSettings = new PlaySettings();

		// Token: 0x040007CC RID: 1996
		public StoryWatcher storyWatcher = new StoryWatcher();

		// Token: 0x040007CD RID: 1997
		public LetterStack letterStack = new LetterStack();

		// Token: 0x040007CE RID: 1998
		public ResearchManager researchManager = new ResearchManager();

		// Token: 0x040007CF RID: 1999
		public GameEnder gameEnder = new GameEnder();

		// Token: 0x040007D0 RID: 2000
		public Storyteller storyteller = new Storyteller();

		// Token: 0x040007D1 RID: 2001
		public History history = new History();

		// Token: 0x040007D2 RID: 2002
		public TaleManager taleManager = new TaleManager();

		// Token: 0x040007D3 RID: 2003
		public PlayLog playLog = new PlayLog();

		// Token: 0x040007D4 RID: 2004
		public BattleLog battleLog = new BattleLog();

		// Token: 0x040007D5 RID: 2005
		public OutfitDatabase outfitDatabase = new OutfitDatabase();

		// Token: 0x040007D6 RID: 2006
		public DrugPolicyDatabase drugPolicyDatabase = new DrugPolicyDatabase();

		// Token: 0x040007D7 RID: 2007
		public FoodRestrictionDatabase foodRestrictionDatabase = new FoodRestrictionDatabase();

		// Token: 0x040007D8 RID: 2008
		public TickManager tickManager = new TickManager();

		// Token: 0x040007D9 RID: 2009
		public Tutor tutor = new Tutor();

		// Token: 0x040007DA RID: 2010
		public Autosaver autosaver = new Autosaver();

		// Token: 0x040007DB RID: 2011
		public DateNotifier dateNotifier = new DateNotifier();

		// Token: 0x040007DC RID: 2012
		public SignalManager signalManager = new SignalManager();

		// Token: 0x040007DD RID: 2013
		public UniqueIDsManager uniqueIDsManager = new UniqueIDsManager();

		// Token: 0x040007DE RID: 2014
		public QuestManager questManager = new QuestManager();

		// Token: 0x040007DF RID: 2015
		public TransportShipManager transportShipManager = new TransportShipManager();

		// Token: 0x040007E0 RID: 2016
		private static List<Map> tmpPlayerHomeMaps = new List<Map>();
	}
}
