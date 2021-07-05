using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C64 RID: 3172
	public class ComplexLayout : IExposable
	{
		// Token: 0x17000CDE RID: 3294
		// (get) Token: 0x06004A17 RID: 18967 RVA: 0x00187421 File Offset: 0x00185621
		public List<ComplexRoom> Rooms
		{
			get
			{
				return this.rooms;
			}
		}

		// Token: 0x17000CDF RID: 3295
		// (get) Token: 0x06004A18 RID: 18968 RVA: 0x0018742C File Offset: 0x0018562C
		public int Area
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.rooms.Count; i++)
				{
					num += this.rooms[i].Area;
				}
				return num;
			}
		}

		// Token: 0x06004A1A RID: 18970 RVA: 0x0018747C File Offset: 0x0018567C
		public void Init(CellRect container)
		{
			this.container = container;
			this.cellTypes = new RoomLayoutCellType[container.Width, container.Height];
			this.roomIds = new int[container.Width, container.Height];
			for (int i = 0; i < container.Width; i++)
			{
				for (int j = 0; j < container.Height; j++)
				{
					this.roomIds[i, j] = -1;
				}
			}
		}

		// Token: 0x06004A1B RID: 18971 RVA: 0x001874F4 File Offset: 0x001856F4
		public void AddRoom(List<CellRect> rects)
		{
			for (int i = 0; i < rects.Count; i++)
			{
				rects[i] = rects[i].ClipInsideRect(this.container);
			}
			this.rooms.Add(new ComplexRoom(rects));
		}

		// Token: 0x06004A1C RID: 18972 RVA: 0x0018753F File Offset: 0x0018573F
		public void Add(IntVec3 position, RoomLayoutCellType cellType)
		{
			if (this.cellTypes.InBounds(position.x, position.z))
			{
				this.cellTypes[position.x, position.z] = cellType;
			}
		}

		// Token: 0x06004A1D RID: 18973 RVA: 0x00187572 File Offset: 0x00185772
		public bool IsWallAt(IntVec3 position)
		{
			return this.cellTypes.InBounds(position.x, position.z) && this.cellTypes[position.x, position.z] == RoomLayoutCellType.Wall;
		}

		// Token: 0x06004A1E RID: 18974 RVA: 0x001875A9 File Offset: 0x001857A9
		public bool IsFloorAt(IntVec3 position)
		{
			return this.cellTypes.InBounds(position.x, position.z) && this.cellTypes[position.x, position.z] == RoomLayoutCellType.Floor;
		}

		// Token: 0x06004A1F RID: 18975 RVA: 0x001875E0 File Offset: 0x001857E0
		public bool IsDoorAt(IntVec3 position)
		{
			return this.cellTypes.InBounds(position.x, position.z) && this.cellTypes[position.x, position.z] == RoomLayoutCellType.Door;
		}

		// Token: 0x06004A20 RID: 18976 RVA: 0x00187617 File Offset: 0x00185817
		public bool IsEmptyAt(IntVec3 position)
		{
			return this.cellTypes.InBounds(position.x, position.z) && this.cellTypes[position.x, position.z] == RoomLayoutCellType.Empty;
		}

		// Token: 0x06004A21 RID: 18977 RVA: 0x0018764E File Offset: 0x0018584E
		public bool IsOutside(IntVec3 position)
		{
			return !this.cellTypes.InBounds(position.x, position.z) || this.cellTypes[position.x, position.z] == RoomLayoutCellType.Empty;
		}

		// Token: 0x06004A22 RID: 18978 RVA: 0x00187685 File Offset: 0x00185885
		public int GetRoomIdAt(IntVec3 position)
		{
			if (!this.roomIds.InBounds(position.x, position.z))
			{
				return -2;
			}
			return this.roomIds[position.x, position.z];
		}

		// Token: 0x06004A23 RID: 18979 RVA: 0x001876BC File Offset: 0x001858BC
		public bool TryMinimizeLayoutWithoutDisconnection()
		{
			if (this.rooms.Count == 1)
			{
				return false;
			}
			for (int i = this.rooms.Count - 1; i >= 0; i--)
			{
				if (this.IsAdjacentToLayoutEdge(this.rooms[i]) && !this.WouldDisconnectRoomsIfRemoved(this.rooms[i]))
				{
					this.rooms.RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004A24 RID: 18980 RVA: 0x00187728 File Offset: 0x00185928
		private bool WouldDisconnectRoomsIfRemoved(ComplexRoom room)
		{
			ComplexLayout.tmpRooms.Clear();
			ComplexLayout.tmpRooms.AddRange(this.rooms);
			ComplexLayout.tmpRooms.Remove(room);
			ComplexLayout.tmpSeenRooms.Clear();
			ComplexLayout.tmpRoomQueue.Clear();
			ComplexLayout.tmpRoomQueue.Enqueue(ComplexLayout.tmpRooms.First<ComplexRoom>());
			while (ComplexLayout.tmpRoomQueue.Count > 0)
			{
				ComplexRoom complexRoom = ComplexLayout.tmpRoomQueue.Dequeue();
				ComplexLayout.tmpSeenRooms.Add(complexRoom);
				foreach (ComplexRoom complexRoom2 in ComplexLayout.tmpRooms)
				{
					if (complexRoom != complexRoom2 && !ComplexLayout.tmpSeenRooms.Contains(complexRoom2) && complexRoom.IsAdjacentTo(complexRoom2, 3))
					{
						ComplexLayout.tmpRoomQueue.Enqueue(complexRoom2);
					}
				}
			}
			int count = ComplexLayout.tmpRooms.Count;
			int count2 = ComplexLayout.tmpSeenRooms.Count;
			ComplexLayout.tmpRooms.Clear();
			ComplexLayout.tmpSeenRooms.Clear();
			return count2 != count;
		}

		// Token: 0x06004A25 RID: 18981 RVA: 0x0018783C File Offset: 0x00185A3C
		public bool IsAdjacentToLayoutEdge(ComplexRoom room)
		{
			for (int i = 0; i < room.rects.Count; i++)
			{
				if (room.rects[i].minX == this.container.minX || room.rects[i].maxX == this.container.maxX || room.rects[i].minZ == this.container.minZ || room.rects[i].maxZ == this.container.maxZ)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004A26 RID: 18982 RVA: 0x001878E0 File Offset: 0x00185AE0
		public void Finish()
		{
			for (int i = 0; i < 4; i++)
			{
				Rot4 dir = new Rot4(i);
				foreach (ComplexRoom complexRoom in this.rooms)
				{
					for (int j = 0; j < complexRoom.rects.Count; j++)
					{
						foreach (IntVec3 intVec in complexRoom.rects[j].GetEdgeCells(dir))
						{
							IntVec3 facingCell = dir.FacingCell + intVec;
							if (!this.IsWallAt(facingCell) && !complexRoom.rects.Any((CellRect r) => r.Contains(facingCell)))
							{
								this.Add(intVec, RoomLayoutCellType.Wall);
							}
						}
					}
					foreach (CellRect cellRect in complexRoom.rects)
					{
						foreach (IntVec3 intVec2 in cellRect.Cells)
						{
							this.roomIds[intVec2.x, intVec2.z] = this.currentRoomId;
							if (!this.IsWallAt(intVec2))
							{
								this.Add(intVec2, RoomLayoutCellType.Floor);
							}
						}
					}
					this.currentRoomId++;
				}
			}
			for (int k = this.container.minX; k < this.container.maxX; k++)
			{
				for (int l = this.container.minZ; l < this.container.maxZ; l++)
				{
					IntVec3 intVec3 = new IntVec3(k, 0, l);
					if (!this.IsWallAt(intVec3) && this.IsFloorAt(intVec3))
					{
						int num = 0;
						foreach (IntVec3 b in GenAdj.CardinalDirections)
						{
							if (this.IsWallAt(intVec3 + b))
							{
								num++;
							}
						}
						int num2 = 0;
						int num3 = 0;
						foreach (IntVec3 b2 in GenAdj.DiagonalDirections)
						{
							if (this.IsWallAt(intVec3 + b2))
							{
								num2++;
							}
							else if (!this.IsFloorAt(intVec3 + b2))
							{
								num3++;
							}
						}
						if (num > 1 && (num2 < 2 || num3 > 0))
						{
							this.Add(intVec3, RoomLayoutCellType.Wall);
						}
					}
				}
			}
		}

		// Token: 0x06004A27 RID: 18983 RVA: 0x00187C1C File Offset: 0x00185E1C
		public void ExposeData()
		{
			Scribe_Values.Look<CellRect>(ref this.container, "container", default(CellRect), false);
			Scribe_Collections.Look<ComplexRoom>(ref this.rooms, "rooms", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.Init(this.container);
				this.Finish();
			}
		}

		// Token: 0x04002CFC RID: 11516
		private const int MinAdjacencyForDisconnectedRoom = 3;

		// Token: 0x04002CFD RID: 11517
		public CellRect container;

		// Token: 0x04002CFE RID: 11518
		public RoomLayoutCellType[,] cellTypes;

		// Token: 0x04002CFF RID: 11519
		public int[,] roomIds;

		// Token: 0x04002D00 RID: 11520
		private int currentRoomId;

		// Token: 0x04002D01 RID: 11521
		private List<ComplexRoom> rooms = new List<ComplexRoom>();

		// Token: 0x04002D02 RID: 11522
		private static List<ComplexRoom> tmpRooms = new List<ComplexRoom>();

		// Token: 0x04002D03 RID: 11523
		private static Queue<ComplexRoom> tmpRoomQueue = new Queue<ComplexRoom>();

		// Token: 0x04002D04 RID: 11524
		private static HashSet<ComplexRoom> tmpSeenRooms = new HashSet<ComplexRoom>();
	}
}
