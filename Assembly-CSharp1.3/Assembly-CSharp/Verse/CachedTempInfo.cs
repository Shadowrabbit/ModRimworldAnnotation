using System;

namespace Verse
{
	// Token: 0x0200021D RID: 541
	public struct CachedTempInfo
	{
		// Token: 0x06000F77 RID: 3959 RVA: 0x00057EFC File Offset: 0x000560FC
		public static CachedTempInfo NewCachedTempInfo()
		{
			CachedTempInfo result = default(CachedTempInfo);
			result.Reset();
			return result;
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x00057F19 File Offset: 0x00056119
		public void Reset()
		{
			this.roomID = -1;
			this.numCells = 0;
			this.temperature = 0f;
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x00057F34 File Offset: 0x00056134
		public CachedTempInfo(int roomID, int numCells, float temperature)
		{
			this.roomID = roomID;
			this.numCells = numCells;
			this.temperature = temperature;
		}

		// Token: 0x04000C24 RID: 3108
		public int roomID;

		// Token: 0x04000C25 RID: 3109
		public int numCells;

		// Token: 0x04000C26 RID: 3110
		public float temperature;
	}
}
