using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013F3 RID: 5107
	public static class GenDate
	{
		// Token: 0x170015B2 RID: 5554
		// (get) Token: 0x06007C6C RID: 31852 RVA: 0x002C26C1 File Offset: 0x002C08C1
		private static int TicksGame
		{
			get
			{
				return Find.TickManager.TicksGame;
			}
		}

		// Token: 0x170015B3 RID: 5555
		// (get) Token: 0x06007C6D RID: 31853 RVA: 0x002C26CD File Offset: 0x002C08CD
		public static int DaysPassed
		{
			get
			{
				return GenDate.DaysPassedAt(GenDate.TicksGame);
			}
		}

		// Token: 0x170015B4 RID: 5556
		// (get) Token: 0x06007C6E RID: 31854 RVA: 0x002C26D9 File Offset: 0x002C08D9
		public static float DaysPassedFloat
		{
			get
			{
				return (float)GenDate.TicksGame / 60000f;
			}
		}

		// Token: 0x170015B5 RID: 5557
		// (get) Token: 0x06007C6F RID: 31855 RVA: 0x002C26E7 File Offset: 0x002C08E7
		public static int TwelfthsPassed
		{
			get
			{
				return GenDate.TwelfthsPassedAt(GenDate.TicksGame);
			}
		}

		// Token: 0x170015B6 RID: 5558
		// (get) Token: 0x06007C70 RID: 31856 RVA: 0x002C26F3 File Offset: 0x002C08F3
		public static float TwelfthsPassedFloat
		{
			get
			{
				return (float)GenDate.TicksGame / 300000f;
			}
		}

		// Token: 0x170015B7 RID: 5559
		// (get) Token: 0x06007C71 RID: 31857 RVA: 0x002C2701 File Offset: 0x002C0901
		public static int YearsPassed
		{
			get
			{
				return GenDate.YearsPassedAt(GenDate.TicksGame);
			}
		}

		// Token: 0x170015B8 RID: 5560
		// (get) Token: 0x06007C72 RID: 31858 RVA: 0x002C270D File Offset: 0x002C090D
		public static float YearsPassedFloat
		{
			get
			{
				return (float)GenDate.TicksGame / 3600000f;
			}
		}

		// Token: 0x06007C73 RID: 31859 RVA: 0x002C271B File Offset: 0x002C091B
		public static int TickAbsToGame(int absTick)
		{
			return absTick - Find.TickManager.gameStartAbsTick;
		}

		// Token: 0x06007C74 RID: 31860 RVA: 0x002C2729 File Offset: 0x002C0929
		public static int TickGameToAbs(int gameTick)
		{
			return gameTick + Find.TickManager.gameStartAbsTick;
		}

		// Token: 0x06007C75 RID: 31861 RVA: 0x002C2737 File Offset: 0x002C0937
		public static int DaysPassedAt(int gameTicks)
		{
			return Mathf.FloorToInt((float)gameTicks / 60000f);
		}

		// Token: 0x06007C76 RID: 31862 RVA: 0x002C2746 File Offset: 0x002C0946
		public static int TwelfthsPassedAt(int gameTicks)
		{
			return Mathf.FloorToInt((float)gameTicks / 300000f);
		}

		// Token: 0x06007C77 RID: 31863 RVA: 0x002C2755 File Offset: 0x002C0955
		public static int YearsPassedAt(int gameTicks)
		{
			return Mathf.FloorToInt((float)gameTicks / 3600000f);
		}

		// Token: 0x06007C78 RID: 31864 RVA: 0x002C2764 File Offset: 0x002C0964
		public static long LocalTicksOffsetFromLongitude(float longitude)
		{
			return (long)GenDate.TimeZoneAt(longitude) * 2500L;
		}

		// Token: 0x06007C79 RID: 31865 RVA: 0x002C2774 File Offset: 0x002C0974
		public static int HourOfDay(long absTicks, float longitude)
		{
			return GenMath.PositiveModRemap(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 2500, 24);
		}

		// Token: 0x06007C7A RID: 31866 RVA: 0x002C278A File Offset: 0x002C098A
		public static int DayOfTwelfth(long absTicks, float longitude)
		{
			return GenMath.PositiveModRemap(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 60000, 5);
		}

		// Token: 0x06007C7B RID: 31867 RVA: 0x002C279F File Offset: 0x002C099F
		public static int DayOfYear(long absTicks, float longitude)
		{
			return GenMath.PositiveModRemap(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 60000, 60);
		}

		// Token: 0x06007C7C RID: 31868 RVA: 0x002C27B5 File Offset: 0x002C09B5
		public static Twelfth Twelfth(long absTicks, float longitude)
		{
			return (Twelfth)GenMath.PositiveModRemap(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 300000, 12);
		}

		// Token: 0x06007C7D RID: 31869 RVA: 0x002C27CC File Offset: 0x002C09CC
		public static Season Season(long absTicks, Vector2 longLat)
		{
			return GenDate.Season(absTicks, longLat.y, longLat.x);
		}

		// Token: 0x06007C7E RID: 31870 RVA: 0x002C27E0 File Offset: 0x002C09E0
		public static Season Season(long absTicks, float latitude, float longitude)
		{
			return SeasonUtility.GetReportedSeason(GenDate.YearPercent(absTicks, longitude), latitude);
		}

		// Token: 0x06007C7F RID: 31871 RVA: 0x002C27EF File Offset: 0x002C09EF
		public static Quadrum Quadrum(long absTicks, float longitude)
		{
			return GenDate.Twelfth(absTicks, longitude).GetQuadrum();
		}

		// Token: 0x06007C80 RID: 31872 RVA: 0x002C2800 File Offset: 0x002C0A00
		public static int Year(long absTicks, float longitude)
		{
			long num = absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude);
			return 5500 + Mathf.FloorToInt((float)num / 3600000f);
		}

		// Token: 0x06007C81 RID: 31873 RVA: 0x002C2829 File Offset: 0x002C0A29
		public static int DayOfSeason(long absTicks, float longitude)
		{
			return (GenDate.DayOfYear(absTicks, longitude) - (int)(SeasonUtility.FirstSeason.GetFirstTwelfth(0f) * RimWorld.Twelfth.Sixth)) % 15;
		}

		// Token: 0x06007C82 RID: 31874 RVA: 0x002C2847 File Offset: 0x002C0A47
		public static int DayOfQuadrum(long absTicks, float longitude)
		{
			return (GenDate.DayOfYear(absTicks, longitude) - (int)(QuadrumUtility.FirstQuadrum.GetFirstTwelfth() * RimWorld.Twelfth.Sixth)) % 15;
		}

		// Token: 0x06007C83 RID: 31875 RVA: 0x002C2860 File Offset: 0x002C0A60
		public static int DayTick(long absTicks, float longitude)
		{
			return (int)GenMath.PositiveMod(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 60000L);
		}

		// Token: 0x06007C84 RID: 31876 RVA: 0x002C2878 File Offset: 0x002C0A78
		public static float DayPercent(long absTicks, float longitude)
		{
			int num = GenDate.DayTick(absTicks, longitude);
			if (num == 0)
			{
				num = 1;
			}
			return (float)num / 60000f;
		}

		// Token: 0x06007C85 RID: 31877 RVA: 0x002C289A File Offset: 0x002C0A9A
		public static float YearPercent(long absTicks, float longitude)
		{
			return (float)((int)GenMath.PositiveMod(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 3600000L)) / 3600000f;
		}

		// Token: 0x06007C86 RID: 31878 RVA: 0x002C2774 File Offset: 0x002C0974
		public static int HourInteger(long absTicks, float longitude)
		{
			return GenMath.PositiveModRemap(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 2500, 24);
		}

		// Token: 0x06007C87 RID: 31879 RVA: 0x002C28B7 File Offset: 0x002C0AB7
		public static float HourFloat(long absTicks, float longitude)
		{
			return GenDate.DayPercent(absTicks, longitude) * 24f;
		}

		// Token: 0x06007C88 RID: 31880 RVA: 0x002C28C8 File Offset: 0x002C0AC8
		public static string DateFullStringAt(long absTicks, Vector2 location)
		{
			int num = GenDate.DayOfSeason(absTicks, location.x) + 1;
			string value = Find.ActiveLanguageWorker.OrdinalNumber(num, Gender.None);
			return "FullDate".Translate(value, GenDate.Quadrum(absTicks, location.x).Label(), GenDate.Year(absTicks, location.x), num);
		}

		// Token: 0x06007C89 RID: 31881 RVA: 0x002C2933 File Offset: 0x002C0B33
		public static string DateFullStringWithHourAt(long absTicks, Vector2 location)
		{
			return GenDate.DateFullStringAt(absTicks, location) + ", " + GenDate.HourInteger(absTicks, location.x) + "LetterHour".Translate();
		}

		// Token: 0x06007C8A RID: 31882 RVA: 0x002C296C File Offset: 0x002C0B6C
		public static string DateReadoutStringAt(long absTicks, Vector2 location)
		{
			int num = GenDate.DayOfSeason(absTicks, location.x) + 1;
			string value = Find.ActiveLanguageWorker.OrdinalNumber(num, Gender.None);
			return "DateReadout".Translate(value, GenDate.Quadrum(absTicks, location.x).Label(), GenDate.Year(absTicks, location.x), num);
		}

		// Token: 0x06007C8B RID: 31883 RVA: 0x002C29D8 File Offset: 0x002C0BD8
		public static string DateShortStringAt(long absTicks, Vector2 location)
		{
			int value = GenDate.DayOfSeason(absTicks, location.x) + 1;
			return "ShortDate".Translate(value, GenDate.Quadrum(absTicks, location.x).LabelShort(), GenDate.Year(absTicks, location.x), value);
		}

		// Token: 0x06007C8C RID: 31884 RVA: 0x002C2A38 File Offset: 0x002C0C38
		public static string SeasonDateStringAt(long absTicks, Vector2 longLat)
		{
			int num = GenDate.DayOfSeason(absTicks, longLat.x) + 1;
			string value = Find.ActiveLanguageWorker.OrdinalNumber(num, Gender.None);
			return "SeasonFullDate".Translate(value, GenDate.Season(absTicks, longLat).Label(), num);
		}

		// Token: 0x06007C8D RID: 31885 RVA: 0x002C2A8D File Offset: 0x002C0C8D
		public static string SeasonDateStringAt(Twelfth twelfth, Vector2 longLat)
		{
			return GenDate.SeasonDateStringAt((long)((int)twelfth * 300000 + 1), longLat);
		}

		// Token: 0x06007C8E RID: 31886 RVA: 0x002C2AA0 File Offset: 0x002C0CA0
		public static string QuadrumDateStringAt(long absTicks, float longitude)
		{
			int num = GenDate.DayOfQuadrum(absTicks, longitude) + 1;
			string value = Find.ActiveLanguageWorker.OrdinalNumber(num, Gender.None);
			return "SeasonFullDate".Translate(value, GenDate.Quadrum(absTicks, longitude).Label(), num);
		}

		// Token: 0x06007C8F RID: 31887 RVA: 0x002C2AF0 File Offset: 0x002C0CF0
		public static string QuadrumDateStringAt(Quadrum quadrum)
		{
			return GenDate.QuadrumDateStringAt((long)((int)quadrum * 900000 + 1), 0f);
		}

		// Token: 0x06007C90 RID: 31888 RVA: 0x002C2B06 File Offset: 0x002C0D06
		public static string QuadrumDateStringAt(Twelfth twelfth)
		{
			return GenDate.QuadrumDateStringAt((long)((int)twelfth * 300000 + 1), 0f);
		}

		// Token: 0x06007C91 RID: 31889 RVA: 0x002C2B1C File Offset: 0x002C0D1C
		public static float TicksToDays(this int numTicks)
		{
			return (float)numTicks / 60000f;
		}

		// Token: 0x06007C92 RID: 31890 RVA: 0x002C2B28 File Offset: 0x002C0D28
		public static string ToStringTicksToDays(this int numTicks, string format = "F1")
		{
			string text = numTicks.TicksToDays().ToString(format);
			if (text == "1")
			{
				return "Period1Day".Translate();
			}
			return text + " " + "DaysLower".Translate();
		}

		// Token: 0x06007C93 RID: 31891 RVA: 0x002C2B84 File Offset: 0x002C0D84
		public static string ToStringTicksToPeriod(this int numTicks, bool allowSeconds = true, bool shortForm = false, bool canUseDecimals = true, bool allowYears = true)
		{
			if (allowSeconds && numTicks < 2500 && (numTicks < 600 || Math.Round((double)((float)numTicks / 2500f), 1) == 0.0))
			{
				int num = Mathf.RoundToInt((float)numTicks / 60f);
				if (shortForm)
				{
					return num + "LetterSecond".Translate();
				}
				if (num == 1)
				{
					return "Period1Second".Translate();
				}
				return "PeriodSeconds".Translate(num);
			}
			else if (numTicks < 60000)
			{
				if (shortForm)
				{
					return Mathf.RoundToInt((float)numTicks / 2500f) + "LetterHour".Translate();
				}
				if (numTicks < 2500)
				{
					string text = ((float)numTicks / 2500f).ToString("0.#");
					if (text == "1")
					{
						return "Period1Hour".Translate();
					}
					return "PeriodHours".Translate(text);
				}
				else
				{
					int num2 = Mathf.RoundToInt((float)numTicks / 2500f);
					if (num2 == 1)
					{
						return "Period1Hour".Translate();
					}
					return "PeriodHours".Translate(num2);
				}
			}
			else if (numTicks < 3600000 || !allowYears)
			{
				if (shortForm)
				{
					return Mathf.RoundToInt((float)numTicks / 60000f) + "LetterDay".Translate();
				}
				string text2;
				if (canUseDecimals)
				{
					text2 = ((float)numTicks / 60000f).ToStringDecimalIfSmall();
				}
				else
				{
					text2 = Mathf.RoundToInt((float)numTicks / 60000f).ToString();
				}
				if (text2 == "1")
				{
					return "Period1Day".Translate();
				}
				return "PeriodDays".Translate(text2);
			}
			else
			{
				if (shortForm)
				{
					return Mathf.RoundToInt((float)numTicks / 3600000f) + "LetterYear".Translate();
				}
				string text3;
				if (canUseDecimals)
				{
					text3 = ((float)numTicks / 3600000f).ToStringDecimalIfSmall();
				}
				else
				{
					text3 = Mathf.RoundToInt((float)numTicks / 3600000f).ToString();
				}
				if (text3 == "1")
				{
					return "Period1Year".Translate();
				}
				return "PeriodYears".Translate(text3);
			}
		}

		// Token: 0x06007C94 RID: 31892 RVA: 0x002C2DFC File Offset: 0x002C0FFC
		public static string ToStringTicksToPeriodVerbose(this int numTicks, bool allowHours = true, bool allowQuadrums = true)
		{
			if (numTicks < 0)
			{
				return "0";
			}
			int num;
			int num2;
			int num3;
			float num4;
			numTicks.TicksToPeriod(out num, out num2, out num3, out num4);
			if (!allowQuadrums)
			{
				num3 += 15 * num2;
				num2 = 0;
			}
			if (num > 0)
			{
				string text;
				if (num == 1)
				{
					text = "Period1Year".Translate();
				}
				else
				{
					text = "PeriodYears".Translate(num);
				}
				if (num2 > 0)
				{
					text += ", ";
					if (num2 == 1)
					{
						text += "Period1Quadrum".Translate();
					}
					else
					{
						text += "PeriodQuadrums".Translate(num2);
					}
				}
				return text;
			}
			if (num2 > 0)
			{
				string text2;
				if (num2 == 1)
				{
					text2 = "Period1Quadrum".Translate();
				}
				else
				{
					text2 = "PeriodQuadrums".Translate(num2);
				}
				if (num3 > 0)
				{
					text2 += ", ";
					if (num3 == 1)
					{
						text2 += "Period1Day".Translate();
					}
					else
					{
						text2 += "PeriodDays".Translate(num3);
					}
				}
				return text2;
			}
			if (num3 > 0)
			{
				string text3;
				if (num3 == 1)
				{
					text3 = "Period1Day".Translate();
				}
				else
				{
					text3 = "PeriodDays".Translate(num3);
				}
				int num5 = (int)num4;
				if (allowHours && num5 > 0)
				{
					text3 += ", ";
					if (num5 == 1)
					{
						text3 += "Period1Hour".Translate();
					}
					else
					{
						text3 += "PeriodHours".Translate(num5);
					}
				}
				return text3;
			}
			if (!allowHours)
			{
				return "PeriodDays".Translate(0);
			}
			if (num4 > 1f)
			{
				int num6 = Mathf.RoundToInt(num4);
				if (num6 == 1)
				{
					return "Period1Hour".Translate();
				}
				return "PeriodHours".Translate(num6);
			}
			else
			{
				if (Math.Round((double)num4, 1) == 1.0)
				{
					return "Period1Hour".Translate();
				}
				return "PeriodHours".Translate(num4.ToString("0.#"));
			}
		}

		// Token: 0x06007C95 RID: 31893 RVA: 0x002C3060 File Offset: 0x002C1260
		public static string ToStringTicksToPeriodVague(this int numTicks, bool vagueMin = true, bool vagueMax = true)
		{
			if (vagueMax && numTicks > 36000000)
			{
				return "OverADecade".Translate();
			}
			if (vagueMin && numTicks < 60000)
			{
				return "LessThanADay".Translate();
			}
			return numTicks.ToStringTicksToPeriod(true, false, true, true);
		}

		// Token: 0x06007C96 RID: 31894 RVA: 0x002C30AD File Offset: 0x002C12AD
		public static void TicksToPeriod(this int numTicks, out int years, out int quadrums, out int days, out float hoursFloat)
		{
			((long)numTicks).TicksToPeriod(out years, out quadrums, out days, out hoursFloat);
		}

		// Token: 0x06007C97 RID: 31895 RVA: 0x002C30BC File Offset: 0x002C12BC
		public static void TicksToPeriod(this long numTicks, out int years, out int quadrums, out int days, out float hoursFloat)
		{
			if (numTicks < 0L)
			{
				Log.ErrorOnce("Tried to calculate period for negative ticks", 12841103);
			}
			years = (int)(numTicks / 3600000L);
			long num = numTicks - (long)years * 3600000L;
			quadrums = (int)(num / 900000L);
			num -= (long)quadrums * 900000L;
			days = (int)(num / 60000L);
			num -= (long)days * 60000L;
			hoursFloat = (float)num / 2500f;
		}

		// Token: 0x06007C98 RID: 31896 RVA: 0x002C3134 File Offset: 0x002C1334
		public static string ToStringApproxAge(this float yearsFloat)
		{
			if (yearsFloat >= 1f)
			{
				return ((int)yearsFloat).ToStringCached();
			}
			int num;
			int num2;
			int num3;
			float num4;
			Mathf.Min((int)(yearsFloat * 3600000f), 3599999).TicksToPeriod(out num, out num2, out num3, out num4);
			if (num > 0)
			{
				if (num == 1)
				{
					return "Period1Year".Translate();
				}
				return "PeriodYears".Translate(num);
			}
			else if (num2 > 0)
			{
				if (num2 == 1)
				{
					return "Period1Quadrum".Translate();
				}
				return "PeriodQuadrums".Translate(num2);
			}
			else if (num3 > 0)
			{
				if (num3 == 1)
				{
					return "Period1Day".Translate();
				}
				return "PeriodDays".Translate(num3);
			}
			else
			{
				int num5 = (int)num4;
				if (num5 == 1)
				{
					return "Period1Hour".Translate();
				}
				return "PeriodHours".Translate(num5);
			}
		}

		// Token: 0x06007C99 RID: 31897 RVA: 0x002C3229 File Offset: 0x002C1429
		public static int TimeZoneAt(float longitude)
		{
			return Mathf.RoundToInt(GenDate.TimeZoneFloatAt(longitude));
		}

		// Token: 0x06007C9A RID: 31898 RVA: 0x002C3236 File Offset: 0x002C1436
		public static float TimeZoneFloatAt(float longitude)
		{
			return longitude / 15f;
		}

		// Token: 0x040044DE RID: 17630
		public const int TicksPerDay = 60000;

		// Token: 0x040044DF RID: 17631
		public const int HoursPerDay = 24;

		// Token: 0x040044E0 RID: 17632
		public const int DaysPerTwelfth = 5;

		// Token: 0x040044E1 RID: 17633
		public const int TwelfthsPerYear = 12;

		// Token: 0x040044E2 RID: 17634
		public const int GameStartHourOfDay = 6;

		// Token: 0x040044E3 RID: 17635
		public const int TicksPerTwelfth = 300000;

		// Token: 0x040044E4 RID: 17636
		public const int TicksPerSeason = 900000;

		// Token: 0x040044E5 RID: 17637
		public const int TicksPerQuadrum = 900000;

		// Token: 0x040044E6 RID: 17638
		public const int TicksPerYear = 3600000;

		// Token: 0x040044E7 RID: 17639
		public const int DaysPerYear = 60;

		// Token: 0x040044E8 RID: 17640
		public const int DaysPerSeason = 15;

		// Token: 0x040044E9 RID: 17641
		public const int DaysPerQuadrum = 15;

		// Token: 0x040044EA RID: 17642
		public const int TicksPerHour = 2500;

		// Token: 0x040044EB RID: 17643
		public const float TimeZoneWidth = 15f;

		// Token: 0x040044EC RID: 17644
		public const int DefaultStartingYear = 5500;
	}
}
