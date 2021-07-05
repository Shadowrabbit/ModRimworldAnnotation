using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010BE RID: 4286
	public static class PlantPosIndices
	{
		// Token: 0x06006697 RID: 26263 RVA: 0x0022A4D8 File Offset: 0x002286D8
		static PlantPosIndices()
		{
			for (int i = 0; i < 25; i++)
			{
				PlantPosIndices.rootList[i] = new int[8][];
				for (int j = 0; j < 8; j++)
				{
					int[] array = new int[i + 1];
					for (int k = 0; k < i; k++)
					{
						array[k] = k;
					}
					array.Shuffle<int>();
					PlantPosIndices.rootList[i][j] = array;
				}
			}
		}

		// Token: 0x06006698 RID: 26264 RVA: 0x0022A540 File Offset: 0x00228740
		public static int[] GetPositionIndices(Plant p)
		{
			int maxMeshCount = p.def.plant.maxMeshCount;
			int num = (p.thingIDNumber ^ 42348528) % 8;
			return PlantPosIndices.rootList[maxMeshCount - 1][num];
		}

		// Token: 0x040039E7 RID: 14823
		private static int[][][] rootList = new int[25][][];

		// Token: 0x040039E8 RID: 14824
		private const int ListCount = 8;
	}
}
