using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x020001FF RID: 511
	public class RegionAndRoomUpdater
	{
		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000E71 RID: 3697 RVA: 0x00051ABA File Offset: 0x0004FCBA
		// (set) Token: 0x06000E72 RID: 3698 RVA: 0x00051AC2 File Offset: 0x0004FCC2
		public bool Enabled
		{
			get
			{
				return this.enabledInt;
			}
			set
			{
				this.enabledInt = value;
			}
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000E73 RID: 3699 RVA: 0x00051ACB File Offset: 0x0004FCCB
		public bool AnythingToRebuild
		{
			get
			{
				return this.map.regionDirtyer.AnyDirty || !this.initialized;
			}
		}

		// Token: 0x06000E74 RID: 3700 RVA: 0x00051AEC File Offset: 0x0004FCEC
		public RegionAndRoomUpdater(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000E75 RID: 3701 RVA: 0x00051B70 File Offset: 0x0004FD70
		public void RebuildAllRegionsAndRooms()
		{
			if (!this.Enabled)
			{
				Log.Warning("Called RebuildAllRegionsAndRooms() but RegionAndRoomUpdater is disabled. Regions won't be rebuilt.");
			}
			this.map.temperatureCache.ResetTemperatureCache();
			this.map.regionDirtyer.SetAllDirty();
			this.TryRebuildDirtyRegionsAndRooms();
		}

		// Token: 0x06000E76 RID: 3702 RVA: 0x00051BAC File Offset: 0x0004FDAC
		public void TryRebuildDirtyRegionsAndRooms()
		{
			if (this.working || !this.Enabled)
			{
				return;
			}
			this.working = true;
			if (!this.initialized)
			{
				this.RebuildAllRegionsAndRooms();
			}
			if (!this.map.regionDirtyer.AnyDirty)
			{
				this.working = false;
				return;
			}
			try
			{
				this.RegenerateNewRegionsFromDirtyCells();
				this.CreateOrUpdateRooms();
			}
			catch (Exception arg)
			{
				Log.Error("Exception while rebuilding dirty regions: " + arg);
			}
			this.newRegions.Clear();
			this.map.regionDirtyer.SetAllClean();
			this.initialized = true;
			this.working = false;
			if (DebugSettings.detectRegionListersBugs)
			{
				Autotests_RegionListers.CheckBugs(this.map);
			}
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x00051C68 File Offset: 0x0004FE68
		private void RegenerateNewRegionsFromDirtyCells()
		{
			this.newRegions.Clear();
			List<IntVec3> dirtyCells = this.map.regionDirtyer.DirtyCells;
			for (int i = 0; i < dirtyCells.Count; i++)
			{
				IntVec3 intVec = dirtyCells[i];
				if (intVec.GetRegion(this.map, RegionType.Set_All) == null)
				{
					Region region = this.map.regionMaker.TryGenerateRegionFrom(intVec);
					if (region != null)
					{
						this.newRegions.Add(region);
					}
				}
			}
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x00051CDC File Offset: 0x0004FEDC
		private void CreateOrUpdateRooms()
		{
			this.newDistricts.Clear();
			this.reusedOldDistricts.Clear();
			this.newRooms.Clear();
			this.reusedOldRooms.Clear();
			int numRegionGroups = this.CombineNewRegionsIntoContiguousGroups();
			this.CreateOrAttachToExistingDistricts(numRegionGroups);
			int numRooms = this.CombineNewAndReusedDistrictsIntoContiguousRooms();
			this.CreateOrAttachToExistingRooms(numRooms);
			this.NotifyAffectedDistrictsAndRoomsAndUpdateTemperature();
			this.newDistricts.Clear();
			this.reusedOldDistricts.Clear();
			this.newRooms.Clear();
			this.reusedOldRooms.Clear();
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x00051D64 File Offset: 0x0004FF64
		private int CombineNewRegionsIntoContiguousGroups()
		{
			int num = 0;
			for (int i = 0; i < this.newRegions.Count; i++)
			{
				if (this.newRegions[i].newRegionGroupIndex < 0)
				{
					RegionTraverser.FloodAndSetNewRegionIndex(this.newRegions[i], num);
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x00051DB4 File Offset: 0x0004FFB4
		private void CreateOrAttachToExistingDistricts(int numRegionGroups)
		{
			for (int i = 0; i < numRegionGroups; i++)
			{
				this.currentRegionGroup.Clear();
				for (int j = 0; j < this.newRegions.Count; j++)
				{
					if (this.newRegions[j].newRegionGroupIndex == i)
					{
						this.currentRegionGroup.Add(this.newRegions[j]);
					}
				}
				if (!this.currentRegionGroup[0].type.AllowsMultipleRegionsPerDistrict())
				{
					if (this.currentRegionGroup.Count != 1)
					{
						Log.Error("Region type doesn't allow multiple regions per room but there are >1 regions in this group.");
					}
					District district = District.MakeNew(this.map);
					this.currentRegionGroup[0].District = district;
					this.newDistricts.Add(district);
				}
				else
				{
					bool flag;
					District district2 = this.FindCurrentRegionGroupNeighborWithMostRegions(out flag);
					if (district2 == null)
					{
						District item = RegionTraverser.FloodAndSetDistricts(this.currentRegionGroup[0], this.map, null);
						this.newDistricts.Add(item);
					}
					else if (!flag)
					{
						for (int k = 0; k < this.currentRegionGroup.Count; k++)
						{
							this.currentRegionGroup[k].District = district2;
						}
						this.reusedOldDistricts.Add(district2);
					}
					else
					{
						RegionTraverser.FloodAndSetDistricts(this.currentRegionGroup[0], this.map, district2);
						this.reusedOldDistricts.Add(district2);
					}
				}
			}
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x00051F1C File Offset: 0x0005011C
		private int CombineNewAndReusedDistrictsIntoContiguousRooms()
		{
			int num = 0;
			foreach (District district in this.reusedOldDistricts)
			{
				district.newOrReusedRoomIndex = -1;
			}
			foreach (District district2 in this.reusedOldDistricts.Concat(this.newDistricts))
			{
				if (district2.newOrReusedRoomIndex < 0)
				{
					this.tmpDistrictStack.Clear();
					this.tmpDistrictStack.Push(district2);
					district2.newOrReusedRoomIndex = num;
					while (this.tmpDistrictStack.Count != 0)
					{
						District district3 = this.tmpDistrictStack.Pop();
						foreach (District district4 in district3.Neighbors)
						{
							if (district4.newOrReusedRoomIndex < 0 && this.ShouldBeInTheSameRoom(district3, district4))
							{
								district4.newOrReusedRoomIndex = num;
								this.tmpDistrictStack.Push(district4);
							}
						}
					}
					this.tmpDistrictStack.Clear();
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x00052074 File Offset: 0x00050274
		private void CreateOrAttachToExistingRooms(int numRooms)
		{
			for (int i = 0; i < numRooms; i++)
			{
				this.currentDistrictGroup.Clear();
				foreach (District district in this.reusedOldDistricts)
				{
					if (district.newOrReusedRoomIndex == i)
					{
						this.currentDistrictGroup.Add(district);
					}
				}
				for (int j = 0; j < this.newDistricts.Count; j++)
				{
					if (this.newDistricts[j].newOrReusedRoomIndex == i)
					{
						this.currentDistrictGroup.Add(this.newDistricts[j]);
					}
				}
				bool flag;
				Room room = this.FindCurrentRoomNeighborWithMostRegions(out flag);
				if (room == null)
				{
					Room room2 = Room.MakeNew(this.map);
					this.FloodAndSetRooms(this.currentDistrictGroup[0], room2);
					this.newRooms.Add(room2);
				}
				else if (!flag)
				{
					for (int k = 0; k < this.currentDistrictGroup.Count; k++)
					{
						this.currentDistrictGroup[k].Room = room;
					}
					this.reusedOldRooms.Add(room);
				}
				else
				{
					this.FloodAndSetRooms(this.currentDistrictGroup[0], room);
					this.reusedOldRooms.Add(room);
				}
			}
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x000521D8 File Offset: 0x000503D8
		private void FloodAndSetRooms(District start, Room room)
		{
			this.tmpDistrictStack.Clear();
			this.tmpDistrictStack.Push(start);
			this.tmpVisitedDistricts.Clear();
			this.tmpVisitedDistricts.Add(start);
			while (this.tmpDistrictStack.Count != 0)
			{
				District district = this.tmpDistrictStack.Pop();
				district.Room = room;
				foreach (District district2 in district.Neighbors)
				{
					if (!this.tmpVisitedDistricts.Contains(district2) && this.ShouldBeInTheSameRoom(district, district2))
					{
						this.tmpDistrictStack.Push(district2);
						this.tmpVisitedDistricts.Add(district2);
					}
				}
			}
			this.tmpVisitedDistricts.Clear();
			this.tmpDistrictStack.Clear();
		}

		// Token: 0x06000E7E RID: 3710 RVA: 0x000522BC File Offset: 0x000504BC
		private void NotifyAffectedDistrictsAndRoomsAndUpdateTemperature()
		{
			foreach (District district in this.reusedOldDistricts)
			{
				district.Notify_RoomShapeOrContainedBedsChanged();
			}
			for (int i = 0; i < this.newDistricts.Count; i++)
			{
				this.newDistricts[i].Notify_RoomShapeOrContainedBedsChanged();
			}
			foreach (Room room in this.reusedOldRooms)
			{
				room.Notify_RoomShapeChanged();
			}
			for (int j = 0; j < this.newRooms.Count; j++)
			{
				Room room2 = this.newRooms[j];
				room2.Notify_RoomShapeChanged();
				float temperature;
				if (this.map.temperatureCache.TryGetAverageCachedRoomTemp(room2, out temperature))
				{
					room2.Temperature = temperature;
				}
			}
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x000523BC File Offset: 0x000505BC
		private District FindCurrentRegionGroupNeighborWithMostRegions(out bool multipleOldNeighborDistricts)
		{
			multipleOldNeighborDistricts = false;
			District district = null;
			for (int i = 0; i < this.currentRegionGroup.Count; i++)
			{
				foreach (Region region in this.currentRegionGroup[i].NeighborsOfSameType)
				{
					if (region.District != null && !this.reusedOldDistricts.Contains(region.District))
					{
						if (district == null)
						{
							district = region.District;
						}
						else if (region.District != district)
						{
							multipleOldNeighborDistricts = true;
							if (region.District.RegionCount > district.RegionCount)
							{
								district = region.District;
							}
						}
					}
				}
			}
			return district;
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x0005247C File Offset: 0x0005067C
		private Room FindCurrentRoomNeighborWithMostRegions(out bool multipleOldNeighborRooms)
		{
			multipleOldNeighborRooms = false;
			Room room = null;
			for (int i = 0; i < this.currentDistrictGroup.Count; i++)
			{
				foreach (District district in this.currentDistrictGroup[i].Neighbors)
				{
					if (district.Room != null && this.ShouldBeInTheSameRoom(this.currentDistrictGroup[i], district) && !this.reusedOldRooms.Contains(district.Room))
					{
						if (room == null)
						{
							room = district.Room;
						}
						else if (district.Room != room)
						{
							multipleOldNeighborRooms = true;
							if (district.Room.RegionCount > room.RegionCount)
							{
								room = district.Room;
							}
						}
					}
				}
			}
			return room;
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x00052558 File Offset: 0x00050758
		private bool ShouldBeInTheSameRoom(District a, District b)
		{
			RegionType regionType = a.RegionType;
			RegionType regionType2 = b.RegionType;
			return (regionType == RegionType.Normal || regionType == RegionType.ImpassableFreeAirExchange || regionType == RegionType.Fence) && (regionType2 == RegionType.Normal || regionType2 == RegionType.ImpassableFreeAirExchange || regionType2 == RegionType.Fence);
		}

		// Token: 0x04000BAF RID: 2991
		private Map map;

		// Token: 0x04000BB0 RID: 2992
		private List<Region> newRegions = new List<Region>();

		// Token: 0x04000BB1 RID: 2993
		private List<District> newDistricts = new List<District>();

		// Token: 0x04000BB2 RID: 2994
		private HashSet<District> reusedOldDistricts = new HashSet<District>();

		// Token: 0x04000BB3 RID: 2995
		private List<Room> newRooms = new List<Room>();

		// Token: 0x04000BB4 RID: 2996
		private HashSet<Room> reusedOldRooms = new HashSet<Room>();

		// Token: 0x04000BB5 RID: 2997
		private List<Region> currentRegionGroup = new List<Region>();

		// Token: 0x04000BB6 RID: 2998
		private List<District> currentDistrictGroup = new List<District>();

		// Token: 0x04000BB7 RID: 2999
		private Stack<District> tmpDistrictStack = new Stack<District>();

		// Token: 0x04000BB8 RID: 3000
		private HashSet<District> tmpVisitedDistricts = new HashSet<District>();

		// Token: 0x04000BB9 RID: 3001
		private bool initialized;

		// Token: 0x04000BBA RID: 3002
		private bool working;

		// Token: 0x04000BBB RID: 3003
		private bool enabledInt = true;
	}
}
