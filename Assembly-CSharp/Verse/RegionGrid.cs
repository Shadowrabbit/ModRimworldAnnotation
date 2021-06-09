using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002D5 RID: 725
	public sealed class RegionGrid
	{
		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06001274 RID: 4724 RVA: 0x000C6FE8 File Offset: 0x000C51E8
		public Region[] DirectGrid
		{
			get
			{
				if (!this.map.regionAndRoomUpdater.Enabled && this.map.regionAndRoomUpdater.AnythingToRebuild)
				{
					Log.Warning("Trying to get the region grid but RegionAndRoomUpdater is disabled. The result may be incorrect.", false);
				}
				this.map.regionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms();
				return this.regionGrid;
			}
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06001275 RID: 4725 RVA: 0x0001347F File Offset: 0x0001167F
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

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06001276 RID: 4726 RVA: 0x0001348F File Offset: 0x0001168F
		public IEnumerable<Region> AllRegions
		{
			get
			{
				if (!this.map.regionAndRoomUpdater.Enabled && this.map.regionAndRoomUpdater.AnythingToRebuild)
				{
					Log.Warning("Trying to get all valid regions but RegionAndRoomUpdater is disabled. The result may be incorrect.", false);
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

		// Token: 0x06001277 RID: 4727 RVA: 0x0001349F File Offset: 0x0001169F
		public RegionGrid(Map map)
		{
			this.map = map;
			this.regionGrid = new Region[map.cellIndices.NumGridCells];
		}

		// Token: 0x06001278 RID: 4728 RVA: 0x000C703C File Offset: 0x000C523C
		public Region GetValidRegionAt(IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				Log.Error("Tried to get valid region out of bounds at " + c, false);
				return null;
			}
			if (!this.map.regionAndRoomUpdater.Enabled && this.map.regionAndRoomUpdater.AnythingToRebuild)
			{
				Log.Warning("Trying to get valid region at " + c + " but RegionAndRoomUpdater is disabled. The result may be incorrect.", false);
			}
			this.map.regionAndRoomUpdater.TryRebuildDirtyRegionsAndRooms();
			Region region = this.regionGrid[this.map.cellIndices.CellToIndex(c)];
			if (region != null && region.valid)
			{
				return region;
			}
			return null;
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x000C70E8 File Offset: 0x000C52E8
		public Region GetValidRegionAt_NoRebuild(IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				Log.Error("Tried to get valid region out of bounds at " + c, false);
				return null;
			}
			Region region = this.regionGrid[this.map.cellIndices.CellToIndex(c)];
			if (region != null && region.valid)
			{
				return region;
			}
			return null;
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x000134DA File Offset: 0x000116DA
		public Region GetRegionAt_NoRebuild_InvalidAllowed(IntVec3 c)
		{
			return this.regionGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x000134F4 File Offset: 0x000116F4
		public void SetRegionAt(IntVec3 c, Region reg)
		{
			this.regionGrid[this.map.cellIndices.CellToIndex(c)] = reg;
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x000C7144 File Offset: 0x000C5344
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

		// Token: 0x0600127D RID: 4733 RVA: 0x000C71AC File Offset: 0x000C53AC
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
				if (DebugViewSettings.drawRooms)
				{
					Room room = intVec.GetRoom(this.map, RegionType.Set_All);
					if (room != null)
					{
						room.DebugDraw();
					}
				}
				if (DebugViewSettings.drawRoomGroups)
				{
					RoomGroup roomGroup = intVec.GetRoomGroup(this.map);
					if (roomGroup != null)
					{
						roomGroup.DebugDraw();
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

		// Token: 0x04000ED3 RID: 3795
		private Map map;

		// Token: 0x04000ED4 RID: 3796
		private Region[] regionGrid;

		// Token: 0x04000ED5 RID: 3797
		private int curCleanIndex;

		// Token: 0x04000ED6 RID: 3798
		public List<Room> allRooms = new List<Room>();

		// Token: 0x04000ED7 RID: 3799
		public static HashSet<Region> allRegionsYielded = new HashSet<Region>();

		// Token: 0x04000ED8 RID: 3800
		private const int CleanSquaresPerFrame = 16;

		// Token: 0x04000ED9 RID: 3801
		public HashSet<Region> drawnRegions = new HashSet<Region>();
	}
}
