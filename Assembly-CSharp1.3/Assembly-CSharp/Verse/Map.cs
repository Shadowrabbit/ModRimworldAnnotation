using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using Verse.AI;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x020001C3 RID: 451
	public sealed class Map : IIncidentTarget, ILoadReferenceable, IThingHolder, IExposable
	{
		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000CFF RID: 3327 RVA: 0x00045450 File Offset: 0x00043650
		public int Index
		{
			get
			{
				return Find.Maps.IndexOf(this);
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000D00 RID: 3328 RVA: 0x0004545D File Offset: 0x0004365D
		public IntVec3 Size
		{
			get
			{
				return this.info.Size;
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000D01 RID: 3329 RVA: 0x0004546A File Offset: 0x0004366A
		public IntVec3 Center
		{
			get
			{
				return new IntVec3(this.Size.x / 2, 0, this.Size.z / 2);
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000D02 RID: 3330 RVA: 0x0004548C File Offset: 0x0004368C
		public Faction ParentFaction
		{
			get
			{
				return this.info.parent.Faction;
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000D03 RID: 3331 RVA: 0x0004549E File Offset: 0x0004369E
		public int Area
		{
			get
			{
				return this.Size.x * this.Size.z;
			}
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000D04 RID: 3332 RVA: 0x000454B7 File Offset: 0x000436B7
		public IThingHolder ParentHolder
		{
			get
			{
				return this.info.parent;
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000D05 RID: 3333 RVA: 0x000454C4 File Offset: 0x000436C4
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

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000D06 RID: 3334 RVA: 0x000454D4 File Offset: 0x000436D4
		public bool IsPlayerHome
		{
			get
			{
				return this.info != null && this.info.parent.def.canBePlayerHome && this.info.parent.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000D07 RID: 3335 RVA: 0x0004550E File Offset: 0x0004370E
		public bool IsTempIncidentMap
		{
			get
			{
				return this.info.parent.def.isTempIncidentMapOwner;
			}
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x00045525 File Offset: 0x00043725
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

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000D09 RID: 3337 RVA: 0x00045534 File Offset: 0x00043734
		public int Tile
		{
			get
			{
				return this.info.Tile;
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000D0A RID: 3338 RVA: 0x00045541 File Offset: 0x00043741
		public Tile TileInfo
		{
			get
			{
				return Find.WorldGrid[this.Tile];
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000D0B RID: 3339 RVA: 0x00045553 File Offset: 0x00043753
		public BiomeDef Biome
		{
			get
			{
				return this.TileInfo.biome;
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000D0C RID: 3340 RVA: 0x00045560 File Offset: 0x00043760
		public StoryState StoryState
		{
			get
			{
				return this.storyState;
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000D0D RID: 3341 RVA: 0x00045568 File Offset: 0x00043768
		public GameConditionManager GameConditionManager
		{
			get
			{
				return this.gameConditionManager;
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000D0E RID: 3342 RVA: 0x00045570 File Offset: 0x00043770
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
				if (Find.Storyteller.difficulty.fixedWealthMode)
				{
					return StorytellerUtility.FixedWealthModeMapWealthFromTimeCurve.Evaluate(this.AgeInDays * Find.Storyteller.difficulty.fixedWealthTimeFactor);
				}
				return this.wealthWatcher.WealthItems + this.wealthWatcher.WealthBuildings * 0.5f + this.wealthWatcher.WealthPawns;
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000D0F RID: 3343 RVA: 0x00045658 File Offset: 0x00043858
		public IEnumerable<Pawn> PlayerPawnsForStoryteller
		{
			get
			{
				return this.mapPawns.PawnsInFaction(Faction.OfPlayer);
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000D10 RID: 3344 RVA: 0x0004566A File Offset: 0x0004386A
		public FloatRange IncidentPointsRandomFactorRange
		{
			get
			{
				return FloatRange.One;
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000D11 RID: 3345 RVA: 0x000454B7 File Offset: 0x000436B7
		public MapParent Parent
		{
			get
			{
				return this.info.parent;
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000D12 RID: 3346 RVA: 0x00045671 File Offset: 0x00043871
		public float AgeInDays
		{
			get
			{
				return (float)(Find.TickManager.TicksGame - this.generationTick) / 60000f;
			}
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x0004568B File Offset: 0x0004388B
		public IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
		{
			return this.info.parent.IncidentTargetTags();
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x000456A0 File Offset: 0x000438A0
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
			this.flecks = new FleckManager(this);
			this.thingGrid = new ThingGrid(this);
			this.coverGrid = new CoverGrid(this);
			this.edificeGrid = new EdificeGrid(this);
			this.blueprintGrid = new BlueprintGrid(this);
			this.fogGrid = new FogGrid(this);
			this.glowGrid = new GlowGrid(this);
			this.regionGrid = new RegionGrid(this);
			this.terrainGrid = new TerrainGrid(this);
			this.pathing = new Pathing(this);
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
			this.listerBuildingWithTagInProximity = new ListerBuildingWithTagInProximity(this);
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
			this.animalPenManager = new AnimalPenManager(this);
			this.plantGrowthRateCalculator = new MapPlantGrowthRateCalculator();
			this.autoSlaughterManager = new AutoSlaughterManager(this);
			this.treeDestructionTracker = new TreeDestructionTracker(this);
			this.components.Clear();
			this.FillComponents();
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x00045ADC File Offset: 0x00043CDC
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
											Log.Error("Saving Thing with already-used ID " + thing.ThingID);
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
									}));
								}
							}
							goto IL_157;
						}
					}
					finally
					{
						Scribe.ExitNode();
					}
				}
				Log.Error("Could not enter the things node while saving.");
				IL_157:
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

		// Token: 0x06000D16 RID: 3350 RVA: 0x00045CE8 File Offset: 0x00043EE8
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
						}));
					}
				}
			}
			this.roadInfo = this.GetComponent<RoadInfo>();
			this.waterInfo = this.GetComponent<WaterInfo>();
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x00045DE4 File Offset: 0x00043FE4
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
						}));
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
					}));
				}
			}
			BackCompatibility.PostCheckSpawnBackCompatibleThingAfterLoading(this);
			DeepProfiler.End();
			this.FinalizeInit();
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x00045FF4 File Offset: 0x000441F4
		public void FinalizeInit()
		{
			this.pathing.RecalculateAllPerceivedPathCosts();
			this.regionAndRoomUpdater.Enabled = true;
			this.regionAndRoomUpdater.RebuildAllRegionsAndRooms();
			this.powerNetManager.UpdatePowerNetsAndConnections_First();
			this.temperatureCache.temperatureSaveLoad.ApplyLoadedDataToRegions();
			this.avoidGrid.Regenerate();
			this.animalPenManager.RebuildAllPens();
			this.plantGrowthRateCalculator.BuildFor(this.Tile);
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
					}));
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

		// Token: 0x06000D19 RID: 3353 RVA: 0x0004612C File Offset: 0x0004432C
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
			Scribe_Deep.Look<FleckManager>(ref this.flecks, "flecks", new object[]
			{
				this
			});
			Scribe_Deep.Look<AutoSlaughterManager>(ref this.autoSlaughterManager, "autoSlaughterManager", new object[]
			{
				this
			});
			Scribe_Deep.Look<TreeDestructionTracker>(ref this.treeDestructionTracker, "treeDestructionTracker", new object[]
			{
				this
			});
			Scribe_Collections.Look<MapComponent>(ref this.components, "components", LookMode.Deep, new object[]
			{
				this
			});
			this.FillComponents();
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x0004643C File Offset: 0x0004463C
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
				Log.Error(ex.ToString());
			}
			this.roofCollapseBufferResolver.CollapseRoofsMarkedToCollapse();
			this.windManager.WindManagerTick();
			try
			{
				this.mapTemperature.MapTemperatureTick();
			}
			catch (Exception ex2)
			{
				Log.Error(ex2.ToString());
			}
			this.temporaryThingDrawer.Tick();
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x000464D0 File Offset: 0x000446D0
		public void MapPostTick()
		{
			try
			{
				this.wildAnimalSpawner.WildAnimalSpawnerTick();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
			}
			try
			{
				this.wildPlantSpawner.WildPlantSpawnerTick();
			}
			catch (Exception ex2)
			{
				Log.Error(ex2.ToString());
			}
			try
			{
				this.powerNetManager.PowerNetsTick();
			}
			catch (Exception ex3)
			{
				Log.Error(ex3.ToString());
			}
			try
			{
				this.steadyEnvironmentEffects.SteadyEnvironmentEffectsTick();
			}
			catch (Exception ex4)
			{
				Log.Error(ex4.ToString());
			}
			try
			{
				this.lordManager.LordManagerTick();
			}
			catch (Exception ex5)
			{
				Log.Error(ex5.ToString());
			}
			try
			{
				this.passingShipManager.PassingShipManagerTick();
			}
			catch (Exception ex6)
			{
				Log.Error(ex6.ToString());
			}
			try
			{
				this.debugDrawer.DebugDrawerTick();
			}
			catch (Exception ex7)
			{
				Log.Error(ex7.ToString());
			}
			try
			{
				this.lordsStarter.VoluntarilyJoinableLordsStarterTick();
			}
			catch (Exception ex8)
			{
				Log.Error(ex8.ToString());
			}
			try
			{
				this.gameConditionManager.GameConditionManagerTick();
			}
			catch (Exception ex9)
			{
				Log.Error(ex9.ToString());
			}
			try
			{
				this.weatherManager.WeatherManagerTick();
			}
			catch (Exception ex10)
			{
				Log.Error(ex10.ToString());
			}
			try
			{
				this.resourceCounter.ResourceCounterTick();
			}
			catch (Exception ex11)
			{
				Log.Error(ex11.ToString());
			}
			try
			{
				this.weatherDecider.WeatherDeciderTick();
			}
			catch (Exception ex12)
			{
				Log.Error(ex12.ToString());
			}
			try
			{
				this.fireWatcher.FireWatcherTick();
			}
			catch (Exception ex13)
			{
				Log.Error(ex13.ToString());
			}
			try
			{
				this.flecks.FleckManagerTick();
			}
			catch (Exception ex14)
			{
				Log.Error(ex14.ToString());
			}
			MapComponentUtility.MapComponentTick(this);
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x000466FC File Offset: 0x000448FC
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
				BreachingGridDebug.DebugDrawAllOnMap(this);
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
				this.flecks.FleckManagerDraw();
			}
			try
			{
				this.areaManager.AreaManagerUpdate();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
			}
			this.weatherManager.WeatherManagerUpdate();
			try
			{
				this.flecks.FleckManagerUpdate();
			}
			catch (Exception ex2)
			{
				Log.Error(ex2.ToString());
			}
			MapComponentUtility.MapComponentUpdate(this);
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x00046864 File Offset: 0x00044A64
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

		// Token: 0x06000D1E RID: 3358 RVA: 0x000468B4 File Offset: 0x00044AB4
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

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000D1F RID: 3359 RVA: 0x000468FE File Offset: 0x00044AFE
		public int ConstantRandSeed
		{
			get
			{
				return this.uniqueID ^ 16622162;
			}
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x0004690C File Offset: 0x00044B0C
		public string GetUniqueLoadID()
		{
			return "Map_" + this.uniqueID;
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x00046924 File Offset: 0x00044B24
		public override string ToString()
		{
			string text = "Map-" + this.uniqueID;
			if (this.IsPlayerHome)
			{
				text += "-PlayerHome";
			}
			return text;
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x0004695C File Offset: 0x00044B5C
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.spawnedThings;
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x00046964 File Offset: 0x00044B64
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

		// Token: 0x04000A74 RID: 2676
		public MapFileCompressor compressor;

		// Token: 0x04000A75 RID: 2677
		private List<Thing> loadedFullThings;

		// Token: 0x04000A76 RID: 2678
		public int uniqueID = -1;

		// Token: 0x04000A77 RID: 2679
		public int generationTick;

		// Token: 0x04000A78 RID: 2680
		public MapInfo info = new MapInfo();

		// Token: 0x04000A79 RID: 2681
		public List<MapComponent> components = new List<MapComponent>();

		// Token: 0x04000A7A RID: 2682
		public ThingOwner spawnedThings;

		// Token: 0x04000A7B RID: 2683
		public CellIndices cellIndices;

		// Token: 0x04000A7C RID: 2684
		public ListerThings listerThings;

		// Token: 0x04000A7D RID: 2685
		public ListerBuildings listerBuildings;

		// Token: 0x04000A7E RID: 2686
		public MapPawns mapPawns;

		// Token: 0x04000A7F RID: 2687
		public DynamicDrawManager dynamicDrawManager;

		// Token: 0x04000A80 RID: 2688
		public MapDrawer mapDrawer;

		// Token: 0x04000A81 RID: 2689
		public PawnDestinationReservationManager pawnDestinationReservationManager;

		// Token: 0x04000A82 RID: 2690
		public TooltipGiverList tooltipGiverList;

		// Token: 0x04000A83 RID: 2691
		public ReservationManager reservationManager;

		// Token: 0x04000A84 RID: 2692
		public PhysicalInteractionReservationManager physicalInteractionReservationManager;

		// Token: 0x04000A85 RID: 2693
		public DesignationManager designationManager;

		// Token: 0x04000A86 RID: 2694
		public LordManager lordManager;

		// Token: 0x04000A87 RID: 2695
		public PassingShipManager passingShipManager;

		// Token: 0x04000A88 RID: 2696
		public HaulDestinationManager haulDestinationManager;

		// Token: 0x04000A89 RID: 2697
		public DebugCellDrawer debugDrawer;

		// Token: 0x04000A8A RID: 2698
		public GameConditionManager gameConditionManager;

		// Token: 0x04000A8B RID: 2699
		public WeatherManager weatherManager;

		// Token: 0x04000A8C RID: 2700
		public ZoneManager zoneManager;

		// Token: 0x04000A8D RID: 2701
		public ResourceCounter resourceCounter;

		// Token: 0x04000A8E RID: 2702
		public MapTemperature mapTemperature;

		// Token: 0x04000A8F RID: 2703
		public TemperatureCache temperatureCache;

		// Token: 0x04000A90 RID: 2704
		public AreaManager areaManager;

		// Token: 0x04000A91 RID: 2705
		public AttackTargetsCache attackTargetsCache;

		// Token: 0x04000A92 RID: 2706
		public AttackTargetReservationManager attackTargetReservationManager;

		// Token: 0x04000A93 RID: 2707
		public VoluntarilyJoinableLordsStarter lordsStarter;

		// Token: 0x04000A94 RID: 2708
		public FleckManager flecks;

		// Token: 0x04000A95 RID: 2709
		public ThingGrid thingGrid;

		// Token: 0x04000A96 RID: 2710
		public CoverGrid coverGrid;

		// Token: 0x04000A97 RID: 2711
		public EdificeGrid edificeGrid;

		// Token: 0x04000A98 RID: 2712
		public BlueprintGrid blueprintGrid;

		// Token: 0x04000A99 RID: 2713
		public FogGrid fogGrid;

		// Token: 0x04000A9A RID: 2714
		public RegionGrid regionGrid;

		// Token: 0x04000A9B RID: 2715
		public GlowGrid glowGrid;

		// Token: 0x04000A9C RID: 2716
		public TerrainGrid terrainGrid;

		// Token: 0x04000A9D RID: 2717
		public Pathing pathing;

		// Token: 0x04000A9E RID: 2718
		public RoofGrid roofGrid;

		// Token: 0x04000A9F RID: 2719
		public FertilityGrid fertilityGrid;

		// Token: 0x04000AA0 RID: 2720
		public SnowGrid snowGrid;

		// Token: 0x04000AA1 RID: 2721
		public DeepResourceGrid deepResourceGrid;

		// Token: 0x04000AA2 RID: 2722
		public ExitMapGrid exitMapGrid;

		// Token: 0x04000AA3 RID: 2723
		public AvoidGrid avoidGrid;

		// Token: 0x04000AA4 RID: 2724
		public LinkGrid linkGrid;

		// Token: 0x04000AA5 RID: 2725
		public GlowFlooder glowFlooder;

		// Token: 0x04000AA6 RID: 2726
		public PowerNetManager powerNetManager;

		// Token: 0x04000AA7 RID: 2727
		public PowerNetGrid powerNetGrid;

		// Token: 0x04000AA8 RID: 2728
		public RegionMaker regionMaker;

		// Token: 0x04000AA9 RID: 2729
		public PathFinder pathFinder;

		// Token: 0x04000AAA RID: 2730
		public PawnPathPool pawnPathPool;

		// Token: 0x04000AAB RID: 2731
		public RegionAndRoomUpdater regionAndRoomUpdater;

		// Token: 0x04000AAC RID: 2732
		public RegionLinkDatabase regionLinkDatabase;

		// Token: 0x04000AAD RID: 2733
		public MoteCounter moteCounter;

		// Token: 0x04000AAE RID: 2734
		public GatherSpotLister gatherSpotLister;

		// Token: 0x04000AAF RID: 2735
		public WindManager windManager;

		// Token: 0x04000AB0 RID: 2736
		public ListerBuildingsRepairable listerBuildingsRepairable;

		// Token: 0x04000AB1 RID: 2737
		public ListerHaulables listerHaulables;

		// Token: 0x04000AB2 RID: 2738
		public ListerMergeables listerMergeables;

		// Token: 0x04000AB3 RID: 2739
		public ListerArtificialBuildingsForMeditation listerArtificialBuildingsForMeditation;

		// Token: 0x04000AB4 RID: 2740
		public ListerBuldingOfDefInProximity listerBuldingOfDefInProximity;

		// Token: 0x04000AB5 RID: 2741
		public ListerBuildingWithTagInProximity listerBuildingWithTagInProximity;

		// Token: 0x04000AB6 RID: 2742
		public ListerFilthInHomeArea listerFilthInHomeArea;

		// Token: 0x04000AB7 RID: 2743
		public Reachability reachability;

		// Token: 0x04000AB8 RID: 2744
		public ItemAvailability itemAvailability;

		// Token: 0x04000AB9 RID: 2745
		public AutoBuildRoofAreaSetter autoBuildRoofAreaSetter;

		// Token: 0x04000ABA RID: 2746
		public RoofCollapseBufferResolver roofCollapseBufferResolver;

		// Token: 0x04000ABB RID: 2747
		public RoofCollapseBuffer roofCollapseBuffer;

		// Token: 0x04000ABC RID: 2748
		public WildAnimalSpawner wildAnimalSpawner;

		// Token: 0x04000ABD RID: 2749
		public WildPlantSpawner wildPlantSpawner;

		// Token: 0x04000ABE RID: 2750
		public SteadyEnvironmentEffects steadyEnvironmentEffects;

		// Token: 0x04000ABF RID: 2751
		public SkyManager skyManager;

		// Token: 0x04000AC0 RID: 2752
		public OverlayDrawer overlayDrawer;

		// Token: 0x04000AC1 RID: 2753
		public FloodFiller floodFiller;

		// Token: 0x04000AC2 RID: 2754
		public WeatherDecider weatherDecider;

		// Token: 0x04000AC3 RID: 2755
		public FireWatcher fireWatcher;

		// Token: 0x04000AC4 RID: 2756
		public DangerWatcher dangerWatcher;

		// Token: 0x04000AC5 RID: 2757
		public DamageWatcher damageWatcher;

		// Token: 0x04000AC6 RID: 2758
		public StrengthWatcher strengthWatcher;

		// Token: 0x04000AC7 RID: 2759
		public WealthWatcher wealthWatcher;

		// Token: 0x04000AC8 RID: 2760
		public RegionDirtyer regionDirtyer;

		// Token: 0x04000AC9 RID: 2761
		public MapCellsInRandomOrder cellsInRandomOrder;

		// Token: 0x04000ACA RID: 2762
		public RememberedCameraPos rememberedCameraPos;

		// Token: 0x04000ACB RID: 2763
		public MineStrikeManager mineStrikeManager;

		// Token: 0x04000ACC RID: 2764
		public StoryState storyState;

		// Token: 0x04000ACD RID: 2765
		public RoadInfo roadInfo;

		// Token: 0x04000ACE RID: 2766
		public WaterInfo waterInfo;

		// Token: 0x04000ACF RID: 2767
		public RetainedCaravanData retainedCaravanData;

		// Token: 0x04000AD0 RID: 2768
		public TemporaryThingDrawer temporaryThingDrawer;

		// Token: 0x04000AD1 RID: 2769
		public AnimalPenManager animalPenManager;

		// Token: 0x04000AD2 RID: 2770
		public MapPlantGrowthRateCalculator plantGrowthRateCalculator;

		// Token: 0x04000AD3 RID: 2771
		public AutoSlaughterManager autoSlaughterManager;

		// Token: 0x04000AD4 RID: 2772
		public TreeDestructionTracker treeDestructionTracker;

		// Token: 0x04000AD5 RID: 2773
		public const string ThingSaveKey = "thing";

		// Token: 0x04000AD6 RID: 2774
		[TweakValue("Graphics_Shadow", 0f, 100f)]
		private static bool AlwaysRedrawShadows;
	}
}
