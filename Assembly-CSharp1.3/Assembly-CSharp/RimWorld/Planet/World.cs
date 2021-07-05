using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet
{
	// Token: 0x0200177A RID: 6010
	public sealed class World : IThingHolder, IExposable, IIncidentTarget, ILoadReferenceable
	{
		// Token: 0x1700169A RID: 5786
		// (get) Token: 0x06008A9A RID: 35482 RVA: 0x0031BE89 File Offset: 0x0031A089
		public float PlanetCoverage
		{
			get
			{
				return this.info.planetCoverage;
			}
		}

		// Token: 0x1700169B RID: 5787
		// (get) Token: 0x06008A9B RID: 35483 RVA: 0x00002688 File Offset: 0x00000888
		public IThingHolder ParentHolder
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700169C RID: 5788
		// (get) Token: 0x06008A9C RID: 35484 RVA: 0x000B955A File Offset: 0x000B775A
		public int Tile
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x1700169D RID: 5789
		// (get) Token: 0x06008A9D RID: 35485 RVA: 0x0031BE96 File Offset: 0x0031A096
		public StoryState StoryState
		{
			get
			{
				return this.storyState;
			}
		}

		// Token: 0x1700169E RID: 5790
		// (get) Token: 0x06008A9E RID: 35486 RVA: 0x0031BE9E File Offset: 0x0031A09E
		public GameConditionManager GameConditionManager
		{
			get
			{
				return this.gameConditionManager;
			}
		}

		// Token: 0x1700169F RID: 5791
		// (get) Token: 0x06008A9F RID: 35487 RVA: 0x0031BEA8 File Offset: 0x0031A0A8
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

		// Token: 0x170016A0 RID: 5792
		// (get) Token: 0x06008AA0 RID: 35488 RVA: 0x0031BF12 File Offset: 0x0031A112
		public IEnumerable<Pawn> PlayerPawnsForStoryteller
		{
			get
			{
				return PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction;
			}
		}

		// Token: 0x170016A1 RID: 5793
		// (get) Token: 0x06008AA1 RID: 35489 RVA: 0x0004566A File Offset: 0x0004386A
		public FloatRange IncidentPointsRandomFactorRange
		{
			get
			{
				return FloatRange.One;
			}
		}

		// Token: 0x06008AA2 RID: 35490 RVA: 0x0031BF19 File Offset: 0x0031A119
		public IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
		{
			yield return IncidentTargetTagDefOf.World;
			yield break;
		}

		// Token: 0x06008AA3 RID: 35491 RVA: 0x0031BF24 File Offset: 0x0031A124
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

		// Token: 0x06008AA4 RID: 35492 RVA: 0x0031BFA0 File Offset: 0x0031A1A0
		public void ExposeComponents()
		{
			Scribe_Deep.Look<FactionManager>(ref this.factionManager, "factionManager", Array.Empty<object>());
			Scribe_Deep.Look<IdeoManager>(ref this.ideoManager, "ideoManager", Array.Empty<object>());
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

		// Token: 0x06008AA5 RID: 35493 RVA: 0x0031C074 File Offset: 0x0031A274
		public void ConstructComponents()
		{
			this.worldObjects = new WorldObjectsHolder();
			this.factionManager = new FactionManager();
			this.ideoManager = new IdeoManager();
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

		// Token: 0x06008AA6 RID: 35494 RVA: 0x0031C13C File Offset: 0x0031A33C
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
						}));
					}
				}
			}
			this.tileTemperatures = this.GetComponent<TileTemperaturesComp>();
			this.genData = this.GetComponent<WorldGenData>();
		}

		// Token: 0x06008AA7 RID: 35495 RVA: 0x0031C238 File Offset: 0x0031A438
		public void FinalizeInit()
		{
			this.pathGrid.RecalculateAllPerceivedPathCosts();
			AmbientSoundManager.EnsureWorldAmbientSoundCreated();
			WorldComponentUtility.FinalizeInit(this);
		}

		// Token: 0x06008AA8 RID: 35496 RVA: 0x0031C250 File Offset: 0x0031A450
		public void WorldTick()
		{
			this.worldPawns.WorldPawnsTick();
			this.factionManager.FactionManagerTick();
			this.worldObjects.WorldObjectsHolderTick();
			this.debugDrawer.WorldDebugDrawerTick();
			this.pathGrid.WorldPathGridTick();
			WorldComponentUtility.WorldComponentTick(this);
			this.ideoManager.IdeoManagerTick();
		}

		// Token: 0x06008AA9 RID: 35497 RVA: 0x0031C2A8 File Offset: 0x0031A4A8
		public void WorldPostTick()
		{
			try
			{
				this.gameConditionManager.GameConditionManagerTick();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
			}
		}

		// Token: 0x06008AAA RID: 35498 RVA: 0x0031C2E0 File Offset: 0x0031A4E0
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

		// Token: 0x06008AAB RID: 35499 RVA: 0x0031C330 File Offset: 0x0031A530
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

		// Token: 0x06008AAC RID: 35500 RVA: 0x0031C380 File Offset: 0x0031A580
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

		// Token: 0x06008AAD RID: 35501 RVA: 0x0031C3CC File Offset: 0x0031A5CC
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

		// Token: 0x06008AAE RID: 35502 RVA: 0x0031C4BC File Offset: 0x0031A6BC
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

		// Token: 0x06008AAF RID: 35503 RVA: 0x0031C518 File Offset: 0x0031A718
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

		// Token: 0x06008AB0 RID: 35504 RVA: 0x0031C5DE File Offset: 0x0031A7DE
		public bool Impassable(int tileID)
		{
			return !this.pathGrid.Passable(tileID);
		}

		// Token: 0x06008AB1 RID: 35505 RVA: 0x00002688 File Offset: 0x00000888
		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		// Token: 0x06008AB2 RID: 35506 RVA: 0x0031C5F0 File Offset: 0x0031A7F0
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

		// Token: 0x06008AB3 RID: 35507 RVA: 0x0031C6B4 File Offset: 0x0031A8B4
		public string GetUniqueLoadID()
		{
			return "World";
		}

		// Token: 0x170016A2 RID: 5794
		// (get) Token: 0x06008AB4 RID: 35508 RVA: 0x0031C6BB File Offset: 0x0031A8BB
		public int ConstantRandSeed
		{
			get
			{
				return this.info.persistentRandomValue;
			}
		}

		// Token: 0x06008AB5 RID: 35509 RVA: 0x0031C6C8 File Offset: 0x0031A8C8
		public override string ToString()
		{
			return "World-" + this.info.name;
		}

		// Token: 0x04005830 RID: 22576
		public WorldInfo info = new WorldInfo();

		// Token: 0x04005831 RID: 22577
		public List<WorldComponent> components = new List<WorldComponent>();

		// Token: 0x04005832 RID: 22578
		public FactionManager factionManager;

		// Token: 0x04005833 RID: 22579
		public IdeoManager ideoManager;

		// Token: 0x04005834 RID: 22580
		public WorldPawns worldPawns;

		// Token: 0x04005835 RID: 22581
		public WorldObjectsHolder worldObjects;

		// Token: 0x04005836 RID: 22582
		public GameConditionManager gameConditionManager;

		// Token: 0x04005837 RID: 22583
		public StoryState storyState;

		// Token: 0x04005838 RID: 22584
		public WorldFeatures features;

		// Token: 0x04005839 RID: 22585
		public WorldGrid grid;

		// Token: 0x0400583A RID: 22586
		public WorldPathGrid pathGrid;

		// Token: 0x0400583B RID: 22587
		public WorldRenderer renderer;

		// Token: 0x0400583C RID: 22588
		public WorldInterface UI;

		// Token: 0x0400583D RID: 22589
		public WorldDebugDrawer debugDrawer;

		// Token: 0x0400583E RID: 22590
		public WorldDynamicDrawManager dynamicDrawManager;

		// Token: 0x0400583F RID: 22591
		public WorldPathFinder pathFinder;

		// Token: 0x04005840 RID: 22592
		public WorldPathPool pathPool;

		// Token: 0x04005841 RID: 22593
		public WorldReachability reachability;

		// Token: 0x04005842 RID: 22594
		public WorldFloodFiller floodFiller;

		// Token: 0x04005843 RID: 22595
		public ConfiguredTicksAbsAtGameStartCache ticksAbsCache;

		// Token: 0x04005844 RID: 22596
		public TileTemperaturesComp tileTemperatures;

		// Token: 0x04005845 RID: 22597
		public WorldGenData genData;

		// Token: 0x04005846 RID: 22598
		private List<ThingDef> allNaturalRockDefs;

		// Token: 0x04005847 RID: 22599
		private static List<ThingDef> tmpNaturalRockDefs = new List<ThingDef>();

		// Token: 0x04005848 RID: 22600
		private static List<int> tmpNeighbors = new List<int>();

		// Token: 0x04005849 RID: 22601
		private static List<Rot4> tmpOceanDirs = new List<Rot4>();
	}
}
