using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000372 RID: 882
	public class Thing : Entity, IExposable, ISelectable, ILoadReferenceable, ISignalReceiver
	{
		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x060018FC RID: 6396 RVA: 0x000938E1 File Offset: 0x00091AE1
		// (set) Token: 0x060018FD RID: 6397 RVA: 0x000938E9 File Offset: 0x00091AE9
		public virtual int HitPoints
		{
			get
			{
				return this.hitPointsInt;
			}
			set
			{
				this.hitPointsInt = value;
			}
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x060018FE RID: 6398 RVA: 0x000938F2 File Offset: 0x00091AF2
		public int MaxHitPoints
		{
			get
			{
				return Mathf.RoundToInt(this.GetStatValue(StatDefOf.MaxHitPoints, true));
			}
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x060018FF RID: 6399 RVA: 0x00093905 File Offset: 0x00091B05
		public float MarketValue
		{
			get
			{
				return this.GetStatValue(StatDefOf.MarketValue, true);
			}
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x06001900 RID: 6400 RVA: 0x00093913 File Offset: 0x00091B13
		public virtual float RoyalFavorValue
		{
			get
			{
				return this.GetStatValue(StatDefOf.RoyalFavorValue, true);
			}
		}

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x06001901 RID: 6401 RVA: 0x00093921 File Offset: 0x00091B21
		// (set) Token: 0x06001902 RID: 6402 RVA: 0x00093929 File Offset: 0x00091B29
		public bool EverSeenByPlayer
		{
			get
			{
				return this.GetEverSeenByPlayer();
			}
			set
			{
				this.SetEverSeenByPlayer(value);
			}
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06001903 RID: 6403 RVA: 0x00093932 File Offset: 0x00091B32
		// (set) Token: 0x06001904 RID: 6404 RVA: 0x0009393A File Offset: 0x00091B3A
		public ThingStyleDef StyleDef
		{
			get
			{
				return this.GetStyleDef();
			}
			set
			{
				this.styleGraphicInt = null;
				this.SetStyleDef(value);
			}
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06001905 RID: 6405 RVA: 0x0009394A File Offset: 0x00091B4A
		// (set) Token: 0x06001906 RID: 6406 RVA: 0x00093952 File Offset: 0x00091B52
		public Precept_ThingStyle StyleSourcePrecept
		{
			get
			{
				return this.GetStyleSourcePrecept();
			}
			set
			{
				this.SetStyleSourcePrecept(value);
			}
		}

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x06001907 RID: 6407 RVA: 0x0009395C File Offset: 0x00091B5C
		public bool FlammableNow
		{
			get
			{
				if (this.GetStatValue(StatDefOf.Flammability, true) < 0.01f)
				{
					return false;
				}
				if (this.Spawned && !this.FireBulwark)
				{
					List<Thing> thingList = this.Position.GetThingList(this.Map);
					if (thingList != null)
					{
						for (int i = 0; i < thingList.Count; i++)
						{
							if (thingList[i].FireBulwark)
							{
								return false;
							}
						}
					}
				}
				return true;
			}
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06001908 RID: 6408 RVA: 0x000939C5 File Offset: 0x00091BC5
		public virtual bool FireBulwark
		{
			get
			{
				return this.def.Fillage == FillCategory.Full;
			}
		}

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06001909 RID: 6409 RVA: 0x000939D5 File Offset: 0x00091BD5
		public bool Destroyed
		{
			get
			{
				return this.mapIndexOrState == -2 || this.mapIndexOrState == -3;
			}
		}

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x0600190A RID: 6410 RVA: 0x000939ED File Offset: 0x00091BED
		public bool Discarded
		{
			get
			{
				return this.mapIndexOrState == -3;
			}
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x0600190B RID: 6411 RVA: 0x000939F9 File Offset: 0x00091BF9
		public bool Spawned
		{
			get
			{
				if (this.mapIndexOrState < 0)
				{
					return false;
				}
				if ((int)this.mapIndexOrState < Find.Maps.Count)
				{
					return true;
				}
				Log.ErrorOnce("Thing is associated with invalid map index", 64664487);
				return false;
			}
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x0600190C RID: 6412 RVA: 0x00093A2A File Offset: 0x00091C2A
		public bool SpawnedOrAnyParentSpawned
		{
			get
			{
				return this.SpawnedParentOrMe != null;
			}
		}

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x0600190D RID: 6413 RVA: 0x00093A35 File Offset: 0x00091C35
		public Thing SpawnedParentOrMe
		{
			get
			{
				if (this.Spawned)
				{
					return this;
				}
				if (this.ParentHolder != null)
				{
					return ThingOwnerUtility.SpawnedParentOrMe(this.ParentHolder);
				}
				return null;
			}
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x0600190E RID: 6414 RVA: 0x00093A56 File Offset: 0x00091C56
		public Map Map
		{
			get
			{
				if (this.mapIndexOrState >= 0)
				{
					return Find.Maps[(int)this.mapIndexOrState];
				}
				return null;
			}
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x0600190F RID: 6415 RVA: 0x00093A73 File Offset: 0x00091C73
		public Map MapHeld
		{
			get
			{
				if (this.Spawned)
				{
					return this.Map;
				}
				if (this.ParentHolder != null)
				{
					return ThingOwnerUtility.GetRootMap(this.ParentHolder);
				}
				return null;
			}
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06001910 RID: 6416 RVA: 0x00093A99 File Offset: 0x00091C99
		// (set) Token: 0x06001911 RID: 6417 RVA: 0x00093AA4 File Offset: 0x00091CA4
		public IntVec3 Position
		{
			get
			{
				return this.positionInt;
			}
			set
			{
				if (value == this.positionInt)
				{
					return;
				}
				if (this.Spawned)
				{
					if (this.def.AffectsRegions)
					{
						Log.Warning("Changed position of a spawned thing which affects regions. This is not supported.");
					}
					this.DirtyMapMesh(this.Map);
					RegionListersUpdater.DeregisterInRegions(this, this.Map);
					this.Map.thingGrid.Deregister(this, false);
				}
				this.positionInt = value;
				if (this.Spawned)
				{
					this.Map.thingGrid.Register(this);
					RegionListersUpdater.RegisterInRegions(this, this.Map);
					this.DirtyMapMesh(this.Map);
					if (this.def.AffectsReachability)
					{
						this.Map.reachability.ClearCache();
					}
				}
			}
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06001912 RID: 6418 RVA: 0x00093B60 File Offset: 0x00091D60
		public IntVec3 PositionHeld
		{
			get
			{
				if (this.Spawned)
				{
					return this.Position;
				}
				IntVec3 rootPosition = ThingOwnerUtility.GetRootPosition(this.ParentHolder);
				if (rootPosition.IsValid)
				{
					return rootPosition;
				}
				return this.Position;
			}
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06001913 RID: 6419 RVA: 0x00093B99 File Offset: 0x00091D99
		// (set) Token: 0x06001914 RID: 6420 RVA: 0x00093BA4 File Offset: 0x00091DA4
		public Rot4 Rotation
		{
			get
			{
				return this.rotationInt;
			}
			set
			{
				if (value == this.rotationInt)
				{
					return;
				}
				if (this.Spawned && (this.def.size.x != 1 || this.def.size.z != 1))
				{
					if (this.def.AffectsRegions)
					{
						Log.Warning("Changed rotation of a spawned non-single-cell thing which affects regions. This is not supported.");
					}
					RegionListersUpdater.DeregisterInRegions(this, this.Map);
					this.Map.thingGrid.Deregister(this, false);
				}
				this.rotationInt = value;
				if (this.Spawned && (this.def.size.x != 1 || this.def.size.z != 1))
				{
					this.Map.thingGrid.Register(this);
					RegionListersUpdater.RegisterInRegions(this, this.Map);
					if (this.def.AffectsReachability)
					{
						this.Map.reachability.ClearCache();
					}
				}
			}
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06001915 RID: 6421 RVA: 0x00093C92 File Offset: 0x00091E92
		public bool Smeltable
		{
			get
			{
				return this.def.smeltable && (!this.def.MadeFromStuff || this.Stuff.smeltable);
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06001916 RID: 6422 RVA: 0x00093CBD File Offset: 0x00091EBD
		public bool BurnableByRecipe
		{
			get
			{
				return this.def.burnableByRecipe && (!this.def.MadeFromStuff || this.Stuff.burnableByRecipe);
			}
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06001917 RID: 6423 RVA: 0x00093CE8 File Offset: 0x00091EE8
		public IThingHolder ParentHolder
		{
			get
			{
				if (this.holdingOwner == null)
				{
					return null;
				}
				return this.holdingOwner.Owner;
			}
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06001918 RID: 6424 RVA: 0x00093CFF File Offset: 0x00091EFF
		public Faction Faction
		{
			get
			{
				return this.factionInt;
			}
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06001919 RID: 6425 RVA: 0x00093D07 File Offset: 0x00091F07
		// (set) Token: 0x0600191A RID: 6426 RVA: 0x00093D3D File Offset: 0x00091F3D
		public string ThingID
		{
			get
			{
				if (this.def.HasThingIDNumber)
				{
					return this.def.defName + this.thingIDNumber.ToString();
				}
				return this.def.defName;
			}
			set
			{
				this.thingIDNumber = Thing.IDNumberFromThingID(value);
			}
		}

		// Token: 0x0600191B RID: 6427 RVA: 0x00093D4C File Offset: 0x00091F4C
		public static int IDNumberFromThingID(string thingID)
		{
			string value = Regex.Match(thingID, "\\d+$").Value;
			int result = 0;
			try
			{
				CultureInfo invariantCulture = CultureInfo.InvariantCulture;
				result = Convert.ToInt32(value, invariantCulture);
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new string[]
				{
					"Could not convert id number from thingID=",
					thingID,
					", numString=",
					value,
					" Exception=",
					ex.ToString()
				}));
			}
			return result;
		}

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x0600191C RID: 6428 RVA: 0x00093DCC File Offset: 0x00091FCC
		public IntVec2 RotatedSize
		{
			get
			{
				if (!this.rotationInt.IsHorizontal)
				{
					return this.def.size;
				}
				return new IntVec2(this.def.size.z, this.def.size.x);
			}
		}

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x0600191D RID: 6429 RVA: 0x00093E0C File Offset: 0x0009200C
		public virtual CellRect? CustomRectForSelector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x0600191E RID: 6430 RVA: 0x00093E22 File Offset: 0x00092022
		public override string Label
		{
			get
			{
				if (this.stackCount > 1)
				{
					return this.LabelNoCount + " x" + this.stackCount.ToStringCached();
				}
				return this.LabelNoCount;
			}
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x0600191F RID: 6431 RVA: 0x00093E4F File Offset: 0x0009204F
		public virtual string LabelNoCount
		{
			get
			{
				return GenLabel.ThingLabel(this, 1, true);
			}
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06001920 RID: 6432 RVA: 0x00093E59 File Offset: 0x00092059
		public override string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.def);
			}
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06001921 RID: 6433 RVA: 0x00093E6C File Offset: 0x0009206C
		public virtual string LabelCapNoCount
		{
			get
			{
				return this.LabelNoCount.CapitalizeFirst(this.def);
			}
		}

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06001922 RID: 6434 RVA: 0x00093E7F File Offset: 0x0009207F
		public override string LabelShort
		{
			get
			{
				return this.LabelNoCount;
			}
		}

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06001923 RID: 6435 RVA: 0x00093E87 File Offset: 0x00092087
		public virtual bool IngestibleNow
		{
			get
			{
				return !this.IsBurning() && this.def.IsIngestible;
			}
		}

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06001924 RID: 6436 RVA: 0x00093E9E File Offset: 0x0009209E
		public ThingDef Stuff
		{
			get
			{
				return this.stuffInt;
			}
		}

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06001925 RID: 6437 RVA: 0x00093EA6 File Offset: 0x000920A6
		public Graphic DefaultGraphic
		{
			get
			{
				if (this.graphicInt == null)
				{
					if (this.def.graphicData == null)
					{
						return BaseContent.BadGraphic;
					}
					this.graphicInt = this.def.graphicData.GraphicColoredFor(this);
				}
				return this.graphicInt;
			}
		}

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06001926 RID: 6438 RVA: 0x00093EE0 File Offset: 0x000920E0
		public virtual Graphic Graphic
		{
			get
			{
				ThingStyleDef styleDef = this.StyleDef;
				if (styleDef != null && styleDef.Graphic != null)
				{
					if (this.styleGraphicInt == null)
					{
						if (styleDef.graphicData != null)
						{
							this.styleGraphicInt = styleDef.graphicData.GraphicColoredFor(this);
						}
						else
						{
							this.styleGraphicInt = styleDef.Graphic;
						}
					}
					return this.styleGraphicInt;
				}
				return this.DefaultGraphic;
			}
		}

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06001927 RID: 6439 RVA: 0x00093F3C File Offset: 0x0009213C
		public virtual IntVec3 InteractionCell
		{
			get
			{
				return ThingUtility.InteractionCellWhenAt(this.def, this.Position, this.Rotation, this.Map);
			}
		}

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06001928 RID: 6440 RVA: 0x00093F5C File Offset: 0x0009215C
		public float AmbientTemperature
		{
			get
			{
				if (this.Spawned)
				{
					return GenTemperature.GetTemperatureForCell(this.Position, this.Map);
				}
				if (this.ParentHolder != null)
				{
					for (IThingHolder parentHolder = this.ParentHolder; parentHolder != null; parentHolder = parentHolder.ParentHolder)
					{
						float result;
						if (ThingOwnerUtility.TryGetFixedTemperature(parentHolder, this, out result))
						{
							return result;
						}
					}
				}
				if (this.SpawnedOrAnyParentSpawned)
				{
					return GenTemperature.GetTemperatureForCell(this.PositionHeld, this.MapHeld);
				}
				if (this.Tile >= 0)
				{
					return GenTemperature.GetTemperatureAtTile(this.Tile);
				}
				return 21f;
			}
		}

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06001929 RID: 6441 RVA: 0x00093FDF File Offset: 0x000921DF
		public int Tile
		{
			get
			{
				if (this.Spawned)
				{
					return this.Map.Tile;
				}
				if (this.ParentHolder != null)
				{
					return ThingOwnerUtility.GetRootTile(this.ParentHolder);
				}
				return -1;
			}
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x0600192A RID: 6442 RVA: 0x0009400A File Offset: 0x0009220A
		public bool Suspended
		{
			get
			{
				return !this.Spawned && this.ParentHolder != null && ThingOwnerUtility.ContentsSuspended(this.ParentHolder);
			}
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x0600192B RID: 6443 RVA: 0x0009402B File Offset: 0x0009222B
		public virtual string DescriptionDetailed
		{
			get
			{
				return this.def.DescriptionDetailed;
			}
		}

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x0600192C RID: 6444 RVA: 0x00094038 File Offset: 0x00092238
		public virtual string DescriptionFlavor
		{
			get
			{
				return this.def.description;
			}
		}

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x0600192D RID: 6445 RVA: 0x00094045 File Offset: 0x00092245
		public TerrainAffordanceDef TerrainAffordanceNeeded
		{
			get
			{
				return this.def.GetTerrainAffordanceNeed(this.stuffInt);
			}
		}

		// Token: 0x0600192F RID: 6447 RVA: 0x00094092 File Offset: 0x00092292
		public virtual void PostMake()
		{
			ThingIDMaker.GiveIDTo(this);
			if (this.def.useHitPoints)
			{
				this.HitPoints = Mathf.RoundToInt((float)this.MaxHitPoints * Mathf.Clamp01(this.def.startingHpRange.RandomInRange));
			}
		}

		// Token: 0x06001930 RID: 6448 RVA: 0x000940D0 File Offset: 0x000922D0
		public virtual void PostPostMake()
		{
			if (!this.def.randomStyle.NullOrEmpty<ThingStyleChance>() && Rand.Chance(this.def.randomStyleChance))
			{
				this.StyleDef = this.def.randomStyle.RandomElementByWeight((ThingStyleChance x) => x.Chance).StyleDef;
			}
		}

		// Token: 0x06001931 RID: 6449 RVA: 0x0009413B File Offset: 0x0009233B
		public string GetUniqueLoadID()
		{
			return "Thing_" + this.ThingID;
		}

		// Token: 0x06001932 RID: 6450 RVA: 0x00094150 File Offset: 0x00092350
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (this.Destroyed)
			{
				Log.Error(string.Concat(new object[]
				{
					"Spawning destroyed thing ",
					this,
					" at ",
					this.Position,
					". Correcting."
				}));
				this.mapIndexOrState = -1;
				if (this.HitPoints <= 0 && this.def.useHitPoints)
				{
					this.HitPoints = 1;
				}
			}
			if (this.Spawned)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to spawn already-spawned thing ",
					this,
					" at ",
					this.Position
				}));
				return;
			}
			int num = Find.Maps.IndexOf(map);
			if (num < 0)
			{
				Log.Error("Tried to spawn thing " + this + ", but the map provided does not exist.");
				return;
			}
			if (this.stackCount > this.def.stackLimit)
			{
				Log.Error(string.Concat(new object[]
				{
					"Spawned ",
					this,
					" with stackCount ",
					this.stackCount,
					" but stackLimit is ",
					this.def.stackLimit,
					". Truncating."
				}));
				this.stackCount = this.def.stackLimit;
			}
			this.mapIndexOrState = (sbyte)num;
			RegionListersUpdater.RegisterInRegions(this, map);
			if (!map.spawnedThings.TryAdd(this, false))
			{
				Log.Error("Couldn't add thing " + this + " to spawned things.");
			}
			map.listerThings.Add(this);
			map.thingGrid.Register(this);
			if (map.IsPlayerHome)
			{
				this.EverSeenByPlayer = true;
			}
			if (Find.TickManager != null)
			{
				Find.TickManager.RegisterAllTickabilityFor(this);
			}
			this.DirtyMapMesh(map);
			if (this.def.drawerType != DrawerType.MapMeshOnly)
			{
				map.dynamicDrawManager.RegisterDrawable(this);
			}
			map.tooltipGiverList.Notify_ThingSpawned(this);
			if (this.def.CanAffectLinker)
			{
				map.linkGrid.Notify_LinkerCreatedOrDestroyed(this);
				map.mapDrawer.MapMeshDirty(this.Position, MapMeshFlag.Things, true, false);
			}
			if (!this.def.CanOverlapZones)
			{
				map.zoneManager.Notify_NoZoneOverlapThingSpawned(this);
			}
			if (this.def.AffectsRegions)
			{
				map.regionDirtyer.Notify_ThingAffectingRegionsSpawned(this);
			}
			if (this.def.pathCost != 0 || this.def.passability == Traversability.Impassable)
			{
				map.pathing.RecalculatePerceivedPathCostUnderThing(this);
			}
			if (this.def.AffectsReachability)
			{
				map.reachability.ClearCache();
			}
			map.coverGrid.Register(this);
			if (this.def.category == ThingCategory.Item)
			{
				map.listerHaulables.Notify_Spawned(this);
				map.listerMergeables.Notify_Spawned(this);
			}
			map.attackTargetsCache.Notify_ThingSpawned(this);
			Region validRegionAt_NoRebuild = map.regionGrid.GetValidRegionAt_NoRebuild(this.Position);
			Room room = (validRegionAt_NoRebuild == null) ? null : validRegionAt_NoRebuild.Room;
			if (room != null)
			{
				room.Notify_ContainedThingSpawnedOrDespawned(this);
			}
			StealAIDebugDrawer.Notify_ThingChanged(this);
			IHaulDestination haulDestination = this as IHaulDestination;
			if (haulDestination != null)
			{
				map.haulDestinationManager.AddHaulDestination(haulDestination);
			}
			if (this is IThingHolder && Find.ColonistBar != null)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
			if (this.def.category == ThingCategory.Item)
			{
				SlotGroup slotGroup = this.Position.GetSlotGroup(map);
				if (slotGroup != null && slotGroup.parent != null)
				{
					slotGroup.parent.Notify_ReceivedThing(this);
				}
			}
			if (this.def.receivesSignals)
			{
				Find.SignalManager.RegisterReceiver(this);
			}
			if (!respawningAfterLoad)
			{
				QuestUtility.SendQuestTargetSignals(this.questTags, "Spawned", this.Named("SUBJECT"));
			}
		}

		// Token: 0x06001933 RID: 6451 RVA: 0x000944DC File Offset: 0x000926DC
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.Destroyed)
			{
				Log.Error("Tried to despawn " + this.ToStringSafe<Thing>() + " which is already destroyed.");
				return;
			}
			if (!this.Spawned)
			{
				Log.Error("Tried to despawn " + this.ToStringSafe<Thing>() + " which is not spawned.");
				return;
			}
			Map map = this.Map;
			map.overlayDrawer.DisposeHandle(this);
			RegionListersUpdater.DeregisterInRegions(this, map);
			map.spawnedThings.Remove(this);
			map.listerThings.Remove(this);
			map.thingGrid.Deregister(this, false);
			map.coverGrid.DeRegister(this);
			if (this.def.receivesSignals)
			{
				Find.SignalManager.DeregisterReceiver(this);
			}
			map.tooltipGiverList.Notify_ThingDespawned(this);
			if (this.def.CanAffectLinker)
			{
				map.linkGrid.Notify_LinkerCreatedOrDestroyed(this);
				map.mapDrawer.MapMeshDirty(this.Position, MapMeshFlag.Things, true, false);
			}
			if (Find.Selector.IsSelected(this))
			{
				Find.Selector.Deselect(this);
				Find.MainButtonsRoot.tabs.Notify_SelectedObjectDespawned();
			}
			this.DirtyMapMesh(map);
			if (this.def.drawerType != DrawerType.MapMeshOnly)
			{
				map.dynamicDrawManager.DeRegisterDrawable(this);
			}
			Region validRegionAt_NoRebuild = map.regionGrid.GetValidRegionAt_NoRebuild(this.Position);
			Room room = (validRegionAt_NoRebuild == null) ? null : validRegionAt_NoRebuild.Room;
			if (room != null)
			{
				room.Notify_ContainedThingSpawnedOrDespawned(this);
			}
			if (this.def.AffectsRegions)
			{
				map.regionDirtyer.Notify_ThingAffectingRegionsDespawned(this);
			}
			if (this.def.pathCost != 0 || this.def.passability == Traversability.Impassable)
			{
				map.pathing.RecalculatePerceivedPathCostUnderThing(this);
			}
			if (this.def.AffectsReachability)
			{
				map.reachability.ClearCache();
			}
			Find.TickManager.DeRegisterAllTickabilityFor(this);
			this.mapIndexOrState = -1;
			if (this.def.category == ThingCategory.Item)
			{
				map.listerHaulables.Notify_DeSpawned(this);
				map.listerMergeables.Notify_DeSpawned(this);
			}
			map.attackTargetsCache.Notify_ThingDespawned(this);
			map.physicalInteractionReservationManager.ReleaseAllForTarget(this);
			StealAIDebugDrawer.Notify_ThingChanged(this);
			IHaulDestination haulDestination = this as IHaulDestination;
			if (haulDestination != null)
			{
				map.haulDestinationManager.RemoveHaulDestination(haulDestination);
			}
			if (this is IThingHolder && Find.ColonistBar != null)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
			if (this.def.category == ThingCategory.Item)
			{
				SlotGroup slotGroup = this.Position.GetSlotGroup(map);
				if (slotGroup != null && slotGroup.parent != null)
				{
					slotGroup.parent.Notify_LostThing(this);
				}
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "Despawned", this.Named("SUBJECT"));
		}

		// Token: 0x06001934 RID: 6452 RVA: 0x00094770 File Offset: 0x00092970
		public virtual void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
		{
			this.Destroy(DestroyMode.KillFinalize);
		}

		// Token: 0x06001935 RID: 6453 RVA: 0x0009477C File Offset: 0x0009297C
		public virtual void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (!Thing.allowDestroyNonDestroyable && !this.def.destroyable)
			{
				Log.Error("Tried to destroy non-destroyable thing " + this);
				return;
			}
			if (this.Destroyed)
			{
				Log.Error("Tried to destroy already-destroyed thing " + this);
				return;
			}
			bool spawned = this.Spawned;
			Map map = this.Map;
			if (this.StyleSourcePrecept != null)
			{
				this.StyleSourcePrecept.Notify_ThingLost(this, spawned);
			}
			if (this.Spawned)
			{
				this.DeSpawn(mode);
			}
			this.mapIndexOrState = -2;
			if (this.def.DiscardOnDestroyed)
			{
				this.Discard(false);
			}
			CompExplosive compExplosive = this.TryGetComp<CompExplosive>();
			if (spawned)
			{
				List<Thing> list = (compExplosive != null) ? new List<Thing>() : null;
				GenLeaving.DoLeavingsFor(this, map, mode, list);
				if (compExplosive != null)
				{
					compExplosive.AddThingsIgnoredByExplosion(list);
				}
			}
			if (this.holdingOwner != null)
			{
				this.holdingOwner.Notify_ContainedItemDestroyed(this);
			}
			this.RemoveAllReservationsAndDesignationsOnThis();
			if (!(this is Pawn))
			{
				this.stackCount = 0;
			}
			if (mode != DestroyMode.QuestLogic)
			{
				QuestUtility.SendQuestTargetSignals(this.questTags, "Destroyed", this.Named("SUBJECT"));
			}
			if (mode == DestroyMode.KillFinalize)
			{
				QuestUtility.SendQuestTargetSignals(this.questTags, "Killed", this.Named("SUBJECT"));
			}
		}

		// Token: 0x06001936 RID: 6454 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
		}

		// Token: 0x06001937 RID: 6455 RVA: 0x000948A5 File Offset: 0x00092AA5
		public virtual void PostGeneratedForTrader(TraderKindDef trader, int forTile, Faction forFaction)
		{
			if (this.def.colorGeneratorInTraderStock != null)
			{
				this.SetColor(this.def.colorGeneratorInTraderStock.NewRandomizedColor(), true);
			}
		}

		// Token: 0x06001938 RID: 6456 RVA: 0x000948CC File Offset: 0x00092ACC
		public virtual void Notify_MyMapRemoved()
		{
			if (this.def.receivesSignals)
			{
				Find.SignalManager.DeregisterReceiver(this);
			}
			if (this.StyleSourcePrecept != null)
			{
				this.StyleSourcePrecept.Notify_ThingLost(this, false);
			}
			if (!ThingOwnerUtility.AnyParentIs<Pawn>(this))
			{
				this.mapIndexOrState = -3;
			}
			this.RemoveAllReservationsAndDesignationsOnThis();
		}

		// Token: 0x06001939 RID: 6457 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_LordDestroyed()
		{
		}

		// Token: 0x0600193A RID: 6458 RVA: 0x0009491C File Offset: 0x00092B1C
		public void ForceSetStateToUnspawned()
		{
			this.mapIndexOrState = -1;
		}

		// Token: 0x0600193B RID: 6459 RVA: 0x00094928 File Offset: 0x00092B28
		public void DecrementMapIndex()
		{
			if (this.mapIndexOrState <= 0)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to decrement map index for ",
					this,
					", but mapIndexOrState=",
					this.mapIndexOrState
				}));
				return;
			}
			this.mapIndexOrState -= 1;
		}

		// Token: 0x0600193C RID: 6460 RVA: 0x00094980 File Offset: 0x00092B80
		private void RemoveAllReservationsAndDesignationsOnThis()
		{
			if (this.def.category == ThingCategory.Mote)
			{
				return;
			}
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				maps[i].reservationManager.ReleaseAllForTarget(this);
				maps[i].physicalInteractionReservationManager.ReleaseAllForTarget(this);
				IAttackTarget attackTarget = this as IAttackTarget;
				if (attackTarget != null)
				{
					maps[i].attackTargetReservationManager.ReleaseAllForTarget(attackTarget);
				}
				maps[i].designationManager.RemoveAllDesignationsOn(this, false);
			}
		}

		// Token: 0x0600193D RID: 6461 RVA: 0x00094A0C File Offset: 0x00092C0C
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.def, "def");
			if (this.def.HasThingIDNumber)
			{
				string thingID = this.ThingID;
				Scribe_Values.Look<string>(ref thingID, "id", null, false);
				this.ThingID = thingID;
			}
			Scribe_Values.Look<sbyte>(ref this.mapIndexOrState, "map", -1, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.mapIndexOrState >= 0)
			{
				this.mapIndexOrState = -1;
			}
			Scribe_Values.Look<IntVec3>(ref this.positionInt, "pos", IntVec3.Invalid, false);
			Scribe_Values.Look<Rot4>(ref this.rotationInt, "rot", Rot4.North, false);
			if (this.def.useHitPoints)
			{
				Scribe_Values.Look<int>(ref this.hitPointsInt, "health", -1, false);
			}
			bool flag = this.def.tradeability != Tradeability.None && this.def.category == ThingCategory.Item;
			if (this.def.stackLimit > 1 || flag)
			{
				Scribe_Values.Look<int>(ref this.stackCount, "stackCount", 0, true);
			}
			Scribe_Defs.Look<ThingDef>(ref this.stuffInt, "stuff");
			string facID = (this.factionInt != null) ? this.factionInt.GetUniqueLoadID() : "null";
			Scribe_Values.Look<string>(ref facID, "faction", "null", false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (facID == "null")
				{
					this.factionInt = null;
				}
				else if (Find.World != null && Find.FactionManager != null)
				{
					this.factionInt = Find.FactionManager.AllFactions.FirstOrDefault((Faction fa) => fa.GetUniqueLoadID() == facID);
				}
				else
				{
					Thing.facIDsCached.SetOrAdd(this, facID);
				}
			}
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				if (facID == "null" && Thing.facIDsCached.TryGetValue(this, out facID))
				{
					Thing.facIDsCached.Remove(this);
				}
				if (facID != "null")
				{
					this.factionInt = Find.FactionManager.AllFactions.FirstOrDefault((Faction fa) => fa.GetUniqueLoadID() == facID);
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Thing.facIDsCached.Clear();
			}
			Scribe_Collections.Look<string>(ref this.questTags, "questTags", LookMode.Value, Array.Empty<object>());
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0600193E RID: 6462 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostMapInit()
		{
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x0600193F RID: 6463 RVA: 0x00094C55 File Offset: 0x00092E55
		public virtual Vector3 DrawPos
		{
			get
			{
				return this.TrueCenter();
			}
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06001940 RID: 6464 RVA: 0x00094C5D File Offset: 0x00092E5D
		// (set) Token: 0x06001941 RID: 6465 RVA: 0x00094C9C File Offset: 0x00092E9C
		public virtual Color DrawColor
		{
			get
			{
				if (this.Stuff != null)
				{
					return this.def.GetColorForStuff(this.Stuff);
				}
				if (this.def.graphicData != null)
				{
					return this.def.graphicData.color;
				}
				return Color.white;
			}
			set
			{
				Log.Error(string.Concat(new object[]
				{
					"Cannot set instance color on non-ThingWithComps ",
					this.LabelCap,
					" at ",
					this.Position,
					"."
				}));
			}
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x06001942 RID: 6466 RVA: 0x00094CE8 File Offset: 0x00092EE8
		public virtual Color DrawColorTwo
		{
			get
			{
				if (this.def.graphicData != null)
				{
					return this.def.graphicData.colorTwo;
				}
				return Color.white;
			}
		}

		// Token: 0x06001943 RID: 6467 RVA: 0x00094D0D File Offset: 0x00092F0D
		public virtual void Draw()
		{
			if (this.def.drawerType == DrawerType.RealtimeOnly)
			{
				this.DrawAt(this.DrawPos, false);
			}
		}

		// Token: 0x06001944 RID: 6468 RVA: 0x00094D2C File Offset: 0x00092F2C
		public virtual void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.Graphic.Draw(drawLoc, flip ? this.Rotation.Opposite : this.Rotation, this, 0f);
		}

		// Token: 0x06001945 RID: 6469 RVA: 0x00094D64 File Offset: 0x00092F64
		public virtual void Print(SectionLayer layer)
		{
			this.Graphic.Print(layer, this, 0f);
		}

		// Token: 0x06001946 RID: 6470 RVA: 0x00094D78 File Offset: 0x00092F78
		public void DirtyMapMesh(Map map)
		{
			if (this.def.drawerType != DrawerType.RealtimeOnly)
			{
				foreach (IntVec3 loc in this.OccupiedRect())
				{
					map.mapDrawer.MapMeshDirty(loc, MapMeshFlag.Things);
				}
			}
		}

		// Token: 0x06001947 RID: 6471 RVA: 0x00094DE4 File Offset: 0x00092FE4
		public virtual void DrawGUIOverlay()
		{
			if (Find.CameraDriver.CurrentZoom == CameraZoomRange.Closest)
			{
				if (this.def.stackLimit > 1)
				{
					GenMapUI.DrawThingLabel(this, this.stackCount.ToStringCached());
					return;
				}
				QualityCategory cat;
				if (this.def.drawGUIOverlayQuality && this.TryGetQuality(out cat))
				{
					GenMapUI.DrawThingLabel(this, cat.GetLabelShort());
				}
			}
		}

		// Token: 0x06001948 RID: 6472 RVA: 0x00094E40 File Offset: 0x00093040
		public virtual void DrawExtraSelectionOverlays()
		{
			if (this.def.specialDisplayRadius > 0.1f)
			{
				GenDraw.DrawRadiusRing(this.Position, this.def.specialDisplayRadius);
			}
			if (this.def.drawPlaceWorkersWhileSelected && this.def.PlaceWorkers != null)
			{
				for (int i = 0; i < this.def.PlaceWorkers.Count; i++)
				{
					this.def.PlaceWorkers[i].DrawGhost(this.def, this.Position, this.Rotation, Color.white, this);
				}
			}
			if (this.def.hasInteractionCell)
			{
				GenDraw.DrawInteractionCell(this.def, this.Position, this.rotationInt);
			}
		}

		// Token: 0x06001949 RID: 6473 RVA: 0x00094EFC File Offset: 0x000930FC
		public virtual string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			QuestUtility.AppendInspectStringsFromQuestParts(stringBuilder, this);
			return stringBuilder.ToString();
		}

		// Token: 0x0600194A RID: 6474 RVA: 0x00094F10 File Offset: 0x00093110
		public virtual string GetInspectStringLowPriority()
		{
			string result = null;
			Thing.tmpDeteriorationReasons.Clear();
			SteadyEnvironmentEffects.FinalDeteriorationRate(this, Thing.tmpDeteriorationReasons);
			if (Thing.tmpDeteriorationReasons.Count != 0)
			{
				result = string.Format("{0}: {1}", "DeterioratingBecauseOf".Translate(), Thing.tmpDeteriorationReasons.ToCommaList(false, false).CapitalizeFirst());
			}
			return result;
		}

		// Token: 0x0600194B RID: 6475 RVA: 0x00094F6D File Offset: 0x0009316D
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			if (ModsConfig.IdeologyActive)
			{
				foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
				{
					int num;
					for (int i = 0; i < ideo.PreceptsListForReading.Count; i = num + 1)
					{
						Precept_Ritual precept_Ritual;
						if ((precept_Ritual = (ideo.PreceptsListForReading[i] as Precept_Ritual)) != null && precept_Ritual.ShouldShowGizmo(this))
						{
							foreach (Gizmo gizmo in precept_Ritual.GetGizmoFor(this))
							{
								yield return gizmo;
							}
							IEnumerator<Gizmo> enumerator2 = null;
						}
						num = i;
					}
					ideo = null;
				}
				IEnumerator<Ideo> enumerator = null;
				List<LordJob_Ritual> activeRituals = Find.IdeoManager.GetActiveRituals(this.MapHeld);
				foreach (LordJob_Ritual lordJob_Ritual in activeRituals)
				{
					if (lordJob_Ritual.selectedTarget == this)
					{
						yield return lordJob_Ritual.GetCancelGizmo();
					}
				}
				List<LordJob_Ritual>.Enumerator enumerator3 = default(List<LordJob_Ritual>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x0600194C RID: 6476 RVA: 0x00094F7D File Offset: 0x0009317D
		public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			Thing.<>c__DisplayClass144_0 CS$<>8__locals1 = new Thing.<>c__DisplayClass144_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.selPawn = selPawn;
			if (ModsConfig.IdeologyActive)
			{
				foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
				{
					int num;
					for (int i = 0; i < ideo.PreceptsListForReading.Count; i = num + 1)
					{
						Thing.<>c__DisplayClass144_1 CS$<>8__locals2 = new Thing.<>c__DisplayClass144_1();
						CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
						Precept precept = ideo.PreceptsListForReading[i];
						if ((CS$<>8__locals2.ritual = (precept as Precept_Ritual)) != null)
						{
							string disableReason = CS$<>8__locals2.ritual.behavior.CanStartRitualNow(this, CS$<>8__locals2.ritual, CS$<>8__locals2.CS$<>8__locals1.selPawn, null);
							if (!CS$<>8__locals2.ritual.activeObligations.NullOrEmpty<RitualObligation>())
							{
								using (List<RitualObligation>.Enumerator enumerator2 = CS$<>8__locals2.ritual.activeObligations.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										Thing.<>c__DisplayClass144_2 CS$<>8__locals3 = new Thing.<>c__DisplayClass144_2();
										CS$<>8__locals3.CS$<>8__locals2 = CS$<>8__locals2;
										CS$<>8__locals3.obligation = enumerator2.Current;
										if (CS$<>8__locals3.CS$<>8__locals2.ritual.CanUseTarget(this, CS$<>8__locals3.obligation).canUse)
										{
											string text = CS$<>8__locals3.CS$<>8__locals2.ritual.obligationTargetFilter.LabelExtraPart(CS$<>8__locals3.obligation);
											string text2;
											if (text.NullOrEmpty() || CS$<>8__locals3.CS$<>8__locals2.ritual.mergeGizmosForObligations)
											{
												text2 = "BeginRitual".Translate(CS$<>8__locals3.CS$<>8__locals2.ritual.Label);
											}
											else
											{
												text2 = "BeginRitualFor".Translate(CS$<>8__locals3.CS$<>8__locals2.ritual.Label, text);
											}
											bool disabled = !disableReason.NullOrEmpty();
											if (disabled)
											{
												text2 = text2 + " (" + disableReason + ")";
											}
											Action action = delegate()
											{
												CS$<>8__locals3.CS$<>8__locals2.ritual.ShowRitualBeginWindow(CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this, CS$<>8__locals3.obligation, CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.selPawn);
											};
											yield return new FloatMenuOption(text2, (!disabled) ? action : null, CS$<>8__locals3.CS$<>8__locals2.ritual.Icon, ideo.Color, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
											if (!disabled && CS$<>8__locals3.CS$<>8__locals2.ritual.mergeGizmosForObligations)
											{
												break;
											}
											CS$<>8__locals3 = null;
										}
									}
								}
								List<RitualObligation>.Enumerator enumerator2 = default(List<RitualObligation>.Enumerator);
							}
							else if (CS$<>8__locals2.ritual.isAnytime && CS$<>8__locals2.ritual.ShouldShowGizmo(this))
							{
								TaggedString taggedString = "BeginRitual".Translate(CS$<>8__locals2.ritual.Label);
								RitualTargetUseReport ritualTargetUseReport = CS$<>8__locals2.ritual.CanUseTarget(this, null);
								if (!disableReason.NullOrEmpty())
								{
									taggedString += " (" + disableReason + ")";
								}
								else if (!ritualTargetUseReport.failReason.NullOrEmpty())
								{
									taggedString += " (" + ritualTargetUseReport.failReason + ")";
								}
								Action action2 = delegate()
								{
									CS$<>8__locals2.ritual.ShowRitualBeginWindow(CS$<>8__locals2.CS$<>8__locals1.<>4__this, null, CS$<>8__locals2.CS$<>8__locals1.selPawn);
								};
								yield return new FloatMenuOption(taggedString, (disableReason.NullOrEmpty() && ritualTargetUseReport.failReason.NullOrEmpty()) ? action2 : null, CS$<>8__locals2.ritual.Icon, ideo.Color, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
							}
							disableReason = null;
						}
						CS$<>8__locals2 = null;
						num = i;
					}
					ideo = null;
				}
				IEnumerator<Ideo> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600194D RID: 6477 RVA: 0x00094F94 File Offset: 0x00093194
		public virtual IEnumerable<FloatMenuOption> GetMultiSelectFloatMenuOptions(List<Pawn> selPawns)
		{
			yield break;
		}

		// Token: 0x0600194E RID: 6478 RVA: 0x00094F9D File Offset: 0x0009319D
		public virtual IEnumerable<InspectTabBase> GetInspectTabs()
		{
			return this.def.inspectorTabsResolved;
		}

		// Token: 0x0600194F RID: 6479 RVA: 0x00094FAA File Offset: 0x000931AA
		public virtual string GetCustomLabelNoCount(bool includeHp = true)
		{
			return GenLabel.ThingLabel(this, 1, includeHp);
		}

		// Token: 0x06001950 RID: 6480 RVA: 0x00094FB4 File Offset: 0x000931B4
		public DamageWorker.DamageResult TakeDamage(DamageInfo dinfo)
		{
			if (this.Destroyed)
			{
				return new DamageWorker.DamageResult();
			}
			if (dinfo.Amount == 0f)
			{
				return new DamageWorker.DamageResult();
			}
			if (this.def.damageMultipliers != null)
			{
				for (int i = 0; i < this.def.damageMultipliers.Count; i++)
				{
					if (this.def.damageMultipliers[i].damageDef == dinfo.Def)
					{
						int num = Mathf.RoundToInt(dinfo.Amount * this.def.damageMultipliers[i].multiplier);
						dinfo.SetAmount((float)num);
					}
				}
			}
			bool flag;
			this.PreApplyDamage(ref dinfo, out flag);
			if (flag)
			{
				return new DamageWorker.DamageResult();
			}
			bool spawnedOrAnyParentSpawned = this.SpawnedOrAnyParentSpawned;
			Map mapHeld = this.MapHeld;
			DamageWorker.DamageResult damageResult = dinfo.Def.Worker.Apply(dinfo, this);
			if (dinfo.Def.harmsHealth && spawnedOrAnyParentSpawned)
			{
				mapHeld.damageWatcher.Notify_DamageTaken(this, damageResult.totalDamageDealt);
			}
			if (dinfo.Def.ExternalViolenceFor(this))
			{
				if (dinfo.SpawnFilth)
				{
					GenLeaving.DropFilthDueToDamage(this, damageResult.totalDamageDealt);
				}
				if (dinfo.Instigator != null)
				{
					Pawn pawn = dinfo.Instigator as Pawn;
					if (pawn != null)
					{
						pawn.records.AddTo(RecordDefOf.DamageDealt, damageResult.totalDamageDealt);
					}
				}
			}
			this.PostApplyDamage(dinfo, damageResult.totalDamageDealt);
			return damageResult;
		}

		// Token: 0x06001951 RID: 6481 RVA: 0x0009511A File Offset: 0x0009331A
		public virtual void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			absorbed = false;
		}

		// Token: 0x06001952 RID: 6482 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
		}

		// Token: 0x06001953 RID: 6483 RVA: 0x00095120 File Offset: 0x00093320
		public virtual bool CanStackWith(Thing other)
		{
			return !this.Destroyed && !other.Destroyed && this.def.category == ThingCategory.Item && this.def == other.def && this.Stuff == other.Stuff;
		}

		// Token: 0x06001954 RID: 6484 RVA: 0x00095170 File Offset: 0x00093370
		public virtual bool TryAbsorbStack(Thing other, bool respectStackLimit)
		{
			if (!this.CanStackWith(other))
			{
				return false;
			}
			int num = ThingUtility.TryAbsorbStackNumToTake(this, other, respectStackLimit);
			if (this.def.useHitPoints)
			{
				this.HitPoints = Mathf.CeilToInt((float)(this.HitPoints * this.stackCount + other.HitPoints * num) / (float)(this.stackCount + num));
			}
			this.stackCount += num;
			other.stackCount -= num;
			if (this.Map != null)
			{
				this.DirtyMapMesh(this.Map);
			}
			StealAIDebugDrawer.Notify_ThingChanged(this);
			if (this.Spawned)
			{
				this.Map.listerMergeables.Notify_ThingStackChanged(this);
			}
			if (other.stackCount <= 0)
			{
				other.Destroy(DestroyMode.Vanish);
				return true;
			}
			return false;
		}

		// Token: 0x06001955 RID: 6485 RVA: 0x0009522C File Offset: 0x0009342C
		public virtual Thing SplitOff(int count)
		{
			if (count <= 0)
			{
				throw new ArgumentException("SplitOff with count <= 0", "count");
			}
			if (count >= this.stackCount)
			{
				if (count > this.stackCount)
				{
					Log.Error(string.Concat(new object[]
					{
						"Tried to split off ",
						count,
						" of ",
						this,
						" but there are only ",
						this.stackCount
					}));
				}
				if (this.Spawned)
				{
					this.DeSpawn(DestroyMode.Vanish);
				}
				if (this.holdingOwner != null)
				{
					this.holdingOwner.Remove(this);
				}
				return this;
			}
			Thing thing = ThingMaker.MakeThing(this.def, this.Stuff);
			thing.stackCount = count;
			this.stackCount -= count;
			if (this.Map != null)
			{
				this.DirtyMapMesh(this.Map);
			}
			if (this.Spawned)
			{
				this.Map.listerMergeables.Notify_ThingStackChanged(this);
			}
			if (this.def.useHitPoints)
			{
				thing.HitPoints = this.HitPoints;
			}
			return thing;
		}

		// Token: 0x06001956 RID: 6486 RVA: 0x00095336 File Offset: 0x00093536
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (this.Stuff != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Stat_Stuff_Name".Translate(), this.Stuff.LabelCap, "Stat_Stuff_Desc".Translate(), 1100, null, new Dialog_InfoCard.Hyperlink[]
				{
					new Dialog_InfoCard.Hyperlink(this.Stuff, -1)
				}, false);
			}
			if (ModsConfig.IdeologyActive)
			{
				Thing.tmpIdeoNames.Clear();
				ThingStyleDef styleDef = this.StyleDef;
				StyleCategoryDef styleCategoryDef = ((styleDef != null) ? styleDef.Category : null) ?? this.def.dominantStyleCategory;
				if (styleCategoryDef != null)
				{
					foreach (Ideo ideo in Find.IdeoManager.IdeosListForReading)
					{
						if (IdeoUtility.ThingSatisfiesIdeo(this, ideo))
						{
							Thing.tmpIdeoNames.Add(ideo.name.Colorize(ideo.Color));
						}
					}
					yield return new StatDrawEntry(StatCategoryDefOf.BasicsNonPawn, "Stat_Thing_StyleDominanceCategory".Translate(), styleCategoryDef.LabelCap, "Stat_Thing_StyleDominanceCategoryDesc".Translate() + "\n\n" + "Stat_Thing_IdeosSatisfied".Translate() + ":" + "\n" + Thing.tmpIdeoNames.ToLineList("  - "), 6005, null, null, false);
				}
			}
			yield break;
		}

		// Token: 0x06001957 RID: 6487 RVA: 0x00095348 File Offset: 0x00093548
		public virtual void Notify_ColorChanged()
		{
			this.graphicInt = null;
			if (this.Spawned && (this.def.drawerType == DrawerType.MapMeshOnly || this.def.drawerType == DrawerType.MapMeshAndRealTime))
			{
				this.Map.mapDrawer.MapMeshDirty(this.Position, MapMeshFlag.Things);
			}
		}

		// Token: 0x06001958 RID: 6488 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_Equipped(Pawn pawn)
		{
		}

		// Token: 0x06001959 RID: 6489 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_Unequipped(Pawn pawn)
		{
		}

		// Token: 0x0600195A RID: 6490 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_UsedWeapon(Pawn pawn)
		{
		}

		// Token: 0x0600195B RID: 6491 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_SignalReceived(Signal signal)
		{
		}

		// Token: 0x0600195C RID: 6492 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_Explosion(Explosion explosion)
		{
		}

		// Token: 0x0600195D RID: 6493 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_BulletImpactNearby(BulletImpactData impactData)
		{
		}

		// Token: 0x0600195E RID: 6494 RVA: 0x00095398 File Offset: 0x00093598
		public virtual TipSignal GetTooltip()
		{
			string text = this.LabelCap;
			if (this.def.useHitPoints)
			{
				text = string.Concat(new object[]
				{
					text,
					"\n",
					this.HitPoints,
					" / ",
					this.MaxHitPoints
				});
			}
			return new TipSignal(text, this.thingIDNumber * 251235);
		}

		// Token: 0x0600195F RID: 6495 RVA: 0x00095407 File Offset: 0x00093607
		public virtual bool BlocksPawn(Pawn p)
		{
			return this.def.passability == Traversability.Impassable || (this.def.IsFence && p.def.race.FenceBlocked);
		}

		// Token: 0x06001960 RID: 6496 RVA: 0x0009543B File Offset: 0x0009363B
		public void SetFactionDirect(Faction newFaction)
		{
			if (!this.def.CanHaveFaction)
			{
				Log.Error("Tried to SetFactionDirect on " + this + " which cannot have a faction.");
				return;
			}
			this.factionInt = newFaction;
		}

		// Token: 0x06001961 RID: 6497 RVA: 0x00095468 File Offset: 0x00093668
		public virtual void SetFaction(Faction newFaction, Pawn recruiter = null)
		{
			if (!this.def.CanHaveFaction)
			{
				Log.Error("Tried to SetFaction on " + this + " which cannot have a faction.");
				return;
			}
			this.factionInt = newFaction;
			if (this.Spawned)
			{
				IAttackTarget attackTarget = this as IAttackTarget;
				if (attackTarget != null)
				{
					this.Map.attackTargetsCache.UpdateTarget(attackTarget);
				}
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "ChangedFaction", this.Named("SUBJECT"), newFaction.Named("FACTION"));
			if (newFaction != Faction.OfPlayer)
			{
				QuestUtility.SendQuestTargetSignals(this.questTags, "ChangedFactionToNonPlayer", this.Named("SUBJECT"), newFaction.Named("FACTION"));
			}
		}

		// Token: 0x06001962 RID: 6498 RVA: 0x00095516 File Offset: 0x00093716
		public void SetPositionDirect(IntVec3 newPos)
		{
			this.positionInt = newPos;
		}

		// Token: 0x06001963 RID: 6499 RVA: 0x0009551F File Offset: 0x0009371F
		public void SetStuffDirect(ThingDef newStuff)
		{
			this.stuffInt = newStuff;
		}

		// Token: 0x06001964 RID: 6500 RVA: 0x00095528 File Offset: 0x00093728
		public override string ToString()
		{
			if (this.def != null)
			{
				return this.ThingID;
			}
			return base.GetType().ToString();
		}

		// Token: 0x06001965 RID: 6501 RVA: 0x00095544 File Offset: 0x00093744
		public override int GetHashCode()
		{
			return this.thingIDNumber;
		}

		// Token: 0x06001966 RID: 6502 RVA: 0x0009554C File Offset: 0x0009374C
		public virtual void Discard(bool silentlyRemoveReferences = false)
		{
			if (this.mapIndexOrState != -2)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to discard ",
					this,
					" whose state is ",
					this.mapIndexOrState,
					"."
				}));
				return;
			}
			this.mapIndexOrState = -3;
		}

		// Token: 0x06001967 RID: 6503 RVA: 0x000955A6 File Offset: 0x000937A6
		public virtual IEnumerable<Thing> ButcherProducts(Pawn butcher, float efficiency)
		{
			if (this.def.butcherProducts != null)
			{
				int num2;
				for (int i = 0; i < this.def.butcherProducts.Count; i = num2 + 1)
				{
					ThingDefCountClass thingDefCountClass = this.def.butcherProducts[i];
					int num = GenMath.RoundRandom((float)thingDefCountClass.count * efficiency);
					if (num > 0)
					{
						Thing thing = ThingMaker.MakeThing(thingDefCountClass.thingDef, null);
						thing.stackCount = num;
						yield return thing;
					}
					num2 = i;
				}
			}
			yield break;
		}

		// Token: 0x06001968 RID: 6504 RVA: 0x000955BD File Offset: 0x000937BD
		public virtual IEnumerable<Thing> SmeltProducts(float efficiency)
		{
			List<ThingDefCountClass> costListAdj = this.def.CostListAdjusted(this.Stuff, true);
			int num2;
			for (int i = 0; i < costListAdj.Count; i = num2 + 1)
			{
				if (!costListAdj[i].thingDef.intricate)
				{
					int num = GenMath.RoundRandom((float)costListAdj[i].count * 0.25f);
					if (num > 0)
					{
						Thing thing = ThingMaker.MakeThing(costListAdj[i].thingDef, null);
						thing.stackCount = num;
						yield return thing;
					}
				}
				num2 = i;
			}
			if (this.def.smeltProducts != null)
			{
				for (int i = 0; i < this.def.smeltProducts.Count; i = num2 + 1)
				{
					ThingDefCountClass thingDefCountClass = this.def.smeltProducts[i];
					Thing thing2 = ThingMaker.MakeThing(thingDefCountClass.thingDef, null);
					thing2.stackCount = thingDefCountClass.count;
					yield return thing2;
					num2 = i;
				}
			}
			yield break;
		}

		// Token: 0x06001969 RID: 6505 RVA: 0x000955D0 File Offset: 0x000937D0
		public float Ingested(Pawn ingester, float nutritionWanted)
		{
			if (this.Destroyed)
			{
				Log.Error(ingester + " ingested destroyed thing " + this);
				return 0f;
			}
			if (!this.IngestibleNow)
			{
				Log.Error(ingester + " ingested IngestibleNow=false thing " + this);
				return 0f;
			}
			ingester.mindState.lastIngestTick = Find.TickManager.TicksGame;
			if (ingester.needs.mood != null)
			{
				List<FoodUtility.ThoughtFromIngesting> list = FoodUtility.ThoughtsFromIngesting(ingester, this, this.def);
				for (int i = 0; i < list.Count; i++)
				{
					ingester.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(list[i].thought, list[i].fromPrecept), null);
				}
			}
			if (ingester.needs.drugsDesire != null)
			{
				ingester.needs.drugsDesire.Notify_IngestedDrug(this);
			}
			bool flag = FoodUtility.IsHumanlikeMeatOrHumanlikeCorpse(this, this.def);
			if (flag && ingester.IsColonist)
			{
				TaleRecorder.RecordTale(TaleDefOf.AteRawHumanlikeMeat, new object[]
				{
					ingester
				});
			}
			if (flag)
			{
				ingester.mindState.lastHumanMeatIngestedTick = Find.TickManager.TicksGame;
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteHumanMeat, ingester.Named(HistoryEventArgsNames.Doer)), false);
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteHumanMeatDirect, ingester.Named(HistoryEventArgsNames.Doer)), false);
			}
			else if (ModsConfig.IdeologyActive && (!this.def.IsDrug || this.def.ingestible.drugCategory != DrugCategory.Medical) && this.def.ingestible.CachedNutrition > 0f)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteNonCannibalFood, ingester.Named(HistoryEventArgsNames.Doer)), false);
			}
			if (this.def.ingestible.ateEvent != null)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(this.def.ingestible.ateEvent, ingester.Named(HistoryEventArgsNames.Doer)), false);
			}
			if (ModsConfig.IdeologyActive)
			{
				if (FoodUtility.IsConsideredMeatIfIngested(this))
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteMeat, ingester.Named(HistoryEventArgsNames.Doer)), false);
				}
				else if (!this.def.IsDrug && this.def.ingestible.CachedNutrition > 0f)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteNonMeat, ingester.Named(HistoryEventArgsNames.Doer)), false);
				}
				if (FoodUtility.IsVeneratedAnimalMeatOrCorpseOrHasIngredients(this, ingester))
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteVeneratedAnimalMeat, ingester.Named(HistoryEventArgsNames.Doer)), false);
				}
				if (this.def.thingCategories != null && this.def.thingCategories.Contains(ThingCategoryDefOf.PlantFoodRaw))
				{
					if (this.def.IsFungus)
					{
						Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteFungus, ingester.Named(HistoryEventArgsNames.Doer)), false);
					}
					else
					{
						Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteNonFungusPlant, ingester.Named(HistoryEventArgsNames.Doer)), false);
					}
				}
			}
			CompIngredients compIngredients = this.TryGetComp<CompIngredients>();
			if (compIngredients != null)
			{
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = false;
				bool flag5 = false;
				bool flag6 = false;
				for (int j = 0; j < compIngredients.ingredients.Count; j++)
				{
					if (!flag2 && FoodUtility.GetMeatSourceCategory(compIngredients.ingredients[j]) == MeatSourceCategory.Humanlike)
					{
						ingester.mindState.lastHumanMeatIngestedTick = Find.TickManager.TicksGame;
						Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteHumanMeatAsIngredient, ingester.Named(HistoryEventArgsNames.Doer)), false);
						flag2 = true;
					}
					else if (!flag3 && ingester.Ideo != null && compIngredients.ingredients[j].IsMeat && ingester.Ideo.IsVeneratedAnimal(compIngredients.ingredients[j].ingestible.sourceDef))
					{
						Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteVeneratedAnimalMeat, ingester.Named(HistoryEventArgsNames.Doer)), false);
						flag3 = true;
					}
					if (!flag4 && FoodUtility.GetMeatSourceCategory(compIngredients.ingredients[j]) == MeatSourceCategory.Insect)
					{
						Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteInsectMeatAsIngredient, ingester.Named(HistoryEventArgsNames.Doer)), false);
						flag4 = true;
					}
					if (ModsConfig.IdeologyActive && !flag5 && compIngredients.ingredients[j].thingCategories.Contains(ThingCategoryDefOf.PlantFoodRaw))
					{
						if (compIngredients.ingredients[j].IsFungus)
						{
							Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteFungusAsIngredient, ingester.Named(HistoryEventArgsNames.Doer)), false);
							flag5 = true;
						}
						else
						{
							flag6 = true;
						}
					}
				}
				if (ModsConfig.IdeologyActive && !flag5 && flag6)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteNonFungusMealWithPlants, ingester.Named(HistoryEventArgsNames.Doer)), false);
				}
			}
			int num;
			float result;
			this.IngestedCalculateAmounts(ingester, nutritionWanted, out num, out result);
			if (!ingester.Dead && ingester.needs.joy != null && Mathf.Abs(this.def.ingestible.joy) > 0.0001f && num > 0)
			{
				JoyKindDef joyKind = (this.def.ingestible.joyKind != null) ? this.def.ingestible.joyKind : JoyKindDefOf.Gluttonous;
				ingester.needs.joy.GainJoy((float)num * this.def.ingestible.joy, joyKind);
			}
			float num2;
			float chance = FoodUtility.TryGetFoodPoisoningChanceOverrideFromTraits(ingester, this, out num2) ? num2 : (this.GetStatValue(StatDefOf.FoodPoisonChanceFixedHuman, true) * FoodUtility.GetFoodPoisonChanceFactor(ingester));
			if (ingester.RaceProps.Humanlike && Rand.Chance(chance))
			{
				FoodUtility.AddFoodPoisoningHediff(ingester, this, FoodPoisonCause.DangerousFoodType);
			}
			bool flag7 = false;
			if (num > 0)
			{
				if (this.stackCount == 0)
				{
					Log.Error(this + " stack count is 0.");
				}
				if (num == this.stackCount)
				{
					flag7 = true;
				}
				else
				{
					this.SplitOff(num);
				}
			}
			this.PrePostIngested(ingester);
			if (flag7)
			{
				ingester.carryTracker.innerContainer.Remove(this);
			}
			if (this.def.ingestible.outcomeDoers != null)
			{
				for (int k = 0; k < this.def.ingestible.outcomeDoers.Count; k++)
				{
					this.def.ingestible.outcomeDoers[k].DoIngestionOutcome(ingester, this);
				}
			}
			if (flag7)
			{
				this.Destroy(DestroyMode.Vanish);
			}
			this.PostIngested(ingester);
			return result;
		}

		// Token: 0x0600196A RID: 6506 RVA: 0x0000313F File Offset: 0x0000133F
		protected virtual void PrePostIngested(Pawn ingester)
		{
		}

		// Token: 0x0600196B RID: 6507 RVA: 0x0000313F File Offset: 0x0000133F
		protected virtual void PostIngested(Pawn ingester)
		{
		}

		// Token: 0x0600196C RID: 6508 RVA: 0x00095C4C File Offset: 0x00093E4C
		protected virtual void IngestedCalculateAmounts(Pawn ingester, float nutritionWanted, out int numTaken, out float nutritionIngested)
		{
			numTaken = Mathf.CeilToInt(nutritionWanted / this.GetStatValue(StatDefOf.Nutrition, true));
			numTaken = Mathf.Min(new int[]
			{
				numTaken,
				this.def.ingestible.maxNumToIngestAtOnce,
				this.stackCount
			});
			numTaken = Mathf.Max(numTaken, 1);
			nutritionIngested = (float)numTaken * this.GetStatValue(StatDefOf.Nutrition, true);
		}

		// Token: 0x0600196D RID: 6509 RVA: 0x00095CB9 File Offset: 0x00093EB9
		public virtual bool PreventPlayerSellingThingsNearby(out string reason)
		{
			reason = null;
			return false;
		}

		// Token: 0x0600196E RID: 6510 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual ushort PathFindCostFor(Pawn p)
		{
			return 0;
		}

		// Token: 0x040010F1 RID: 4337
		public ThingDef def;

		// Token: 0x040010F2 RID: 4338
		public int thingIDNumber = -1;

		// Token: 0x040010F3 RID: 4339
		private sbyte mapIndexOrState = -1;

		// Token: 0x040010F4 RID: 4340
		private IntVec3 positionInt = IntVec3.Invalid;

		// Token: 0x040010F5 RID: 4341
		private Rot4 rotationInt = Rot4.North;

		// Token: 0x040010F6 RID: 4342
		public int stackCount = 1;

		// Token: 0x040010F7 RID: 4343
		protected Faction factionInt;

		// Token: 0x040010F8 RID: 4344
		private ThingDef stuffInt;

		// Token: 0x040010F9 RID: 4345
		private Graphic graphicInt;

		// Token: 0x040010FA RID: 4346
		private Graphic styleGraphicInt;

		// Token: 0x040010FB RID: 4347
		private int hitPointsInt = -1;

		// Token: 0x040010FC RID: 4348
		public ThingOwner holdingOwner;

		// Token: 0x040010FD RID: 4349
		public List<string> questTags;

		// Token: 0x040010FE RID: 4350
		protected const sbyte UnspawnedState = -1;

		// Token: 0x040010FF RID: 4351
		private const sbyte MemoryState = -2;

		// Token: 0x04001100 RID: 4352
		private const sbyte DiscardedState = -3;

		// Token: 0x04001101 RID: 4353
		public static bool allowDestroyNonDestroyable = false;

		// Token: 0x04001102 RID: 4354
		private static Dictionary<Thing, string> facIDsCached = new Dictionary<Thing, string>();

		// Token: 0x04001103 RID: 4355
		private static List<string> tmpDeteriorationReasons = new List<string>();

		// Token: 0x04001104 RID: 4356
		private static List<string> tmpIdeoNames = new List<string>();

		// Token: 0x04001105 RID: 4357
		public const float SmeltCostRecoverFraction = 0.25f;
	}
}
