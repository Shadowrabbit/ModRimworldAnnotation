using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020013E7 RID: 5095
	public static class Autotests_RandomNumbers
	{
		// Token: 0x06007BEE RID: 31726 RVA: 0x002BC07B File Offset: 0x002BA27B
		public static void Run()
		{
			Log.Message("Running random numbers tests.");
			Autotests_RandomNumbers.CheckSimpleFloats();
			Autotests_RandomNumbers.CheckIntsRange();
			Autotests_RandomNumbers.CheckIntsDistribution();
			Autotests_RandomNumbers.CheckSeed();
			Log.Message("Finished.");
		}

		// Token: 0x06007BEF RID: 31727 RVA: 0x002BC0A8 File Offset: 0x002BA2A8
		private static void CheckSimpleFloats()
		{
			List<float> list = Autotests_RandomNumbers.RandomFloats(500).ToList<float>();
			if (list.Any((float x) => x < 0f || x > 1f))
			{
				Log.Error("Float out of range.");
			}
			if (list.Any((float x) => x < 0.1f))
			{
				if (list.Any((float x) => (double)x > 0.5 && (double)x < 0.6))
				{
					if (list.Any((float x) => (double)x > 0.9))
					{
						goto IL_C0;
					}
				}
			}
			Log.Warning("Possibly uneven distribution.");
			IL_C0:
			list = Autotests_RandomNumbers.RandomFloats(1300000).ToList<float>();
			int num = list.Count((float x) => (double)x < 0.1);
			Log.Message("< 0.1 count (should be ~10%): " + (float)num / (float)list.Count<float>() * 100f + "%");
			num = list.Count((float x) => (double)x < 0.0001);
			Log.Message("< 0.0001 count (should be ~0.01%): " + (float)num / (float)list.Count<float>() * 100f + "%");
		}

		// Token: 0x06007BF0 RID: 31728 RVA: 0x002BC223 File Offset: 0x002BA423
		private static IEnumerable<float> RandomFloats(int count)
		{
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				yield return Rand.Value;
				num = i;
			}
			yield break;
		}

		// Token: 0x06007BF1 RID: 31729 RVA: 0x002BC234 File Offset: 0x002BA434
		private static void CheckIntsRange()
		{
			int num = -7;
			int num2 = 4;
			int num3 = 0;
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			for (;;)
			{
				bool flag = true;
				for (int i = num; i <= num2; i++)
				{
					if (!dictionary.ContainsKey(i))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					goto IL_9B;
				}
				num3++;
				if (num3 == 200000)
				{
					break;
				}
				int num4 = Rand.RangeInclusive(num, num2);
				if (num4 < num || num4 > num2)
				{
					Log.Error("Value out of range.");
				}
				if (dictionary.ContainsKey(num4))
				{
					Dictionary<int, int> dictionary2 = dictionary;
					int key = num4;
					int num5 = dictionary2[key];
					dictionary2[key] = num5 + 1;
				}
				else
				{
					dictionary.Add(num4, 1);
				}
			}
			Log.Error("Failed to find all numbers in a range.");
			return;
			IL_9B:
			Log.Message(string.Concat(new object[]
			{
				"Values between ",
				num,
				" and ",
				num2,
				" (value: number of occurrences):"
			}));
			for (int j = num; j <= num2; j++)
			{
				Log.Message(j + ": " + dictionary[j]);
			}
		}

		// Token: 0x06007BF2 RID: 31730 RVA: 0x002BC34C File Offset: 0x002BA54C
		private static void CheckIntsDistribution()
		{
			List<int> list = new List<int>();
			for (int j = 0; j < 1000000; j++)
			{
				int num = Rand.RangeInclusive(-2, 1);
				list.Add(num + 2);
			}
			Log.Message("Ints distribution (should be even):");
			int i;
			int i2;
			for (i = 0; i < 4; i = i2 + 1)
			{
				Log.Message(string.Concat(new object[]
				{
					i,
					": ",
					(float)list.Count((int x) => x == i) / (float)list.Count<int>() * 100f,
					"%"
				}));
				i2 = i;
			}
		}

		// Token: 0x06007BF3 RID: 31731 RVA: 0x002BC40C File Offset: 0x002BA60C
		private static void CheckSeed()
		{
			int seed = Rand.Seed = 10;
			int @int = Rand.Int;
			int int2 = Rand.Int;
			Rand.Seed = seed;
			int int3 = Rand.Int;
			int int4 = Rand.Int;
			if (@int != int3 || int2 != int4)
			{
				Log.Error("Same seed, different values.");
			}
			Autotests_RandomNumbers.TestPushSeed(15, 20);
			Autotests_RandomNumbers.TestPushSeed(-2147483645, 20);
			Autotests_RandomNumbers.TestPushSeed(6, int.MaxValue);
			Autotests_RandomNumbers.TestPushSeed(-2147483645, 2147483642);
			Autotests_RandomNumbers.TestPushSeed(-1947483645, 1147483642);
			Autotests_RandomNumbers.TestPushSeed(455, 648023);
		}

		// Token: 0x06007BF4 RID: 31732 RVA: 0x002BC4A0 File Offset: 0x002BA6A0
		private static void TestPushSeed(int seed1, int seed2)
		{
			Rand.Seed = seed1;
			int @int = Rand.Int;
			int int2 = Rand.Int;
			Rand.PushState();
			Rand.Seed = seed2;
			int int3 = Rand.Int;
			Rand.PopState();
			Rand.Seed = seed1;
			int int4 = Rand.Int;
			Rand.PushState();
			Rand.Seed = seed2;
			int int5 = Rand.Int;
			Rand.PopState();
			int int6 = Rand.Int;
			if (@int != int4 || int2 != int6 || int3 != int5)
			{
				Log.Error("PushSeed broken.");
			}
		}
	}
}
