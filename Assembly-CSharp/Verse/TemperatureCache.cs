using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200030E RID: 782
	public sealed class TemperatureCache : IExposable
	{
		// Token: 0x060013FE RID: 5118 RVA: 0x000CCD20 File Offset: 0x000CAF20
		public TemperatureCache(Map map)
		{
			this.map = map;
			this.tempCache = new CachedTempInfo[map.cellIndices.NumGridCells];
			this.temperatureSaveLoad = new TemperatureSaveLoad(map);
		}

		// Token: 0x060013FF RID: 5119 RVA: 0x000CCD74 File Offset: 0x000CAF74
		public void ResetTemperatureCache()
		{
			int numGridCells = this.map.cellIndices.NumGridCells;
			for (int i = 0; i < numGridCells; i++)
			{
				this.tempCache[i].Reset();
			}
		}

		// Token: 0x06001400 RID: 5120 RVA: 0x000145E2 File Offset: 0x000127E2
		public void ExposeData()
		{
			this.temperatureSaveLoad.DoExposeWork();
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x000145EF File Offset: 0x000127EF
		public void ResetCachedCellInfo(IntVec3 c)
		{
			this.tempCache[this.map.cellIndices.CellToIndex(c)].Reset();
		}

		// Token: 0x06001402 RID: 5122 RVA: 0x00014612 File Offset: 0x00012812
		private void SetCachedCellInfo(IntVec3 c, CachedTempInfo info)
		{
			this.tempCache[this.map.cellIndices.CellToIndex(c)] = info;
		}

		// Token: 0x06001403 RID: 5123 RVA: 0x000CCDB0 File Offset: 0x000CAFB0
		public void TryCacheRegionTempInfo(IntVec3 c, Region reg)
		{
			Room room = reg.Room;
			if (room != null)
			{
				RoomGroup group = room.Group;
				this.SetCachedCellInfo(c, new CachedTempInfo(group.ID, group.CellCount, group.Temperature));
			}
		}

		// Token: 0x06001404 RID: 5124 RVA: 0x000CCDEC File Offset: 0x000CAFEC
		public bool TryGetAverageCachedRoomGroupTemp(RoomGroup r, out float result)
		{
			CellIndices cellIndices = this.map.cellIndices;
			foreach (IntVec3 c in r.Cells)
			{
				CachedTempInfo cachedTempInfo = this.map.temperatureCache.tempCache[cellIndices.CellToIndex(c)];
				if (cachedTempInfo.numCells > 0 && !this.processedRoomGroupIDs.Contains(cachedTempInfo.roomGroupID))
				{
					this.relevantTempInfoList.Add(cachedTempInfo);
					this.processedRoomGroupIDs.Add(cachedTempInfo.roomGroupID);
				}
			}
			int num = 0;
			float num2 = 0f;
			foreach (CachedTempInfo cachedTempInfo2 in this.relevantTempInfoList)
			{
				num += cachedTempInfo2.numCells;
				num2 += cachedTempInfo2.temperature * (float)cachedTempInfo2.numCells;
			}
			result = num2 / (float)num;
			bool result2 = !this.relevantTempInfoList.NullOrEmpty<CachedTempInfo>();
			this.processedRoomGroupIDs.Clear();
			this.relevantTempInfoList.Clear();
			return result2;
		}

		// Token: 0x04000FB1 RID: 4017
		private Map map;

		// Token: 0x04000FB2 RID: 4018
		internal TemperatureSaveLoad temperatureSaveLoad;

		// Token: 0x04000FB3 RID: 4019
		public CachedTempInfo[] tempCache;

		// Token: 0x04000FB4 RID: 4020
		private HashSet<int> processedRoomGroupIDs = new HashSet<int>();

		// Token: 0x04000FB5 RID: 4021
		private List<CachedTempInfo> relevantTempInfoList = new List<CachedTempInfo>();
	}
}
