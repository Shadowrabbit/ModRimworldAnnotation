using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using Verse.Noise;

namespace Verse
{
	// Token: 0x0200008A RID: 138
	public static class Rand
	{
		// Token: 0x170000DD RID: 221
		// (set) Token: 0x060004DB RID: 1243 RVA: 0x0000A417 File Offset: 0x00008617
		public static int Seed
		{
			set
			{
				if (Rand.stateStack.Count == 0)
				{
					Log.ErrorOnce("Modifying the initial rand seed. Call PushState() first. The initial rand seed should always be based on the startup time and set only once.", 825343540, false);
				}
				Rand.seed = (uint)value;
				Rand.iterations = 0U;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060004DC RID: 1244 RVA: 0x0000A441 File Offset: 0x00008641
		public static float Value
		{
			get
			{
				return (float)(((double)MurmurHash.GetInt(Rand.seed, Rand.iterations++) - -2147483648.0) / 4294967295.0);
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x0000A470 File Offset: 0x00008670
		public static bool Bool
		{
			get
			{
				return Rand.Value < 0.5f;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060004DE RID: 1246 RVA: 0x0000A47E File Offset: 0x0000867E
		public static int Sign
		{
			get
			{
				if (!Rand.Bool)
				{
					return -1;
				}
				return 1;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060004DF RID: 1247 RVA: 0x0000A48A File Offset: 0x0000868A
		public static int Int
		{
			get
			{
				return MurmurHash.GetInt(Rand.seed, Rand.iterations++);
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060004E0 RID: 1248 RVA: 0x000895EC File Offset: 0x000877EC
		public static Vector3 UnitVector3
		{
			get
			{
				return new Vector3(Rand.Gaussian(0f, 1f), Rand.Gaussian(0f, 1f), Rand.Gaussian(0f, 1f)).normalized;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060004E1 RID: 1249 RVA: 0x00089634 File Offset: 0x00087834
		public static Vector2 UnitVector2
		{
			get
			{
				return new Vector2(Rand.Gaussian(0f, 1f), Rand.Gaussian(0f, 1f)).normalized;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060004E2 RID: 1250 RVA: 0x0008966C File Offset: 0x0008786C
		public static Vector2 InsideUnitCircle
		{
			get
			{
				Vector2 result;
				do
				{
					result = new Vector2(Rand.Value - 0.5f, Rand.Value - 0.5f) * 2f;
				}
				while (result.sqrMagnitude > 1f);
				return result;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060004E3 RID: 1251 RVA: 0x000896B0 File Offset: 0x000878B0
		public static Vector3 InsideUnitCircleVec3
		{
			get
			{
				Vector2 insideUnitCircle = Rand.InsideUnitCircle;
				return new Vector3(insideUnitCircle.x, 0f, insideUnitCircle.y);
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060004E4 RID: 1252 RVA: 0x0000A4A3 File Offset: 0x000086A3
		// (set) Token: 0x060004E5 RID: 1253 RVA: 0x0000A4B5 File Offset: 0x000086B5
		private static ulong StateCompressed
		{
			get
			{
				return (ulong)Rand.seed | (ulong)Rand.iterations << 32;
			}
			set
			{
				Rand.seed = (uint)(value & (ulong)-1);
				Rand.iterations = (uint)(value >> 32 & (ulong)-1);
			}
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x000896DC File Offset: 0x000878DC
		static Rand()
		{
			Rand.seed = (uint)DateTime.Now.GetHashCode();
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x0000A4CE File Offset: 0x000086CE
		public static void EnsureStateStackEmpty()
		{
			if (Rand.stateStack.Count > 0)
			{
				Log.Warning("Random state stack is not empty. There were more calls to PushState than PopState. Fixing.", false);
				while (Rand.stateStack.Any<ulong>())
				{
					Rand.PopState();
				}
			}
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x00089718 File Offset: 0x00087918
		public static float Gaussian(float centerX = 0f, float widthFactor = 1f)
		{
			float value = Rand.Value;
			float value2 = Rand.Value;
			return Mathf.Sqrt(-2f * Mathf.Log(value)) * Mathf.Sin(6.2831855f * value2) * widthFactor + centerX;
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00089754 File Offset: 0x00087954
		public static float GaussianAsymmetric(float centerX = 0f, float lowerWidthFactor = 1f, float upperWidthFactor = 1f)
		{
			float value = Rand.Value;
			float value2 = Rand.Value;
			float num = Mathf.Sqrt(-2f * Mathf.Log(value)) * Mathf.Sin(6.2831855f * value2);
			if (num <= 0f)
			{
				return num * lowerWidthFactor + centerX;
			}
			return num * upperWidthFactor + centerX;
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0000A4FB File Offset: 0x000086FB
		public static int Range(int min, int max)
		{
			if (max <= min)
			{
				return min;
			}
			return min + Mathf.Abs(Rand.Int % (max - min));
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0000A513 File Offset: 0x00008713
		public static int RangeInclusive(int min, int max)
		{
			if (max <= min)
			{
				return min;
			}
			return Rand.Range(min, max + 1);
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0000A524 File Offset: 0x00008724
		public static float Range(float min, float max)
		{
			if (max <= min)
			{
				return min;
			}
			return Rand.Value * (max - min) + min;
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x0000A537 File Offset: 0x00008737
		public static bool Chance(float chance)
		{
			return chance > 0f && (chance >= 1f || Rand.Value < chance);
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0000A555 File Offset: 0x00008755
		public static bool ChanceSeeded(float chance, int specialSeed)
		{
			Rand.PushState(specialSeed);
			bool result = Rand.Chance(chance);
			Rand.PopState();
			return result;
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0000A568 File Offset: 0x00008768
		public static float ValueSeeded(int specialSeed)
		{
			Rand.PushState(specialSeed);
			float value = Rand.Value;
			Rand.PopState();
			return value;
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0000A57A File Offset: 0x0000877A
		public static float RangeSeeded(float min, float max, int specialSeed)
		{
			Rand.PushState(specialSeed);
			float result = Rand.Range(min, max);
			Rand.PopState();
			return result;
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0000A58E File Offset: 0x0000878E
		public static int RangeSeeded(int min, int max, int specialSeed)
		{
			Rand.PushState(specialSeed);
			int result = Rand.Range(min, max);
			Rand.PopState();
			return result;
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0000A5A2 File Offset: 0x000087A2
		public static int RangeInclusiveSeeded(int min, int max, int specialSeed)
		{
			Rand.PushState(specialSeed);
			int result = Rand.RangeInclusive(min, max);
			Rand.PopState();
			return result;
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0000A5B6 File Offset: 0x000087B6
		public static T Element<T>(T a, T b)
		{
			if (!Rand.Bool)
			{
				return b;
			}
			return a;
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x000897A0 File Offset: 0x000879A0
		public static T Element<T>(T a, T b, T c)
		{
			float value = Rand.Value;
			if (value < 0.33333f)
			{
				return a;
			}
			if (value < 0.66666f)
			{
				return b;
			}
			return c;
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x000897C8 File Offset: 0x000879C8
		public static T Element<T>(T a, T b, T c, T d)
		{
			float value = Rand.Value;
			if (value < 0.25f)
			{
				return a;
			}
			if (value < 0.5f)
			{
				return b;
			}
			if (value < 0.75f)
			{
				return c;
			}
			return d;
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x000897FC File Offset: 0x000879FC
		public static T Element<T>(T a, T b, T c, T d, T e)
		{
			float value = Rand.Value;
			if (value < 0.2f)
			{
				return a;
			}
			if (value < 0.4f)
			{
				return b;
			}
			if (value < 0.6f)
			{
				return c;
			}
			if (value < 0.8f)
			{
				return d;
			}
			return e;
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0008983C File Offset: 0x00087A3C
		public static T Element<T>(T a, T b, T c, T d, T e, T f)
		{
			float value = Rand.Value;
			if (value < 0.16666f)
			{
				return a;
			}
			if (value < 0.33333f)
			{
				return b;
			}
			if (value < 0.5f)
			{
				return c;
			}
			if (value < 0.66666f)
			{
				return d;
			}
			if (value < 0.83333f)
			{
				return e;
			}
			return f;
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x00089884 File Offset: 0x00087A84
		public static T ElementByWeight<T>(T a, float weightA, T b, float weightB)
		{
			float num = weightA + weightB;
			if (Rand.Value < weightA / num)
			{
				return a;
			}
			return b;
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x000898A4 File Offset: 0x00087AA4
		public static T ElementByWeight<T>(T a, float weightA, T b, float weightB, T c, float weightC)
		{
			float num = weightA + weightB + weightC;
			float value = Rand.Value;
			if (value < weightA / num)
			{
				return a;
			}
			if (value < (weightA + weightB) / num)
			{
				return b;
			}
			return c;
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x000898D4 File Offset: 0x00087AD4
		public static T ElementByWeight<T>(T a, float weightA, T b, float weightB, T c, float weightC, T d, float weightD)
		{
			float num = weightA + weightB + weightC + weightD;
			float value = Rand.Value;
			if (value < weightA / num)
			{
				return a;
			}
			if (value < (weightA + weightB) / num)
			{
				return b;
			}
			if (value < (weightA + weightB + weightC) / num)
			{
				return c;
			}
			return d;
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00089914 File Offset: 0x00087B14
		public static T ElementByWeight<T>(T a, float weightA, T b, float weightB, T c, float weightC, T d, float weightD, T e, float weightE)
		{
			float num = weightA + weightB + weightC + weightD + weightE;
			float value = Rand.Value;
			if (value < weightA / num)
			{
				return a;
			}
			if (value < (weightA + weightB) / num)
			{
				return b;
			}
			if (value < (weightA + weightB + weightC) / num)
			{
				return c;
			}
			if (value < (weightA + weightB + weightC + weightD) / num)
			{
				return d;
			}
			return e;
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00089968 File Offset: 0x00087B68
		public static T ElementByWeight<T>(T a, float weightA, T b, float weightB, T c, float weightC, T d, float weightD, T e, float weightE, T f, float weightF)
		{
			float num = weightA + weightB + weightC + weightD + weightE + weightF;
			float value = Rand.Value;
			if (value < weightA / num)
			{
				return a;
			}
			if (value < (weightA + weightB) / num)
			{
				return b;
			}
			if (value < (weightA + weightB + weightC) / num)
			{
				return c;
			}
			if (value < (weightA + weightB + weightC + weightD) / num)
			{
				return d;
			}
			if (value < (weightA + weightB + weightC + weightD + weightE) / num)
			{
				return e;
			}
			return f;
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0000A5C2 File Offset: 0x000087C2
		public static void PushState()
		{
			Rand.stateStack.Push(Rand.StateCompressed);
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0000A5D3 File Offset: 0x000087D3
		public static void PushState(int replacementSeed)
		{
			Rand.PushState();
			Rand.Seed = replacementSeed;
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x0000A5E0 File Offset: 0x000087E0
		public static void PopState()
		{
			Rand.StateCompressed = Rand.stateStack.Pop();
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x000899D4 File Offset: 0x00087BD4
		public static float ByCurve(SimpleCurve curve)
		{
			if (curve.PointsCount < 3)
			{
				throw new ArgumentException("curve has < 3 points");
			}
			if (curve[0].y != 0f || curve[curve.PointsCount - 1].y != 0f)
			{
				throw new ArgumentException("curve has start/end point with y != 0");
			}
			float num = 0f;
			for (int i = 0; i < curve.PointsCount - 1; i++)
			{
				if (curve[i].y < 0f)
				{
					throw new ArgumentException("curve has point with y < 0");
				}
				num += (curve[i + 1].x - curve[i].x) * (curve[i].y + curve[i + 1].y);
			}
			float num2 = Rand.Range(0f, num);
			for (int j = 0; j < curve.PointsCount - 1; j++)
			{
				float num3 = (curve[j + 1].x - curve[j].x) * (curve[j].y + curve[j + 1].y);
				if (num3 >= num2)
				{
					float num4 = curve[j + 1].x - curve[j].x;
					float y = curve[j].y;
					float y2 = curve[j + 1].y;
					float num5 = num2 / (y + y2);
					if (Rand.Range(0f, (y + y2) / 2f) > Mathf.Lerp(y, y2, num5 / num4))
					{
						num5 = num4 - num5;
					}
					return num5 + curve[j].x;
				}
				num2 -= num3;
			}
			throw new Exception("Reached end of Rand.ByCurve without choosing a point.");
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x00089BD4 File Offset: 0x00087DD4
		public static float ByCurveAverage(SimpleCurve curve)
		{
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < curve.PointsCount - 1; i++)
			{
				num += (curve[i + 1].x - curve[i].x) * (curve[i].y + curve[i + 1].y);
				num2 += (curve[i + 1].x - curve[i].x) * (curve[i].x * (2f * curve[i].y + curve[i + 1].y) + curve[i + 1].x * (curve[i].y + 2f * curve[i + 1].y));
			}
			return num2 / num / 3f;
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x00089CEC File Offset: 0x00087EEC
		public static bool MTBEventOccurs(float mtb, float mtbUnit, float checkDuration)
		{
			if (mtb == float.PositiveInfinity)
			{
				return false;
			}
			if (mtb <= 0f)
			{
				Log.Error("MTBEventOccurs with mtb=" + mtb, false);
				return true;
			}
			if (mtbUnit <= 0f)
			{
				Log.Error("MTBEventOccurs with mtbUnit=" + mtbUnit, false);
				return false;
			}
			if (checkDuration <= 0f)
			{
				Log.Error("MTBEventOccurs with checkDuration=" + checkDuration, false);
				return false;
			}
			double num = (double)checkDuration / ((double)mtb * (double)mtbUnit);
			if (num <= 0.0)
			{
				Log.Error(string.Concat(new object[]
				{
					"chancePerCheck is ",
					num,
					". mtb=",
					mtb,
					", mtbUnit=",
					mtbUnit,
					", checkDuration=",
					checkDuration
				}), false);
				return false;
			}
			double num2 = 1.0;
			if (num < 0.0001)
			{
				while (num < 0.0001)
				{
					num *= 8.0;
					num2 /= 8.0;
				}
				if ((double)Rand.Value > num2)
				{
					return false;
				}
			}
			return (double)Rand.Value < num;
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x00089E20 File Offset: 0x00088020
		public static void SplitRandomly(float valueToSplit, int splitIntoCount, List<float> outValues, List<float> zeroIfFractionBelow = null, List<float> minFractions = null)
		{
			outValues.Clear();
			if (splitIntoCount > 0)
			{
				if (minFractions != null)
				{
					float num = 0f;
					for (int i = 0; i < minFractions.Count; i++)
					{
						if (minFractions[i] > 1f)
						{
							throw new ArgumentException("minFractions[i] > 1");
						}
						num += minFractions[i];
					}
					if (num > 1f)
					{
						throw new ArgumentException("minFractions sum > 1");
					}
				}
				float num2 = 0f;
				int num3 = 0;
				for (;;)
				{
					num3++;
					if (num3 > 10000)
					{
						break;
					}
					outValues.Clear();
					for (int j = 0; j < splitIntoCount; j++)
					{
						float num4 = Rand.Range(0f, 1f);
						num2 += num4;
						outValues.Add(num4);
					}
					bool flag = true;
					if (minFractions != null)
					{
						for (int k = 0; k < minFractions.Count; k++)
						{
							if (outValues[k] / num2 < minFractions[k])
							{
								flag = false;
								break;
							}
						}
					}
					if (flag)
					{
						goto IL_150;
					}
				}
				Log.Error(string.Concat(new object[]
				{
					"Could not meet SplitRandomly requirements. valueToSplit=",
					valueToSplit,
					", splitIntoCount=",
					splitIntoCount,
					", zeroIfFractionsBelow=",
					zeroIfFractionBelow.ToStringSafeEnumerable(),
					", minFractions=",
					minFractions.ToStringSafeEnumerable()
				}), false);
				IL_150:
				if (zeroIfFractionBelow != null)
				{
					for (int l = 0; l < zeroIfFractionBelow.Count; l++)
					{
						if (outValues[l] / num2 < zeroIfFractionBelow[l])
						{
							outValues[l] = 0f;
							num2 = 0f;
							for (int m = 0; m < outValues.Count; m++)
							{
								num2 += outValues[m];
							}
						}
					}
				}
				if (num2 != 0f)
				{
					for (int n = 0; n < outValues.Count; n++)
					{
						outValues[n] = outValues[n] / num2 * valueToSplit;
					}
				}
				return;
			}
			if (valueToSplit == 0f)
			{
				return;
			}
			throw new ArgumentException("splitIntoCount <= 0");
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0008A010 File Offset: 0x00088210
		[DebugOutput("System", false)]
		internal static void RandTests()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Random generation tests");
			stringBuilder.Append("To see texture outputs, turn on 'draw recorded noise' and run this again.");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Performance test with " + 10000000 + " values");
			Stopwatch stopwatch = new Stopwatch();
			float num = 0f;
			Rand.PushState();
			stopwatch.Start();
			for (int i = 0; i < 10000000; i++)
			{
				num += Rand.Value;
			}
			stopwatch.Stop();
			Rand.PopState();
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"Time: ",
				stopwatch.ElapsedMilliseconds.ToString(),
				"ms (for sum ",
				num,
				")"
			}));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Distribution test with " + 100000 + " values");
			DebugHistogram debugHistogram = new DebugHistogram(new float[]
			{
				0f,
				0.1f,
				0.2f,
				0.3f,
				0.4f,
				0.5f,
				0.6f,
				0.7f,
				0.8f,
				0.9f,
				1f
			});
			DebugHistogram debugHistogram2 = new DebugHistogram(new float[]
			{
				0f,
				0.01f,
				0.02f,
				0.03f,
				0.04f,
				0.05f,
				0.06f,
				0.07f,
				0.08f,
				0.09f,
				0.1f,
				1f
			});
			Rand.PushState();
			for (int j = 0; j < 100000; j++)
			{
				debugHistogram.Add(Rand.Value);
				debugHistogram2.Add(Rand.Value);
			}
			Rand.PopState();
			stringBuilder.AppendLine("Gross histogram:");
			debugHistogram.Display(stringBuilder);
			stringBuilder.AppendLine("Fine histogram:");
			debugHistogram2.Display(stringBuilder);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Long-term tests");
			for (int k = 0; k < 3; k++)
			{
				int num2 = 0;
				for (int l = 0; l < 5000000; l++)
				{
					if (Rand.MTBEventOccurs(250f, 60000f, 60f))
					{
						num2++;
					}
				}
				string value = string.Concat(new object[]
				{
					"MTB=",
					250,
					" days, MTBUnit=",
					60000,
					", check duration=",
					60,
					" Simulated ",
					5000,
					" days (",
					5000000,
					" tests). Got ",
					num2,
					" events."
				});
				stringBuilder.AppendLine(value);
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Short-term tests");
			for (int m = 0; m < 5; m++)
			{
				int num3 = 0;
				for (int n = 0; n < 10000; n++)
				{
					if (Rand.MTBEventOccurs(1f, 24000f, 12000f))
					{
						num3++;
					}
				}
				string value2 = string.Concat(new object[]
				{
					"MTB=",
					1f,
					" days, MTBUnit=",
					24000f,
					", check duration=",
					12000f,
					", ",
					10000,
					" tests got ",
					num3,
					" events."
				});
				stringBuilder.AppendLine(value2);
			}
			for (int num4 = 0; num4 < 5; num4++)
			{
				int num5 = 0;
				for (int num6 = 0; num6 < 10000; num6++)
				{
					if (Rand.MTBEventOccurs(2f, 24000f, 6000f))
					{
						num5++;
					}
				}
				string value3 = string.Concat(new object[]
				{
					"MTB=",
					2f,
					" days, MTBUnit=",
					24000f,
					", check duration=",
					6000f,
					", ",
					10000,
					" tests got ",
					num5,
					" events."
				});
				stringBuilder.AppendLine(value3);
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Near seed tests");
			DebugHistogram debugHistogram3 = new DebugHistogram(new float[]
			{
				0f,
				0.1f,
				0.2f,
				0.3f,
				0.4f,
				0.5f,
				0.6f,
				0.7f,
				0.8f,
				0.9f,
				1f
			});
			Rand.PushState();
			for (int num7 = 0; num7 < 1000; num7++)
			{
				Rand.Seed = num7;
				debugHistogram3.Add(Rand.Value);
			}
			Rand.PopState();
			debugHistogram3.Display(stringBuilder);
			int @int = Rand.Int;
			stringBuilder.AppendLine("Repeating single ValueSeeded with seed " + @int + ". This should give the same result:");
			for (int num8 = 0; num8 < 4; num8++)
			{
				stringBuilder.AppendLine("   " + Rand.ValueSeeded(@int));
			}
			Log.Message(stringBuilder.ToString(), false);
			if (DebugViewSettings.drawRecordedNoise)
			{
				int[] array = new int[65536];
				Rand.PushState();
				for (int num9 = 0; num9 < 65536; num9++)
				{
					array[num9] = (int)(Rand.Value * 1000f);
				}
				Rand.PopState();
				Rand.DebugStoreBucketsAsTexture("One rand output per pixel", array, 1000f, 256);
				int[] array2 = new int[65536];
				Rand.PushState();
				for (int num10 = 0; num10 < 65536; num10++)
				{
					Rand.Seed = num10;
					array2[num10] = (int)(Rand.Value * 1000f);
				}
				Rand.PopState();
				Rand.DebugStoreBucketsAsTexture("One seed per pixel", array2, 1000f, 256);
				int[] array3 = new int[65536];
				Rand.PushState();
				for (int num11 = 0; num11 < 300000; num11++)
				{
					int num12 = (int)(Rand.Value * 65536f);
					array3[num12]++;
				}
				Rand.PopState();
				float whiteValue = 9.155273f;
				Rand.DebugStoreBucketsAsTexture("Brighten random pixel index", array3, whiteValue, 256);
				int[] array4 = new int[65536];
				Rand.PushState();
				for (int num13 = 0; num13 < 300000; num13++)
				{
					int num14 = (int)(Rand.Value * 256f);
					int num15 = (int)(Rand.Value * 256f);
					int num16 = num14 + 256 * num15;
					array4[num16]++;
				}
				Rand.PopState();
				float whiteValue2 = 9.155273f;
				Rand.DebugStoreBucketsAsTexture("Brighten random pixel coords", array4, whiteValue2, 256);
			}
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0008A690 File Offset: 0x00088890
		private static void DebugStoreBucketsAsTexture(string name, int[] buckets, float whiteValue, int texSize)
		{
			Texture2D texture2D = new Texture2D(texSize, texSize);
			for (int i = 0; i < texSize; i++)
			{
				for (int j = 0; j < texSize; j++)
				{
					int num = i + j * texSize;
					float num2 = (float)buckets[num] / whiteValue;
					num2 = Mathf.Clamp01(num2);
					texture2D.SetPixel(i, j, new Color(num2, num2, num2));
				}
			}
			texture2D.Apply();
			NoiseDebugUI.StoreTexture(texture2D, name);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0000A5F1 File Offset: 0x000087F1
		public static int RandSeedForHour(this Thing t, int salt)
		{
			return Gen.HashCombineInt(Gen.HashCombineInt(t.HashOffset(), Find.TickManager.TicksAbs / 2500), salt);
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0008A6F4 File Offset: 0x000888F4
		public static bool TryRangeInclusiveWhere(int from, int to, Predicate<int> predicate, out int value)
		{
			int num = to - from + 1;
			if (num <= 0)
			{
				value = 0;
				return false;
			}
			int num2 = Mathf.Max(Mathf.RoundToInt(Mathf.Sqrt((float)num)), 5);
			for (int i = 0; i < num2; i++)
			{
				int num3 = Rand.RangeInclusive(from, to);
				if (predicate(num3))
				{
					value = num3;
					return true;
				}
			}
			Rand.tmpRange.Clear();
			for (int j = from; j <= to; j++)
			{
				Rand.tmpRange.Add(j);
			}
			Rand.tmpRange.Shuffle<int>();
			int k = 0;
			int count = Rand.tmpRange.Count;
			while (k < count)
			{
				if (predicate(Rand.tmpRange[k]))
				{
					value = Rand.tmpRange[k];
					return true;
				}
				k++;
			}
			value = 0;
			return false;
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x0008A7BC File Offset: 0x000889BC
		public static Vector3 PointOnSphereCap(Vector3 center, float angle)
		{
			if (angle <= 0f)
			{
				return center;
			}
			if (angle >= 180f)
			{
				return Rand.UnitVector3;
			}
			float num = Rand.Range(Mathf.Cos(angle * 0.017453292f), 1f);
			float f = Rand.Range(0f, 6.2831855f);
			Vector3 point = new Vector3(Mathf.Sqrt(1f - num * num) * Mathf.Cos(f), Mathf.Sqrt(1f - num * num) * Mathf.Sin(f), num);
			return Quaternion.FromToRotation(Vector3.forward, center) * point;
		}

		// Token: 0x04000251 RID: 593
		private static uint seed;

		// Token: 0x04000252 RID: 594
		private static uint iterations = 0U;

		// Token: 0x04000253 RID: 595
		private static Stack<ulong> stateStack = new Stack<ulong>();

		// Token: 0x04000254 RID: 596
		private static List<int> tmpRange = new List<int>();
	}
}
