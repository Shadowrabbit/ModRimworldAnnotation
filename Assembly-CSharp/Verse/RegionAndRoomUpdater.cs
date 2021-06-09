using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x020002D3 RID: 723
	public class RegionAndRoomUpdater
	{
		// Token: 0x17000361 RID: 865
		// (get) Token: 0x0600125A RID: 4698 RVA: 0x000133D7 File Offset: 0x000115D7
		// (set) Token: 0x0600125B RID: 4699 RVA: 0x000133DF File Offset: 0x000115DF
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

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x0600125C RID: 4700 RVA: 0x000133E8 File Offset: 0x000115E8
		public bool AnythingToRebuild
		{
			get
			{
				return this.map.regionDirtyer.AnyDirty || !this.initialized;
			}
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x000C607C File Offset: 0x000C427C
		public RegionAndRoomUpdater(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x00013407 File Offset: 0x00011607
		public void RebuildAllRegionsAndRooms()
		{
			if (!this.Enabled)
			{
				Log.Warning("Called RebuildAllRegionsAndRooms() but RegionAndRoomUpdater is disabled. Regions won't be rebuilt.", false);
			}
			this.map.temperatureCache.ResetTemperatureCache();
			this.map.regionDirtyer.SetAllDirty();
			this.TryRebuildDirtyRegionsAndRooms();
		}

		// Token: 0x0600125F RID: 4703 RVA: 0x000C6100 File Offset: 0x000C4300
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
				Log.Error("Exception while rebuilding dirty regions: " + arg, false);
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

		// Token: 0x06001260 RID: 4704 RVA: 0x000C61BC File Offset: 0x000C43BC
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

		// Token: 0x06001261 RID: 4705 RVA: 0x000C6230 File Offset: 0x000C4430
		private void CreateOrUpdateRooms()
		{
			this.newRooms.Clear();
			this.reusedOldRooms.Clear();
			this.newRoomGroups.Clear();
			this.reusedOldRoomGroups.Clear();
			int numRegionGroups = this.CombineNewRegionsIntoContiguousGroups();
			this.CreateOrAttachToExistingRooms(numRegionGroups);
			int numRoomGroups = this.CombineNewAndReusedRoomsIntoContiguousGroups();
			this.CreateOrAttachToExistingRoomGroups(numRoomGroups);
			this.NotifyAffectedRoomsAndRoomGroupsAndUpdateTemperature();
			this.newRooms.Clear();
			this.reusedOldRooms.Clear();
			this.newRoomGroups.Clear();
			this.reusedOldRoomGroups.Clear();
		}

		// Token: 0x06001262 RID: 4706 RVA: 0x000C62B8 File Offset: 0x000C44B8
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

		// Token: 0x06001263 RID: 4707 RVA: 0x000C6308 File Offset: 0x000C4508
		private void CreateOrAttachToExistingRooms(int numRegionGroups)
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
				if (!this.currentRegionGroup[0].type.AllowsMultipleRegionsPerRoom())
				{
					if (this.currentRegionGroup.Count != 1)
					{
						Log.Error("Region type doesn't allow multiple regions per room but there are >1 regions in this group.", false);
					}
					Room room = Room.MakeNew(this.map);
					this.currentRegionGroup[0].Room = room;
					this.newRooms.Add(room);
				}
				else
				{
					bool flag;
					Room room2 = this.FindCurrentRegionGroupNeighborWithMostRegions(out flag);
					if (room2 == null)
					{
						Room item = RegionTraverser.FloodAndSetRooms(this.currentRegionGroup[0], this.map, null);
						this.newRooms.Add(item);
					}
					else if (!flag)
					{
						for (int k = 0; k < this.currentRegionGroup.Count; k++)
						{
							this.currentRegionGroup[k].Room = room2;
						}
						this.reusedOldRooms.Add(room2);
					}
					else
					{
						RegionTraverser.FloodAndSetRooms(this.currentRegionGroup[0], this.map, room2);
						this.reusedOldRooms.Add(room2);
					}
				}
			}
		}

		// Token: 0x06001264 RID: 4708 RVA: 0x000C6470 File Offset: 0x000C4670
		private int CombineNewAndReusedRoomsIntoContiguousGroups()
		{
			int num = 0;
			foreach (Room room in this.reusedOldRooms)
			{
				room.newOrReusedRoomGroupIndex = -1;
			}
			foreach (Room room2 in this.reusedOldRooms.Concat(this.newRooms))
			{
				if (room2.newOrReusedRoomGroupIndex < 0)
				{
					this.tmpRoomStack.Clear();
					this.tmpRoomStack.Push(room2);
					room2.newOrReusedRoomGroupIndex = num;
					while (this.tmpRoomStack.Count != 0)
					{
						Room room3 = this.tmpRoomStack.Pop();
						foreach (Room room4 in room3.Neighbors)
						{
							if (room4.newOrReusedRoomGroupIndex < 0 && this.ShouldBeInTheSameRoomGroup(room3, room4))
							{
								room4.newOrReusedRoomGroupIndex = num;
								this.tmpRoomStack.Push(room4);
							}
						}
					}
					this.tmpRoomStack.Clear();
					num++;
				}
			}
			return num;
		}

		// Token: 0x06001265 RID: 4709 RVA: 0x000C65C8 File Offset: 0x000C47C8
		private void CreateOrAttachToExistingRoomGroups(int numRoomGroups)
		{
			for (int i = 0; i < numRoomGroups; i++)
			{
				this.currentRoomGroup.Clear();
				foreach (Room room in this.reusedOldRooms)
				{
					if (room.newOrReusedRoomGroupIndex == i)
					{
						this.currentRoomGroup.Add(room);
					}
				}
				for (int j = 0; j < this.newRooms.Count; j++)
				{
					if (this.newRooms[j].newOrReusedRoomGroupIndex == i)
					{
						this.currentRoomGroup.Add(this.newRooms[j]);
					}
				}
				bool flag;
				RoomGroup roomGroup = this.FindCurrentRoomGroupNeighborWithMostRegions(out flag);
				if (roomGroup == null)
				{
					RoomGroup roomGroup2 = RoomGroup.MakeNew(this.map);
					this.FloodAndSetRoomGroups(this.currentRoomGroup[0], roomGroup2);
					this.newRoomGroups.Add(roomGroup2);
				}
				else if (!flag)
				{
					for (int k = 0; k < this.currentRoomGroup.Count; k++)
					{
						this.currentRoomGroup[k].Group = roomGroup;
					}
					this.reusedOldRoomGroups.Add(roomGroup);
				}
				else
				{
					this.FloodAndSetRoomGroups(this.currentRoomGroup[0], roomGroup);
					this.reusedOldRoomGroups.Add(roomGroup);
				}
			}
		}

		// Token: 0x06001266 RID: 4710 RVA: 0x000C672C File Offset: 0x000C492C
		private void FloodAndSetRoomGroups(Room start, RoomGroup roomGroup)
		{
			this.tmpRoomStack.Clear();
			this.tmpRoomStack.Push(start);
			this.tmpVisitedRooms.Clear();
			this.tmpVisitedRooms.Add(start);
			while (this.tmpRoomStack.Count != 0)
			{
				Room room = this.tmpRoomStack.Pop();
				room.Group = roomGroup;
				foreach (Room room2 in room.Neighbors)
				{
					if (!this.tmpVisitedRooms.Contains(room2) && this.ShouldBeInTheSameRoomGroup(room, room2))
					{
						this.tmpRoomStack.Push(room2);
						this.tmpVisitedRooms.Add(room2);
					}
				}
			}
			this.tmpVisitedRooms.Clear();
			this.tmpRoomStack.Clear();
		}

		// Token: 0x06001267 RID: 4711 RVA: 0x000C6810 File Offset: 0x000C4A10
		private void NotifyAffectedRoomsAndRoomGroupsAndUpdateTemperature()
		{
			foreach (Room room in this.reusedOldRooms)
			{
				room.Notify_RoomShapeOrContainedBedsChanged();
			}
			for (int i = 0; i < this.newRooms.Count; i++)
			{
				this.newRooms[i].Notify_RoomShapeOrContainedBedsChanged();
			}
			foreach (RoomGroup roomGroup in this.reusedOldRoomGroups)
			{
				roomGroup.Notify_RoomGroupShapeChanged();
			}
			for (int j = 0; j < this.newRoomGroups.Count; j++)
			{
				RoomGroup roomGroup2 = this.newRoomGroups[j];
				roomGroup2.Notify_RoomGroupShapeChanged();
				float temperature;
				if (this.map.temperatureCache.TryGetAverageCachedRoomGroupTemp(roomGroup2, out temperature))
				{
					roomGroup2.Temperature = temperature;
				}
			}
		}

		// Token: 0x06001268 RID: 4712 RVA: 0x000C6910 File Offset: 0x000C4B10
		private Room FindCurrentRegionGroupNeighborWithMostRegions(out bool multipleOldNeighborRooms)
		{
			multipleOldNeighborRooms = false;
			Room room = null;
			for (int i = 0; i < this.currentRegionGroup.Count; i++)
			{
				foreach (Region region in this.currentRegionGroup[i].NeighborsOfSameType)
				{
					if (region.Room != null && !this.reusedOldRooms.Contains(region.Room))
					{
						if (room == null)
						{
							room = region.Room;
						}
						else if (region.Room != room)
						{
							multipleOldNeighborRooms = true;
							if (region.Room.RegionCount > room.RegionCount)
							{
								room = region.Room;
							}
						}
					}
				}
			}
			return room;
		}

		// Token: 0x06001269 RID: 4713 RVA: 0x000C69D0 File Offset: 0x000C4BD0
		private RoomGroup FindCurrentRoomGroupNeighborWithMostRegions(out bool multipleOldNeighborRoomGroups)
		{
			multipleOldNeighborRoomGroups = false;
			RoomGroup roomGroup = null;
			for (int i = 0; i < this.currentRoomGroup.Count; i++)
			{
				foreach (Room room in this.currentRoomGroup[i].Neighbors)
				{
					if (room.Group != null && this.ShouldBeInTheSameRoomGroup(this.currentRoomGroup[i], room) && !this.reusedOldRoomGroups.Contains(room.Group))
					{
						if (roomGroup == null)
						{
							roomGroup = room.Group;
						}
						else if (room.Group != roomGroup)
						{
							multipleOldNeighborRoomGroups = true;
							if (room.Group.RegionCount > roomGroup.RegionCount)
							{
								roomGroup = room.Group;
							}
						}
					}
				}
			}
			return roomGroup;
		}

		// Token: 0x0600126A RID: 4714 RVA: 0x000C6AAC File Offset: 0x000C4CAC
		private bool ShouldBeInTheSameRoomGroup(Room a, Room b)
		{
			RegionType regionType = a.RegionType;
			RegionType regionType2 = b.RegionType;
			return (regionType == RegionType.Normal || regionType == RegionType.ImpassableFreeAirExchange) && (regionType2 == RegionType.Normal || regionType2 == RegionType.ImpassableFreeAirExchange);
		}

		// Token: 0x04000EC3 RID: 3779
		private Map map;

		// Token: 0x04000EC4 RID: 3780
		private List<Region> newRegions = new List<Region>();

		// Token: 0x04000EC5 RID: 3781
		private List<Room> newRooms = new List<Room>();

		// Token: 0x04000EC6 RID: 3782
		private HashSet<Room> reusedOldRooms = new HashSet<Room>();

		// Token: 0x04000EC7 RID: 3783
		private List<RoomGroup> newRoomGroups = new List<RoomGroup>();

		// Token: 0x04000EC8 RID: 3784
		private HashSet<RoomGroup> reusedOldRoomGroups = new HashSet<RoomGroup>();

		// Token: 0x04000EC9 RID: 3785
		private List<Region> currentRegionGroup = new List<Region>();

		// Token: 0x04000ECA RID: 3786
		private List<Room> currentRoomGroup = new List<Room>();

		// Token: 0x04000ECB RID: 3787
		private Stack<Room> tmpRoomStack = new Stack<Room>();

		// Token: 0x04000ECC RID: 3788
		private HashSet<Room> tmpVisitedRooms = new HashSet<Room>();

		// Token: 0x04000ECD RID: 3789
		private bool initialized;

		// Token: 0x04000ECE RID: 3790
		private bool working;

		// Token: 0x04000ECF RID: 3791
		private bool enabledInt = true;
	}
}
