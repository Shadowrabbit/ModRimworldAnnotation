using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using Verse.AI;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x02000280 RID: 640
	public sealed class Map : IIncidentTarget, ILoadReferenceable, IThingHolder, IExposable
	{
		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06001098 RID: 4248 RVA: 0x00012462 File Offset: 0x00010662
		public int Index
		{
			get
			{
				return Find.Maps.IndexOf(this);
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06001099 RID: 4249 RVA: 0x0001246F File Offset: 0x0001066F
		public IntVec3 Size
		{
			get
			{
				return this.info.Size;
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x0600109A RID: 4250 RVA: 0x0001247C File Offset: 0x0001067C
		public IntVec3 Center
		{
			get
			{
				return new IntVec3(this.Size.x / 2, 0, this.Size.z / 2);
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x0600109B RID: 4251 RVA: 0x0001249E File Offset: 0x0001069E
		public Faction ParentFaction
		{
			get
			{
				return this.info.parent.Faction;
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x0600109C RID: 4252 RVA: 0x000124B0 File Offset: 0x000106B0
		public int Area
		{
			get
			{
				return this.Size.x * this.Size.z;
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x0600109D RID: 4253 RVA: 0x000124C9 File Offset: 0x000106C9
		public IThingHolder ParentHolder
		{
			get
			{
				return this.info.parent;
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x0600109E RID: 4254 RVA: 0x000124D6 File Offset: 0x000106D6
		public IEnumerable<IntVec3> AllCells
		{
			get
			{
				int num;
				for (int z = 0; z < this.Size.z; z = num + 1)
				{
					for (int y = 0; y < this.Size.y; y = num + 1)
					{
						for (int x = 0; x < this.Size.x; x = num + 1)
						{
							yield return new IntVec3(x, y, z);
							num = x;
						}
						num = y;
					}
					num = z;
				}
				yield break;
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x0600109F RID: 4255 RVA: 0x000124E6 File Offset: 0x000106E6
		public bool IsPlayerHome
		{
			get
			{
				return this.info != null && this.info.parent.def.canBePlayerHome && this.info.parent.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x060010A0 RID: 4256 RVA: 0x00012520 File Offset: 0x00010720
		public bool IsTempIncidentMap
		{
			get
			{
				return this.info.parent.def.isTempIncidentMapOwner;
			}
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x00012537 File Offset: 0x00010737
		public IEnumerator<IntVec3> GetEnumerator()
		{
			foreach (IntVec3 intVec in this.AllCells)
			{
				yield return intVec;
			}
			IEnumerator<IntVec3> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x060010A2 RID: 4258 RVA: 0x00012546 File Offset: 0x00010746
		public int Tile
		{
			get
			{
				return this.info.Tile;
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x060010A3 RID: 4259 RVA: 0x00012553 File Offset: 0x00010753
		public Tile TileInfo
		{
			get
			{
				return Find.WorldGrid[this.Tile];
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x060010A4 RID: 4260 RVA: 0x00012565 File Offset: 0x00010765
		public BiomeDef Biome
		{
			get
			{
				return this.TileInfo.biome;
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x060010A5 RID: 4261 RVA: 0x00012572 File Offset: 0x00010772
		public StoryState StoryState
		{
			get
			{
				return this.storyState;
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x060010A6 RID: 4262 RVA: 0x0001257A File Offset: 0x0001077A
		public GameConditionManager GameConditionManager
		{
			get
			{
				return this.gameConditionManager;
			}
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x060010A7 RID: 4263 RVA: 0x000BA8A8 File Offset: 0x000B8AA8
		public float PlayerWealthForStoryteller
		{
			get
			{
				if (!this.IsPlayerHome)
				{
					float num = 0f;
					foreach (Pawn pawn in this.mapPawns.PawnsInFaction(Faction.OfPlayer))
					{
						if (pawn.IsFreeColonist)
						{
							num += WealthWatcher.GetEquipmentApparelAndInventoryWealth(pawn);
						}
						if (pawn.RaceProps.Animal)
						{
							num += pawn.MarketValue;
						}
					}
					return num;
				}
				if (Find.Storyteller.difficultyValues.fixedWealthMode)
				{
					return StorytellerUtility.FixedWealthModeMapWealthFromTimeCurve.Evaluate(this.AgeInDays * Find.Storyteller.difficultyValues.fixedWealthTimeFactor);
				}
				return this.wealthWatcher.WealthItems + this.wealthWatcher.WealthBuildings * 0.5f + this.wealthWatcher.WealthPawns;
			}
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x060010A8 RID: 4264 RVA: 0x00012582 File Offset: 0x00010782
		public IEnumerable<Pawn> PlayerPawnsForStoryteller
		{
			get
			{
				return this.mapPawns.PawnsInFaction(Faction.OfPlayer);
			}
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x060010A9 RID: 4265 RVA: 0x00012594 File Offset: 0x00010794
		public FloatRange IncidentPointsRandomFactorRange
		{
			get
			{
				return FloatRange.One;
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x060010AA RID: 4266 RVA: 0x000124C9 File Offset: 0x000106C9
		public MapParent Parent
		{
			get
			{
				return this.info.parent;
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x060010AB RID: 4267 RVA: 0x0001259B File Offset: 0x0001079B
		public float AgeInDays
		{
			get
			{
				return (float)(Find.TickManager.TicksGame - this.generationTick) / 60000f;
			}
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x000125B5 File Offset: 0x000107B5
		public IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
		{
			return this.info.parent.IncidentTargetTags();
		}

		// Token: 0x060010AD RID: 4269 RVA: 0x000BA990 File Offset: 0x000B8B90
		public void ConstructComponents()
		{
			this.spawnedThings = new ThingOwner<Thing>(this);
			this.cellIndices = new CellIndices(this);
			this.listerThings = new ListerThings(ListerThingsUse.Global);
			this.listerBuildings = new ListerBuildings();
			this.mapPawns = new MapPawns(this);
			this.dynamicDrawManager = new DynamicDrawManager(this);
			this.mapDrawer = new MapDrawer(this);
			this.tooltipGiverList = new TooltipGiverList();
			this.pawnDestinationReservationManager = new PawnDestinationReservationManager();
			this.reservationManager = new ReservationManager(this);
			this.physicalInteractionReservationManager = new PhysicalInteractionReservationManager();
			this.designationManager = new DesignationManager(this);
			this.lordManager = new LordManager(this);
			this.debugDrawer = new DebugCellDrawer();
			this.passingShipManager = new PassingShipManager(this);
			this.haulDestinationManager = new HaulDestinationManager(this);
			this.gameConditionManager = new GameConditionManager(this);
			this.weatherManager = new WeatherManager(this);
			this.zoneManager = new ZoneManager(this);
			this.resourceCounter = new ResourceCounter(this);
			this.mapTemperature = new MapTemperature(this);
			this.temperatureCache = new TemperatureCache(this);
			this.areaManager = new AreaManager(this);
			this.attackTargetsCache = new AttackTargetsCache(this);
			this.attackTargetReservationManager = new AttackTargetReservationManager(this);
			this.lordsStarter = new VoluntarilyJoinableLordsStarter(this);
			this.thingGrid = new ThingGrid(this);
			this.coverGrid = new CoverGrid(this);
			this.edificeGrid = new EdificeGrid(this);
			this.blueprintGrid = new BlueprintGrid(this);
			this.fogGrid = new FogGrid(this);
			this.glowGrid = new GlowGrid(this);
			this.regionGrid = new RegionGrid(this);
			this.terrainGrid = new TerrainGrid(this);
			this.pathGrid = new PathGrid(this);
			this.roofGrid = new RoofGrid(this);
			this.fertilityGrid = new FertilityGrid(this);
			this.snowGrid = new SnowGrid(this);
			this.deepResourceGrid = new DeepResourceGrid(this);
			this.exitMapGrid = new ExitMapGrid(this);
			this.avoidGrid = new AvoidGrid(this);
			this.linkGrid = new LinkGrid(this);
			this.glowFlooder = new GlowFlooder(this);
			this.powerNetManager = new PowerNetManager(this);
			this.powerNetGrid = new PowerNetGrid(this);
			this.regionMaker = new RegionMaker(this);
			this.pathFinder = new PathFinder(this);
			this.pawnPathPool = new PawnPathPool(this);
			this.regionAndRoomUpdater = new RegionAndRoomUpdater(this);
			this.regionLinkDatabase = new RegionLinkDatabase();
			this.moteCounter = new MoteCounter();
			this.gatherSpotLister = new GatherSpotLister();
			this.windManager = new WindManager(this);
			this.listerBuildingsRepairable = new ListerBuildingsRepairable();
			this.listerHaulables = new ListerHaulables(this);
			this.listerMergeables = new ListerMergeables(this);
			this.listerFilthInHomeArea = new ListerFilthInHomeArea(this);
			this.listerArtificialBuildingsForMeditation = new ListerArtificialBuildingsForMeditation(this);
			this.listerBuldingOfDefInProximity = new ListerBuldingOfDefInProximity(this);
			this.reachability = new Reachability(this);
			this.itemAvailability = new ItemAvailability(this);
			this.autoBuildRoofAreaSetter = new AutoBuildRoofAreaSetter(this);
			this.roofCollapseBufferResolver = new RoofCollapseBufferResolver(this);
			this.roofCollapseBuffer = new RoofCollapseBuffer();
			this.wildAnimalSpawner = new WildAnimalSpawner(this);
			this.wildPlantSpawner = new WildPlantSpawner(this);
			this.steadyEnvironmentEffects = new SteadyEnvironmentEffects(this);
			this.skyManager = new SkyManager(this);
			this.overlayDrawer = new OverlayDrawer();
			this.floodFiller = new FloodFiller(this);
			this.weatherDecider = new WeatherDecider(this);
			this.fireWatcher = new FireWatcher(this);
			this.dangerWatcher = new DangerWatcher(this);
			this.damageWatcher = new DamageWatcher();
			this.strengthWatcher = new StrengthWatcher(this);
			this.wealthWatcher = new WealthWatcher(this);
			this.regionDirtyer = new RegionDirtyer(this);
			this.cellsInRandomOrder = new MapCellsInRandomOrder(this);
			this.rememberedCameraPos = new RememberedCameraPos(this);
			this.mineStrikeManager = new MineStrikeManager();
			this.storyState = new StoryState(this);
			this.retainedCaravanData = new RetainedCaravanData(this);
			this.temporaryThingDrawer = new TemporaryThingDrawer();
			this.components.Clear();
			this.FillComponents();
		}

		// Token: 0x060010AE RID: 4270 RVA: 0x000BAD84 File Offset: 0x000B8F84
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.uniqueID, "uniqueID", -1, false);
			Scribe_Values.Look<int>(ref this.generationTick, "generationTick", 0, false);
			Scribe_Deep.Look<MapInfo>(ref this.info, "mapInfo", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.compressor = new MapFileCompressor(this);
				this.compressor.BuildCompressedString();
				this.ExposeComponents();
				this.compressor.ExposeData();
				HashSet<string> hashSet = new HashSet<string>();
				if (Scribe.EnterNode("things"))
				{
					try
					{
						using (List<Thing>.Enumerator enumerator = this.listerThings.AllThings.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								Thing thing = enumerator.Current;
								try
								{
									if (thing.def.isSaveable && !thing.IsSaveCompressible())
									{
										if (hashSet.Contains(thing.ThingID))
										{
											Log.Error("Saving Thing with already-used ID " + thing.ThingID, false);
										}
										else
										{
											hashSet.Add(thing.ThingID);
										}
										Thing thing2 = thing;
										Scribe_Deep.Look<Thing>(ref thing2, "thing", Array.Empty<object>());
									}
								}
								catch (OutOfMemoryException)
								{
									throw;
								}
								catch (Exception ex)
								{
									Log.Error(string.Concat(new object[]
									{
										"Exception saving ",
										thing,
										": ",
										ex
									}), false);
								}
							}
							goto IL_15A;
						}
					}
					finally
					{
						Scribe.ExitNode();
					}
				}
				Log.Error("Could not enter the things node while saving.", false);
				IL_15A:
				this.compressor = null;
				return;
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.ConstructComponents();
				this.regionAndRoomUpdater.Enabled = false;
				this.compressor = new MapFileCompressor(this);
			}
			this.ExposeComponents();
			DeepProfiler.Start("Load compressed things");
			this.compressor.ExposeData();
			DeepProfiler.End();
			DeepProfiler.Start("Load non-compressed things");
			Scribe_Collections.Look<Thing>(ref this.loadedFullThings, "things", LookMode.Deep, Array.Empty<object>());
			DeepProfiler.End();
		}

		// Token: 0x060010AF RID: 4271 RVA: 0x000BAF94 File Offset: 0x000B9194
		private void FillComponents()
		{
			this.components.RemoveAll((MapComponent component) => component == null);
			foreach (Type type in typeof(MapComponent).AllSubclassesNonAbstract())
			{
				if (this.GetComponent(type) == null)
				{
					try
					{
						MapComponent item = (MapComponent)Activator.CreateInstance(type, new object[]
						{
							this
						});
						this.components.Add(item);
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not instantiate a MapComponent of type ",
							type,
							": ",
							ex
						}), false);
					}
				}
			}
			this.roadInfo = this.GetComponent<RoadInfo>();
			this.waterInfo = this.GetComponent<WaterInfo>();
		}

		// Token: 0x060010B0 RID: 4272 RVA: 0x000BB08C File Offset: 0x000B928C
		public void FinalizeLoading()
		{
			List<Thing> list = this.compressor.ThingsToSpawnAfterLoad().ToList<Thing>();
			this.compressor = null;
			DeepProfiler.Start("Merge compressed and non-compressed thing lists");
			List<Thing> list2 = new List<Thing>(this.loadedFullThings.Count + list.Count);
			foreach (Thing item in this.loadedFullThings.Concat(list))
			{
				list2.Add(item);
			}
			this.loadedFullThings.Clear();
			DeepProfiler.End();
			DeepProfiler.Start("Spawn everything into the map");
			BackCompatibility.PreCheckSpawnBackCompatibleThingAfterLoading(this);
			foreach (Thing thing in list2)
			{
				if (!(thing is Building))
				{
					try
					{
						if (!BackCompatibility.CheckSpawnBackCompatibleThingAfterLoading(thing, this))
						{
							GenSpawn.Spawn(thing, thing.Position, this, thing.Rotation, WipeMode.FullRefund, true);
						}
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Exception spawning loaded thing ",
							thing.ToStringSafe<Thing>(),
							": ",
							ex
						}), false);
					}
				}
			}
			foreach (Building building in from t in list2.OfType<Building>()
			orderby t.def.size.Magnitude
			select t)
			{
				try
				{
					GenSpawn.SpawnBuildingAsPossible(building, this, true);
				}
				catch (Exception ex2)
				{
					Log.Error(string.Concat(new object[]
					{
						"Exception spawning loaded thing ",
						building.ToStringSafe<Building>(),
						": ",
						ex2
					}), false);
				}
			}
			BackCompatibility.PostCheckSpawnBackCompatibleThingAfterLoading(this);
			DeepProfiler.End();
			this.FinalizeInit();
		}

		// Token: 0x060010B1 RID: 4273 RVA: 0x000BB2A0 File Offset: 0x000B94A0
		public void FinalizeInit()
		{
			this.pathGrid.RecalculateAllPerceivedPathCosts();
			this.regionAndRoomUpdater.Enabled = true;
			this.regionAndRoomUpdater.RebuildAllRegionsAndRooms();
			this.powerNetManager.UpdatePowerNetsAndConnections_First();
			this.temperatureCache.temperatureSaveLoad.ApplyLoadedDataToRegions();
			this.avoidGrid.Regenerate();
			foreach (Thing thing in this.listerThings.AllThings.ToList<Thing>())
			{
				try
				{
					thing.PostMapInit();
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Error in PostMapInit() for ",
						thing.ToStringSafe<Thing>(),
						": ",
						ex
					}), false);
				}
			}
			this.listerFilthInHomeArea.RebuildAll();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.mapDrawer.RegenerateEverythingNow();
			});
			this.resourceCounter.UpdateResourceCounts();
			this.wealthWatcher.ForceRecount(true);
			MapComponentUtility.FinalizeInit(this);
		}

		// Token: 0x060010B2 RID: 4274 RVA: 0x000BB3BC File Offset: 0x000B95BC
		private void ExposeComponents()
		{
			Scribe_Deep.Look<WeatherManager>(ref this.weatherManager, "weatherManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<ReservationManager>(ref this.reservationManager, "reservationManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<PhysicalInteractionReservationManager>(ref this.physicalInteractionReservationManager, "physicalInteractionReservationManager", Array.Empty<object>());
			Scribe_Deep.Look<DesignationManager>(ref this.designationManager, "designationManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<PawnDestinationReservationManager>(ref this.pawnDestinationReservationManager, "pawnDestinationReservationManager", Array.Empty<object>());
			Scribe_Deep.Look<LordManager>(ref this.lordManager, "lordManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<PassingShipManager>(ref this.passingShipManager, "visitorManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<GameConditionManager>(ref this.gameConditionManager, "gameConditionManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<FogGrid>(ref this.fogGrid, "fogGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<RoofGrid>(ref this.roofGrid, "roofGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<TerrainGrid>(ref this.terrainGrid, "terrainGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<ZoneManager>(ref this.zoneManager, "zoneManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<TemperatureCache>(ref this.temperatureCache, "temperatureCache", new object[]
			{
				this
			});
			Scribe_Deep.Look<SnowGrid>(ref this.snowGrid, "snowGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<AreaManager>(ref this.areaManager, "areaManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<VoluntarilyJoinableLordsStarter>(ref this.lordsStarter, "lordsStarter", new object[]
			{
				this
			});
			Scribe_Deep.Look<AttackTargetReservationManager>(ref this.attackTargetReservationManager, "attackTargetReservationManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<DeepResourceGrid>(ref this.deepResourceGrid, "deepResourceGrid", new object[]
			{
				this
			});
			Scribe_Deep.Look<WeatherDecider>(ref this.weatherDecider, "weatherDecider", new object[]
			{
				this
			});
			Scribe_Deep.Look<DamageWatcher>(ref this.damageWatcher, "damageWatcher", Array.Empty<object>());
			Scribe_Deep.Look<RememberedCameraPos>(ref this.rememberedCameraPos, "rememberedCameraPos", new object[]
			{
				this
			});
			Scribe_Deep.Look<MineStrikeManager>(ref this.mineStrikeManager, "mineStrikeManager", Array.Empty<object>());
			Scribe_Deep.Look<RetainedCaravanData>(ref this.retainedCaravanData, "retainedCaravanData", new object[]
			{
				this
			});
			Scribe_Deep.Look<StoryState>(ref this.storyState, "storyState", new object[]
			{
				this
			});
			Scribe_Deep.Look<WildPlantSpawner>(ref this.wildPlantSpawner, "wildPlantSpawner", new object[]
			{
				this
			});
			Scribe_Deep.Look<TemporaryThingDrawer>(ref this.temporaryThingDrawer, "temporaryThingDrawer", Array.Empty<object>());
			Scribe_Collections.Look<MapComponent>(ref this.components, "components", LookMode.Deep, new object[]
			{
				this
			});
			this.FillComponents();
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x060010B3 RID: 4275 RVA: 0x000BB67C File Offset: 0x000B987C
		public void MapPreTick()
		{
			this.itemAvailability.Tick();
			this.listerHaulables.ListerHaulablesTick();
			try
			{
				this.autoBuildRoofAreaSetter.AutoBuildRoofAreaSetterTick_First();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString(), false);
			}
			this.roofCollapseBufferResolver.CollapseRoofsMarkedToCollapse();
			this.windManager.WindManagerTick();
			try
			{
				this.mapTemperature.MapTemperatureTick();
			}
			catch (Exception ex2)
			{
				Log.Error(ex2.ToString(), false);
			}
			this.temporaryThingDrawer.Tick();
		}

		// Token: 0x060010B4 RID: 4276 RVA: 0x000BB710 File Offset: 0x000B9910
		public void MapPostTick()
		{
			try
			{
				this.wildAnimalSpawner.WildAnimalSpawnerTick();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString(), false);
			}
			try
			{
				this.wildPlantSpawner.WildPlantSpawnerTick();
			}
			catch (Exception ex2)
			{
				Log.Error(ex2.ToString(), false);
			}
			try
			{
				this.powerNetManager.PowerNetsTick();
			}
			catch (Exception ex3)
			{
				Log.Error(ex3.ToString(), false);
			}
			try
			{
				this.steadyEnvironmentEffects.SteadyEnvironmentEffectsTick();
			}
			catch (Exception ex4)
			{
				Log.Error(ex4.ToString(), false);
			}
			try
			{
				this.lordManager.LordManagerTick();
			}
			catch (Exception ex5)
			{
				Log.Error(ex5.ToString(), false);
			}
			try
			{
				this.passingShipManager.PassingShipManagerTick();
			}
			catch (Exception ex6)
			{
				Log.Error(ex6.ToString(), false);
			}
			try
			{
				this.debugDrawer.DebugDrawerTick();
			}
			catch (Exception ex7)
			{
				Log.Error(ex7.ToString(), false);
			}
			try
			{
				this.lordsStarter.VoluntarilyJoinableLordsStarterTick();
			}
			catch (Exception ex8)
			{
				Log.Error(ex8.ToString(), false);
			}
			try
			{
				this.gameConditionManager.GameConditionManagerTick();
			}
			catch (Exception ex9)
			{
				Log.Error(ex9.ToString(), false);
			}
			try
			{
				this.weatherManager.WeatherManagerTick();
			}
			catch (Exception ex10)
			{
				Log.Error(ex10.ToString(), false);
			}
			try
			{
				this.resourceCounter.ResourceCounterTick();
			}
			catch (Exception ex11)
			{
				Log.Error(ex11.ToString(), false);
			}
			try
			{
				this.weatherDecider.WeatherDeciderTick();
			}
			catch (Exception ex12)
			{
				Log.Error(ex12.ToString(), false);
			}
			try
			{
				this.fireWatcher.FireWatcherTick();
			}
			catch (Exception ex13)
			{
				Log.Error(ex13.ToString(), false);
			}
			MapComponentUtility.MapComponentTick(this);
		}

		// Token: 0x060010B5 RID: 4277 RVA: 0x000BB924 File Offset: 0x000B9B24
		public void MapUpdate()
		{
			bool worldRenderedNow = WorldRendererUtility.WorldRenderedNow;
			this.skyManager.SkyManagerUpdate();
			this.powerNetManager.UpdatePowerNetsAndConnections_First();
			this.regionGrid.UpdateClean();
			this.regionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms();
			this.glowGrid.GlowGridUpdate_First();
			this.lordManager.LordManagerUpdate();
			if (!worldRenderedNow && Find.CurrentMap == this)
			{
				if (Map.AlwaysRedrawShadows)
				{
					this.mapDrawer.WholeMapChanged(MapMeshFlag.Things);
				}
				PlantFallColors.SetFallShaderGlobals(this);
				this.waterInfo.SetTextures();
				this.avoidGrid.DebugDrawOnMap();
				this.mapDrawer.MapMeshDrawerUpdate_First();
				this.powerNetGrid.DrawDebugPowerNetGrid();
				DoorsDebugDrawer.DrawDebug();
				this.mapDrawer.DrawMapMesh();
				this.dynamicDrawManager.DrawDynamicThings();
				this.gameConditionManager.GameConditionManagerDraw(this);
				MapEdgeClipDrawer.DrawClippers(this);
				this.designationManager.DrawDesignations();
				this.overlayDrawer.DrawAllOverlays();
				this.temporaryThingDrawer.Draw();
			}
			try
			{
				this.areaManager.AreaManagerUpdate();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString(), false);
			}
			this.weatherManager.WeatherManagerUpdate();
			MapComponentUtility.MapComponentUpdate(this);
		}

		// Token: 0x060010B6 RID: 4278 RVA: 0x000BBA58 File Offset: 0x000B9C58
		public T GetComponent<T>() where T : MapComponent
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

		// Token: 0x060010B7 RID: 4279 RVA: 0x000BBAA8 File Offset: 0x000B9CA8
		public MapComponent GetComponent(Type type)
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

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x060010B8 RID: 4280 RVA: 0x000125C7 File Offset: 0x000107C7
		public int ConstantRandSeed
		{
			get
			{
				return this.uniqueID ^ 16622162;
			}
		}

		// Token: 0x060010B9 RID: 4281 RVA: 0x000125D5 File Offset: 0x000107D5
		public string GetUniqueLoadID()
		{
			return "Map_" + this.uniqueID;
		}

		// Token: 0x060010BA RID: 4282 RVA: 0x000BBAF4 File Offset: 0x000B9CF4
		public override string ToString()
		{
			string text = "Map-" + this.uniqueID;
			if (this.IsPlayerHome)
			{
				text += "-PlayerHome";
			}
			return text;
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x000125EC File Offset: 0x000107EC
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.spawnedThings;
		}

		// Token: 0x060010BC RID: 4284 RVA: 0x000BBB2C File Offset: 0x000B9D2C
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.listerThings.ThingsInGroup(ThingRequestGroup.ThingHolder));
			List<PassingShip> passingShips = this.passingShipManager.passingShips;
			for (int i = 0; i < passingShips.Count; i++)
			{
				IThingHolder thingHolder = passingShips[i] as IThingHolder;
				if (thingHolder != null)
				{
					outChildren.Add(thingHolder);
				}
			}
			for (int j = 0; j < this.components.Count; j++)
			{
				IThingHolder thingHolder2 = this.components[j] as IThingHolder;
				if (thingHolder2 != null)
				{
					outChildren.Add(thingHolder2);
				}
			}
		}

		// Token: 0x04000D48 RID: 3400
		public MapFileCompressor compressor;

		// Token: 0x04000D49 RID: 3401
		private List<Thing> loadedFullThings;

		// Token: 0x04000D4A RID: 3402
		public int uniqueID = -1;

		// Token: 0x04000D4B RID: 3403
		public int generationTick;

		// Token: 0x04000D4C RID: 3404
		public MapInfo info = new MapInfo();

		// Token: 0x04000D4D RID: 3405
		public List<MapComponent> components = new List<MapComponent>();

		// Token: 0x04000D4E RID: 3406
		public ThingOwner spawnedThings;

		// Token: 0x04000D4F RID: 3407
		public CellIndices cellIndices;

		// Token: 0x04000D50 RID: 3408
		public ListerThings listerThings;

		// Token: 0x04000D51 RID: 3409
		public ListerBuildings listerBuildings;

		// Token: 0x04000D52 RID: 3410
		public MapPawns mapPawns;

		// Token: 0x04000D53 RID: 3411
		public DynamicDrawManager dynamicDrawManager;

		// Token: 0x04000D54 RID: 3412
		public MapDrawer mapDrawer;

		// Token: 0x04000D55 RID: 3413
		public PawnDestinationReservationManager pawnDestinationReservationManager;

		// Token: 0x04000D56 RID: 3414
		public TooltipGiverList tooltipGiverList;

		// Token: 0x04000D57 RID: 3415
		public ReservationManager reservationManager;

		// Token: 0x04000D58 RID: 3416
		public PhysicalInteractionReservationManager physicalInteractionReservationManager;

		// Token: 0x04000D59 RID: 3417
		public DesignationManager designationManager;

		// Token: 0x04000D5A RID: 3418
		public LordManager lordManager;

		// Token: 0x04000D5B RID: 3419
		public PassingShipManager passingShipManager;

		// Token: 0x04000D5C RID: 3420
		public HaulDestinationManager haulDestinationManager;

		// Token: 0x04000D5D RID: 3421
		public DebugCellDrawer debugDrawer;

		// Token: 0x04000D5E RID: 3422
		public GameConditionManager gameConditionManager;

		// Token: 0x04000D5F RID: 3423
		public WeatherManager weatherManager;

		// Token: 0x04000D60 RID: 3424
		public ZoneManager zoneManager;

		// Token: 0x04000D61 RID: 3425
		public ResourceCounter resourceCounter;

		// Token: 0x04000D62 RID: 3426
		public MapTemperature mapTemperature;

		// Token: 0x04000D63 RID: 3427
		public TemperatureCache temperatureCache;

		// Token: 0x04000D64 RID: 3428
		public AreaManager areaManager;

		// Token: 0x04000D65 RID: 3429
		public AttackTargetsCache attackTargetsCache;

		// Token: 0x04000D66 RID: 3430
		public AttackTargetReservationManager attackTargetReservationManager;

		// Token: 0x04000D67 RID: 3431
		public VoluntarilyJoinableLordsStarter lordsStarter;

		// Token: 0x04000D68 RID: 3432
		public ThingGrid thingGrid;

		// Token: 0x04000D69 RID: 3433
		public CoverGrid coverGrid;

		// Token: 0x04000D6A RID: 3434
		public EdificeGrid edificeGrid;

		// Token: 0x04000D6B RID: 3435
		public BlueprintGrid blueprintGrid;

		// Token: 0x04000D6C RID: 3436
		public FogGrid fogGrid;

		// Token: 0x04000D6D RID: 3437
		public RegionGrid regionGrid;

		// Token: 0x04000D6E RID: 3438
		public GlowGrid glowGrid;

		// Token: 0x04000D6F RID: 3439
		public TerrainGrid terrainGrid;

		// Token: 0x04000D70 RID: 3440
		public PathGrid pathGrid;

		// Token: 0x04000D71 RID: 3441
		public RoofGrid roofGrid;

		// Token: 0x04000D72 RID: 3442
		public FertilityGrid fertilityGrid;

		// Token: 0x04000D73 RID: 3443
		public SnowGrid snowGrid;

		// Token: 0x04000D74 RID: 3444
		public DeepResourceGrid deepResourceGrid;

		// Token: 0x04000D75 RID: 3445
		public ExitMapGrid exitMapGrid;

		// Token: 0x04000D76 RID: 3446
		public AvoidGrid avoidGrid;

		// Token: 0x04000D77 RID: 3447
		public LinkGrid linkGrid;

		// Token: 0x04000D78 RID: 3448
		public GlowFlooder glowFlooder;

		// Token: 0x04000D79 RID: 3449
		public PowerNetManager powerNetManager;

		// Token: 0x04000D7A RID: 3450
		public PowerNetGrid powerNetGrid;

		// Token: 0x04000D7B RID: 3451
		public RegionMaker regionMaker;

		// Token: 0x04000D7C RID: 3452
		public PathFinder pathFinder;

		// Token: 0x04000D7D RID: 3453
		public PawnPathPool pawnPathPool;

		// Token: 0x04000D7E RID: 3454
		public RegionAndRoomUpdater regionAndRoomUpdater;

		// Token: 0x04000D7F RID: 3455
		public RegionLinkDatabase regionLinkDatabase;

		// Token: 0x04000D80 RID: 3456
		public MoteCounter moteCounter;

		// Token: 0x04000D81 RID: 3457
		public GatherSpotLister gatherSpotLister;

		// Token: 0x04000D82 RID: 3458
		public WindManager windManager;

		// Token: 0x04000D83 RID: 3459
		public ListerBuildingsRepairable listerBuildingsRepairable;

		// Token: 0x04000D84 RID: 3460
		public ListerHaulables listerHaulables;

		// Token: 0x04000D85 RID: 3461
		public ListerMergeables listerMergeables;

		// Token: 0x04000D86 RID: 3462
		public ListerArtificialBuildingsForMeditation listerArtificialBuildingsForMeditation;

		// Token: 0x04000D87 RID: 3463
		public ListerBuldingOfDefInProximity listerBuldingOfDefInProximity;

		// Token: 0x04000D88 RID: 3464
		public ListerFilthInHomeArea listerFilthInHomeArea;

		// Token: 0x04000D89 RID: 3465
		public Reachability reachability;

		// Token: 0x04000D8A RID: 3466
		public ItemAvailability itemAvailability;

		// Token: 0x04000D8B RID: 3467
		public AutoBuildRoofAreaSetter autoBuildRoofAreaSetter;

		// Token: 0x04000D8C RID: 3468
		public RoofCollapseBufferResolver roofCollapseBufferResolver;

		// Token: 0x04000D8D RID: 3469
		public RoofCollapseBuffer roofCollapseBuffer;

		// Token: 0x04000D8E RID: 3470
		public WildAnimalSpawner wildAnimalSpawner;

		// Token: 0x04000D8F RID: 3471
		public WildPlantSpawner wildPlantSpawner;

		// Token: 0x04000D90 RID: 3472
		public SteadyEnvironmentEffects steadyEnvironmentEffects;

		// Token: 0x04000D91 RID: 3473
		public SkyManager skyManager;

		// Token: 0x04000D92 RID: 3474
		public OverlayDrawer overlayDrawer;

		// Token: 0x04000D93 RID: 3475
		public FloodFiller floodFiller;

		// Token: 0x04000D94 RID: 3476
		public WeatherDecider weatherDecider;

		// Token: 0x04000D95 RID: 3477
		public FireWatcher fireWatcher;

		// Token: 0x04000D96 RID: 3478
		public DangerWatcher dangerWatcher;

		// Token: 0x04000D97 RID: 3479
		public DamageWatcher damageWatcher;

		// Token: 0x04000D98 RID: 3480
		public StrengthWatcher strengthWatcher;

		// Token: 0x04000D99 RID: 3481
		public WealthWatcher wealthWatcher;

		// Token: 0x04000D9A RID: 3482
		public RegionDirtyer regionDirtyer;

		// Token: 0x04000D9B RID: 3483
		public MapCellsInRandomOrder cellsInRandomOrder;

		// Token: 0x04000D9C RID: 3484
		public RememberedCameraPos rememberedCameraPos;

		// Token: 0x04000D9D RID: 3485
		public MineStrikeManager mineStrikeManager;

		// Token: 0x04000D9E RID: 3486
		public StoryState storyState;

		// Token: 0x04000D9F RID: 3487
		public RoadInfo roadInfo;

		// Token: 0x04000DA0 RID: 3488
		public WaterInfo waterInfo;

		// Token: 0x04000DA1 RID: 3489
		public RetainedCaravanData retainedCaravanData;

		// Token: 0x04000DA2 RID: 3490
		public TemporaryThingDrawer temporaryThingDrawer;

		// Token: 0x04000DA3 RID: 3491
		public const string ThingSaveKey = "thing";

		// Token: 0x04000DA4 RID: 3492
		[TweakValue("Graphics_Shadow", 0f, 100f)]
		private static bool AlwaysRedrawShadows;
	}
}
