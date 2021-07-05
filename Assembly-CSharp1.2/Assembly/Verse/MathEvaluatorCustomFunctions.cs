using System;
using System.Globalization;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200007D RID: 125
	public static class MathEvaluatorCustomFunctions
	{
		// Token: 0x060004A9 RID: 1193 RVA: 0x000887A4 File Offset: 0x000869A4
		private static object Min(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return Math.Min(Convert.ToDouble(args[0], invariantCulture), Convert.ToDouble(args[1], invariantCulture));
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x000887D4 File Offset: 0x000869D4
		private static object Max(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return Math.Max(Convert.ToDouble(args[0], invariantCulture), Convert.ToDouble(args[1], invariantCulture));
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x00088804 File Offset: 0x00086A04
		private static object Round(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return Math.Round(Convert.ToDouble(args[0], invariantCulture));
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0008882C File Offset: 0x00086A2C
		private static object RoundToDigits(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return Math.Round(Convert.ToDouble(args[0], invariantCulture), Convert.ToInt32(args[1], invariantCulture));
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0008885C File Offset: 0x00086A5C
		private static object Floor(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return Math.Floor(Convert.ToDouble(args[0], invariantCulture));
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x00088884 File Offset: 0x00086A84
		private static object RoundRandom(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return (double)GenMath.RoundRandom(Convert.ToSingle(args[0], invariantCulture));
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x000888AC File Offset: 0x00086AAC
		private static object RandInt(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return (double)Rand.RangeInclusive(Convert.ToInt32(args[0], invariantCulture), Convert.ToInt32(args[1], invariantCulture));
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x000888DC File Offset: 0x00086ADC
		private static object RandFloat(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return (double)Rand.Range(Convert.ToSingle(args[0], invariantCulture), Convert.ToSingle(args[1], invariantCulture));
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x0008890C File Offset: 0x00086B0C
		private static object RangeAverage(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return (double)FloatRange.FromString(Convert.ToString(args[0], invariantCulture)).Average;
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x0008893C File Offset: 0x00086B3C
		private static object RoundToTicksRough(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			int num = Convert.ToInt32(args[0], invariantCulture);
			if (num <= 250)
			{
				return 250;
			}
			if (num < 5000)
			{
				return GenMath.RoundTo(num, 250);
			}
			if (num < 60000)
			{
				return GenMath.RoundTo(num, 2500);
			}
			if (num < 120000)
			{
				return GenMath.RoundTo(num, 6000);
			}
			return GenMath.RoundTo(num, 60000);
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x000889C8 File Offset: 0x00086BC8
		public static object Lerp(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return Mathf.Lerp(Convert.ToSingle(args[0], invariantCulture), Convert.ToSingle(args[1], invariantCulture), Convert.ToSingle(args[2], invariantCulture));
		}

		// Token: 0x04000225 RID: 549
		public static readonly MathEvaluatorCustomFunctions.FunctionType[] FunctionTypes = new MathEvaluatorCustomFunctions.FunctionType[]
		{
			new MathEvaluatorCustomFunctions.FunctionType
			{
				name = "min",
				minArgs = 2,
				maxArgs = 2,
				func = new Func<object[], object>(MathEvaluatorCustomFunctions.Min)
			},
			new MathEvaluatorCustomFunctions.FunctionType
			{
				name = "max",
				minArgs = 2,
				maxArgs = 2,
				func = new Func<object[], object>(MathEvaluatorCustomFunctions.Max)
			},
			new MathEvaluatorCustomFunctions.FunctionType
			{
				name = "round",
				minArgs = 1,
				maxArgs = 1,
				func = new Func<object[], object>(MathEvaluatorCustomFunctions.Round)
			},
			new MathEvaluatorCustomFunctions.FunctionType
			{
				name = "roundToDigits",
				minArgs = 2,
				maxArgs = 2,
				func = new Func<object[], object>(MathEvaluatorCustomFunctions.RoundToDigits)
			},
			new MathEvaluatorCustomFunctions.FunctionType
			{
				name = "floor",
				minArgs = 1,
				maxArgs = 1,
				func = new Func<object[], object>(MathEvaluatorCustomFunctions.Floor)
			},
			new MathEvaluatorCustomFunctions.FunctionType
			{
				name = "roundRandom",
				minArgs = 1,
				maxArgs = 1,
				func = new Func<object[], object>(MathEvaluatorCustomFunctions.RoundRandom)
			},
			new MathEvaluatorCustomFunctions.FunctionType
			{
				name = "randInt",
				minArgs = 2,
				maxArgs = 2,
				func = new Func<object[], object>(MathEvaluatorCustomFunctions.RandInt)
			},
			new MathEvaluatorCustomFunctions.FunctionType
			{
				name = "randFloat",
				minArgs = 2,
				maxArgs = 2,
				func = new Func<object[], object>(MathEvaluatorCustomFunctions.RandFloat)
			},
			new MathEvaluatorCustomFunctions.FunctionType
			{
				name = "rangeAverage",
				minArgs = 1,
				maxArgs = 1,
				func = new Func<object[], object>(MathEvaluatorCustomFunctions.RangeAverage)
			},
			new MathEvaluatorCustomFunctions.FunctionType
			{
				name = "roundToTicksRough",
				minArgs = 1,
				maxArgs = 1,
				func = new Func<object[], object>(MathEvaluatorCustomFunctions.RoundToTicksRough)
			},
			new MathEvaluatorCustomFunctions.FunctionType
			{
				name = "lerp",
				minArgs = 3,
				maxArgs = 3,
				func = new Func<object[], object>(MathEvaluatorCustomFunctions.Lerp)
			}
		};

		// Token: 0x0200007E RID: 126
		public class FunctionType
		{
			// Token: 0x04000226 RID: 550
			public string name;

			// Token: 0x04000227 RID: 551
			public int minArgs;

			// Token: 0x04000228 RID: 552
			public int maxArgs;

			// Token: 0x04000229 RID: 553
			public Func<object[], object> func;
		}
	}
}
