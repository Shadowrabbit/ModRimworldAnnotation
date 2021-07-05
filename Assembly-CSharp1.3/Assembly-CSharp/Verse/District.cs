using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020001F3 RID: 499
	public sealed class District
	{
		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000E02 RID: 3586 RVA: 0x0004EE68 File Offset: 0x0004D068
		public Map Map
		{
			get
			{
				if (this.mapIndex >= 0)
				{
					return Find.Maps[(int)this.mapIndex];
				}
				return null;
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000E03 RID: 3587 RVA: 0x0004EE85 File Offset: 0x0004D085
		public RegionType RegionType
		{
			get
			{
				if (!this.regions.Any<Region>())
				{
					return RegionType.None;
				}
				return this.regions[0].type;
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000E04 RID: 3588 RVA: 0x0004EEA7 File Offset: 0x0004D0A7
		public List<Region> Regions
		{
			get
			{
				return this.regions;
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000E05 RID: 3589 RVA: 0x0004EEAF File Offset: 0x0004D0AF
		public int RegionCount
		{
			get
			{
				return this.regions.Count;
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000E06 RID: 3590 RVA: 0x0004EEBC File Offset: 0x0004D0BC
		public bool TouchesMapEdge
		{
			get
			{
				return this.numRegionsTouchingMapEdge > 0;
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000E07 RID: 3591 RVA: 0x0004EEC7 File Offset: 0x0004D0C7
		public bool Passable
		{
			get
			{
				return this.RegionType.Passable();
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000E08 RID: 3592 RVA: 0x0004EED4 File Offset: 0x0004D0D4
		public bool IsDoorway
		{
			get
			{
				return this.regions.Count == 1 && this.regions[0].IsDoorway;
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000E09 RID: 3593 RVA: 0x0004EEF7 File Offset: 0x0004D0F7
		// (set) Token: 0x06000E0A RID: 3594 RVA: 0x0004EEFF File Offset: 0x0004D0FF
		public Room Room
		{
			get
			{
				return this.roomInt;
			}
			set
			{
				if (value == this.roomInt)
				{
					return;
				}
				if (this.roomInt != null)
				{
					this.roomInt.RemoveDistrict(this);
				}
				this.roomInt = value;
				if (this.roomInt != null)
				{
					this.roomInt.AddDistrict(this);
				}
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000E0B RID: 3595 RVA: 0x0004EF3C File Offset: 0x0004D13C
		public int CellCount
		{
			get
			{
				if (this.cachedCellCount == -1)
				{
					this.cachedCellCount = 0;
					for (int i = 0; i < this.regions.Count; i++)
					{
						this.cachedCellCount += this.regions[i].CellCount;
					}
				}
				return this.cachedCellCount;
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000E0C RID: 3596 RVA: 0x0004EF94 File Offset: 0x0004D194
		public List<District> Neighbors
		{
			get
			{
				this.uniqueNeighborsSet.Clear();
				this.uniqueNeighbors.Clear();
				for (int i = 0; i < this.regions.Count; i++)
				{
					foreach (Region region in this.regions[i].Neighbors)
					{
						if (this.uniqueNeighborsSet.Add(region.District) && region.District != this)
						{
							this.uniqueNeighbors.Add(region.District);
						}
					}
				}
				this.uniqueNeighborsSet.Clear();
				return this.uniqueNeighbors;
			}
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000E0D RID: 3597 RVA: 0x0004F050 File Offset: 0x0004D250
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				int num;
				for (int i = 0; i < this.regions.Count; i = num + 1)
				{
					foreach (IntVec3 intVec in this.regions[i].Cells)
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

		// Token: 0x06000E0E RID: 3598 RVA: 0x0004F060 File Offset: 0x0004D260
		public static District MakeNew(Map map)
		{
			District district = new District();
			district.mapIndex = (sbyte)map.Index;
			district.ID = District.nextDistrictID;
			District.nextDistrictID++;
			return district;
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x0004F08C File Offset: 0x0004D28C
		public void AddRegion(Region r)
		{
			if (this.regions.Contains(r))
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to add the same region twice to District. region=",
					r,
					", district=",
					this
				}));
				return;
			}
			this.regions.Add(r);
			if (r.touchesMapEdge)
			{
				this.numRegionsTouchingMapEdge++;
			}
			if (this.regions.Count == 1)
			{
				this.Map.regionGrid.allDistricts.Add(this);
			}
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x0004F118 File Offset: 0x0004D318
		public void RemoveRegion(Region r)
		{
			if (!this.regions.Contains(r))
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to remove region from District but this region is not here. region=",
					r,
					", district=",
					this
				}));
				return;
			}
			this.regions.Remove(r);
			if (r.touchesMapEdge)
			{
				this.numRegionsTouchingMapEdge--;
			}
			if (this.regions.Count == 0)
			{
				this.Room = null;
				this.cachedOpenRoofCount = -1;
				this.cachedOpenRoofState = null;
				this.Map.regionGrid.allDistricts.Remove(this);
			}
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x0004F1B8 File Offset: 0x0004D3B8
		public void Notify_MyMapRemoved()
		{
			this.mapIndex = -1;
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x0004F1C1 File Offset: 0x0004D3C1
		public void Notify_RoofChanged()
		{
			this.cachedOpenRoofCount = -1;
			this.cachedOpenRoofState = null;
			this.Room.Notify_RoofChanged();
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x0004F1DC File Offset: 0x0004D3DC
		public void Notify_RoomShapeOrContainedBedsChanged()
		{
			this.cachedCellCount = -1;
			this.cachedOpenRoofCount = -1;
			this.cachedOpenRoofState = null;
			this.lastChangeTick = Find.TickManager.TicksGame;
			FacilitiesUtility.NotifyFacilitiesAboutChangedLOSBlockers(this.regions);
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x0004F210 File Offset: 0x0004D410
		public void DecrementMapIndex()
		{
			if (this.mapIndex <= 0)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to decrement map index for district ",
					this.ID,
					", but mapIndex=",
					this.mapIndex
				}));
				return;
			}
			this.mapIndex -= 1;
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x0004F274 File Offset: 0x0004D474
		public int OpenRoofCountStopAt(int threshold)
		{
			if (this.cachedOpenRoofCount == -1 && this.cachedOpenRoofState == null)
			{
				this.cachedOpenRoofCount = 0;
				this.cachedOpenRoofState = this.Cells.GetEnumerator();
			}
			if (this.cachedOpenRoofCount < threshold && this.cachedOpenRoofState != null)
			{
				RoofGrid roofGrid = this.Map.roofGrid;
				while (this.cachedOpenRoofCount < threshold && this.cachedOpenRoofState.MoveNext())
				{
					if (!roofGrid.Roofed(this.cachedOpenRoofState.Current))
					{
						this.cachedOpenRoofCount++;
					}
				}
				if (this.cachedOpenRoofCount < threshold)
				{
					this.cachedOpenRoofState = null;
				}
			}
			return this.cachedOpenRoofCount;
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x0004F318 File Offset: 0x0004D518
		internal void DebugDraw()
		{
			int hashCode = this.GetHashCode();
			foreach (IntVec3 c in this.Cells)
			{
				CellRenderer.RenderCell(c, (float)hashCode * 0.01f);
			}
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x0004F374 File Offset: 0x0004D574
		internal string DebugString()
		{
			return string.Concat(new object[]
			{
				"District ID=",
				this.ID,
				"\n  first cell=",
				this.Cells.FirstOrDefault<IntVec3>(),
				"\n  RegionCount=",
				this.RegionCount,
				"\n  RegionType=",
				this.RegionType,
				"\n  CellCount=",
				this.CellCount,
				"\n  OpenRoofCount=",
				this.OpenRoofCountStopAt(int.MaxValue),
				"\n  numRegionsTouchingMapEdge=",
				this.numRegionsTouchingMapEdge,
				"\n  lastChangeTick=",
				this.lastChangeTick,
				"\n  Room=",
				(this.Room != null) ? this.Room.ID.ToString() : "null"
			});
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x0004F47C File Offset: 0x0004D67C
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"District(districtID=",
				this.ID,
				", first=",
				this.Cells.FirstOrDefault<IntVec3>().ToString(),
				", RegionsCount=",
				this.RegionCount.ToString(),
				", lastChangeTick=",
				this.lastChangeTick,
				")"
			});
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x0004F506 File Offset: 0x0004D706
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.ID, 1538478890);
		}

		// Token: 0x04000B6A RID: 2922
		public sbyte mapIndex = -1;

		// Token: 0x04000B6B RID: 2923
		private Room roomInt;

		// Token: 0x04000B6C RID: 2924
		private List<Region> regions = new List<Region>();

		// Token: 0x04000B6D RID: 2925
		public int ID = -16161616;

		// Token: 0x04000B6E RID: 2926
		public int lastChangeTick = -1;

		// Token: 0x04000B6F RID: 2927
		private int numRegionsTouchingMapEdge;

		// Token: 0x04000B70 RID: 2928
		private int cachedOpenRoofCount = -1;

		// Token: 0x04000B71 RID: 2929
		private IEnumerator<IntVec3> cachedOpenRoofState;

		// Token: 0x04000B72 RID: 2930
		private int cachedCellCount = -1;

		// Token: 0x04000B73 RID: 2931
		public int newOrReusedRoomIndex = -1;

		// Token: 0x04000B74 RID: 2932
		private static int nextDistrictID;

		// Token: 0x04000B75 RID: 2933
		private HashSet<District> uniqueNeighborsSet = new HashSet<District>();

		// Token: 0x04000B76 RID: 2934
		private List<District> uniqueNeighbors = new List<District>();
	}
}
