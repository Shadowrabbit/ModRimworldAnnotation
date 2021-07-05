using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020013FC RID: 5116
	public static class TwelfthUtility
	{
		// Token: 0x06007CE4 RID: 31972 RVA: 0x002C41A4 File Offset: 0x002C23A4
		public static Quadrum GetQuadrum(this Twelfth twelfth)
		{
			switch (twelfth)
			{
			case Twelfth.First:
				return Quadrum.Aprimay;
			case Twelfth.Second:
				return Quadrum.Aprimay;
			case Twelfth.Third:
				return Quadrum.Aprimay;
			case Twelfth.Fourth:
				return Quadrum.Jugust;
			case Twelfth.Fifth:
				return Quadrum.Jugust;
			case Twelfth.Sixth:
				return Quadrum.Jugust;
			case Twelfth.Seventh:
				return Quadrum.Septober;
			case Twelfth.Eighth:
				return Quadrum.Septober;
			case Twelfth.Ninth:
				return Quadrum.Septober;
			case Twelfth.Tenth:
				return Quadrum.Decembary;
			case Twelfth.Eleventh:
				return Quadrum.Decembary;
			case Twelfth.Twelfth:
				return Quadrum.Decembary;
			default:
				return Quadrum.Undefined;
			}
		}

		// Token: 0x06007CE5 RID: 31973 RVA: 0x002C4204 File Offset: 0x002C2404
		public static Twelfth PreviousTwelfth(this Twelfth twelfth)
		{
			if (twelfth == Twelfth.Undefined)
			{
				return Twelfth.Undefined;
			}
			int num = (int)(twelfth - Twelfth.Second);
			if (num == -1)
			{
				num = 11;
			}
			return (Twelfth)num;
		}

		// Token: 0x06007CE6 RID: 31974 RVA: 0x002C4226 File Offset: 0x002C2426
		public static Twelfth NextTwelfth(this Twelfth twelfth)
		{
			if (twelfth == Twelfth.Undefined)
			{
				return Twelfth.Undefined;
			}
			return (twelfth + 1) % Twelfth.Undefined;
		}

		// Token: 0x06007CE7 RID: 31975 RVA: 0x002C4237 File Offset: 0x002C2437
		public static float GetMiddleYearPct(this Twelfth twelfth)
		{
			return ((float)twelfth + 0.5f) / 12f;
		}

		// Token: 0x06007CE8 RID: 31976 RVA: 0x002C4247 File Offset: 0x002C2447
		public static float GetBeginningYearPct(this Twelfth twelfth)
		{
			return (float)twelfth / 12f;
		}

		// Token: 0x06007CE9 RID: 31977 RVA: 0x002C4254 File Offset: 0x002C2454
		public static Twelfth FindStartingWarmTwelfth(int tile)
		{
			Twelfth twelfth = GenTemperature.EarliestTwelfthInAverageTemperatureRange(tile, 12f, 9999f);
			if (twelfth == Twelfth.Undefined)
			{
				twelfth = Season.Summer.GetFirstTwelfth(Find.WorldGrid.LongLatOf(tile).y);
			}
			return twelfth;
		}

		// Token: 0x06007CEA RID: 31978 RVA: 0x002C4290 File Offset: 0x002C2490
		public static Twelfth GetLeftMostTwelfth(List<Twelfth> twelfths, Twelfth rootTwelfth)
		{
			if (twelfths.Count >= 12)
			{
				return Twelfth.Undefined;
			}
			Twelfth result;
			do
			{
				result = rootTwelfth;
				rootTwelfth = TwelfthUtility.TwelfthBefore(rootTwelfth);
			}
			while (twelfths.Contains(rootTwelfth));
			return result;
		}

		// Token: 0x06007CEB RID: 31979 RVA: 0x002C42C0 File Offset: 0x002C24C0
		public static Twelfth GetRightMostTwelfth(List<Twelfth> twelfths, Twelfth rootTwelfth)
		{
			if (twelfths.Count >= 12)
			{
				return Twelfth.Undefined;
			}
			Twelfth m;
			do
			{
				m = rootTwelfth;
				rootTwelfth = TwelfthUtility.TwelfthAfter(rootTwelfth);
			}
			while (twelfths.Contains(rootTwelfth));
			return TwelfthUtility.TwelfthAfter(m);
		}

		// Token: 0x06007CEC RID: 31980 RVA: 0x002C42F3 File Offset: 0x002C24F3
		public static Twelfth TwelfthBefore(Twelfth m)
		{
			if (m == Twelfth.First)
			{
				return Twelfth.Twelfth;
			}
			return (Twelfth)(m - Twelfth.Second);
		}

		// Token: 0x06007CED RID: 31981 RVA: 0x002C42FF File Offset: 0x002C24FF
		public static Twelfth TwelfthAfter(Twelfth m)
		{
			if (m == Twelfth.Twelfth)
			{
				return Twelfth.First;
			}
			return m + 1;
		}
	}
}
