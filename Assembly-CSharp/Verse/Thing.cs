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
	// Token: 0x02000503 RID: 1283
	public class Thing : Entity, IExposable, ISelectable, ILoadReferenceable, ISignalReceiver
	{
		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06001FF9 RID: 8185 RVA: 0x0001C314 File Offset: 0x0001A514
		// (set) Token: 0x06001FFA RID: 8186 RVA: 0x0001C31C File Offset: 0x0001A51C
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

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x06001FFB RID: 8187 RVA: 0x0001C325 File Offset: 0x0001A525
		public int MaxHitPoints
		{
			get
			{
				return Mathf.RoundToInt(this.GetStatValue(StatDefOf.MaxHitPoints, true));
			}
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x06001FFC RID: 8188 RVA: 0x0001C338 File Offset: 0x0001A538
		public float MarketValue
		{
			get
			{
				return this.GetStatValue(StatDefOf.MarketValue, true);
			}
		}

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x06001FFD RID: 8189 RVA: 0x0001C346 File Offset: 0x0001A546
		public virtual float RoyalFavorValue
		{
			get
			{
				return this.GetStatValue(StatDefOf.RoyalFavorValue, true);
			}
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06001FFE RID: 8190 RVA: 0x0010227C File Offset: 0x0010047C
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

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x06001FFF RID: 8191 RVA: 0x0001C354 File Offset: 0x0001A554
		public virtual bool FireBulwark
		{
			get
			{
				return this.def.Fillage == FillCategory.Full;
			}
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x06002000 RID: 8192 RVA: 0x0001C364 File Offset: 0x0001A564
		public bool Destroyed
		{
			get
			{
				return this.mapIndexOrState == -2 || this.mapIndexOrState == -3;
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x06002001 RID: 8193 RVA: 0x0001C37C File Offset: 0x0001A57C
		public bool Discarded
		{
			get
			{
				return this.mapIndexOrState == -3;
			}
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x06002002 RID: 8194 RVA: 0x0001C388 File Offset: 0x0001A588
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
				Log.ErrorOnce("Thing is associated with invalid map index", 64664487, false);
				return false;
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06002003 RID: 8195 RVA: 0x0001C3BA File Offset: 0x0001A5BA
		public bool SpawnedOrAnyParentSpawned
		{
			get
			{
				return this.SpawnedParentOrMe != null;
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06002004 RID: 8196 RVA: 0x0001C3C5 File Offset: 0x0001A5C5
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

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06002005 RID: 8197 RVA: 0x0001C3E6 File Offset: 0x0001A5E6
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

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06002006 RID: 8198 RVA: 0x0001C403 File Offset: 0x0001A603
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

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06002007 RID: 8199 RVA: 0x0001C429 File Offset: 0x0001A629
		// (set) Token: 0x06002008 RID: 8200 RVA: 0x001022E8 File Offset: 0x001004E8
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
						Log.Warning("Changed position of a spawned thing which affects regions. This is not supported.", false);
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

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06002009 RID: 8201 RVA: 0x001023A4 File Offset: 0x001005A4
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

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x0600200A RID: 8202 RVA: 0x0001C431 File Offset: 0x0001A631
		// (set) Token: 0x0600200B RID: 8203 RVA: 0x001023E0 File Offset: 0x001005E0
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
						Log.Warning("Changed rotation of a spawned non-single-cell thing which affects regions. This is not supported.", false);
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

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x0600200C RID: 8204 RVA: 0x0001C439 File Offset: 0x0001A639
		public bool Smeltable
		{
			get
			{
				return this.def.smeltable && (!this.def.MadeFromStuff || this.Stuff.smeltable);
			}
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x0600200D RID: 8205 RVA: 0x0001C464 File Offset: 0x0001A664
		public bool BurnableByRecipe
		{
			get
			{
				return this.def.burnableByRecipe && (!this.def.MadeFromStuff || this.Stuff.burnableByRecipe);
			}
		}

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x0600200E RID: 8206 RVA: 0x0001C48F File Offset: 0x0001A68F
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

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x0600200F RID: 8207 RVA: 0x0001C4A6 File Offset: 0x0001A6A6
		public Faction Faction
		{
			get
			{
				return this.factionInt;
			}
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06002010 RID: 8208 RVA: 0x0001C4AE File Offset: 0x0001A6AE
		// (set) Token: 0x06002011 RID: 8209 RVA: 0x0001C4E4 File Offset: 0x0001A6E4
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

		// Token: 0x06002012 RID: 8210 RVA: 0x001024D0 File Offset: 0x001006D0
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
				}), false);
			}
			return result;
		}

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x06002013 RID: 8211 RVA: 0x0001C4F2 File Offset: 0x0001A6F2
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

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x06002014 RID: 8212 RVA: 0x00102550 File Offset: 0x00100750
		public virtual CellRect? CustomRectForSelector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x06002015 RID: 8213 RVA: 0x0001C532 File Offset: 0x0001A732
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

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x06002016 RID: 8214 RVA: 0x0001C55F File Offset: 0x0001A75F
		public virtual string LabelNoCount
		{
			get
			{
				return GenLabel.ThingLabel(this, 1, true);
			}
		}

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x06002017 RID: 8215 RVA: 0x0001C569 File Offset: 0x0001A769
		public override string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.def);
			}
		}

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x06002018 RID: 8216 RVA: 0x0001C57C File Offset: 0x0001A77C
		public virtual string LabelCapNoCount
		{
			get
			{
				return this.LabelNoCount.CapitalizeFirst(this.def);
			}
		}

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x06002019 RID: 8217 RVA: 0x0001C58F File Offset: 0x0001A78F
		public override string LabelShort
		{
			get
			{
				return this.LabelNoCount;
			}
		}

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x0600201A RID: 8218 RVA: 0x0001C597 File Offset: 0x0001A797
		public virtual bool IngestibleNow
		{
			get
			{
				return !this.IsBurning() && this.def.IsIngestible;
			}
		}

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x0600201B RID: 8219 RVA: 0x0001C5AE File Offset: 0x0001A7AE
		public ThingDef Stuff
		{
			get
			{
				return this.stuffInt;
			}
		}

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x0600201C RID: 8220 RVA: 0x0001C5B6 File Offset: 0x0001A7B6
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

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x0600201D RID: 8221 RVA: 0x0001C5F0 File Offset: 0x0001A7F0
		public virtual Graphic Graphic
		{
			get
			{
				return this.DefaultGraphic;
			}
		}

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x0600201E RID: 8222 RVA: 0x0001C5F8 File Offset: 0x0001A7F8
		public virtual IntVec3 InteractionCell
		{
			get
			{
				return ThingUtility.InteractionCellWhenAt(this.def, this.Position, this.Rotation, this.Map);
			}
		}

		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x0600201F RID: 8223 RVA: 0x00102568 File Offset: 0x00100768
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

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x06002020 RID: 8224 RVA: 0x0001C617 File Offset: 0x0001A817
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

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x06002021 RID: 8225 RVA: 0x0001C642 File Offset: 0x0001A842
		public bool Suspended
		{
			get
			{
				return !this.Spawned && this.ParentHolder != null && ThingOwnerUtility.ContentsSuspended(this.ParentHolder);
			}
		}

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x06002022 RID: 8226 RVA: 0x0001C663 File Offset: 0x0001A863
		public virtual string DescriptionDetailed
		{
			get
			{
				return this.def.DescriptionDetailed;
			}
		}

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x06002023 RID: 8227 RVA: 0x0001C670 File Offset: 0x0001A870
		public virtual string DescriptionFlavor
		{
			get
			{
				return this.def.description;
			}
		}

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x06002024 RID: 8228 RVA: 0x0001C67D File Offset: 0x0001A87D
		public TerrainAffordanceDef TerrainAffordanceNeeded
		{
			get
			{
				return this.def.GetTerrainAffordanceNeed(this.stuffInt);
			}
		}

		// Token: 0x06002026 RID: 8230 RVA: 0x0001C6CA File Offset: 0x0001A8CA
		public virtual void PostMake()
		{
			ThingIDMaker.GiveIDTo(this);
			if (this.def.useHitPoints)
			{
				this.HitPoints = Mathf.RoundToInt((float)this.MaxHitPoints * Mathf.Clamp01(this.def.startingHpRange.RandomInRange));
			}
		}

		// Token: 0x06002027 RID: 8231 RVA: 0x0001C707 File Offset: 0x0001A907
		public string GetUniqueLoadID()
		{
			return "Thing_" + this.ThingID;
		}

		// Token: 0x06002028 RID: 8232 RVA: 0x001025EC File Offset: 0x001007EC
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
				}), false);
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
				}), false);
				return;
			}
			int num = Find.Maps.IndexOf(map);
			if (num < 0)
			{
				Log.Error("Tried to spawn thing " + this + ", but the map provided does not exist.", false);
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
				}), false);
				this.stackCount = this.def.stackLimit;
			}
			this.mapIndexOrState = (sbyte)num;
			RegionListersUpdater.RegisterInRegions(this, map);
			if (!map.spawnedThings.TryAdd(this, false))
			{
				Log.Error("Couldn't add thing " + this + " to spawned things.", false);
			}
			map.listerThings.Add(this);
			map.thingGrid.Register(this);
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
			if (this.def.graphicData != null && this.def.graphicData.Linked)
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
				map.pathGrid.RecalculatePerceivedPathCostUnderThing(this);
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

		// Token: 0x06002029 RID: 8233 RVA: 0x00102980 File Offset: 0x00100B80
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.Destroyed)
			{
				Log.Error("Tried to despawn " + this.ToStringSafe<Thing>() + " which is already destroyed.", false);
				return;
			}
			if (!this.Spawned)
			{
				Log.Error("Tried to despawn " + this.ToStringSafe<Thing>() + " which is not spawned.", false);
				return;
			}
			Map map = this.Map;
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
			if (this.def.graphicData != null && this.def.graphicData.Linked)
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
				map.pathGrid.RecalculatePerceivedPathCostUnderThing(this);
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

		// Token: 0x0600202A RID: 8234 RVA: 0x0001C719 File Offset: 0x0001A919
		public virtual void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
		{
			this.Destroy(DestroyMode.KillFinalize);
		}

		// Token: 0x0600202B RID: 8235 RVA: 0x00102C1C File Offset: 0x00100E1C
		public virtual void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (!Thing.allowDestroyNonDestroyable && !this.def.destroyable)
			{
				Log.Error("Tried to destroy non-destroyable thing " + this, false);
				return;
			}
			if (this.Destroyed)
			{
				Log.Error("Tried to destroy already-destroyed thing " + this, false);
				return;
			}
			bool spawned = this.Spawned;
			Map map = this.Map;
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

		// Token: 0x0600202C RID: 8236 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
		}

		// Token: 0x0600202D RID: 8237 RVA: 0x0001C722 File Offset: 0x0001A922
		public virtual void PostGeneratedForTrader(TraderKindDef trader, int forTile, Faction forFaction)
		{
			if (this.def.colorGeneratorInTraderStock != null)
			{
				this.SetColor(this.def.colorGeneratorInTraderStock.NewRandomizedColor(), true);
			}
		}

		// Token: 0x0600202E RID: 8238 RVA: 0x0001C748 File Offset: 0x0001A948
		public virtual void Notify_MyMapRemoved()
		{
			if (this.def.receivesSignals)
			{
				Find.SignalManager.DeregisterReceiver(this);
			}
			if (!ThingOwnerUtility.AnyParentIs<Pawn>(this))
			{
				this.mapIndexOrState = -3;
			}
			this.RemoveAllReservationsAndDesignationsOnThis();
		}

		// Token: 0x0600202F RID: 8239 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_LordDestroyed()
		{
		}

		// Token: 0x06002030 RID: 8240 RVA: 0x0001C778 File Offset: 0x0001A978
		public void ForceSetStateToUnspawned()
		{
			this.mapIndexOrState = -1;
		}

		// Token: 0x06002031 RID: 8241 RVA: 0x00102D30 File Offset: 0x00100F30
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
				}), false);
				return;
			}
			this.mapIndexOrState -= 1;
		}

		// Token: 0x06002032 RID: 8242 RVA: 0x00102D8C File Offset: 0x00100F8C
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

		// Token: 0x06002033 RID: 8243 RVA: 0x00102E18 File Offset: 0x00101018
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
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.ResolvingCrossRefs || Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (facID == "null")
				{
					this.factionInt = null;
				}
				else if (Find.World != null && Find.FactionManager != null)
				{
					this.factionInt = Find.FactionManager.AllFactions.FirstOrDefault((Faction fa) => fa.GetUniqueLoadID() == facID);
				}
			}
			Scribe_Collections.Look<string>(ref this.questTags, "questTags", LookMode.Value, Array.Empty<object>());
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06002034 RID: 8244 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostMapInit()
		{
		}

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x06002035 RID: 8245 RVA: 0x0001C781 File Offset: 0x0001A981
		public virtual Vector3 DrawPos
		{
			get
			{
				return this.TrueCenter();
			}
		}

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x06002036 RID: 8246 RVA: 0x0001C789 File Offset: 0x0001A989
		// (set) Token: 0x06002037 RID: 8247 RVA: 0x00102FE0 File Offset: 0x001011E0
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
				}), false);
			}
		}

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x06002038 RID: 8248 RVA: 0x0001C7C8 File Offset: 0x0001A9C8
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

		// Token: 0x06002039 RID: 8249 RVA: 0x0001C7ED File Offset: 0x0001A9ED
		public virtual void Draw()
		{
			this.DrawAt(this.DrawPos, false);
		}

		// Token: 0x0600203A RID: 8250 RVA: 0x00103030 File Offset: 0x00101230
		public virtual void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.Graphic.Draw(drawLoc, flip ? this.Rotation.Opposite : this.Rotation, this, 0f);
		}

		// Token: 0x0600203B RID: 8251 RVA: 0x0001C7FC File Offset: 0x0001A9FC
		public virtual void Print(SectionLayer layer)
		{
			this.Graphic.Print(layer, this);
		}

		// Token: 0x0600203C RID: 8252 RVA: 0x00103068 File Offset: 0x00101268
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

		// Token: 0x0600203D RID: 8253 RVA: 0x001030D4 File Offset: 0x001012D4
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

		// Token: 0x0600203E RID: 8254 RVA: 0x00103130 File Offset: 0x00101330
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

		// Token: 0x0600203F RID: 8255 RVA: 0x0001C80B File Offset: 0x0001AA0B
		public virtual string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			QuestUtility.AppendInspectStringsFromQuestParts(stringBuilder, this);
			return stringBuilder.ToString();
		}

		// Token: 0x06002040 RID: 8256 RVA: 0x001031EC File Offset: 0x001013EC
		public virtual string GetInspectStringLowPriority()
		{
			string result = null;
			Thing.tmpDeteriorationReasons.Clear();
			SteadyEnvironmentEffects.FinalDeteriorationRate(this, Thing.tmpDeteriorationReasons);
			if (Thing.tmpDeteriorationReasons.Count != 0)
			{
				result = string.Format("{0}: {1}", "DeterioratingBecauseOf".Translate(), Thing.tmpDeteriorationReasons.ToCommaList(false).CapitalizeFirst());
			}
			return result;
		}

		// Token: 0x06002041 RID: 8257 RVA: 0x0001C81E File Offset: 0x0001AA1E
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			yield break;
		}

		// Token: 0x06002042 RID: 8258 RVA: 0x0001C827 File Offset: 0x0001AA27
		public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			yield break;
		}

		// Token: 0x06002043 RID: 8259 RVA: 0x0001C830 File Offset: 0x0001AA30
		public virtual IEnumerable<FloatMenuOption> GetMultiSelectFloatMenuOptions(List<Pawn> selPawns)
		{
			yield break;
		}

		// Token: 0x06002044 RID: 8260 RVA: 0x0001C839 File Offset: 0x0001AA39
		public virtual IEnumerable<InspectTabBase> GetInspectTabs()
		{
			return this.def.inspectorTabsResolved;
		}

		// Token: 0x06002045 RID: 8261 RVA: 0x0001C846 File Offset: 0x0001AA46
		public virtual string GetCustomLabelNoCount(bool includeHp = true)
		{
			return GenLabel.ThingLabel(this, 1, includeHp);
		}

		// Token: 0x06002046 RID: 8262 RVA: 0x00103248 File Offset: 0x00101448
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
				GenLeaving.DropFilthDueToDamage(this, damageResult.totalDamageDealt);
				if (dinfo.Instigator != null)
				{
					Pawn pawn = dinfo.Instigator as Pawn;
					if (pawn != null)
					{
						pawn.records.AddTo(RecordDefOf.DamageDealt, damageResult.totalDamageDealt);
						pawn.records.AccumulateStoryEvent(StoryEventDefOf.DamageDealt);
					}
				}
			}
			this.PostApplyDamage(dinfo, damageResult.totalDamageDealt);
			return damageResult;
		}

		// Token: 0x06002047 RID: 8263 RVA: 0x0001C850 File Offset: 0x0001AA50
		public virtual void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			absorbed = false;
		}

		// Token: 0x06002048 RID: 8264 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
		}

		// Token: 0x06002049 RID: 8265 RVA: 0x001033B8 File Offset: 0x001015B8
		public virtual bool CanStackWith(Thing other)
		{
			return !this.Destroyed && !other.Destroyed && this.def.category == ThingCategory.Item && this.def == other.def && this.Stuff == other.Stuff;
		}

		// Token: 0x0600204A RID: 8266 RVA: 0x00103408 File Offset: 0x00101608
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

		// Token: 0x0600204B RID: 8267 RVA: 0x001034B0 File Offset: 0x001016B0
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
					}), false);
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

		// Token: 0x0600204C RID: 8268 RVA: 0x0001C855 File Offset: 0x0001AA55
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (this.Stuff != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Stat_Stuff_Name".Translate(), this.Stuff.LabelCap, "Stat_Stuff_Desc".Translate(), 1100, null, new Dialog_InfoCard.Hyperlink[]
				{
					new Dialog_InfoCard.Hyperlink(this.Stuff, -1)
				}, false);
			}
			yield break;
		}

		// Token: 0x0600204D RID: 8269 RVA: 0x001035A8 File Offset: 0x001017A8
		public virtual void Notify_ColorChanged()
		{
			this.graphicInt = null;
			if (this.Spawned && (this.def.drawerType == DrawerType.MapMeshOnly || this.def.drawerType == DrawerType.MapMeshAndRealTime))
			{
				this.Map.mapDrawer.MapMeshDirty(this.Position, MapMeshFlag.Things);
			}
		}

		// Token: 0x0600204E RID: 8270 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_Equipped(Pawn pawn)
		{
		}

		// Token: 0x0600204F RID: 8271 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_UsedWeapon(Pawn pawn)
		{
		}

		// Token: 0x06002050 RID: 8272 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_SignalReceived(Signal signal)
		{
		}

		// Token: 0x06002051 RID: 8273 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_Explosion(Explosion explosion)
		{
		}

		// Token: 0x06002052 RID: 8274 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_BulletImpactNearby(BulletImpactData impactData)
		{
		}

		// Token: 0x06002053 RID: 8275 RVA: 0x001035F8 File Offset: 0x001017F8
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

		// Token: 0x06002054 RID: 8276 RVA: 0x0001C865 File Offset: 0x0001AA65
		public virtual bool BlocksPawn(Pawn p)
		{
			return this.def.passability == Traversability.Impassable;
		}

		// Token: 0x06002055 RID: 8277 RVA: 0x0001C875 File Offset: 0x0001AA75
		public void SetFactionDirect(Faction newFaction)
		{
			if (!this.def.CanHaveFaction)
			{
				Log.Error("Tried to SetFactionDirect on " + this + " which cannot have a faction.", false);
				return;
			}
			this.factionInt = newFaction;
		}

		// Token: 0x06002056 RID: 8278 RVA: 0x00103668 File Offset: 0x00101868
		public virtual void SetFaction(Faction newFaction, Pawn recruiter = null)
		{
			if (!this.def.CanHaveFaction)
			{
				Log.Error("Tried to SetFaction on " + this + " which cannot have a faction.", false);
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
		}

		// Token: 0x06002057 RID: 8279 RVA: 0x0001C8A2 File Offset: 0x0001AAA2
		public void SetPositionDirect(IntVec3 newPos)
		{
			this.positionInt = newPos;
		}

		// Token: 0x06002058 RID: 8280 RVA: 0x0001C8AB File Offset: 0x0001AAAB
		public void SetStuffDirect(ThingDef newStuff)
		{
			this.stuffInt = newStuff;
		}

		// Token: 0x06002059 RID: 8281 RVA: 0x0001C8B4 File Offset: 0x0001AAB4
		public override string ToString()
		{
			if (this.def != null)
			{
				return this.ThingID;
			}
			return base.GetType().ToString();
		}

		// Token: 0x0600205A RID: 8282 RVA: 0x0001C8D0 File Offset: 0x0001AAD0
		public override int GetHashCode()
		{
			return this.thingIDNumber;
		}

		// Token: 0x0600205B RID: 8283 RVA: 0x001036EC File Offset: 0x001018EC
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
				}), false);
				return;
			}
			this.mapIndexOrState = -3;
		}

		// Token: 0x0600205C RID: 8284 RVA: 0x0001C8D8 File Offset: 0x0001AAD8
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

		// Token: 0x0600205D RID: 8285 RVA: 0x0001C8EF File Offset: 0x0001AAEF
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

		// Token: 0x0600205E RID: 8286 RVA: 0x00103748 File Offset: 0x00101948
		public float Ingested(Pawn ingester, float nutritionWanted)
		{
			if (this.Destroyed)
			{
				Log.Error(ingester + " ingested destroyed thing " + this, false);
				return 0f;
			}
			if (!this.IngestibleNow)
			{
				Log.Error(ingester + " ingested IngestibleNow=false thing " + this, false);
				return 0f;
			}
			ingester.mindState.lastIngestTick = Find.TickManager.TicksGame;
			if (ingester.needs.mood != null)
			{
				List<ThoughtDef> list = FoodUtility.ThoughtsFromIngesting(ingester, this, this.def);
				for (int i = 0; i < list.Count; i++)
				{
					ingester.needs.mood.thoughts.memories.TryGainMemory(list[i], null);
				}
			}
			if (ingester.needs.drugsDesire != null)
			{
				ingester.needs.drugsDesire.Notify_IngestedDrug(this);
			}
			if (ingester.IsColonist && FoodUtility.IsHumanlikeMeatOrHumanlikeCorpse(this))
			{
				TaleRecorder.RecordTale(TaleDefOf.AteRawHumanlikeMeat, new object[]
				{
					ingester
				});
			}
			int num;
			float result;
			this.IngestedCalculateAmounts(ingester, nutritionWanted, out num, out result);
			if (!ingester.Dead && ingester.needs.joy != null && Mathf.Abs(this.def.ingestible.joy) > 0.0001f && num > 0)
			{
				JoyKindDef joyKind = (this.def.ingestible.joyKind != null) ? this.def.ingestible.joyKind : JoyKindDefOf.Gluttonous;
				ingester.needs.joy.GainJoy((float)num * this.def.ingestible.joy, joyKind);
			}
			if (ingester.RaceProps.Humanlike && Rand.Chance(this.GetStatValue(StatDefOf.FoodPoisonChanceFixedHuman, true) * FoodUtility.GetFoodPoisonChanceFactor(ingester)))
			{
				FoodUtility.AddFoodPoisoningHediff(ingester, this, FoodPoisonCause.DangerousFoodType);
			}
			bool flag = false;
			if (num > 0)
			{
				if (this.stackCount == 0)
				{
					Log.Error(this + " stack count is 0.", false);
				}
				if (num == this.stackCount)
				{
					flag = true;
				}
				else
				{
					this.SplitOff(num);
				}
			}
			this.PrePostIngested(ingester);
			if (flag)
			{
				ingester.carryTracker.innerContainer.Remove(this);
			}
			if (this.def.ingestible.outcomeDoers != null)
			{
				for (int j = 0; j < this.def.ingestible.outcomeDoers.Count; j++)
				{
					this.def.ingestible.outcomeDoers[j].DoIngestionOutcome(ingester, this);
				}
			}
			if (flag)
			{
				this.Destroy(DestroyMode.Vanish);
			}
			this.PostIngested(ingester);
			return result;
		}

		// Token: 0x0600205F RID: 8287 RVA: 0x00006A05 File Offset: 0x00004C05
		protected virtual void PrePostIngested(Pawn ingester)
		{
		}

		// Token: 0x06002060 RID: 8288 RVA: 0x00006A05 File Offset: 0x00004C05
		protected virtual void PostIngested(Pawn ingester)
		{
		}

		// Token: 0x06002061 RID: 8289 RVA: 0x001039B8 File Offset: 0x00101BB8
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

		// Token: 0x06002062 RID: 8290 RVA: 0x0001C8FF File Offset: 0x0001AAFF
		public virtual bool PreventPlayerSellingThingsNearby(out string reason)
		{
			reason = null;
			return false;
		}

		// Token: 0x06002063 RID: 8291 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual ushort PathFindCostFor(Pawn p)
		{
			return 0;
		}

		// Token: 0x04001671 RID: 5745
		public ThingDef def;

		// Token: 0x04001672 RID: 5746
		public int thingIDNumber = -1;

		// Token: 0x04001673 RID: 5747
		private sbyte mapIndexOrState = -1;

		// Token: 0x04001674 RID: 5748
		private IntVec3 positionInt = IntVec3.Invalid;

		// Token: 0x04001675 RID: 5749
		private Rot4 rotationInt = Rot4.North;

		// Token: 0x04001676 RID: 5750
		public int stackCount = 1;

		// Token: 0x04001677 RID: 5751
		protected Faction factionInt;

		// Token: 0x04001678 RID: 5752
		private ThingDef stuffInt;

		// Token: 0x04001679 RID: 5753
		private Graphic graphicInt;

		// Token: 0x0400167A RID: 5754
		private int hitPointsInt = -1;

		// Token: 0x0400167B RID: 5755
		public ThingOwner holdingOwner;

		// Token: 0x0400167C RID: 5756
		public List<string> questTags;

		// Token: 0x0400167D RID: 5757
		protected const sbyte UnspawnedState = -1;

		// Token: 0x0400167E RID: 5758
		private const sbyte MemoryState = -2;

		// Token: 0x0400167F RID: 5759
		private const sbyte DiscardedState = -3;

		// Token: 0x04001680 RID: 5760
		public static bool allowDestroyNonDestroyable = false;

		// Token: 0x04001681 RID: 5761
		private static List<string> tmpDeteriorationReasons = new List<string>();

		// Token: 0x04001682 RID: 5762
		public const float SmeltCostRecoverFraction = 0.25f;
	}
}
