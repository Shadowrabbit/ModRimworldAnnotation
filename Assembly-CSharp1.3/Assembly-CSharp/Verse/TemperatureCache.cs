using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200021E RID: 542
	public sealed class TemperatureCache : IExposable
	{
		// Token: 0x06000F7A RID: 3962 RVA: 0x00057F4C File Offset: 0x0005614C
		public TemperatureCache(Map map)
		{
			this.map = map;
			this.tempCache = new CachedTempInfo[map.cellIndices.NumGridCells];
			this.temperatureSaveLoad = new TemperatureSaveLoad(map);
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x00057FA0 File Offset: 0x000561A0
		public void ResetTemperatureCache()
		{
			int numGridCells = this.map.cellIndices.NumGridCells;
			for (int i = 0; i < numGridCells; i++)
			{
				this.tempCache[i].Reset();
			}
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x00057FDB File Offset: 0x000561DB
		public void ExposeData()
		{
			this.temperatureSaveLoad.DoExposeWork();
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x00057FE8 File Offset: 0x000561E8
		public void ResetCachedCellInfo(IntVec3 c)
		{
			this.tempCache[this.map.cellIndices.CellToIndex(c)].Reset();
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x0005800B File Offset: 0x0005620B
		private void SetCachedCellInfo(IntVec3 c, CachedTempInfo info)
		{
			this.tempCache[this.map.cellIndices.CellToIndex(c)] = info;
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x0005802C File Offset: 0x0005622C
		public void TryCacheRegionTempInfo(IntVec3 c, Region reg)
		{
			Room room = reg.Room;
			if (room != null)
			{
				this.SetCachedCellInfo(c, new CachedTempInfo(room.ID, room.CellCount, room.Temperature));
			}
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x00058064 File Offset: 0x00056264
		public bool TryGetAverageCachedRoomTemp(Room r, out float result)
		{
			CellIndices cellIndices = this.map.cellIndices;
			foreach (IntVec3 c in r.Cells)
			{
				CachedTempInfo cachedTempInfo = this.map.temperatureCache.tempCache[cellIndices.CellToIndex(c)];
				if (cachedTempInfo.numCells > 0 && !this.processedRoomIDs.Contains(cachedTempInfo.roomID))
				{
					this.relevantTempInfoList.Add(cachedTempInfo);
					this.processedRoomIDs.Add(cachedTempInfo.roomID);
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
			this.processedRoomIDs.Clear();
			this.relevantTempInfoList.Clear();
			return result2;
		}

		// Token: 0x04000C27 RID: 3111
		private Map map;

		// Token: 0x04000C28 RID: 3112
		internal TemperatureSaveLoad temperatureSaveLoad;

		// Token: 0x04000C29 RID: 3113
		public CachedTempInfo[] tempCache;

		// Token: 0x04000C2A RID: 3114
		private HashSet<int> processedRoomIDs = new HashSet<int>();

		// Token: 0x04000C2B RID: 3115
		private List<CachedTempInfo> relevantTempInfoList = new List<CachedTempInfo>();
	}
}
