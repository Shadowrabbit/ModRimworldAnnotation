using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C3B RID: 7227
	public static class SeasonUtility
	{
		// Token: 0x170018C9 RID: 6345
		// (get) Token: 0x06009F27 RID: 40743 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public static Season FirstSeason
		{
			get
			{
				return Season.Spring;
			}
		}

		// Token: 0x06009F28 RID: 40744 RVA: 0x002E9F70 File Offset: 0x002E8170
		public static Season GetReportedSeason(float yearPct, float latitude)
		{
			float by;
			float by2;
			float by3;
			float by4;
			float num;
			float num2;
			SeasonUtility.GetSeason(yearPct, latitude, out by, out by2, out by3, out by4, out num, out num2);
			if (num == 1f)
			{
				return Season.PermanentSummer;
			}
			if (num2 == 1f)
			{
				return Season.PermanentWinter;
			}
			return GenMath.MaxBy<Season>(Season.Spring, by, Season.Summer, by2, Season.Fall, by3, Season.Winter, by4);
		}

		// Token: 0x06009F29 RID: 40745 RVA: 0x002E9FB4 File Offset: 0x002E81B4
		public static Season GetDominantSeason(float yearPct, float latitude)
		{
			float by;
			float by2;
			float by3;
			float by4;
			float by5;
			float by6;
			SeasonUtility.GetSeason(yearPct, latitude, out by, out by2, out by3, out by4, out by5, out by6);
			return GenMath.MaxBy<Season>(Season.Spring, by, Season.Summer, by2, Season.Fall, by3, Season.Winter, by4, Season.PermanentSummer, by5, Season.PermanentWinter, by6);
		}

		// Token: 0x06009F2A RID: 40746 RVA: 0x002E9FE8 File Offset: 0x002E81E8
		public static void GetSeason(float yearPct, float latitude, out float spring, out float summer, out float fall, out float winter, out float permanentSummer, out float permanentWinter)
		{
			yearPct = Mathf.Clamp01(yearPct);
			float num;
			float num2;
			float num3;
			LatitudeSectionUtility.GetLatitudeSection(latitude, out num, out num2, out num3);
			float num4;
			float num5;
			float num6;
			float num7;
			SeasonUtility.GetSeasonalAreaSeason(yearPct, out num4, out num5, out num6, out num7, true);
			float num8;
			float num9;
			float num10;
			float num11;
			SeasonUtility.GetSeasonalAreaSeason(yearPct, out num8, out num9, out num10, out num11, false);
			float num12 = Mathf.InverseLerp(-2.5f, 2.5f, latitude);
			float num13 = num12 * num4 + (1f - num12) * num8;
			float num14 = num12 * num5 + (1f - num12) * num9;
			float num15 = num12 * num6 + (1f - num12) * num10;
			float num16 = num12 * num7 + (1f - num12) * num11;
			spring = num13 * num2;
			summer = num14 * num2;
			fall = num15 * num2;
			winter = num16 * num2;
			permanentSummer = num;
			permanentWinter = num3;
		}

		// Token: 0x06009F2B RID: 40747 RVA: 0x002EA0A8 File Offset: 0x002E82A8
		private static void GetSeasonalAreaSeason(float yearPct, out float spring, out float summer, out float fall, out float winter, bool northernHemisphere)
		{
			yearPct = Mathf.Clamp01(yearPct);
			float x = northernHemisphere ? yearPct : ((yearPct + 0.5f) % 1f);
			float num = SeasonUtility.SeasonalAreaSeasons.Evaluate(x);
			if (num <= 1f)
			{
				winter = 1f - num;
				spring = num;
				summer = 0f;
				fall = 0f;
				return;
			}
			if (num <= 2f)
			{
				spring = 1f - (num - 1f);
				summer = num - 1f;
				fall = 0f;
				winter = 0f;
				return;
			}
			if (num <= 3f)
			{
				summer = 1f - (num - 2f);
				fall = num - 2f;
				spring = 0f;
				winter = 0f;
				return;
			}
			if (num <= 4f)
			{
				fall = 1f - (num - 3f);
				winter = num - 3f;
				spring = 0f;
				summer = 0f;
				return;
			}
			winter = 1f - (num - 4f);
			spring = num - 4f;
			summer = 0f;
			fall = 0f;
		}

		// Token: 0x06009F2C RID: 40748 RVA: 0x002EA1BC File Offset: 0x002E83BC
		public static Twelfth GetFirstTwelfth(this Season season, float latitude)
		{
			if (latitude >= 0f)
			{
				switch (season)
				{
				case Season.Spring:
					return Twelfth.First;
				case Season.Summer:
					return Twelfth.Fourth;
				case Season.Fall:
					return Twelfth.Seventh;
				case Season.Winter:
					return Twelfth.Tenth;
				case Season.PermanentSummer:
					return Twelfth.First;
				case Season.PermanentWinter:
					return Twelfth.First;
				}
			}
			else
			{
				switch (season)
				{
				case Season.Spring:
					return Twelfth.Seventh;
				case Season.Summer:
					return Twelfth.Tenth;
				case Season.Fall:
					return Twelfth.First;
				case Season.Winter:
					return Twelfth.Fourth;
				case Season.PermanentSummer:
					return Twelfth.First;
				case Season.PermanentWinter:
					return Twelfth.First;
				}
			}
			return Twelfth.Undefined;
		}

		// Token: 0x06009F2D RID: 40749 RVA: 0x002EA234 File Offset: 0x002E8434
		public static Twelfth GetMiddleTwelfth(this Season season, float latitude)
		{
			if (latitude >= 0f)
			{
				switch (season)
				{
				case Season.Spring:
					return Twelfth.Second;
				case Season.Summer:
					return Twelfth.Fifth;
				case Season.Fall:
					return Twelfth.Eighth;
				case Season.Winter:
					return Twelfth.Eleventh;
				case Season.PermanentSummer:
					return Twelfth.Sixth;
				case Season.PermanentWinter:
					return Twelfth.Sixth;
				}
			}
			else
			{
				switch (season)
				{
				case Season.Spring:
					return Twelfth.Eighth;
				case Season.Summer:
					return Twelfth.Eleventh;
				case Season.Fall:
					return Twelfth.Second;
				case Season.Winter:
					return Twelfth.Fifth;
				case Season.PermanentSummer:
					return Twelfth.Sixth;
				case Season.PermanentWinter:
					return Twelfth.Sixth;
				}
			}
			return Twelfth.Undefined;
		}

		// Token: 0x06009F2E RID: 40750 RVA: 0x0006A054 File Offset: 0x00068254
		public static Season GetPreviousSeason(this Season season)
		{
			switch (season)
			{
			case Season.Undefined:
				return Season.Undefined;
			case Season.Spring:
				return Season.Winter;
			case Season.Summer:
				return Season.Spring;
			case Season.Fall:
				return Season.Summer;
			case Season.Winter:
				return Season.Fall;
			case Season.PermanentSummer:
				return Season.PermanentSummer;
			case Season.PermanentWinter:
				return Season.PermanentWinter;
			default:
				return Season.Undefined;
			}
		}

		// Token: 0x06009F2F RID: 40751 RVA: 0x0006A089 File Offset: 0x00068289
		public static float GetMiddleYearPct(this Season season, float latitude)
		{
			if (season == Season.Undefined)
			{
				return 0.5f;
			}
			return season.GetMiddleTwelfth(latitude).GetMiddleYearPct();
		}

		// Token: 0x06009F30 RID: 40752 RVA: 0x002EA2AC File Offset: 0x002E84AC
		public static string Label(this Season season)
		{
			switch (season)
			{
			case Season.Spring:
				return "SeasonSpring".Translate();
			case Season.Summer:
				return "SeasonSummer".Translate();
			case Season.Fall:
				return "SeasonFall".Translate();
			case Season.Winter:
				return "SeasonWinter".Translate();
			case Season.PermanentSummer:
				return "SeasonPermanentSummer".Translate();
			case Season.PermanentWinter:
				return "SeasonPermanentWinter".Translate();
			default:
				return "Unknown season";
			}
		}

		// Token: 0x06009F31 RID: 40753 RVA: 0x0006A0A0 File Offset: 0x000682A0
		public static string LabelCap(this Season season)
		{
			return season.Label().CapitalizeFirst();
		}

		// Token: 0x06009F32 RID: 40754 RVA: 0x002EA340 File Offset: 0x002E8540
		public static string SeasonsRangeLabel(List<Twelfth> twelfths, Vector2 longLat)
		{
			if (twelfths.Count == 0)
			{
				return "";
			}
			if (twelfths.Count == 12)
			{
				return "WholeYear".Translate();
			}
			string text = "";
			for (int i = 0; i < 12; i++)
			{
				Twelfth twelfth = (Twelfth)i;
				if (twelfths.Contains(twelfth))
				{
					if (!text.NullOrEmpty())
					{
						text += ", ";
					}
					text += SeasonUtility.SeasonsContinuousRangeLabel(twelfths, twelfth, longLat);
				}
			}
			return text;
		}

		// Token: 0x06009F33 RID: 40755 RVA: 0x002EA3B8 File Offset: 0x002E85B8
		private static string SeasonsContinuousRangeLabel(List<Twelfth> twelfths, Twelfth rootTwelfth, Vector2 longLat)
		{
			Twelfth leftMostTwelfth = TwelfthUtility.GetLeftMostTwelfth(twelfths, rootTwelfth);
			Twelfth rightMostTwelfth = TwelfthUtility.GetRightMostTwelfth(twelfths, rootTwelfth);
			for (Twelfth twelfth = leftMostTwelfth; twelfth != rightMostTwelfth; twelfth = TwelfthUtility.TwelfthAfter(twelfth))
			{
				if (!twelfths.Contains(twelfth))
				{
					Log.Error(string.Concat(new object[]
					{
						"Twelfths doesn't contain ",
						twelfth,
						" (",
						leftMostTwelfth,
						"..",
						rightMostTwelfth,
						")"
					}), false);
					break;
				}
				twelfths.Remove(twelfth);
			}
			twelfths.Remove(rightMostTwelfth);
			return GenDate.SeasonDateStringAt(leftMostTwelfth, longLat) + " - " + GenDate.SeasonDateStringAt(rightMostTwelfth, longLat);
		}

		// Token: 0x04006561 RID: 25953
		private const float HemisphereLerpDistance = 5f;

		// Token: 0x04006562 RID: 25954
		private const float SeasonYearPctLerpDistance = 0.085f;

		// Token: 0x04006563 RID: 25955
		private static readonly SimpleCurve SeasonalAreaSeasons = new SimpleCurve
		{
			{
				new CurvePoint(-0.042500004f, 0f),
				true
			},
			{
				new CurvePoint(0.042500004f, 1f),
				true
			},
			{
				new CurvePoint(0.2075f, 1f),
				true
			},
			{
				new CurvePoint(0.29250002f, 2f),
				true
			},
			{
				new CurvePoint(0.45749998f, 2f),
				true
			},
			{
				new CurvePoint(0.5425f, 3f),
				true
			},
			{
				new CurvePoint(0.7075f, 3f),
				true
			},
			{
				new CurvePoint(0.7925f, 4f),
				true
			},
			{
				new CurvePoint(0.9575f, 4f),
				true
			},
			{
				new CurvePoint(1.0425f, 5f),
				true
			}
		};
	}
}
