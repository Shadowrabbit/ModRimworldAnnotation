using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200032B RID: 811
	public sealed class ZoneManager : IExposable
	{
		// Token: 0x170003CA RID: 970
		// (get) Token: 0x060014A8 RID: 5288 RVA: 0x00014CE2 File Offset: 0x00012EE2
		public List<Zone> AllZones
		{
			get
			{
				return this.allZones;
			}
		}

		// Token: 0x060014A9 RID: 5289 RVA: 0x00014CEA File Offset: 0x00012EEA
		public ZoneManager(Map map)
		{
			this.map = map;
			this.zoneGrid = new Zone[map.cellIndices.NumGridCells];
		}

		// Token: 0x060014AA RID: 5290 RVA: 0x00014D1A File Offset: 0x00012F1A
		public void ExposeData()
		{
			Scribe_Collections.Look<Zone>(ref this.allZones, "allZones", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.UpdateZoneManagerLinks();
				this.RebuildZoneGrid();
			}
		}

		// Token: 0x060014AB RID: 5291 RVA: 0x000CF6B8 File Offset: 0x000CD8B8
		private void UpdateZoneManagerLinks()
		{
			for (int i = 0; i < this.allZones.Count; i++)
			{
				this.allZones[i].zoneManager = this;
			}
		}

		// Token: 0x060014AC RID: 5292 RVA: 0x000CF6F0 File Offset: 0x000CD8F0
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

		// Token: 0x060014AD RID: 5293 RVA: 0x00014D46 File Offset: 0x00012F46
		public void RegisterZone(Zone newZone)
		{
			this.allZones.Add(newZone);
			newZone.PostRegister();
		}

		// Token: 0x060014AE RID: 5294 RVA: 0x00014D5A File Offset: 0x00012F5A
		public void DeregisterZone(Zone oldZone)
		{
			this.allZones.Remove(oldZone);
			oldZone.PostDeregister();
		}

		// Token: 0x060014AF RID: 5295 RVA: 0x00014D6F File Offset: 0x00012F6F
		internal void AddZoneGridCell(Zone zone, IntVec3 c)
		{
			this.zoneGrid[this.map.cellIndices.CellToIndex(c)] = zone;
		}

		// Token: 0x060014B0 RID: 5296 RVA: 0x00014D8A File Offset: 0x00012F8A
		internal void ClearZoneGridCell(IntVec3 c)
		{
			this.zoneGrid[this.map.cellIndices.CellToIndex(c)] = null;
		}

		// Token: 0x060014B1 RID: 5297 RVA: 0x00014DA5 File Offset: 0x00012FA5
		public Zone ZoneAt(IntVec3 c)
		{
			return this.zoneGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x060014B2 RID: 5298 RVA: 0x000CF79C File Offset: 0x000CD99C
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
			Log.Error("Ran out of zone names.", false);
			return "Zone X";
		}

		// Token: 0x060014B3 RID: 5299 RVA: 0x000CF808 File Offset: 0x000CDA08
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

		// Token: 0x04001023 RID: 4131
		public Map map;

		// Token: 0x04001024 RID: 4132
		private List<Zone> allZones = new List<Zone>();

		// Token: 0x04001025 RID: 4133
		private Zone[] zoneGrid;
	}
}
