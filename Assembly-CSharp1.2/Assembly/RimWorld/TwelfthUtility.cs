using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C3D RID: 7229
	public static class TwelfthUtility
	{
		// Token: 0x06009F35 RID: 40757 RVA: 0x002EA558 File Offset: 0x002E8758
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

		// Token: 0x06009F36 RID: 40758 RVA: 0x002EA5B8 File Offset: 0x002E87B8
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

		// Token: 0x06009F37 RID: 40759 RVA: 0x0006A0AD File Offset: 0x000682AD
		public static Twelfth NextTwelfth(this Twelfth twelfth)
		{
			if (twelfth == Twelfth.Undefined)
			{
				return Twelfth.Undefined;
			}
			return (twelfth + 1) % Twelfth.Undefined;
		}

		// Token: 0x06009F38 RID: 40760 RVA: 0x0006A0BE File Offset: 0x000682BE
		public static float GetMiddleYearPct(this Twelfth twelfth)
		{
			return ((float)twelfth + 0.5f) / 12f;
		}

		// Token: 0x06009F39 RID: 40761 RVA: 0x0006A0CE File Offset: 0x000682CE
		public static float GetBeginningYearPct(this Twelfth twelfth)
		{
			return (float)twelfth / 12f;
		}

		// Token: 0x06009F3A RID: 40762 RVA: 0x002EA5DC File Offset: 0x002E87DC
		public static Twelfth FindStartingWarmTwelfth(int tile)
		{
			Twelfth twelfth = GenTemperature.EarliestTwelfthInAverageTemperatureRange(tile, 16f, 9999f);
			if (twelfth == Twelfth.Undefined)
			{
				twelfth = Season.Summer.GetFirstTwelfth(Find.WorldGrid.LongLatOf(tile).y);
			}
			return twelfth;
		}

		// Token: 0x06009F3B RID: 40763 RVA: 0x002EA618 File Offset: 0x002E8818
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

		// Token: 0x06009F3C RID: 40764 RVA: 0x002EA648 File Offset: 0x002E8848
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

		// Token: 0x06009F3D RID: 40765 RVA: 0x0006A0D8 File Offset: 0x000682D8
		public static Twelfth TwelfthBefore(Twelfth m)
		{
			if (m == Twelfth.First)
			{
				return Twelfth.Twelfth;
			}
			return (Twelfth)(m - Twelfth.Second);
		}

		// Token: 0x06009F3E RID: 40766 RVA: 0x0006A0E4 File Offset: 0x000682E4
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
