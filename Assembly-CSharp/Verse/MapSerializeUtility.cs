using System;

namespace Verse
{
	// Token: 0x02000304 RID: 772
	public static class MapSerializeUtility
	{
		// Token: 0x060013C5 RID: 5061 RVA: 0x000CBC30 File Offset: 0x000C9E30
		public static byte[] SerializeUshort(Map map, Func<IntVec3, ushort> shortReader)
		{
			return DataSerializeUtility.SerializeUshort(map.info.NumCells, (int idx) => shortReader(map.cellIndices.IndexToCell(idx)));
		}

		// Token: 0x060013C6 RID: 5062 RVA: 0x000CBC74 File Offset: 0x000C9E74
		public static void LoadUshort(byte[] arr, Map map, Action<IntVec3, ushort> shortWriter)
		{
			DataSerializeUtility.LoadUshort(arr, map.info.NumCells, delegate(int idx, ushort data)
			{
				shortWriter(map.cellIndices.IndexToCell(idx), data);
			});
		}
	}
}
