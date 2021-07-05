using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000231 RID: 561
	public sealed class ZoneManager : IExposable
	{
		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000FED RID: 4077 RVA: 0x0005A5D4 File Offset: 0x000587D4
		public List<Zone> AllZones
		{
			get
			{
				return this.allZones;
			}
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x0005A5DC File Offset: 0x000587DC
		public ZoneManager(Map map)
		{
			this.map = map;
			this.zoneGrid = new Zone[map.cellIndices.NumGridCells];
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x0005A60C File Offset: 0x0005880C
		public void ExposeData()
		{
			Scribe_Collections.Look<Zone>(ref this.allZones, "allZones", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.UpdateZoneManagerLinks();
				this.RebuildZoneGrid();
			}
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x0005A638 File Offset: 0x00058838
		private void UpdateZoneManagerLinks()
		{
			for (int i = 0; i < this.allZones.Count; i++)
			{
				this.allZones[i].zoneManager = this;
			}
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x0005A670 File Offset: 0x00058870
		private void RebuildZoneGrid()
		{
			CellIndices cellIndices = this.map.cellIndices;
			this.zoneGrid = new Zone[cellIndices.NumGridCells];
			foreach (Zone zone in this.allZones)
			{
				foreach (IntVec3 c in zone)
				{
					this.zoneGrid[cellIndices.CellToIndex(c)] = zone;
				}
			}
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x0005A71C File Offset: 0x0005891C
		public void RegisterZone(Zone newZone)
		{
			this.allZones.Add(newZone);
			newZone.PostRegister();
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x0005A730 File Offset: 0x00058930
		public void DeregisterZone(Zone oldZone)
		{
			this.allZones.Remove(oldZone);
			oldZone.PostDeregister();
			if (Find.Selector.SelectedZone == oldZone)
			{
				Find.Selector.ClearSelection();
			}
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x0005A75C File Offset: 0x0005895C
		internal void AddZoneGridCell(Zone zone, IntVec3 c)
		{
			this.zoneGrid[this.map.cellIndices.CellToIndex(c)] = zone;
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x0005A777 File Offset: 0x00058977
		internal void ClearZoneGridCell(IntVec3 c)
		{
			this.zoneGrid[this.map.cellIndices.CellToIndex(c)] = null;
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x0005A792 File Offset: 0x00058992
		public Zone ZoneAt(IntVec3 c)
		{
			return this.zoneGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x0005A7AC File Offset: 0x000589AC
		public string NewZoneName(string nameBase)
		{
			for (int i = 1; i <= 1000; i++)
			{
				string cand = nameBase + " " + i;
				if (!this.allZones.Any((Zone z) => z.label == cand))
				{
					return cand;
				}
			}
			Log.Error("Ran out of zone names.");
			return "Zone X";
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x0005A818 File Offset: 0x00058A18
		internal void Notify_NoZoneOverlapThingSpawned(Thing thing)
		{
			CellRect cellRect = thing.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 c = new IntVec3(j, 0, i);
					Zone zone = this.ZoneAt(c);
					if (zone != null)
					{
						zone.RemoveCell(c);
						zone.CheckContiguous();
					}
				}
			}
		}

		// Token: 0x04000C79 RID: 3193
		public Map map;

		// Token: 0x04000C7A RID: 3194
		private List<Zone> allZones = new List<Zone>();

		// Token: 0x04000C7B RID: 3195
		private Zone[] zoneGrid;
	}
}
