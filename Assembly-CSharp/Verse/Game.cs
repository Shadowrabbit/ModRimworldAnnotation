using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using Verse.Profile;

namespace Verse
{
	// Token: 0x020001CE RID: 462
	public class Game : IExposable
	{
		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000BE2 RID: 3042 RVA: 0x0000F373 File Offset: 0x0000D573
		// (set) Token: 0x06000BE3 RID: 3043 RVA: 0x0000F37B File Offset: 0x0000D57B
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

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000BE4 RID: 3044 RVA: 0x0000F384 File Offset: 0x0000D584
		// (set) Token: 0x06000BE5 RID: 3045 RVA: 0x0000F38C File Offset: 0x0000D58C
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

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000BE6 RID: 3046 RVA: 0x0000F39F File Offset: 0x0000D59F
		// (set) Token: 0x06000BE7 RID: 3047 RVA: 0x000A22C8 File Offset: 0x000A04C8
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
						Log.Error("Could not set current map because it does not exist.", false);
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

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000BE8 RID: 3048 RVA: 0x000A231C File Offset: 0x000A051C
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

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000BE9 RID: 3049 RVA: 0x000A2360 File Offset: 0x000A0560
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

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000BEA RID: 3050 RVA: 0x0000F3BD File Offset: 0x0000D5BD
		public List<Map> Maps
		{
			get
			{
				return this.maps;
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000BEB RID: 3051 RVA: 0x0000F3C5 File Offset: 0x0000D5C5
		// (set) Token: 0x06000BEC RID: 3052 RVA: 0x0000F3CD File Offset: 0x0000D5CD
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

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000BED RID: 3053 RVA: 0x0000F3D6 File Offset: 0x0000D5D6
		public GameInfo Info
		{
			get
			{
				return this.info;
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000BEE RID: 3054 RVA: 0x0000F3DE File Offset: 0x0000D5DE
		public GameRules Rules
		{
			get
			{
				return this.rules;
			}
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x000A23D8 File Offset: 0x000A05D8
		public Game()
		{
			this.FillComponents();
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x000A2500 File Offset: 0x000A0700
		public void AddMap(Map map)
		{
			if (map == null)
			{
				Log.Error("Tried to add null map.", false);
				return;
			}
			if (this.maps.Contains(map))
			{
				Log.Error("Tried to add map but it's already here.", false);
				return;
			}
			if (this.maps.Count > 127)
			{
				Log.Error("Can't add map. Reached maps count limit (" + sbyte.MaxValue + ").", false);
				return;
			}
			this.maps.Add(map);
			Find.ColonistBar.MarkColonistsDirty();
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x000A2578 File Offset: 0x000A0778
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

		// Token: 0x06000BF2 RID: 3058 RVA: 0x000A25C4 File Offset: 0x000A07C4
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

		// Token: 0x06000BF3 RID: 3059 RVA: 0x000A260C File Offset: 0x000A080C
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				Log.Error("You must use special LoadData method to load Game.", false);
				return;
			}
			Scribe_Values.Look<sbyte>(ref this.currentMapIndex, "currentMapIndex", -1, false);
			this.ExposeSmallComponents();
			Scribe_Deep.Look<World>(ref this.worldInt, "world", Array.Empty<object>());
			Scribe_Collections.Look<Map>(ref this.maps, "maps", LookMode.Deep, Array.Empty<object>());
			Find.CameraDriver.Expose();
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x000A267C File Offset: 0x000A087C
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
			Scribe_Collections.Look<GameComponent>(ref this.components, "components", LookMode.Deep, new object[]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.FillComponents();
				if (this.rules == null)
				{
					Log.Warning("Save game was missing rules. Replacing with a blank GameRules.", false);
					this.rules = new GameRules();
				}
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x000A2890 File Offset: 0x000A0A90
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
						}), false);
					}
				}
			}
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x000A2970 File Offset: 0x000A0B70
		public void InitNewGame()
		{
			string str = (from mod in LoadedModManager.RunningMods
			select mod.PackageIdPlayerFacing).ToLineList("  - ", false);
			Log.Message("Initializing new game with mods:\n" + str, false);
			if (this.maps.Any<Map>())
			{
				Log.Error("Called InitNewGame() but there already is a map. There should be 0 maps...", false);
				return;
			}
			if (this.initData == null)
			{
				Log.Error("Called InitNewGame() but init data is null. Create it first.", false);
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
					Log.Error("Could not generate starting map because there is no any player faction base.", false);
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
				if (Faction.OfPlayer.def.startingResearchTags != null)
				{
					foreach (ResearchProjectTagDef tag in Faction.OfPlayer.def.startingResearchTags)
					{
						foreach (ResearchProjectDef researchProjectDef in DefDatabase<ResearchProjectDef>.AllDefs)
						{
							if (researchProjectDef.HasTag(tag))
							{
								this.researchManager.FinishProject(researchProjectDef, false, null);
							}
						}
					}
				}
				if (Faction.OfPlayer.def.startingTechprintsResearchTags != null)
				{
					foreach (ResearchProjectTagDef tag2 in Faction.OfPlayer.def.startingTechprintsResearchTags)
					{
						foreach (ResearchProjectDef researchProjectDef2 in DefDatabase<ResearchProjectDef>.AllDefs)
						{
							if (researchProjectDef2.HasTag(tag2))
							{
								int techprints = this.researchManager.GetTechprints(researchProjectDef2);
								if (techprints < researchProjectDef2.TechprintCount)
								{
									this.researchManager.AddTechprints(researchProjectDef2, researchProjectDef2.TechprintCount - techprints);
								}
							}
						}
					}
				}
				GameComponentUtility.StartedNewGame();
				this.initData = null;
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x000A2D20 File Offset: 0x000A0F20
		public void LoadGame()
		{
			if (this.maps.Any<Map>())
			{
				Log.Error("Called LoadGame() but there already is a map. There should be 0 maps...", false);
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
					goto IL_7E;
				}
				finally
				{
					Scribe.ExitNode();
				}
				goto IL_72;
				IL_7E:
				this.World.FinalizeInit();
				LongEventHandler.SetCurrentEventText("LoadingMap".Translate());
				Scribe_Collections.Look<Map>(ref this.maps, "maps", LookMode.Deep, Array.Empty<object>());
				if (this.maps.RemoveAll((Map x) => x == null) != 0)
				{
					Log.Warning("Some maps were null after loading.", false);
				}
				int num = -1;
				Scribe_Values.Look<int>(ref num, "currentMapIndex", -1, false);
				if (num < 0 && this.maps.Any<Map>())
				{
					Log.Error("Current map is null after loading but there are maps available. Setting current map to [0].", false);
					num = 0;
				}
				if (num >= this.maps.Count)
				{
					Log.Error("Current map index out of bounds after loading.", false);
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
						Log.Error("Error in Map.FinalizeLoading(): " + arg, false);
					}
					try
					{
						this.maps[i].Parent.FinalizeLoading();
					}
					catch (Exception arg2)
					{
						Log.Error("Error in MapParent.FinalizeLoading(): " + arg2, false);
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
			IL_72:
			Log.Error("Could not find world XML node.", false);
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x0000F3E6 File Offset: 0x0000D5E6
		public void UpdateEntry()
		{
			GameComponentUtility.GameComponentUpdate();
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x000A2FB0 File Offset: 0x000A11B0
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
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x000A3020 File Offset: 0x000A1220
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

		// Token: 0x06000BFB RID: 3067 RVA: 0x000A3070 File Offset: 0x000A1270
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

		// Token: 0x06000BFC RID: 3068 RVA: 0x0000F3ED File Offset: 0x0000D5ED
		public void FinalizeInit()
		{
			LogSimple.FlushToFileAndOpen();
			this.researchManager.ReapplyAllMods();
			MessagesRepeatAvoider.Reset();
			GameComponentUtility.FinalizeInit();
			Current.ProgramState = ProgramState.Playing;
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x000A30BC File Offset: 0x000A12BC
		public void DeinitAndRemoveMap(Map map)
		{
			if (map == null)
			{
				Log.Error("Tried to remove null map.", false);
				return;
			}
			if (!this.maps.Contains(map))
			{
				Log.Error("Tried to remove map " + map + " but it's not here.", false);
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

		// Token: 0x06000BFE RID: 3070 RVA: 0x000A31AC File Offset: 0x000A13AC
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

		// Token: 0x04000A67 RID: 2663
		private GameInitData initData;

		// Token: 0x04000A68 RID: 2664
		public sbyte currentMapIndex = -1;

		// Token: 0x04000A69 RID: 2665
		private GameInfo info = new GameInfo();

		// Token: 0x04000A6A RID: 2666
		public List<GameComponent> components = new List<GameComponent>();

		// Token: 0x04000A6B RID: 2667
		private GameRules rules = new GameRules();

		// Token: 0x04000A6C RID: 2668
		private Scenario scenarioInt;

		// Token: 0x04000A6D RID: 2669
		private World worldInt;

		// Token: 0x04000A6E RID: 2670
		private List<Map> maps = new List<Map>();

		// Token: 0x04000A6F RID: 2671
		public PlaySettings playSettings = new PlaySettings();

		// Token: 0x04000A70 RID: 2672
		public StoryWatcher storyWatcher = new StoryWatcher();

		// Token: 0x04000A71 RID: 2673
		public LetterStack letterStack = new LetterStack();

		// Token: 0x04000A72 RID: 2674
		public ResearchManager researchManager = new ResearchManager();

		// Token: 0x04000A73 RID: 2675
		public GameEnder gameEnder = new GameEnder();

		// Token: 0x04000A74 RID: 2676
		public Storyteller storyteller = new Storyteller();

		// Token: 0x04000A75 RID: 2677
		public History history = new History();

		// Token: 0x04000A76 RID: 2678
		public TaleManager taleManager = new TaleManager();

		// Token: 0x04000A77 RID: 2679
		public PlayLog playLog = new PlayLog();

		// Token: 0x04000A78 RID: 2680
		public BattleLog battleLog = new BattleLog();

		// Token: 0x04000A79 RID: 2681
		public OutfitDatabase outfitDatabase = new OutfitDatabase();

		// Token: 0x04000A7A RID: 2682
		public DrugPolicyDatabase drugPolicyDatabase = new DrugPolicyDatabase();

		// Token: 0x04000A7B RID: 2683
		public FoodRestrictionDatabase foodRestrictionDatabase = new FoodRestrictionDatabase();

		// Token: 0x04000A7C RID: 2684
		public TickManager tickManager = new TickManager();

		// Token: 0x04000A7D RID: 2685
		public Tutor tutor = new Tutor();

		// Token: 0x04000A7E RID: 2686
		public Autosaver autosaver = new Autosaver();

		// Token: 0x04000A7F RID: 2687
		public DateNotifier dateNotifier = new DateNotifier();

		// Token: 0x04000A80 RID: 2688
		public SignalManager signalManager = new SignalManager();

		// Token: 0x04000A81 RID: 2689
		public UniqueIDsManager uniqueIDsManager = new UniqueIDsManager();

		// Token: 0x04000A82 RID: 2690
		public QuestManager questManager = new QuestManager();

		// Token: 0x04000A83 RID: 2691
		private static List<Map> tmpPlayerHomeMaps = new List<Map>();
	}
}
