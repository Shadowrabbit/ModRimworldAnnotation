using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002F2 RID: 754
	public class RoomGroup
	{
		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06001348 RID: 4936 RVA: 0x00013C66 File Offset: 0x00011E66
		public List<Room> Rooms
		{
			get
			{
				return this.rooms;
			}
		}

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06001349 RID: 4937 RVA: 0x00013C6E File Offset: 0x00011E6E
		public Map Map
		{
			get
			{
				if (!this.rooms.Any<Room>())
				{
					return null;
				}
				return this.rooms[0].Map;
			}
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x0600134A RID: 4938 RVA: 0x00013C90 File Offset: 0x00011E90
		public int RoomCount
		{
			get
			{
				return this.rooms.Count;
			}
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x0600134B RID: 4939 RVA: 0x00013C9D File Offset: 0x00011E9D
		public RoomGroupTempTracker TempTracker
		{
			get
			{
				return this.tempTracker;
			}
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x0600134C RID: 4940 RVA: 0x00013CA5 File Offset: 0x00011EA5
		// (set) Token: 0x0600134D RID: 4941 RVA: 0x00013CB2 File Offset: 0x00011EB2
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

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x0600134E RID: 4942 RVA: 0x00013CC0 File Offset: 0x00011EC0
		public bool UsesOutdoorTemperature
		{
			get
			{
				return this.AnyRoomTouchesMapEdge || this.OpenRoofCount >= Mathf.CeilToInt((float)this.CellCount * 0.25f);
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x0600134F RID: 4943 RVA: 0x00013CE9 File Offset: 0x00011EE9
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				int num;
				for (int i = 0; i < this.rooms.Count; i = num + 1)
				{
					foreach (IntVec3 intVec in this.rooms[i].Cells)
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

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06001350 RID: 4944 RVA: 0x000C9A28 File Offset: 0x000C7C28
		public int CellCount
		{
			get
			{
				if (this.cachedCellCount == -1)
				{
					this.cachedCellCount = 0;
					for (int i = 0; i < this.rooms.Count; i++)
					{
						this.cachedCellCount += this.rooms[i].CellCount;
					}
				}
				return this.cachedCellCount;
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06001351 RID: 4945 RVA: 0x00013CF9 File Offset: 0x00011EF9
		public IEnumerable<Region> Regions
		{
			get
			{
				int num;
				for (int i = 0; i < this.rooms.Count; i = num + 1)
				{
					foreach (Region region in this.rooms[i].Regions)
					{
						yield return region;
					}
					List<Region>.Enumerator enumerator = default(List<Region>.Enumerator);
					num = i;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06001352 RID: 4946 RVA: 0x000C9A80 File Offset: 0x000C7C80
		public int RegionCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.rooms.Count; i++)
				{
					num += this.rooms[i].RegionCount;
				}
				return num;
			}
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06001353 RID: 4947 RVA: 0x000C9ABC File Offset: 0x000C7CBC
		public int OpenRoofCount
		{
			get
			{
				if (this.cachedOpenRoofCount == -1)
				{
					this.cachedOpenRoofCount = 0;
					for (int i = 0; i < this.rooms.Count; i++)
					{
						this.cachedOpenRoofCount += this.rooms[i].OpenRoofCount;
					}
				}
				return this.cachedOpenRoofCount;
			}
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06001354 RID: 4948 RVA: 0x000C9B14 File Offset: 0x000C7D14
		public bool AnyRoomTouchesMapEdge
		{
			get
			{
				for (int i = 0; i < this.rooms.Count; i++)
				{
					if (this.rooms[i].TouchesMapEdge)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x00013D09 File Offset: 0x00011F09
		public static RoomGroup MakeNew(Map map)
		{
			RoomGroup roomGroup = new RoomGroup();
			roomGroup.ID = RoomGroup.nextRoomGroupID;
			roomGroup.tempTracker = new RoomGroupTempTracker(roomGroup, map);
			RoomGroup.nextRoomGroupID++;
			return roomGroup;
		}

		// Token: 0x06001356 RID: 4950 RVA: 0x000C9B50 File Offset: 0x000C7D50
		public void AddRoom(Room room)
		{
			if (this.rooms.Contains(room))
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to add the same room twice to RoomGroup. room=",
					room,
					", roomGroup=",
					this
				}), false);
				return;
			}
			this.rooms.Add(room);
		}

		// Token: 0x06001357 RID: 4951 RVA: 0x000C9BA4 File Offset: 0x000C7DA4
		public void RemoveRoom(Room room)
		{
			if (!this.rooms.Contains(room))
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to remove room from RoomGroup but this room is not here. room=",
					room,
					", roomGroup=",
					this
				}), false);
				return;
			}
			this.rooms.Remove(room);
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x00013D34 File Offset: 0x00011F34
		public bool PushHeat(float energy)
		{
			if (this.UsesOutdoorTemperature)
			{
				return false;
			}
			this.Temperature += energy / (float)this.CellCount;
			return true;
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x00013D57 File Offset: 0x00011F57
		public void Notify_RoofChanged()
		{
			this.cachedOpenRoofCount = -1;
			this.tempTracker.RoofChanged();
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x00013D6B File Offset: 0x00011F6B
		public void Notify_RoomGroupShapeChanged()
		{
			this.cachedCellCount = -1;
			this.cachedOpenRoofCount = -1;
			this.tempTracker.RoomChanged();
		}

		// Token: 0x0600135B RID: 4955 RVA: 0x000C9BF8 File Offset: 0x000C7DF8
		public string DebugString()
		{
			return string.Concat(new object[]
			{
				"RoomGroup ID=",
				this.ID,
				"\n  first cell=",
				this.Cells.FirstOrDefault<IntVec3>(),
				"\n  RoomCount=",
				this.RoomCount,
				"\n  RegionCount=",
				this.RegionCount,
				"\n  CellCount=",
				this.CellCount,
				"\n  OpenRoofCount=",
				this.OpenRoofCount,
				"\n  ",
				this.tempTracker.DebugString()
			});
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x000C9CB8 File Offset: 0x000C7EB8
		internal void DebugDraw()
		{
			int num = Gen.HashCombineInt(this.GetHashCode(), 1948571531);
			foreach (IntVec3 c in this.Cells)
			{
				CellRenderer.RenderCell(c, (float)num * 0.01f);
			}
			this.tempTracker.DebugDraw();
		}

		// Token: 0x04000F55 RID: 3925
		public int ID = -1;

		// Token: 0x04000F56 RID: 3926
		private List<Room> rooms = new List<Room>();

		// Token: 0x04000F57 RID: 3927
		private RoomGroupTempTracker tempTracker;

		// Token: 0x04000F58 RID: 3928
		private int cachedOpenRoofCount = -1;

		// Token: 0x04000F59 RID: 3929
		private int cachedCellCount = -1;

		// Token: 0x04000F5A RID: 3930
		private static int nextRoomGroupID;

		// Token: 0x04000F5B RID: 3931
		private const float UseOutdoorTemperatureUnroofedFraction = 0.25f;
	}
}
