using System;
using System.Collections.Generic;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x0200208C RID: 8332
	public static class PackedListOfLists
	{
		// Token: 0x0600B0A9 RID: 45225 RVA: 0x00072D47 File Offset: 0x00070F47
		public static void AddList<T>(List<int> offsets, List<T> values, List<T> listToAdd)
		{
			offsets.Add(values.Count);
			values.AddRange(listToAdd);
		}

		// Token: 0x0600B0AA RID: 45226 RVA: 0x00334574 File Offset: 0x00332774
		public static void GetList<T>(List<int> offsets, List<T> values, int listIndex, List<T> outList)
		{
			outList.Clear();
			int num = offsets[listIndex];
			int num2 = values.Count;
			if (listIndex + 1 < offsets.Count)
			{
				num2 = offsets[listIndex + 1];
			}
			for (int i = num; i < num2; i++)
			{
				outList.Add(values[i]);
			}
		}

		// Token: 0x0600B0AB RID: 45227 RVA: 0x003345C4 File Offset: 0x003327C4
		public static void GetListValuesIndices<T>(List<int> offsets, List<T> values, int listIndex, List<int> outList)
		{
			outList.Clear();
			int num = offsets[listIndex];
			int num2 = values.Count;
			if (listIndex + 1 < offsets.Count)
			{
				num2 = offsets[listIndex + 1];
			}
			for (int i = num; i < num2; i++)
			{
				outList.Add(i);
			}
		}

		// Token: 0x0600B0AC RID: 45228 RVA: 0x0033460C File Offset: 0x0033280C
		public static int GetListCount<T>(List<int> offsets, List<T> values, int listIndex)
		{
			int num = offsets[listIndex];
			int num2 = values.Count;
			if (listIndex + 1 < offsets.Count)
			{
				num2 = offsets[listIndex + 1];
			}
			return num2 - num;
		}

		// Token: 0x0600B0AD RID: 45229 RVA: 0x00334640 File Offset: 0x00332840
		public static void GenerateVertToTrisPackedList(List<Vector3> verts, List<TriangleIndices> tris, List<int> outOffsets, List<int> outValues)
		{
			outOffsets.Clear();
			outValues.Clear();
			PackedListOfLists.vertAdjacentTrisCount.Clear();
			int i = 0;
			int count = verts.Count;
			while (i < count)
			{
				PackedListOfLists.vertAdjacentTrisCount.Add(0);
				i++;
			}
			int j = 0;
			int count2 = tris.Count;
			while (j < count2)
			{
				TriangleIndices triangleIndices = tris[j];
				List<int> list = PackedListOfLists.vertAdjacentTrisCount;
				int num = triangleIndices.v1;
				int num2 = list[num];
				list[num] = num2 + 1;
				List<int> list2 = PackedListOfLists.vertAdjacentTrisCount;
				num2 = triangleIndices.v2;
				num = list2[num2];
				list2[num2] = num + 1;
				List<int> list3 = PackedListOfLists.vertAdjacentTrisCount;
				num = triangleIndices.v3;
				num2 = list3[num];
				list3[num] = num2 + 1;
				j++;
			}
			int num3 = 0;
			int k = 0;
			int count3 = verts.Count;
			while (k < count3)
			{
				outOffsets.Add(num3);
				int num4 = PackedListOfLists.vertAdjacentTrisCount[k];
				PackedListOfLists.vertAdjacentTrisCount[k] = 0;
				for (int l = 0; l < num4; l++)
				{
					outValues.Add(-1);
				}
				num3 += num4;
				k++;
			}
			int m = 0;
			int count4 = tris.Count;
			while (m < count4)
			{
				TriangleIndices triangleIndices2 = tris[m];
				outValues[outOffsets[triangleIndices2.v1] + PackedListOfLists.vertAdjacentTrisCount[triangleIndices2.v1]] = m;
				outValues[outOffsets[triangleIndices2.v2] + PackedListOfLists.vertAdjacentTrisCount[triangleIndices2.v2]] = m;
				outValues[outOffsets[triangleIndices2.v3] + PackedListOfLists.vertAdjacentTrisCount[triangleIndices2.v3]] = m;
				List<int> list4 = PackedListOfLists.vertAdjacentTrisCount;
				int num2 = triangleIndices2.v1;
				int num = list4[num2];
				list4[num2] = num + 1;
				List<int> list5 = PackedListOfLists.vertAdjacentTrisCount;
				num = triangleIndices2.v2;
				num2 = list5[num];
				list5[num] = num2 + 1;
				List<int> list6 = PackedListOfLists.vertAdjacentTrisCount;
				num2 = triangleIndices2.v3;
				num = list6[num2];
				list6[num2] = num + 1;
				m++;
			}
		}

		// Token: 0x04007997 RID: 31127
		private static List<int> vertAdjacentTrisCount = new List<int>();
	}
}
