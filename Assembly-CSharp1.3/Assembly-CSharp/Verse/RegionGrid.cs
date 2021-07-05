using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000201 RID: 513
	public sealed class RegionGrid
	{
		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000E8B RID: 3723 RVA: 0x00052AD0 File Offset: 0x00050CD0
		public Region[] DirectGrid
		{
			get
			{
				if (!this.map.regionAndRoomUpdater.Enabled && this.map.regionAndRoomUpdater.AnythingToRebuild)
				{
					Log.Warning("Trying to get the region grid but RegionAndRoomUpdater is disabled. The result may be incorrect.");
				}
				this.map.regionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms();
				return this.regionGrid;
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000E8C RID: 3724 RVA: 0x00052B21 File Offset: 0x00050D21
		public IEnumerable<Region> AllRegions_NoRebuild_InvalidAllowed
		{
			get
			{
				RegionGrid.allRegionsYielded.Clear();
				try
				{
					int count = this.map.cellIndices.NumGridCells;
					int num;
					for (int i = 0; i < count; i = num + 1)
					{
						if (this.regionGrid[i] != null && !RegionGrid.allRegionsYielded.Contains(this.regionGrid[i]))
						{
							yield return this.regionGrid[i];
							RegionGrid.allRegionsYielded.Add(this.regionGrid[i]);
						}
						num = i;
					}
				}
				finally
				{
					RegionGrid.allRegionsYielded.Clear();
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000E8D RID: 3725 RVA: 0x00052B31 File Offset: 0x00050D31
		public IEnumerable<Region> AllRegions
		{
			get
			{
				if (!this.map.regionAndRoomUpdater.Enabled && this.map.regionAndRoomUpdater.AnythingToRebuild)
				{
					Log.Warning("Trying to get all valid regions but RegionAndRoomUpdater is disabled. The result may be incorrect.");
				}
				this.map.regionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms();
				RegionGrid.allRegionsYielded.Clear();
				try
				{
					int count = this.map.cellIndices.NumGridCells;
					int num;
					for (int i = 0; i < count; i = num + 1)
					{
						if (this.regionGrid[i] != null && this.regionGrid[i].valid && !RegionGrid.allRegionsYielded.Contains(this.regionGrid[i]))
						{
							yield return this.regionGrid[i];
							RegionGrid.allRegionsYielded.Add(this.regionGrid[i]);
						}
						num = i;
					}
				}
				finally
				{
					RegionGrid.allRegionsYielded.Clear();
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x00052B44 File Offset: 0x00050D44
		public RegionGrid(Map map)
		{
			this.map = map;
			this.regionGrid = new Region[map.cellIndices.NumGridCells];
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x00052B98 File Offset: 0x00050D98
		public Region GetValidRegionAt(IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				Log.Error("Tried to get valid region out of bounds at " + c);
				return null;
			}
			if (!this.map.regionAndRoomUpdater.Enabled && this.map.regionAndRoomUpdater.AnythingToRebuild)
			{
				Log.Warning("Trying to get valid region at " + c + " but RegionAndRoomUpdater is disabled. The result may be incorrect.");
			}
			this.map.regionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms();
			Region region = this.regionGrid[this.map.cellIndices.CellToIndex(c)];
			if (region != null && region.valid)
			{
				return region;
			}
			return null;
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x00052C40 File Offset: 0x00050E40
		public Region GetValidRegionAt_NoRebuild(IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				Log.Error("Tried to get valid region out of bounds at " + c);
				return null;
			}
			Region region = this.regionGrid[this.map.cellIndices.CellToIndex(c)];
			if (region != null && region.valid)
			{
				return region;
			}
			return null;
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x00052C99 File Offset: 0x00050E99
		public Region GetRegionAt_NoRebuild_InvalidAllowed(IntVec3 c)
		{
			return this.regionGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x00052CB3 File Offset: 0x00050EB3
		public void SetRegionAt(IntVec3 c, Region reg)
		{
			this.regionGrid[this.map.cellIndices.CellToIndex(c)] = reg;
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x00052CD0 File Offset: 0x00050ED0
		public void UpdateClean()
		{
			for (int i = 0; i < 16; i++)
			{
				if (this.curCleanIndex >= this.regionGrid.Length)
				{
					this.curCleanIndex = 0;
				}
				Region region = this.regionGrid[this.curCleanIndex];
				if (region != null && !region.valid)
				{
					this.regionGrid[this.curCleanIndex] = null;
				}
				this.curCleanIndex++;
			}
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x00052D38 File Offset: 0x00050F38
		public void DebugDraw()
		{
			if (this.map != Find.CurrentMap)
			{
				return;
			}
			if (DebugViewSettings.drawRegionTraversal)
			{
				CellRect currentViewRect = Find.CameraDriver.CurrentViewRect;
				currentViewRect.ClipInsideMap(this.map);
				foreach (IntVec3 c in currentViewRect)
				{
					Region validRegionAt = this.GetValidRegionAt(c);
					if (validRegionAt != null && !this.drawnRegions.Contains(validRegionAt))
					{
						validRegionAt.DebugDraw();
						this.drawnRegions.Add(validRegionAt);
					}
				}
				this.drawnRegions.Clear();
			}
			IntVec3 intVec = UI.MouseCell();
			if (intVec.InBounds(this.map))
			{
				if (DebugViewSettings.drawDistricts)
				{
					District district = intVec.GetDistrict(this.map, RegionType.Set_Passable);
					if (district != null)
					{
						district.DebugDraw();
					}
				}
				if (DebugViewSettings.drawRooms)
				{
					Room room = intVec.GetRoom(this.map);
					if (room != null)
					{
						room.DebugDraw();
					}
				}
				if (DebugViewSettings.drawRegions || DebugViewSettings.drawRegionLinks || DebugViewSettings.drawRegionThings)
				{
					Region regionAt_NoRebuild_InvalidAllowed = this.GetRegionAt_NoRebuild_InvalidAllowed(intVec);
					if (regionAt_NoRebuild_InvalidAllowed != null)
					{
						regionAt_NoRebuild_InvalidAllowed.DebugDrawMouseover();
					}
				}
			}
		}

		// Token: 0x04000BBF RID: 3007
		private Map map;

		// Token: 0x04000BC0 RID: 3008
		private Region[] regionGrid;

		// Token: 0x04000BC1 RID: 3009
		private int curCleanIndex;

		// Token: 0x04000BC2 RID: 3010
		public List<District> allDistricts = new List<District>();

		// Token: 0x04000BC3 RID: 3011
		public List<Room> allRooms = new List<Room>();

		// Token: 0x04000BC4 RID: 3012
		public static HashSet<Region> allRegionsYielded = new HashSet<Region>();

		// Token: 0x04000BC5 RID: 3013
		private const int CleanSquaresPerFrame = 16;

		// Token: 0x04000BC6 RID: 3014
		public HashSet<Region> drawnRegions = new HashSet<Region>();
	}
}
