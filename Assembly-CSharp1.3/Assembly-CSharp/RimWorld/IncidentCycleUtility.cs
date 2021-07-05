using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BEC RID: 3052
	public class IncidentCycleUtility
	{
		// Token: 0x17000C95 RID: 3221
		// (get) Token: 0x060047D4 RID: 18388 RVA: 0x0017B75F File Offset: 0x0017995F
		private static int QueueIntervalsPassed
		{
			get
			{
				return Find.TickManager.TicksGame / 1000;
			}
		}

		// Token: 0x060047D5 RID: 18389 RVA: 0x0017B774 File Offset: 0x00179974
		public static int IncidentCountThisInterval(IIncidentTarget target, int randSeedSalt, float minDaysPassed, float onDays, float offDays, float minSpacingDays, float minIncidents, float maxIncidents, float acceptFraction = 1f)
		{
			int num = IncidentCycleUtility.DaysToIntervals(minDaysPassed);
			int num2 = IncidentCycleUtility.QueueIntervalsPassed - num;
			if (num2 < 0)
			{
				return 0;
			}
			int num3 = IncidentCycleUtility.DaysToIntervals(onDays);
			int num4 = IncidentCycleUtility.DaysToIntervals(offDays);
			int minSpacingIntervals = IncidentCycleUtility.DaysToIntervals(minSpacingDays);
			int num5 = num3 + num4;
			int num6 = num2 / num5;
			int fixedHit = -9999999;
			for (int i = 0; i <= num6; i++)
			{
				int seed = Gen.HashCombineInt(Find.World.info.persistentRandomValue, target.ConstantRandSeed, randSeedSalt, i);
				int start = i * num5;
				if (IncidentCycleUtility.hits.Count > 0)
				{
					fixedHit = IncidentCycleUtility.hits[IncidentCycleUtility.hits.Count - 1];
				}
				IncidentCycleUtility.hits.Clear();
				IncidentCycleUtility.GenerateHitList(seed, start, num3, minIncidents, maxIncidents, minSpacingIntervals, acceptFraction, fixedHit);
			}
			int num7 = 0;
			for (int j = 0; j < IncidentCycleUtility.hits.Count; j++)
			{
				if (IncidentCycleUtility.hits[j] == num2)
				{
					num7++;
				}
			}
			IncidentCycleUtility.hits.Clear();
			return num7;
		}

		// Token: 0x060047D6 RID: 18390 RVA: 0x0017B874 File Offset: 0x00179A74
		private static void GenerateHitList(int seed, int start, int length, float minIncidents, float maxIncidents, int minSpacingIntervals, float acceptFraction, int fixedHit)
		{
			if (IncidentCycleUtility.hits.Count > 0)
			{
				throw new Exception();
			}
			Rand.PushState();
			Rand.Seed = seed;
			int num = GenMath.RoundRandom(Rand.Range(minIncidents, maxIncidents));
			int num2 = 0;
			for (;;)
			{
				IncidentCycleUtility.hits.Clear();
				if (num2++ > 100)
				{
					break;
				}
				for (int i = 0; i < num; i++)
				{
					int item = Rand.Range(0, length) + start;
					IncidentCycleUtility.hits.Add(item);
				}
				IncidentCycleUtility.hits.Sort();
				if (IncidentCycleUtility.RelaxToSatisfyMinDiff(IncidentCycleUtility.hits, minSpacingIntervals, fixedHit, start + length))
				{
					goto IL_90;
				}
			}
			Log.ErrorOnce("Too many tries finding incident time. minSpacingDays is too high.", 12612131);
			IL_90:
			if (acceptFraction < 1f)
			{
				int num3 = GenMath.RoundRandom((float)IncidentCycleUtility.hits.Count * acceptFraction);
				IncidentCycleUtility.hits.Shuffle<int>();
				IncidentCycleUtility.hits.RemoveRange(num3, IncidentCycleUtility.hits.Count - num3);
			}
			Rand.PopState();
		}

		// Token: 0x060047D7 RID: 18391 RVA: 0x0017B958 File Offset: 0x00179B58
		private static bool RelaxToSatisfyMinDiff(List<int> values, int minDiff, int fixedValue, int max)
		{
			if (values.Count == 0)
			{
				return true;
			}
			for (int i = 0; i < values.Count; i++)
			{
				int num = (i == 0) ? Mathf.Abs(values[i] - fixedValue) : Mathf.Abs(values[i] - values[i - 1]);
				if (num < minDiff)
				{
					int index = i;
					values[index] += minDiff - num;
					for (int j = i + 1; j < values.Count; j++)
					{
						if (values[j] < values[i] + j)
						{
							values[j] = values[i] + j;
						}
					}
				}
			}
			return values[values.Count - 1] <= max;
		}

		// Token: 0x060047D8 RID: 18392 RVA: 0x0017BA1B File Offset: 0x00179C1B
		private static int DaysToIntervals(float days)
		{
			return Mathf.RoundToInt(days * 60f);
		}

		// Token: 0x04002C09 RID: 11273
		private static List<int> hits = new List<int>();
	}
}
