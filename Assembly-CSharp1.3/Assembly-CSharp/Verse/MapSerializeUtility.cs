using System;

namespace Verse
{
	// Token: 0x02000218 RID: 536
	public static class MapSerializeUtility
	{
		// Token: 0x06000F4B RID: 3915 RVA: 0x00056C70 File Offset: 0x00054E70
		public static byte[] SerializeUshort(Map map, Func<IntVec3, ushort> shortReader)
		{
			return DataSerializeUtility.SerializeUshort(map.info.NumCells, (int idx) => shortReader(map.cellIndices.IndexToCell(idx)));
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x00056CB4 File Offset: 0x00054EB4
		public static void LoadUshort(byte[] arr, Map map, Action<IntVec3, ushort> shortWriter)
		{
			DataSerializeUtility.LoadUshort(arr, map.info.NumCells, delegate(int idx, ushort data)
			{
				shortWriter(map.cellIndices.IndexToCell(idx), data);
			});
		}
	}
}
