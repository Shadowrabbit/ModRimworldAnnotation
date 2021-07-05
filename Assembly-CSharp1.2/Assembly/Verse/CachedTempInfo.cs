using System;

namespace Verse
{
	// Token: 0x0200030D RID: 781
	public struct CachedTempInfo
	{
		// Token: 0x060013FB RID: 5115 RVA: 0x000CCD00 File Offset: 0x000CAF00
		public static CachedTempInfo NewCachedTempInfo()
		{
			CachedTempInfo result = default(CachedTempInfo);
			result.Reset();
			return result;
		}

		// Token: 0x060013FC RID: 5116 RVA: 0x000145B0 File Offset: 0x000127B0
		public void Reset()
		{
			this.roomGroupID = -1;
			this.numCells = 0;
			this.temperature = 0f;
		}

		// Token: 0x060013FD RID: 5117 RVA: 0x000145CB File Offset: 0x000127CB
		public CachedTempInfo(int roomGroupID, int numCells, float temperature)
		{
			this.roomGroupID = roomGroupID;
			this.numCells = numCells;
			this.temperature = temperature;
		}

		// Token: 0x04000FAE RID: 4014
		public int roomGroupID;

		// Token: 0x04000FAF RID: 4015
		public int numCells;

		// Token: 0x04000FB0 RID: 4016
		public float temperature;
	}
}
