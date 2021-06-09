using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200171D RID: 5917
	public static class PlantPosIndices
	{
		// Token: 0x06008290 RID: 33424 RVA: 0x0026B668 File Offset: 0x00269868
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

		// Token: 0x06008291 RID: 33425 RVA: 0x0026B6D0 File Offset: 0x002698D0
		public static int[] GetPositionIndices(Plant p)
		{
			int maxMeshCount = p.def.plant.maxMeshCount;
			int num = (p.thingIDNumber ^ 42348528) % 8;
			return PlantPosIndices.rootList[maxMeshCount - 1][num];
		}

		// Token: 0x0400549F RID: 21663
		private static int[][][] rootList = new int[25][][];

		// Token: 0x040054A0 RID: 21664
		private const int ListCount = 8;
	}
}
