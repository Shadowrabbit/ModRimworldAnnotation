using System;
using System.Globalization;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000041 RID: 65
	public static class MathEvaluatorCustomFunctions
	{
		// Token: 0x0600036D RID: 877 RVA: 0x00012774 File Offset: 0x00010974
		private static object Min(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return Math.Min(Convert.ToDouble(args[0], invariantCulture), Convert.ToDouble(args[1], invariantCulture));
		}

		// Token: 0x0600036E RID: 878 RVA: 0x000127A4 File Offset: 0x000109A4
		private static object Max(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return Math.Max(Convert.ToDouble(args[0], invariantCulture), Convert.ToDouble(args[1], invariantCulture));
		}

		// Token: 0x0600036F RID: 879 RVA: 0x000127D4 File Offset: 0x000109D4
		private static object Round(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return Math.Round(Convert.ToDouble(args[0], invariantCulture));
		}

		// Token: 0x06000370 RID: 880 RVA: 0x000127FC File Offset: 0x000109FC
		private static object RoundToDigits(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return Math.Round(Convert.ToDouble(args[0], invariantCulture), Convert.ToInt32(args[1], invariantCulture));
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0001282C File Offset: 0x00010A2C
		private static object Floor(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return Math.Floor(Convert.ToDouble(args[0], invariantCulture));
		}

		// Token: 0x06000372 RID: 882 RVA: 0x00012854 File Offset: 0x00010A54
		private static object RoundRandom(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return (double)GenMath.RoundRandom(Convert.ToSingle(args[0], invariantCulture));
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0001287C File Offset: 0x00010A7C
		private static object RandInt(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return (double)Rand.RangeInclusive(Convert.ToInt32(args[0], invariantCulture), Convert.ToInt32(args[1], invariantCulture));
		}

		// Token: 0x06000374 RID: 884 RVA: 0x000128AC File Offset: 0x00010AAC
		private static object RandFloat(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return (double)Rand.Range(Convert.ToSingle(args[0], invariantCulture), Convert.ToSingle(args[1], invariantCulture));
		}

		// Token: 0x06000375 RID: 885 RVA: 0x000128DC File Offset: 0x00010ADC
		private static object RangeAverage(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return (double)FloatRange.FromString(Convert.ToString(args[0], invariantCulture)).Average;
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0001290C File Offset: 0x00010B0C
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

		// Token: 0x06000377 RID: 887 RVA: 0x00012998 File Offset: 0x00010B98
		public static object Lerp(object[] args)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return Mathf.Lerp(Convert.ToSingle(args[0], invariantCulture), Convert.ToSingle(args[1], invariantCulture), Convert.ToSingle(args[2], invariantCulture));
		}

		// Token: 0x040000D2 RID: 210
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

		// Token: 0x02001899 RID: 6297
		public class FunctionType
		{
			// Token: 0x04005E21 RID: 24097
			public string name;

			// Token: 0x04005E22 RID: 24098
			public int minArgs;

			// Token: 0x04005E23 RID: 24099
			public int maxArgs;

			// Token: 0x04005E24 RID: 24100
			public Func<object[], object> func;
		}
	}
}
