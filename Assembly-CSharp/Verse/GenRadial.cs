using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200004E RID: 78
	public static class GenRadial
	{
		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000349 RID: 841 RVA: 0x00009244 File Offset: 0x00007444
		public static float MaxRadialPatternRadius
		{
			get
			{
				return GenRadial.RadialPatternRadii[GenRadial.RadialPatternRadii.Length - 1];
			}
		}

		// Token: 0x0600034A RID: 842 RVA: 0x00082FE4 File Offset: 0x000811E4
		static GenRadial()
		{
			GenRadial.SetupManualRadialPattern();
			GenRadial.SetupRadialPattern();
		}

		// Token: 0x0600034B RID: 843 RVA: 0x00083038 File Offset: 0x00081238
		private static void SetupManualRadialPattern()
		{
			GenRadial.ManualRadialPattern[0] = new IntVec3(0, 0, 0);
			GenRadial.ManualRadialPattern[1] = new IntVec3(0, 0, -1);
			GenRadial.ManualRadialPattern[2] = new IntVec3(1, 0, 0);
			GenRadial.ManualRadialPattern[3] = new IntVec3(0, 0, 1);
			GenRadial.ManualRadialPattern[4] = new IntVec3(-1, 0, 0);
			GenRadial.ManualRadialPattern[5] = new IntVec3(1, 0, -1);
			GenRadial.ManualRadialPattern[6] = new IntVec3(1, 0, 1);
			GenRadial.ManualRadialPattern[7] = new IntVec3(-1, 0, 1);
			GenRadial.ManualRadialPattern[8] = new IntVec3(-1, 0, -1);
			GenRadial.ManualRadialPattern[9] = new IntVec3(2, 0, 0);
			GenRadial.ManualRadialPattern[10] = new IntVec3(-2, 0, 0);
			GenRadial.ManualRadialPattern[11] = new IntVec3(0, 0, 2);
			GenRadial.ManualRadialPattern[12] = new IntVec3(0, 0, -2);
			GenRadial.ManualRadialPattern[13] = new IntVec3(2, 0, 1);
			GenRadial.ManualRadialPattern[14] = new IntVec3(2, 0, -1);
			GenRadial.ManualRadialPattern[15] = new IntVec3(-2, 0, 1);
			GenRadial.ManualRadialPattern[16] = new IntVec3(-2, 0, -1);
			GenRadial.ManualRadialPattern[17] = new IntVec3(-1, 0, 2);
			GenRadial.ManualRadialPattern[18] = new IntVec3(1, 0, 2);
			GenRadial.ManualRadialPattern[19] = new IntVec3(-1, 0, -2);
			GenRadial.ManualRadialPattern[20] = new IntVec3(1, 0, -2);
			GenRadial.ManualRadialPattern[21] = new IntVec3(2, 0, 2);
			GenRadial.ManualRadialPattern[22] = new IntVec3(-2, 0, -2);
			GenRadial.ManualRadialPattern[23] = new IntVec3(2, 0, -2);
			GenRadial.ManualRadialPattern[24] = new IntVec3(-2, 0, 2);
			GenRadial.ManualRadialPattern[25] = new IntVec3(3, 0, 0);
			GenRadial.ManualRadialPattern[26] = new IntVec3(0, 0, 3);
			GenRadial.ManualRadialPattern[27] = new IntVec3(-3, 0, 0);
			GenRadial.ManualRadialPattern[28] = new IntVec3(0, 0, -3);
			GenRadial.ManualRadialPattern[29] = new IntVec3(3, 0, 1);
			GenRadial.ManualRadialPattern[30] = new IntVec3(-3, 0, -1);
			GenRadial.ManualRadialPattern[31] = new IntVec3(1, 0, 3);
			GenRadial.ManualRadialPattern[32] = new IntVec3(-1, 0, -3);
			GenRadial.ManualRadialPattern[33] = new IntVec3(-3, 0, 1);
			GenRadial.ManualRadialPattern[34] = new IntVec3(3, 0, -1);
			GenRadial.ManualRadialPattern[35] = new IntVec3(-1, 0, 3);
			GenRadial.ManualRadialPattern[36] = new IntVec3(1, 0, -3);
			GenRadial.ManualRadialPattern[37] = new IntVec3(3, 0, 2);
			GenRadial.ManualRadialPattern[38] = new IntVec3(-3, 0, -2);
			GenRadial.ManualRadialPattern[39] = new IntVec3(2, 0, 3);
			GenRadial.ManualRadialPattern[40] = new IntVec3(-2, 0, -3);
			GenRadial.ManualRadialPattern[41] = new IntVec3(-3, 0, 2);
			GenRadial.ManualRadialPattern[42] = new IntVec3(3, 0, -2);
			GenRadial.ManualRadialPattern[43] = new IntVec3(-2, 0, 3);
			GenRadial.ManualRadialPattern[44] = new IntVec3(2, 0, -3);
			GenRadial.ManualRadialPattern[45] = new IntVec3(3, 0, 3);
			GenRadial.ManualRadialPattern[46] = new IntVec3(3, 0, -3);
			GenRadial.ManualRadialPattern[47] = new IntVec3(-3, 0, 3);
			GenRadial.ManualRadialPattern[48] = new IntVec3(-3, 0, -3);
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0008342C File Offset: 0x0008162C
		private static void SetupRadialPattern()
		{
			List<IntVec3> list = new List<IntVec3>();
			for (int i = -60; i < 60; i++)
			{
				for (int j = -60; j < 60; j++)
				{
					list.Add(new IntVec3(i, 0, j));
				}
			}
			list.Sort(delegate(IntVec3 A, IntVec3 B)
			{
				float num = (float)A.LengthHorizontalSquared;
				float num2 = (float)B.LengthHorizontalSquared;
				if (num < num2)
				{
					return -1;
				}
				if (num == num2)
				{
					return 0;
				}
				return 1;
			});
			for (int k = 0; k < 10000; k++)
			{
				GenRadial.RadialPattern[k] = list[k];
				GenRadial.RadialPatternRadii[k] = list[k].LengthHorizontal;
			}
		}

		// Token: 0x0600034D RID: 845 RVA: 0x00009255 File Offset: 0x00007455
		public static int NumCellsToFillForRadius_ManualRadialPattern(int radius)
		{
			if (radius == 0)
			{
				return 1;
			}
			if (radius == 1)
			{
				return 9;
			}
			if (radius == 2)
			{
				return 21;
			}
			if (radius == 3)
			{
				return 37;
			}
			Log.Error("NumSquares radius error", false);
			return 0;
		}

		// Token: 0x0600034E RID: 846 RVA: 0x000834C8 File Offset: 0x000816C8
		public static int NumCellsInRadius(float radius)
		{
			if (radius >= GenRadial.MaxRadialPatternRadius)
			{
				Log.Error(string.Concat(new object[]
				{
					"Not enough squares to get to radius ",
					radius,
					". Max is ",
					GenRadial.MaxRadialPatternRadius
				}), false);
				return 10000;
			}
			float num = radius + float.Epsilon;
			for (int i = 0; i < 10000; i++)
			{
				if (GenRadial.RadialPatternRadii[i] > num)
				{
					return i;
				}
			}
			return 10000;
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000927D File Offset: 0x0000747D
		public static float RadiusOfNumCells(int numCells)
		{
			return GenRadial.RadialPatternRadii[numCells];
		}

		// Token: 0x06000350 RID: 848 RVA: 0x00009286 File Offset: 0x00007486
		public static IEnumerable<IntVec3> RadialPatternInRadius(float radius)
		{
			int numSquares = GenRadial.NumCellsInRadius(radius);
			int num;
			for (int i = 0; i < numSquares; i = num + 1)
			{
				yield return GenRadial.RadialPattern[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x06000351 RID: 849 RVA: 0x00009296 File Offset: 0x00007496
		public static IEnumerable<IntVec3> RadialCellsAround(IntVec3 center, float radius, bool useCenter)
		{
			int numSquares = GenRadial.NumCellsInRadius(radius);
			int num;
			for (int i = useCenter ? 0 : 1; i < numSquares; i = num + 1)
			{
				yield return GenRadial.RadialPattern[i] + center;
				num = i;
			}
			yield break;
		}

		// Token: 0x06000352 RID: 850 RVA: 0x000092B4 File Offset: 0x000074B4
		public static IEnumerable<IntVec3> RadialCellsAround(IntVec3 center, float minRadius, float maxRadius)
		{
			int numSquares = GenRadial.NumCellsInRadius(maxRadius);
			int num;
			for (int i = 0; i < numSquares; i = num + 1)
			{
				if (GenRadial.RadialPattern[i].LengthHorizontal >= minRadius)
				{
					yield return GenRadial.RadialPattern[i] + center;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06000353 RID: 851 RVA: 0x000092D2 File Offset: 0x000074D2
		public static IEnumerable<Thing> RadialDistinctThingsAround(IntVec3 center, Map map, float radius, bool useCenter)
		{
			int numCells = GenRadial.NumCellsInRadius(radius);
			HashSet<Thing> returnedThings = null;
			int num;
			for (int i = useCenter ? 0 : 1; i < numCells; i = num + 1)
			{
				IntVec3 c = GenRadial.RadialPattern[i] + center;
				if (c.InBounds(map))
				{
					List<Thing> thingList = c.GetThingList(map);
					int j = 0;
					while (j < thingList.Count)
					{
						Thing thing = thingList[j];
						if (thing.def.size.x <= 1 && thing.def.size.z <= 1)
						{
							goto IL_FA;
						}
						if (returnedThings == null)
						{
							returnedThings = new HashSet<Thing>();
						}
						if (!returnedThings.Contains(thing))
						{
							returnedThings.Add(thing);
							goto IL_FA;
						}
						IL_111:
						num = j;
						j = num + 1;
						continue;
						IL_FA:
						yield return thing;
						goto IL_111;
					}
					thingList = null;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06000354 RID: 852 RVA: 0x00083544 File Offset: 0x00081744
		public static void ProcessEquidistantCells(IntVec3 center, float radius, Func<List<IntVec3>, bool> processor, Map map = null)
		{
			if (GenRadial.working)
			{
				Log.Error("Nested calls to ProcessEquidistantCells() are not allowed.", false);
				return;
			}
			GenRadial.tmpCells.Clear();
			GenRadial.working = true;
			try
			{
				float num = -1f;
				int num2 = GenRadial.NumCellsInRadius(radius);
				for (int i = 0; i < num2; i++)
				{
					IntVec3 intVec = center + GenRadial.RadialPattern[i];
					if (map == null || intVec.InBounds(map))
					{
						float num3 = (float)intVec.DistanceToSquared(center);
						if (Mathf.Abs(num3 - num) > 0.0001f)
						{
							if (GenRadial.tmpCells.Any<IntVec3>() && processor(GenRadial.tmpCells))
							{
								return;
							}
							num = num3;
							GenRadial.tmpCells.Clear();
						}
						GenRadial.tmpCells.Add(intVec);
					}
				}
				if (GenRadial.tmpCells.Any<IntVec3>())
				{
					processor(GenRadial.tmpCells);
				}
			}
			finally
			{
				GenRadial.tmpCells.Clear();
				GenRadial.working = false;
			}
		}

		// Token: 0x0400014F RID: 335
		public static IntVec3[] ManualRadialPattern = new IntVec3[49];

		// Token: 0x04000150 RID: 336
		public static IntVec3[] RadialPattern = new IntVec3[10000];

		// Token: 0x04000151 RID: 337
		private static float[] RadialPatternRadii = new float[10000];

		// Token: 0x04000152 RID: 338
		private const int RadialPatternCount = 10000;

		// Token: 0x04000153 RID: 339
		private static List<IntVec3> tmpCells = new List<IntVec3>();

		// Token: 0x04000154 RID: 340
		private static bool working = false;
	}
}
