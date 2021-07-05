using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200002B RID: 43
	public static class GenMath
	{
		// Token: 0x06000247 RID: 583 RVA: 0x0000BBE0 File Offset: 0x00009DE0
		public static float RoundedHundredth(float f)
		{
			return Mathf.Round(f * 100f) / 100f;
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000BBF4 File Offset: 0x00009DF4
		public static int RoundTo(int value, int roundToNearest)
		{
			return (int)Math.Round((double)((float)value / (float)roundToNearest)) * roundToNearest;
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000BC04 File Offset: 0x00009E04
		public static float RoundTo(float value, float roundToNearest)
		{
			return (float)((int)Math.Round((double)(value / roundToNearest))) * roundToNearest;
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000BC14 File Offset: 0x00009E14
		public static float ChanceEitherHappens(float chanceA, float chanceB)
		{
			return chanceA + (1f - chanceA) * chanceB;
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000BC21 File Offset: 0x00009E21
		public static float SmootherStep(float edge0, float edge1, float x)
		{
			x = Mathf.Clamp01((x - edge0) / (edge1 - edge0));
			return x * x * x * (x * (x * 6f - 15f) + 10f);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000BC4C File Offset: 0x00009E4C
		public static int RoundRandom(float f)
		{
			return (int)f + ((Rand.Value < f % 1f) ? 1 : 0);
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000BC63 File Offset: 0x00009E63
		public static float WeightedAverage(float A, float weightA, float B, float weightB)
		{
			return (A * weightA + B * weightB) / (weightA + weightB);
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000BC70 File Offset: 0x00009E70
		public static float Median<T>(IList<T> list, Func<T, float> orderBy, float noneValue = 0f, float center = 0.5f)
		{
			if (list.NullOrEmpty<T>())
			{
				return noneValue;
			}
			GenMath.tmpElements.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				GenMath.tmpElements.Add(orderBy(list[i]));
			}
			GenMath.tmpElements.Sort();
			return GenMath.tmpElements[Mathf.Min(Mathf.FloorToInt((float)GenMath.tmpElements.Count * center), GenMath.tmpElements.Count - 1)];
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000BCF0 File Offset: 0x00009EF0
		public static float WeightedMedian(IList<Pair<float, float>> list, float noneValue = 0f, float center = 0.5f)
		{
			GenMath.tmpPairs.Clear();
			GenMath.tmpPairs.AddRange(list);
			float num = 0f;
			for (int i = 0; i < GenMath.tmpPairs.Count; i++)
			{
				float second = GenMath.tmpPairs[i].Second;
				if (second < 0f)
				{
					Log.ErrorOnce("Negative weight in WeightedMedian: " + second, GenMath.tmpPairs.GetHashCode());
				}
				else
				{
					num += second;
				}
			}
			if (num <= 0f)
			{
				return noneValue;
			}
			GenMath.tmpPairs.SortBy((Pair<float, float> x) => x.First);
			float num2 = 0f;
			for (int j = 0; j < GenMath.tmpPairs.Count; j++)
			{
				float first = GenMath.tmpPairs[j].First;
				float second2 = GenMath.tmpPairs[j].Second;
				num2 += second2 / num;
				if (num2 >= center)
				{
					return first;
				}
			}
			return GenMath.tmpPairs.Last<Pair<float, float>>().First;
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000BE10 File Offset: 0x0000A010
		public static float Sqrt(float f)
		{
			return (float)Math.Sqrt((double)f);
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000BE1C File Offset: 0x0000A01C
		public static float LerpDouble(float inFrom, float inTo, float outFrom, float outTo, float x)
		{
			float num = (x - inFrom) / (inTo - inFrom);
			return outFrom + (outTo - outFrom) * num;
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000BE39 File Offset: 0x0000A039
		public static float LerpDoubleClamped(float inFrom, float inTo, float outFrom, float outTo, float x)
		{
			return GenMath.LerpDouble(inFrom, inTo, outFrom, outTo, Mathf.Clamp(x, Mathf.Min(inFrom, inTo), Mathf.Max(inFrom, inTo)));
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000BE59 File Offset: 0x0000A059
		public static float Reflection(float value, float mirror)
		{
			return mirror - (value - mirror);
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000BE60 File Offset: 0x0000A060
		public static Quaternion ToQuat(this float ang)
		{
			return Quaternion.AngleAxis(ang, Vector3.up);
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000BE70 File Offset: 0x0000A070
		public static float GetFactorInInterval(float min, float mid, float max, float power, float x)
		{
			if (min > max)
			{
				return 0f;
			}
			if (x <= min || x >= max)
			{
				return 0f;
			}
			if (x == mid)
			{
				return 1f;
			}
			float f;
			if (x < mid)
			{
				f = 1f - (mid - x) / (mid - min);
			}
			else
			{
				f = 1f - (x - mid) / (max - mid);
			}
			return Mathf.Pow(f, power);
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000BED4 File Offset: 0x0000A0D4
		public static float FlatHill(float min, float lower, float upper, float max, float x)
		{
			if (x < min)
			{
				return 0f;
			}
			if (x < lower)
			{
				return Mathf.InverseLerp(min, lower, x);
			}
			if (x < upper)
			{
				return 1f;
			}
			if (x < max)
			{
				return Mathf.InverseLerp(max, upper, x);
			}
			return 0f;
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000BF10 File Offset: 0x0000A110
		public static float FlatHill(float minY, float min, float lower, float upper, float max, float maxY, float x)
		{
			if (x < min)
			{
				return minY;
			}
			if (x < lower)
			{
				return GenMath.LerpDouble(min, lower, minY, 1f, x);
			}
			if (x < upper)
			{
				return 1f;
			}
			if (x < max)
			{
				return GenMath.LerpDouble(upper, max, 1f, maxY, x);
			}
			return maxY;
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000BF5E File Offset: 0x0000A15E
		public static int OctileDistance(int dx, int dz, int cardinal, int diagonal)
		{
			return cardinal * (dx + dz) + (diagonal - 2 * cardinal) * Mathf.Min(dx, dz);
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000BF73 File Offset: 0x0000A173
		public static float UnboundedValueToFactor(float val)
		{
			if (val > 0f)
			{
				return 1f + val;
			}
			return 1f / (1f - val);
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000BF94 File Offset: 0x0000A194
		[DebugOutput("System", false)]
		public static void TestMathPerf()
		{
			IntVec3 intVec = new IntVec3(72, 0, 65);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Math perf tests (" + 10000000f + " tests each)");
			float num = 0f;
			Stopwatch stopwatch = Stopwatch.StartNew();
			int num2 = 0;
			while ((float)num2 < 10000000f)
			{
				num += (float)Math.Sqrt(101.20999908447266);
				num2++;
			}
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"(float)System.Math.Sqrt(",
				101.21f,
				"): ",
				stopwatch.ElapsedTicks
			}));
			Stopwatch stopwatch2 = Stopwatch.StartNew();
			int num3 = 0;
			while ((float)num3 < 10000000f)
			{
				num += Mathf.Sqrt(101.21f);
				num3++;
			}
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"UnityEngine.Mathf.Sqrt(",
				101.21f,
				"): ",
				stopwatch2.ElapsedTicks
			}));
			Stopwatch stopwatch3 = Stopwatch.StartNew();
			int num4 = 0;
			while ((float)num4 < 10000000f)
			{
				num += GenMath.Sqrt(101.21f);
				num4++;
			}
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"Verse.GenMath.Sqrt(",
				101.21f,
				"): ",
				stopwatch3.ElapsedTicks
			}));
			Stopwatch stopwatch4 = Stopwatch.StartNew();
			int num5 = 0;
			while ((float)num5 < 10000000f)
			{
				num += (float)intVec.LengthManhattan;
				num5++;
			}
			stringBuilder.AppendLine("Verse.IntVec3.LengthManhattan: " + stopwatch4.ElapsedTicks);
			Stopwatch stopwatch5 = Stopwatch.StartNew();
			int num6 = 0;
			while ((float)num6 < 10000000f)
			{
				num += intVec.LengthHorizontal;
				num6++;
			}
			stringBuilder.AppendLine("Verse.IntVec3.LengthHorizontal: " + stopwatch5.ElapsedTicks);
			Stopwatch stopwatch6 = Stopwatch.StartNew();
			int num7 = 0;
			while ((float)num7 < 10000000f)
			{
				num += (float)intVec.LengthHorizontalSquared;
				num7++;
			}
			stringBuilder.AppendLine("Verse.IntVec3.LengthHorizontalSquared: " + stopwatch6.ElapsedTicks);
			stringBuilder.AppendLine("total: " + num);
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000C1FE File Offset: 0x0000A3FE
		public static float Min(float a, float b, float c)
		{
			if (a < b)
			{
				if (a < c)
				{
					return a;
				}
				return c;
			}
			else
			{
				if (b < c)
				{
					return b;
				}
				return c;
			}
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000C213 File Offset: 0x0000A413
		public static int Max(int a, int b, int c)
		{
			if (a > b)
			{
				if (a > c)
				{
					return a;
				}
				return c;
			}
			else
			{
				if (b > c)
				{
					return b;
				}
				return c;
			}
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000C228 File Offset: 0x0000A428
		public static float SphericalDistance(Vector3 normalizedA, Vector3 normalizedB)
		{
			if (normalizedA == normalizedB)
			{
				return 0f;
			}
			return Mathf.Acos(Vector3.Dot(normalizedA, normalizedB));
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000C248 File Offset: 0x0000A448
		public static void DHondtDistribution(List<int> candidates, Func<int, float> scoreGetter, int numToDistribute)
		{
			GenMath.tmpScores.Clear();
			GenMath.tmpCalcList.Clear();
			for (int i = 0; i < candidates.Count; i++)
			{
				float item = scoreGetter(i);
				candidates[i] = 0;
				GenMath.tmpScores.Add(item);
				GenMath.tmpCalcList.Add(item);
			}
			for (int j = 0; j < numToDistribute; j++)
			{
				int num = GenMath.tmpCalcList.IndexOf(GenMath.tmpCalcList.Max());
				int index = num;
				int num2 = candidates[index];
				candidates[index] = num2 + 1;
				GenMath.tmpCalcList[num] = GenMath.tmpScores[num] / ((float)candidates[num] + 1f);
			}
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000C2FF File Offset: 0x0000A4FF
		public static int PositiveMod(int x, int m)
		{
			return (x % m + m) % m;
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000C2FF File Offset: 0x0000A4FF
		public static long PositiveMod(long x, long m)
		{
			return (x % m + m) % m;
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000C2FF File Offset: 0x0000A4FF
		public static float PositiveMod(float x, float m)
		{
			return (x % m + m) % m;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000C308 File Offset: 0x0000A508
		public static int PositiveModRemap(long x, int d, int m)
		{
			if (x < 0L)
			{
				x -= (long)(d - 1);
			}
			return (int)((x / (long)d % (long)m + (long)m) % (long)m);
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000C325 File Offset: 0x0000A525
		public static Vector3 BezierCubicEvaluate(float t, GenMath.BezierCubicControls bcc)
		{
			return GenMath.BezierCubicEvaluate(t, bcc.w0, bcc.w1, bcc.w2, bcc.w3);
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000C348 File Offset: 0x0000A548
		public static Vector3 BezierCubicEvaluate(float t, Vector3 w0, Vector3 w1, Vector3 w2, Vector3 w3)
		{
			float d = t * t;
			float num = 1f - t;
			float d2 = num * num;
			return w0 * d2 * num + 3f * w1 * d2 * t + 3f * w2 * num * d + w3 * d * t;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000C3C0 File Offset: 0x0000A5C0
		public static float CirclesOverlapArea(float x1, float y1, float r1, float x2, float y2, float r2)
		{
			float num = (x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1);
			float num2 = Mathf.Sqrt(num);
			float num3 = r1 * r1;
			float num4 = r2 * r2;
			float num5 = Mathf.Abs(r1 - r2);
			if (num2 >= r1 + r2)
			{
				return 0f;
			}
			if (num2 <= num5 && r1 >= r2)
			{
				return 3.1415927f * num4;
			}
			if (num2 <= num5 && r2 >= r1)
			{
				return 3.1415927f * num3;
			}
			float num6 = Mathf.Acos((num3 - num4 + num) / (2f * r1 * num2)) * 2f;
			float num7 = Mathf.Acos((num4 - num3 + num) / (2f * r2 * num2)) * 2f;
			float num8 = (num7 * num4 - num4 * Mathf.Sin(num7)) * 0.5f;
			float num9 = (num6 * num3 - num3 * Mathf.Sin(num6)) * 0.5f;
			return num8 + num9;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000C492 File Offset: 0x0000A692
		public static bool AnyIntegerInRange(float min, float max)
		{
			return Mathf.Ceil(min) <= max;
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000C4A0 File Offset: 0x0000A6A0
		public static void NormalizeToSum1(ref float a, ref float b, ref float c)
		{
			float num = a + b + c;
			if (num == 0f)
			{
				a = 1f;
				b = 0f;
				c = 0f;
				return;
			}
			a /= num;
			b /= num;
			c /= num;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000C4E6 File Offset: 0x0000A6E6
		public static float InverseLerp(float a, float b, float value)
		{
			if (a != b)
			{
				return Mathf.InverseLerp(a, b, value);
			}
			if (value >= a)
			{
				return 1f;
			}
			return 0f;
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000C504 File Offset: 0x0000A704
		public static T MaxBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3)
		{
			if (by1 >= by2 && by1 >= by3)
			{
				return elem1;
			}
			if (by2 >= by1 && by2 >= by3)
			{
				return elem2;
			}
			return elem3;
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000C51E File Offset: 0x0000A71E
		public static T MaxBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4)
		{
			if (by1 >= by2 && by1 >= by3 && by1 >= by4)
			{
				return elem1;
			}
			if (by2 >= by1 && by2 >= by3 && by2 >= by4)
			{
				return elem2;
			}
			if (by3 >= by1 && by3 >= by2 && by3 >= by4)
			{
				return elem3;
			}
			return elem4;
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000C558 File Offset: 0x0000A758
		public static T MaxBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5)
		{
			if (by1 >= by2 && by1 >= by3 && by1 >= by4 && by1 >= by5)
			{
				return elem1;
			}
			if (by2 >= by1 && by2 >= by3 && by2 >= by4 && by2 >= by5)
			{
				return elem2;
			}
			if (by3 >= by1 && by3 >= by2 && by3 >= by4 && by3 >= by5)
			{
				return elem3;
			}
			if (by4 >= by1 && by4 >= by2 && by4 >= by3 && by4 >= by5)
			{
				return elem4;
			}
			return elem5;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000C5C4 File Offset: 0x0000A7C4
		public static T MaxBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5, T elem6, float by6)
		{
			if (by1 >= by2 && by1 >= by3 && by1 >= by4 && by1 >= by5 && by1 >= by6)
			{
				return elem1;
			}
			if (by2 >= by1 && by2 >= by3 && by2 >= by4 && by2 >= by5 && by2 >= by6)
			{
				return elem2;
			}
			if (by3 >= by1 && by3 >= by2 && by3 >= by4 && by3 >= by5 && by3 >= by6)
			{
				return elem3;
			}
			if (by4 >= by1 && by4 >= by2 && by4 >= by3 && by4 >= by5 && by4 >= by6)
			{
				return elem4;
			}
			if (by5 >= by1 && by5 >= by2 && by5 >= by3 && by5 >= by4 && by5 >= by6)
			{
				return elem5;
			}
			return elem6;
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000C664 File Offset: 0x0000A864
		public static T MaxBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5, T elem6, float by6, T elem7, float by7)
		{
			if (by1 >= by2 && by1 >= by3 && by1 >= by4 && by1 >= by5 && by1 >= by6 && by1 >= by7)
			{
				return elem1;
			}
			if (by2 >= by1 && by2 >= by3 && by2 >= by4 && by2 >= by5 && by2 >= by6 && by2 >= by7)
			{
				return elem2;
			}
			if (by3 >= by1 && by3 >= by2 && by3 >= by4 && by3 >= by5 && by3 >= by6 && by3 >= by7)
			{
				return elem3;
			}
			if (by4 >= by1 && by4 >= by2 && by4 >= by3 && by4 >= by5 && by4 >= by6 && by4 >= by7)
			{
				return elem4;
			}
			if (by5 >= by1 && by5 >= by2 && by5 >= by3 && by5 >= by4 && by5 >= by6 && by5 >= by7)
			{
				return elem5;
			}
			if (by6 >= by1 && by6 >= by2 && by6 >= by3 && by6 >= by4 && by6 >= by5 && by6 >= by7)
			{
				return elem6;
			}
			return elem7;
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000C748 File Offset: 0x0000A948
		public static T MaxBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5, T elem6, float by6, T elem7, float by7, T elem8, float by8)
		{
			if (by1 >= by2 && by1 >= by3 && by1 >= by4 && by1 >= by5 && by1 >= by6 && by1 >= by7 && by1 >= by8)
			{
				return elem1;
			}
			if (by2 >= by1 && by2 >= by3 && by2 >= by4 && by2 >= by5 && by2 >= by6 && by2 >= by7 && by2 >= by8)
			{
				return elem2;
			}
			if (by3 >= by1 && by3 >= by2 && by3 >= by4 && by3 >= by5 && by3 >= by6 && by3 >= by7 && by3 >= by8)
			{
				return elem3;
			}
			if (by4 >= by1 && by4 >= by2 && by4 >= by3 && by4 >= by5 && by4 >= by6 && by4 >= by7 && by4 >= by8)
			{
				return elem4;
			}
			if (by5 >= by1 && by5 >= by2 && by5 >= by3 && by5 >= by4 && by5 >= by6 && by5 >= by7 && by5 >= by8)
			{
				return elem5;
			}
			if (by6 >= by1 && by6 >= by2 && by6 >= by3 && by6 >= by4 && by6 >= by5 && by6 >= by7 && by6 >= by8)
			{
				return elem6;
			}
			if (by7 >= by1 && by7 >= by2 && by7 >= by3 && by7 >= by4 && by7 >= by5 && by7 >= by6 && by7 >= by8)
			{
				return elem7;
			}
			return elem8;
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000C876 File Offset: 0x0000AA76
		public static T MinBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3)
		{
			return GenMath.MaxBy<T>(elem1, -by1, elem2, -by2, elem3, -by3);
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000C888 File Offset: 0x0000AA88
		public static T MinBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4)
		{
			return GenMath.MaxBy<T>(elem1, -by1, elem2, -by2, elem3, -by3, elem4, -by4);
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000C8A0 File Offset: 0x0000AAA0
		public static T MinBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5)
		{
			return GenMath.MaxBy<T>(elem1, -by1, elem2, -by2, elem3, -by3, elem4, -by4, elem5, -by5);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000C8C8 File Offset: 0x0000AAC8
		public static T MinBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5, T elem6, float by6)
		{
			return GenMath.MaxBy<T>(elem1, -by1, elem2, -by2, elem3, -by3, elem4, -by4, elem5, -by5, elem6, -by6);
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000C8F4 File Offset: 0x0000AAF4
		public static T MinBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5, T elem6, float by6, T elem7, float by7)
		{
			return GenMath.MaxBy<T>(elem1, -by1, elem2, -by2, elem3, -by3, elem4, -by4, elem5, -by5, elem6, -by6, elem7, -by7);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000C928 File Offset: 0x0000AB28
		public static T MinBy<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5, T elem6, float by6, T elem7, float by7, T elem8, float by8)
		{
			return GenMath.MaxBy<T>(elem1, -by1, elem2, -by2, elem3, -by3, elem4, -by4, elem5, -by5, elem6, -by6, elem7, -by7, elem8, -by8);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000C960 File Offset: 0x0000AB60
		public static T MaxByRandomIfEqual<T>(T elem1, float by1, T elem2, float by2, T elem3, float by3, T elem4, float by4, T elem5, float by5, T elem6, float by6, T elem7, float by7, T elem8, float by8, float eps = 0.0001f)
		{
			return GenMath.MaxBy<T>(elem1, by1 + Rand.Range(0f, eps), elem2, by2 + Rand.Range(0f, eps), elem3, by3 + Rand.Range(0f, eps), elem4, by4 + Rand.Range(0f, eps), elem5, by5 + Rand.Range(0f, eps), elem6, by6 + Rand.Range(0f, eps), elem7, by7 + Rand.Range(0f, eps), elem8, by8 + Rand.Range(0f, eps));
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000C9F8 File Offset: 0x0000ABF8
		public static float Stddev(IEnumerable<float> data)
		{
			int num = 0;
			double num2 = 0.0;
			double num3 = 0.0;
			foreach (float num4 in data)
			{
				num++;
				num2 += (double)num4;
				num3 += (double)(num4 * num4);
			}
			double num5 = num2 / (double)num;
			return Mathf.Sqrt((float)(num3 / (double)num - num5 * num5));
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000CA7C File Offset: 0x0000AC7C
		public static float InverseParabola(float x)
		{
			x = Mathf.Clamp01(x);
			return -4f * x * (x - 1f);
		}

		// Token: 0x04000064 RID: 100
		public const float BigEpsilon = 1E-07f;

		// Token: 0x04000065 RID: 101
		public const float Sqrt2 = 1.4142135f;

		// Token: 0x04000066 RID: 102
		private static List<float> tmpElements = new List<float>();

		// Token: 0x04000067 RID: 103
		private static List<Pair<float, float>> tmpPairs = new List<Pair<float, float>>();

		// Token: 0x04000068 RID: 104
		private static List<float> tmpScores = new List<float>();

		// Token: 0x04000069 RID: 105
		private static List<float> tmpCalcList = new List<float>();

		// Token: 0x0200187E RID: 6270
		public struct BezierCubicControls
		{
			// Token: 0x04005DAA RID: 23978
			public Vector3 w0;

			// Token: 0x04005DAB RID: 23979
			public Vector3 w1;

			// Token: 0x04005DAC RID: 23980
			public Vector3 w2;

			// Token: 0x04005DAD RID: 23981
			public Vector3 w3;
		}
	}
}
