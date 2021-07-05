using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200020D RID: 525
	public class Room
	{
		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000ECE RID: 3790 RVA: 0x00053EE6 File Offset: 0x000520E6
		public List<District> Districts
		{
			get
			{
				return this.districts;
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000ECF RID: 3791 RVA: 0x00053EEE File Offset: 0x000520EE
		public Map Map
		{
			get
			{
				if (!this.districts.Any<District>())
				{
					return null;
				}
				return this.districts[0].Map;
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000ED0 RID: 3792 RVA: 0x00053F10 File Offset: 0x00052110
		public int DistrictCount
		{
			get
			{
				return this.districts.Count;
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000ED1 RID: 3793 RVA: 0x00053F1D File Offset: 0x0005211D
		public RoomTempTracker TempTracker
		{
			get
			{
				return this.tempTracker;
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000ED2 RID: 3794 RVA: 0x00053F25 File Offset: 0x00052125
		// (set) Token: 0x06000ED3 RID: 3795 RVA: 0x00053F32 File Offset: 0x00052132
		public float Temperature
		{
			get
			{
				return this.tempTracker.Temperature;
			}
			set
			{
				this.tempTracker.Temperature = value;
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000ED4 RID: 3796 RVA: 0x00053F40 File Offset: 0x00052140
		public bool UsesOutdoorTemperature
		{
			get
			{
				return this.TouchesMapEdge || this.OpenRoofCount >= Mathf.CeilToInt((float)this.CellCount * 0.25f);
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000ED5 RID: 3797 RVA: 0x00053F69 File Offset: 0x00052169
		public bool Dereferenced
		{
			get
			{
				return this.RegionCount == 0;
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000ED6 RID: 3798 RVA: 0x00053F74 File Offset: 0x00052174
		public bool IsHuge
		{
			get
			{
				return this.RegionCount > 60;
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000ED7 RID: 3799 RVA: 0x00053F80 File Offset: 0x00052180
		public bool IsPrisonCell
		{
			get
			{
				return this.isPrisonCell;
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000ED8 RID: 3800 RVA: 0x00053F88 File Offset: 0x00052188
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				int num;
				for (int i = 0; i < this.districts.Count; i = num + 1)
				{
					foreach (IntVec3 intVec in this.districts[i].Cells)
					{
						yield return intVec;
					}
					IEnumerator<IntVec3> enumerator = null;
					num = i;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000ED9 RID: 3801 RVA: 0x00053F98 File Offset: 0x00052198
		public int CellCount
		{
			get
			{
				if (this.cachedCellCount == -1)
				{
					this.cachedCellCount = 0;
					for (int i = 0; i < this.districts.Count; i++)
					{
						this.cachedCellCount += this.districts[i].CellCount;
					}
				}
				return this.cachedCellCount;
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000EDA RID: 3802 RVA: 0x00053FF0 File Offset: 0x000521F0
		public Region FirstRegion
		{
			get
			{
				for (int i = 0; i < this.districts.Count; i++)
				{
					List<Region> regions = this.districts[i].Regions;
					if (regions.Count > 0)
					{
						return regions[0];
					}
				}
				return null;
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000EDB RID: 3803 RVA: 0x00054038 File Offset: 0x00052238
		public List<Region> Regions
		{
			get
			{
				this.tmpRegions.Clear();
				for (int i = 0; i < this.districts.Count; i++)
				{
					List<Region> regions = this.districts[i].Regions;
					for (int j = 0; j < regions.Count; j++)
					{
						this.tmpRegions.Add(regions[j]);
					}
				}
				return this.tmpRegions;
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000EDC RID: 3804 RVA: 0x000540A4 File Offset: 0x000522A4
		public int RegionCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.districts.Count; i++)
				{
					num += this.districts[i].RegionCount;
				}
				return num;
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000EDD RID: 3805 RVA: 0x000540E0 File Offset: 0x000522E0
		public CellRect ExtentsClose
		{
			get
			{
				CellRect cellRect = new CellRect(int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);
				foreach (Region region in this.Regions)
				{
					if (region.extentsClose.minX < cellRect.minX)
					{
						cellRect.minX = region.extentsClose.minX;
					}
					if (region.extentsClose.minZ < cellRect.minZ)
					{
						cellRect.minZ = region.extentsClose.minZ;
					}
					if (region.extentsClose.maxX > cellRect.maxX)
					{
						cellRect.maxX = region.extentsClose.maxX;
					}
					if (region.extentsClose.maxZ > cellRect.maxZ)
					{
						cellRect.maxZ = region.extentsClose.maxZ;
					}
				}
				return cellRect;
			}
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x000541E4 File Offset: 0x000523E4
		private int OpenRoofCountStopAt(int threshold)
		{
			if (this.cachedOpenRoofCount != -1)
			{
				return this.cachedOpenRoofCount;
			}
			int num = 0;
			for (int i = 0; i < this.districts.Count; i++)
			{
				num += this.districts[i].OpenRoofCountStopAt(threshold);
				if (num >= threshold)
				{
					return num;
				}
				threshold -= num;
			}
			return num;
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000EDF RID: 3807 RVA: 0x0005423A File Offset: 0x0005243A
		public int OpenRoofCount
		{
			get
			{
				if (this.cachedOpenRoofCount == -1)
				{
					this.cachedOpenRoofCount = this.OpenRoofCountStopAt(int.MaxValue);
				}
				return this.cachedOpenRoofCount;
			}
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000EE0 RID: 3808 RVA: 0x0005425C File Offset: 0x0005245C
		public IEnumerable<IntVec3> BorderCells
		{
			get
			{
				foreach (IntVec3 c in this.Cells)
				{
					int num;
					for (int i = 0; i < 8; i = num)
					{
						IntVec3 intVec = c + GenAdj.AdjacentCells[i];
						Region region = (c + GenAdj.AdjacentCells[i]).GetRegion(this.Map, RegionType.Set_Passable);
						if (region == null || region.Room != this)
						{
							yield return intVec;
						}
						num = i + 1;
					}
				}
				IEnumerator<IntVec3> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000EE1 RID: 3809 RVA: 0x0005426C File Offset: 0x0005246C
		public bool TouchesMapEdge
		{
			get
			{
				for (int i = 0; i < this.districts.Count; i++)
				{
					if (this.districts[i].TouchesMapEdge)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000EE2 RID: 3810 RVA: 0x000542A5 File Offset: 0x000524A5
		public bool PsychologicallyOutdoors
		{
			get
			{
				return this.OpenRoofCountStopAt(300) >= 300 || (this.TouchesMapEdge && (float)this.OpenRoofCount / (float)this.CellCount >= 0.5f);
			}
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000EE3 RID: 3811 RVA: 0x000542DC File Offset: 0x000524DC
		public bool OutdoorsForWork
		{
			get
			{
				return this.OpenRoofCountStopAt(101) > 100 || (float)this.OpenRoofCount > (float)this.CellCount * 0.25f;
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000EE4 RID: 3812 RVA: 0x00054303 File Offset: 0x00052503
		public IEnumerable<Pawn> Owners
		{
			get
			{
				if (this.TouchesMapEdge)
				{
					yield break;
				}
				if (this.IsHuge)
				{
					yield break;
				}
				if (this.Role != RoomRoleDefOf.Bedroom && this.Role != RoomRoleDefOf.PrisonCell && this.Role != RoomRoleDefOf.Barracks && this.Role != RoomRoleDefOf.PrisonBarracks)
				{
					yield break;
				}
				Pawn pawn = null;
				Pawn secondOwner = null;
				foreach (Building_Bed building_Bed in this.ContainedBeds)
				{
					if (building_Bed.def.building.bed_humanlike)
					{
						for (int i = 0; i < building_Bed.OwnersForReading.Count; i++)
						{
							if (pawn == null)
							{
								pawn = building_Bed.OwnersForReading[i];
							}
							else
							{
								if (secondOwner != null)
								{
									yield break;
								}
								secondOwner = building_Bed.OwnersForReading[i];
							}
						}
					}
				}
				if (pawn != null)
				{
					if (secondOwner == null)
					{
						yield return pawn;
					}
					else if (LovePartnerRelationUtility.LovePartnerRelationExists(pawn, secondOwner))
					{
						yield return pawn;
						yield return secondOwner;
					}
				}
				yield break;
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000EE5 RID: 3813 RVA: 0x00054313 File Offset: 0x00052513
		public IEnumerable<Building_Bed> ContainedBeds
		{
			get
			{
				List<Thing> things = this.ContainedAndAdjacentThings;
				int num;
				for (int i = 0; i < things.Count; i = num + 1)
				{
					Building_Bed building_Bed = things[i] as Building_Bed;
					if (building_Bed != null)
					{
						yield return building_Bed;
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000EE6 RID: 3814 RVA: 0x00054323 File Offset: 0x00052523
		public bool Fogged
		{
			get
			{
				return this.RegionCount != 0 && this.FirstRegion.AnyCell.Fogged(this.Map);
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000EE7 RID: 3815 RVA: 0x00054345 File Offset: 0x00052545
		public bool IsDoorway
		{
			get
			{
				return this.districts.Count == 1 && this.districts[0].IsDoorway;
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000EE8 RID: 3816 RVA: 0x00054368 File Offset: 0x00052568
		public List<Thing> ContainedAndAdjacentThings
		{
			get
			{
				this.uniqueContainedThingsSet.Clear();
				this.uniqueContainedThings.Clear();
				List<Region> regions = this.Regions;
				for (int i = 0; i < regions.Count; i++)
				{
					List<Thing> allThings = regions[i].ListerThings.AllThings;
					if (allThings != null)
					{
						for (int j = 0; j < allThings.Count; j++)
						{
							Thing item = allThings[j];
							if (this.uniqueContainedThingsSet.Add(item))
							{
								this.uniqueContainedThings.Add(item);
							}
						}
					}
				}
				this.uniqueContainedThingsSet.Clear();
				return this.uniqueContainedThings;
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000EE9 RID: 3817 RVA: 0x000543FF File Offset: 0x000525FF
		public RoomRoleDef Role
		{
			get
			{
				if (this.statsAndRoleDirty)
				{
					this.UpdateRoomStatsAndRole();
				}
				return this.role;
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000EEA RID: 3818 RVA: 0x00054418 File Offset: 0x00052618
		public bool AnyPassable
		{
			get
			{
				for (int i = 0; i < this.districts.Count; i++)
				{
					if (this.districts[i].Passable)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000EEB RID: 3819 RVA: 0x00054454 File Offset: 0x00052654
		public bool ProperRoom
		{
			get
			{
				if (this.TouchesMapEdge)
				{
					return false;
				}
				for (int i = 0; i < this.districts.Count; i++)
				{
					if (this.districts[i].RegionType == RegionType.Normal)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x00054498 File Offset: 0x00052698
		public static Room MakeNew(Map map)
		{
			Room room = new Room();
			room.ID = Room.nextRoomID;
			room.tempTracker = new RoomTempTracker(room, map);
			Room.nextRoomID++;
			return room;
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x000544C4 File Offset: 0x000526C4
		public void AddDistrict(District district)
		{
			if (this.districts.Contains(district))
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to add the same district twice to Room. district=",
					district,
					", room=",
					this
				}));
				return;
			}
			this.districts.Add(district);
			if (this.districts.Count == 1)
			{
				this.Map.regionGrid.allRooms.Add(this);
			}
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x00054538 File Offset: 0x00052738
		public void RemoveDistrict(District district)
		{
			if (!this.districts.Contains(district))
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to remove district from Room but this district is not here. district=",
					district,
					", room=",
					this
				}));
				return;
			}
			Map map = this.Map;
			this.districts.Remove(district);
			if (this.districts.Count == 0)
			{
				map.regionGrid.allRooms.Remove(this);
			}
			this.statsAndRoleDirty = true;
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x000545B6 File Offset: 0x000527B6
		public bool PushHeat(float energy)
		{
			if (this.UsesOutdoorTemperature)
			{
				return false;
			}
			this.Temperature += energy / (float)this.CellCount;
			return true;
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x000545DC File Offset: 0x000527DC
		public void Notify_ContainedThingSpawnedOrDespawned(Thing th)
		{
			if (th.def.category != ThingCategory.Mote && th.def.category != ThingCategory.Projectile && th.def.category != ThingCategory.Ethereal && th.def.category != ThingCategory.Pawn)
			{
				if (this.IsDoorway)
				{
					List<Region> regions = this.districts[0].Regions;
					for (int i = 0; i < regions[0].links.Count; i++)
					{
						Region otherRegion = regions[0].links[i].GetOtherRegion(regions[0]);
						if (otherRegion != null && !otherRegion.IsDoorway)
						{
							otherRegion.Room.Notify_ContainedThingSpawnedOrDespawned(th);
						}
					}
				}
				this.statsAndRoleDirty = true;
			}
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x0005469F File Offset: 0x0005289F
		public void Notify_TerrainChanged()
		{
			this.statsAndRoleDirty = true;
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x0005469F File Offset: 0x0005289F
		public void Notify_BedTypeChanged()
		{
			this.statsAndRoleDirty = true;
		}

		// Token: 0x06000EF3 RID: 3827 RVA: 0x000546A8 File Offset: 0x000528A8
		public void Notify_RoofChanged()
		{
			this.cachedOpenRoofCount = -1;
			this.tempTracker.RoofChanged();
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x000546BC File Offset: 0x000528BC
		public void Notify_RoomShapeChanged()
		{
			this.cachedCellCount = -1;
			this.cachedOpenRoofCount = -1;
			if (this.Dereferenced)
			{
				this.isPrisonCell = false;
				this.statsAndRoleDirty = true;
				return;
			}
			this.tempTracker.RoomChanged();
			if (Current.ProgramState == ProgramState.Playing && !this.Fogged)
			{
				this.Map.autoBuildRoofAreaSetter.TryGenerateAreaFor(this);
			}
			this.isPrisonCell = false;
			if (Building_Bed.RoomCanBePrisonCell(this))
			{
				List<Thing> containedAndAdjacentThings = this.ContainedAndAdjacentThings;
				for (int i = 0; i < containedAndAdjacentThings.Count; i++)
				{
					Building_Bed building_Bed = containedAndAdjacentThings[i] as Building_Bed;
					if (building_Bed != null && building_Bed.ForPrisoners)
					{
						this.isPrisonCell = true;
						break;
					}
				}
			}
			List<Thing> list = this.Map.listerThings.ThingsOfDef(ThingDefOf.NutrientPasteDispenser);
			for (int j = 0; j < list.Count; j++)
			{
				list[j].Notify_ColorChanged();
			}
			if (Current.ProgramState == ProgramState.Playing && this.isPrisonCell)
			{
				foreach (Building_Bed building_Bed2 in this.ContainedBeds)
				{
					building_Bed2.ForPrisoners = true;
				}
			}
			this.statsAndRoleDirty = true;
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x000547F4 File Offset: 0x000529F4
		public bool ContainsCell(IntVec3 cell)
		{
			return this.Map != null && cell.GetRoom(this.Map) == this;
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x00054810 File Offset: 0x00052A10
		public bool ContainsThing(ThingDef def)
		{
			List<Region> regions = this.Regions;
			for (int i = 0; i < regions.Count; i++)
			{
				if (regions[i].ListerThings.ThingsOfDef(def).Any<Thing>())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x00054851 File Offset: 0x00052A51
		public IEnumerable<Thing> ContainedThings(ThingDef def)
		{
			this.uniqueContainedThingsOfDef.Clear();
			List<Region> regions = this.Regions;
			int num;
			for (int i = 0; i < regions.Count; i = num)
			{
				List<Thing> things = regions[i].ListerThings.ThingsOfDef(def);
				for (int j = 0; j < things.Count; j = num)
				{
					if (this.uniqueContainedThingsOfDef.Add(things[j]))
					{
						yield return things[j];
					}
					num = j + 1;
				}
				things = null;
				num = i + 1;
			}
			this.uniqueContainedThingsOfDef.Clear();
			yield break;
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x00054868 File Offset: 0x00052A68
		public int ThingCount(ThingDef def)
		{
			this.uniqueContainedThingsOfDef.Clear();
			List<Region> regions = this.Regions;
			int num = 0;
			for (int i = 0; i < regions.Count; i++)
			{
				List<Thing> list = regions[i].ListerThings.ThingsOfDef(def);
				for (int j = 0; j < list.Count; j++)
				{
					if (this.uniqueContainedThingsOfDef.Add(list[j]))
					{
						num += list[j].stackCount;
					}
				}
			}
			this.uniqueContainedThingsOfDef.Clear();
			return num;
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x000548F3 File Offset: 0x00052AF3
		public float GetStat(RoomStatDef roomStat)
		{
			if (this.statsAndRoleDirty)
			{
				this.UpdateRoomStatsAndRole();
			}
			if (this.stats == null)
			{
				return roomStat.roomlessScore;
			}
			return this.stats[roomStat];
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x00054920 File Offset: 0x00052B20
		public void DrawFieldEdges()
		{
			Room.fields.Clear();
			Room.fields.AddRange(this.Cells);
			Color color = this.isPrisonCell ? Room.PrisonFieldColor : Room.NonPrisonFieldColor;
			color.a = Pulser.PulseBrightness(1f, 0.6f);
			GenDraw.DrawFieldEdges(Room.fields, color, null);
			Room.fields.Clear();
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x00054990 File Offset: 0x00052B90
		private void UpdateRoomStatsAndRole()
		{
			this.statsAndRoleDirty = false;
			if (this.ProperRoom && this.RegionCount <= 36)
			{
				if (this.stats == null)
				{
					this.stats = new DefMap<RoomStatDef, float>();
				}
				foreach (RoomStatDef roomStatDef in from x in DefDatabase<RoomStatDef>.AllDefs
				orderby x.updatePriority descending
				select x)
				{
					this.stats[roomStatDef] = roomStatDef.Worker.GetScore(this);
				}
				this.role = DefDatabase<RoomRoleDef>.AllDefs.MaxBy((RoomRoleDef x) => x.Worker.GetScore(this));
				return;
			}
			this.stats = null;
			this.role = RoomRoleDefOf.None;
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x00054A74 File Offset: 0x00052C74
		public string DebugString()
		{
			return string.Concat(new object[]
			{
				"Room ID=",
				this.ID,
				"\n  first cell=",
				this.Cells.FirstOrDefault<IntVec3>(),
				"\n  DistrictCount=",
				this.DistrictCount,
				"\n  RegionCount=",
				this.RegionCount,
				"\n  CellCount=",
				this.CellCount,
				"\n  OpenRoofCount=",
				this.OpenRoofCount,
				"\n  PsychologicallyOutdoors=",
				this.PsychologicallyOutdoors.ToString(),
				"\n  OutdoorsForWork=",
				this.OutdoorsForWork.ToString(),
				"\n  WellEnclosed=",
				this.ProperRoom.ToString(),
				"\n  ",
				this.tempTracker.DebugString(),
				DebugViewSettings.writeRoomRoles ? ("\n" + this.DebugRolesString()) : ""
			});
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x00054BA4 File Offset: 0x00052DA4
		private string DebugRolesString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ValueTuple<float, RoomRoleDef> valueTuple in from x in DefDatabase<RoomRoleDef>.AllDefs
			select new ValueTuple<float, RoomRoleDef>(x.Worker.GetScore(this), x) into tuple
			orderby tuple.Item1 descending
			select tuple)
			{
				float item = valueTuple.Item1;
				RoomRoleDef item2 = valueTuple.Item2;
				stringBuilder.AppendLine(string.Format("{0}: {1}", item, item2));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x00054C50 File Offset: 0x00052E50
		internal void DebugDraw()
		{
			int num = Gen.HashCombineInt(this.GetHashCode(), 1948571531);
			foreach (IntVec3 c in this.Cells)
			{
				CellRenderer.RenderCell(c, (float)num * 0.01f);
			}
			this.tempTracker.DebugDraw();
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x00054CC0 File Offset: 0x00052EC0
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Room(roomID=",
				this.ID,
				", first=",
				this.Cells.FirstOrDefault<IntVec3>().ToString(),
				", RegionsCount=",
				this.RegionCount.ToString(),
				")"
			});
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x00054D33 File Offset: 0x00052F33
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.ID, 1538478891);
		}

		// Token: 0x04000BE4 RID: 3044
		public int ID = -1;

		// Token: 0x04000BE5 RID: 3045
		private List<District> districts = new List<District>();

		// Token: 0x04000BE6 RID: 3046
		private RoomTempTracker tempTracker;

		// Token: 0x04000BE7 RID: 3047
		private int cachedOpenRoofCount = -1;

		// Token: 0x04000BE8 RID: 3048
		private int cachedCellCount = -1;

		// Token: 0x04000BE9 RID: 3049
		private bool isPrisonCell;

		// Token: 0x04000BEA RID: 3050
		private bool statsAndRoleDirty = true;

		// Token: 0x04000BEB RID: 3051
		private DefMap<RoomStatDef, float> stats = new DefMap<RoomStatDef, float>();

		// Token: 0x04000BEC RID: 3052
		private RoomRoleDef role;

		// Token: 0x04000BED RID: 3053
		private static int nextRoomID;

		// Token: 0x04000BEE RID: 3054
		private const int RegionCountHuge = 60;

		// Token: 0x04000BEF RID: 3055
		private const float UseOutdoorTemperatureUnroofedFraction = 0.25f;

		// Token: 0x04000BF0 RID: 3056
		private const int MaxRegionsToAssignRoomRole = 36;

		// Token: 0x04000BF1 RID: 3057
		private static readonly Color PrisonFieldColor = new Color(1f, 0.7f, 0.2f);

		// Token: 0x04000BF2 RID: 3058
		private static readonly Color NonPrisonFieldColor = new Color(0.3f, 0.3f, 1f);

		// Token: 0x04000BF3 RID: 3059
		private List<Region> tmpRegions = new List<Region>();

		// Token: 0x04000BF4 RID: 3060
		private HashSet<Thing> uniqueContainedThingsSet = new HashSet<Thing>();

		// Token: 0x04000BF5 RID: 3061
		private List<Thing> uniqueContainedThings = new List<Thing>();

		// Token: 0x04000BF6 RID: 3062
		private HashSet<Thing> uniqueContainedThingsOfDef = new HashSet<Thing>();

		// Token: 0x04000BF7 RID: 3063
		private static List<IntVec3> fields = new List<IntVec3>();
	}
}
