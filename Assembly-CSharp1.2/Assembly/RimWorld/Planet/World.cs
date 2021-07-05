using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet
{
	// Token: 0x02002085 RID: 8325
	public sealed class World : IThingHolder, IExposable, IIncidentTarget, ILoadReferenceable
	{
		// Token: 0x17001A16 RID: 6678
		// (get) Token: 0x0600B073 RID: 45171 RVA: 0x00072BF0 File Offset: 0x00070DF0
		public float PlanetCoverage
		{
			get
			{
				return this.info.planetCoverage;
			}
		}

		// Token: 0x17001A17 RID: 6679
		// (get) Token: 0x0600B074 RID: 45172 RVA: 0x0000C32E File Offset: 0x0000A52E
		public IThingHolder ParentHolder
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001A18 RID: 6680
		// (get) Token: 0x0600B075 RID: 45173 RVA: 0x000236C9 File Offset: 0x000218C9
		public int Tile
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x17001A19 RID: 6681
		// (get) Token: 0x0600B076 RID: 45174 RVA: 0x00072BFD File Offset: 0x00070DFD
		public StoryState StoryState
		{
			get
			{
				return this.storyState;
			}
		}

		// Token: 0x17001A1A RID: 6682
		// (get) Token: 0x0600B077 RID: 45175 RVA: 0x00072C05 File Offset: 0x00070E05
		public GameConditionManager GameConditionManager
		{
			get
			{
				return this.gameConditionManager;
			}
		}

		// Token: 0x17001A1B RID: 6683
		// (get) Token: 0x0600B078 RID: 45176 RVA: 0x003338C0 File Offset: 0x00331AC0
		public float PlayerWealthForStoryteller
		{
			get
			{
				float num = 0f;
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					num += maps[i].PlayerWealthForStoryteller;
				}
				List<Caravan> caravans = Find.WorldObjects.Caravans;
				for (int j = 0; j < caravans.Count; j++)
				{
					num += caravans[j].PlayerWealthForStoryteller;
				}
				return num;
			}
		}

		// Token: 0x17001A1C RID: 6684
		// (get) Token: 0x0600B079 RID: 45177 RVA: 0x00072C0D File Offset: 0x00070E0D
		public IEnumerable<Pawn> PlayerPawnsForStoryteller
		{
			get
			{
				return PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction;
			}
		}

		// Token: 0x17001A1D RID: 6685
		// (get) Token: 0x0600B07A RID: 45178 RVA: 0x00012594 File Offset: 0x00010794
		public FloatRange IncidentPointsRandomFactorRange
		{
			get
			{
				return FloatRange.One;
			}
		}

		// Token: 0x0600B07B RID: 45179 RVA: 0x00072C14 File Offset: 0x00070E14
		public IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
		{
			yield return IncidentTargetTagDefOf.World;
			yield break;
		}

		// Token: 0x0600B07C RID: 45180 RVA: 0x0033392C File Offset: 0x00331B2C
		public void ExposeData()
		{
			Scribe_Deep.Look<WorldInfo>(ref this.info, "info", Array.Empty<object>());
			Scribe_Deep.Look<WorldGrid>(ref this.grid, "grid", Array.Empty<object>());
			if (Scribe.mode != LoadSaveMode.LoadingVars)
			{
				this.ExposeComponents();
				return;
			}
			if (this.grid == null || !this.grid.HasWorldData)
			{
				WorldGenerator.GenerateWithoutWorldData(this.info.seedString);
				return;
			}
			WorldGenerator.GenerateFromScribe(this.info.seedString);
		}

		// Token: 0x0600B07D RID: 45181 RVA: 0x003339A8 File Offset: 0x00331BA8
		public void ExposeComponents()
		{
			Scribe_Deep.Look<FactionManager>(ref this.factionManager, "factionManager", Array.Empty<object>());
			Scribe_Deep.Look<WorldPawns>(ref this.worldPawns, "worldPawns", Array.Empty<object>());
			Scribe_Deep.Look<WorldObjectsHolder>(ref this.worldObjects, "worldObjects", Array.Empty<object>());
			Scribe_Deep.Look<GameConditionManager>(ref this.gameConditionManager, "gameConditionManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<StoryState>(ref this.storyState, "storyState", new object[]
			{
				this
			});
			Scribe_Deep.Look<WorldFeatures>(ref this.features, "features", Array.Empty<object>());
			Scribe_Collections.Look<WorldComponent>(ref this.components, "components", LookMode.Deep, new object[]
			{
				this
			});
			this.FillComponents();
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0600B07E RID: 45182 RVA: 0x00333A64 File Offset: 0x00331C64
		public void ConstructComponents()
		{
			this.worldObjects = new WorldObjectsHolder();
			this.factionManager = new FactionManager();
			this.worldPawns = new WorldPawns();
			this.gameConditionManager = new GameConditionManager(this);
			this.storyState = new StoryState(this);
			this.renderer = new WorldRenderer();
			this.UI = new WorldInterface();
			this.debugDrawer = new WorldDebugDrawer();
			this.dynamicDrawManager = new WorldDynamicDrawManager();
			this.pathFinder = new WorldPathFinder();
			this.pathPool = new WorldPathPool();
			this.reachability = new WorldReachability();
			this.floodFiller = new WorldFloodFiller();
			this.ticksAbsCache = new ConfiguredTicksAbsAtGameStartCache();
			this.components.Clear();
			this.FillComponents();
		}

		// Token: 0x0600B07F RID: 45183 RVA: 0x00333B20 File Offset: 0x00331D20
		private void FillComponents()
		{
			this.components.RemoveAll((WorldComponent component) => component == null);
			foreach (Type type in typeof(WorldComponent).AllSubclassesNonAbstract())
			{
				if (this.GetComponent(type) == null)
				{
					try
					{
						WorldComponent item = (WorldComponent)Activator.CreateInstance(type, new object[]
						{
							this
						});
						this.components.Add(item);
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not instantiate a WorldComponent of type ",
							type,
							": ",
							ex
						}), false);
					}
				}
			}
			this.tileTemperatures = this.GetComponent<TileTemperaturesComp>();
			this.genData = this.GetComponent<WorldGenData>();
		}

		// Token: 0x0600B080 RID: 45184 RVA: 0x00072C1D File Offset: 0x00070E1D
		public void FinalizeInit()
		{
			this.pathGrid.RecalculateAllPerceivedPathCosts();
			AmbientSoundManager.EnsureWorldAmbientSoundCreated();
			WorldComponentUtility.FinalizeInit(this);
		}

		// Token: 0x0600B081 RID: 45185 RVA: 0x00072C35 File Offset: 0x00070E35
		public void WorldTick()
		{
			this.worldPawns.WorldPawnsTick();
			this.factionManager.FactionManagerTick();
			this.worldObjects.WorldObjectsHolderTick();
			this.debugDrawer.WorldDebugDrawerTick();
			this.pathGrid.WorldPathGridTick();
			WorldComponentUtility.WorldComponentTick(this);
		}

		// Token: 0x0600B082 RID: 45186 RVA: 0x00333C18 File Offset: 0x00331E18
		public void WorldPostTick()
		{
			try
			{
				this.gameConditionManager.GameConditionManagerTick();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString(), false);
			}
		}

		// Token: 0x0600B083 RID: 45187 RVA: 0x00333C50 File Offset: 0x00331E50
		public void WorldUpdate()
		{
			bool worldRenderedNow = WorldRendererUtility.WorldRenderedNow;
			this.renderer.CheckActivateWorldCamera();
			if (worldRenderedNow)
			{
				ExpandableWorldObjectsUtility.ExpandableWorldObjectsUpdate();
				this.renderer.DrawWorldLayers();
				this.dynamicDrawManager.DrawDynamicWorldObjects();
				this.features.UpdateFeatures();
				NoiseDebugUI.RenderPlanetNoise();
			}
			WorldComponentUtility.WorldComponentUpdate(this);
		}

		// Token: 0x0600B084 RID: 45188 RVA: 0x00333CA0 File Offset: 0x00331EA0
		public T GetComponent<T>() where T : WorldComponent
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

		// Token: 0x0600B085 RID: 45189 RVA: 0x00333CF0 File Offset: 0x00331EF0
		public WorldComponent GetComponent(Type type)
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

		// Token: 0x0600B086 RID: 45190 RVA: 0x00333D3C File Offset: 0x00331F3C
		public Rot4 CoastDirectionAt(int tileID)
		{
			if (!this.grid[tileID].biome.canBuildBase)
			{
				return Rot4.Invalid;
			}
			World.tmpOceanDirs.Clear();
			this.grid.GetTileNeighbors(tileID, World.tmpNeighbors);
			int i = 0;
			int count = World.tmpNeighbors.Count;
			while (i < count)
			{
				if (this.grid[World.tmpNeighbors[i]].biome == BiomeDefOf.Ocean)
				{
					Rot4 rotFromTo = this.grid.GetRotFromTo(tileID, World.tmpNeighbors[i]);
					if (!World.tmpOceanDirs.Contains(rotFromTo))
					{
						World.tmpOceanDirs.Add(rotFromTo);
					}
				}
				i++;
			}
			if (World.tmpOceanDirs.Count == 0)
			{
				return Rot4.Invalid;
			}
			Rand.PushState();
			Rand.Seed = tileID;
			int index = Rand.Range(0, World.tmpOceanDirs.Count);
			Rand.PopState();
			return World.tmpOceanDirs[index];
		}

		// Token: 0x0600B087 RID: 45191 RVA: 0x00333E2C File Offset: 0x0033202C
		public bool HasCaves(int tile)
		{
			Tile tile2 = this.grid[tile];
			float chance;
			if (tile2.hilliness >= Hilliness.Mountainous)
			{
				chance = 0.5f;
			}
			else
			{
				if (tile2.hilliness < Hilliness.LargeHills)
				{
					return false;
				}
				chance = 0.25f;
			}
			return Rand.ChanceSeeded(chance, Gen.HashCombineInt(Find.World.info.Seed, tile));
		}

		// Token: 0x0600B088 RID: 45192 RVA: 0x00333E88 File Offset: 0x00332088
		public IEnumerable<ThingDef> NaturalRockTypesIn(int tile)
		{
			Rand.PushState();
			Rand.Seed = tile;
			if (this.allNaturalRockDefs == null)
			{
				this.allNaturalRockDefs = (from d in DefDatabase<ThingDef>.AllDefs
				where d.IsNonResourceNaturalRock
				select d).ToList<ThingDef>();
			}
			int num = Rand.RangeInclusive(2, 3);
			if (num > this.allNaturalRockDefs.Count)
			{
				num = this.allNaturalRockDefs.Count;
			}
			World.tmpNaturalRockDefs.Clear();
			World.tmpNaturalRockDefs.AddRange(this.allNaturalRockDefs);
			List<ThingDef> list = new List<ThingDef>();
			for (int i = 0; i < num; i++)
			{
				ThingDef item = World.tmpNaturalRockDefs.RandomElement<ThingDef>();
				World.tmpNaturalRockDefs.Remove(item);
				list.Add(item);
			}
			Rand.PopState();
			return list;
		}

		// Token: 0x0600B089 RID: 45193 RVA: 0x00072C74 File Offset: 0x00070E74
		public bool Impassable(int tileID)
		{
			return !this.pathGrid.Passable(tileID);
		}

		// Token: 0x0600B08A RID: 45194 RVA: 0x0000C32E File Offset: 0x0000A52E
		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		// Token: 0x0600B08B RID: 45195 RVA: 0x00333F50 File Offset: 0x00332150
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
			List<WorldObject> allWorldObjects = this.worldObjects.AllWorldObjects;
			for (int i = 0; i < allWorldObjects.Count; i++)
			{
				IThingHolder thingHolder = allWorldObjects[i] as IThingHolder;
				if (thingHolder != null)
				{
					outChildren.Add(thingHolder);
				}
				List<WorldObjectComp> allComps = allWorldObjects[i].AllComps;
				for (int j = 0; j < allComps.Count; j++)
				{
					IThingHolder thingHolder2 = allComps[j] as IThingHolder;
					if (thingHolder2 != null)
					{
						outChildren.Add(thingHolder2);
					}
				}
			}
			for (int k = 0; k < this.components.Count; k++)
			{
				IThingHolder thingHolder3 = this.components[k] as IThingHolder;
				if (thingHolder3 != null)
				{
					outChildren.Add(thingHolder3);
				}
			}
		}

		// Token: 0x0600B08C RID: 45196 RVA: 0x00072C85 File Offset: 0x00070E85
		public string GetUniqueLoadID()
		{
			return "World";
		}

		// Token: 0x17001A1E RID: 6686
		// (get) Token: 0x0600B08D RID: 45197 RVA: 0x00072C8C File Offset: 0x00070E8C
		public int ConstantRandSeed
		{
			get
			{
				return this.info.persistentRandomValue;
			}
		}

		// Token: 0x0600B08E RID: 45198 RVA: 0x00072C99 File Offset: 0x00070E99
		public override string ToString()
		{
			return "World-" + this.info.name;
		}

		// Token: 0x04007968 RID: 31080
		public WorldInfo info = new WorldInfo();

		// Token: 0x04007969 RID: 31081
		public List<WorldComponent> components = new List<WorldComponent>();

		// Token: 0x0400796A RID: 31082
		public FactionManager factionManager;

		// Token: 0x0400796B RID: 31083
		public WorldPawns worldPawns;

		// Token: 0x0400796C RID: 31084
		public WorldObjectsHolder worldObjects;

		// Token: 0x0400796D RID: 31085
		public GameConditionManager gameConditionManager;

		// Token: 0x0400796E RID: 31086
		public StoryState storyState;

		// Token: 0x0400796F RID: 31087
		public WorldFeatures features;

		// Token: 0x04007970 RID: 31088
		public WorldGrid grid;

		// Token: 0x04007971 RID: 31089
		public WorldPathGrid pathGrid;

		// Token: 0x04007972 RID: 31090
		public WorldRenderer renderer;

		// Token: 0x04007973 RID: 31091
		public WorldInterface UI;

		// Token: 0x04007974 RID: 31092
		public WorldDebugDrawer debugDrawer;

		// Token: 0x04007975 RID: 31093
		public WorldDynamicDrawManager dynamicDrawManager;

		// Token: 0x04007976 RID: 31094
		public WorldPathFinder pathFinder;

		// Token: 0x04007977 RID: 31095
		public WorldPathPool pathPool;

		// Token: 0x04007978 RID: 31096
		public WorldReachability reachability;

		// Token: 0x04007979 RID: 31097
		public WorldFloodFiller floodFiller;

		// Token: 0x0400797A RID: 31098
		public ConfiguredTicksAbsAtGameStartCache ticksAbsCache;

		// Token: 0x0400797B RID: 31099
		public TileTemperaturesComp tileTemperatures;

		// Token: 0x0400797C RID: 31100
		public WorldGenData genData;

		// Token: 0x0400797D RID: 31101
		private List<ThingDef> allNaturalRockDefs;

		// Token: 0x0400797E RID: 31102
		private static List<ThingDef> tmpNaturalRockDefs = new List<ThingDef>();

		// Token: 0x0400797F RID: 31103
		private static List<int> tmpNeighbors = new List<int>();

		// Token: 0x04007980 RID: 31104
		private static List<Rot4> tmpOceanDirs = new List<Rot4>();
	}
}
