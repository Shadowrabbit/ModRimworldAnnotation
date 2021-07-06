using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C34 RID: 7220
	public static class GenDate
	{
		// Token: 0x170018C0 RID: 6336
		// (get) Token: 0x06009EC1 RID: 40641 RVA: 0x00069A30 File Offset: 0x00067C30
		private static int TicksGame
		{
			get
			{
				return Find.TickManager.TicksGame;
			}
		}

		// Token: 0x170018C1 RID: 6337
		// (get) Token: 0x06009EC2 RID: 40642 RVA: 0x00069A3C File Offset: 0x00067C3C
		public static int DaysPassed
		{
			get
			{
				return GenDate.DaysPassedAt(GenDate.TicksGame);
			}
		}

		// Token: 0x170018C2 RID: 6338
		// (get) Token: 0x06009EC3 RID: 40643 RVA: 0x00069A48 File Offset: 0x00067C48
		public static float DaysPassedFloat
		{
			get
			{
				return (float)GenDate.TicksGame / 60000f;
			}
		}

		// Token: 0x170018C3 RID: 6339
		// (get) Token: 0x06009EC4 RID: 40644 RVA: 0x00069A56 File Offset: 0x00067C56
		public static int TwelfthsPassed
		{
			get
			{
				return GenDate.TwelfthsPassedAt(GenDate.TicksGame);
			}
		}

		// Token: 0x170018C4 RID: 6340
		// (get) Token: 0x06009EC5 RID: 40645 RVA: 0x00069A62 File Offset: 0x00067C62
		public static float TwelfthsPassedFloat
		{
			get
			{
				return (float)GenDate.TicksGame / 300000f;
			}
		}

		// Token: 0x170018C5 RID: 6341
		// (get) Token: 0x06009EC6 RID: 40646 RVA: 0x00069A70 File Offset: 0x00067C70
		public static int YearsPassed
		{
			get
			{
				return GenDate.YearsPassedAt(GenDate.TicksGame);
			}
		}

		// Token: 0x170018C6 RID: 6342
		// (get) Token: 0x06009EC7 RID: 40647 RVA: 0x00069A7C File Offset: 0x00067C7C
		public static float YearsPassedFloat
		{
			get
			{
				return (float)GenDate.TicksGame / 3600000f;
			}
		}

		// Token: 0x06009EC8 RID: 40648 RVA: 0x00069A8A File Offset: 0x00067C8A
		public static int TickAbsToGame(int absTick)
		{
			return absTick - Find.TickManager.gameStartAbsTick;
		}

		// Token: 0x06009EC9 RID: 40649 RVA: 0x00069A98 File Offset: 0x00067C98
		public static int TickGameToAbs(int gameTick)
		{
			return gameTick + Find.TickManager.gameStartAbsTick;
		}

		// Token: 0x06009ECA RID: 40650 RVA: 0x00069AA6 File Offset: 0x00067CA6
		public static int DaysPassedAt(int gameTicks)
		{
			return Mathf.FloorToInt((float)gameTicks / 60000f);
		}

		// Token: 0x06009ECB RID: 40651 RVA: 0x00069AB5 File Offset: 0x00067CB5
		public static int TwelfthsPassedAt(int gameTicks)
		{
			return Mathf.FloorToInt((float)gameTicks / 300000f);
		}

		// Token: 0x06009ECC RID: 40652 RVA: 0x00069AC4 File Offset: 0x00067CC4
		public static int YearsPassedAt(int gameTicks)
		{
			return Mathf.FloorToInt((float)gameTicks / 3600000f);
		}

		// Token: 0x06009ECD RID: 40653 RVA: 0x00069AD3 File Offset: 0x00067CD3
		private static long LocalTicksOffsetFromLongitude(float longitude)
		{
			return (long)GenDate.TimeZoneAt(longitude) * 2500L;
		}

		// Token: 0x06009ECE RID: 40654 RVA: 0x00069AE3 File Offset: 0x00067CE3
		public static int HourOfDay(long absTicks, float longitude)
		{
			return GenMath.PositiveModRemap(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 2500, 24);
		}

		// Token: 0x06009ECF RID: 40655 RVA: 0x00069AF9 File Offset: 0x00067CF9
		public static int DayOfTwelfth(long absTicks, float longitude)
		{
			return GenMath.PositiveModRemap(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 60000, 5);
		}

		// Token: 0x06009ED0 RID: 40656 RVA: 0x00069B0E File Offset: 0x00067D0E
		public static int DayOfYear(long absTicks, float longitude)
		{
			return GenMath.PositiveModRemap(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 60000, 60);
		}

		// Token: 0x06009ED1 RID: 40657 RVA: 0x00069B24 File Offset: 0x00067D24
		public static Twelfth Twelfth(long absTicks, float longitude)
		{
			return (Twelfth)GenMath.PositiveModRemap(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 300000, 12);
		}

		// Token: 0x06009ED2 RID: 40658 RVA: 0x00069B3B File Offset: 0x00067D3B
		public static Season Season(long absTicks, Vector2 longLat)
		{
			return GenDate.Season(absTicks, longLat.y, longLat.x);
		}

		// Token: 0x06009ED3 RID: 40659 RVA: 0x00069B4F File Offset: 0x00067D4F
		public static Season Season(long absTicks, float latitude, float longitude)
		{
			return SeasonUtility.GetReportedSeason(GenDate.YearPercent(absTicks, longitude), latitude);
		}

		// Token: 0x06009ED4 RID: 40660 RVA: 0x00069B5E File Offset: 0x00067D5E
		public static Quadrum Quadrum(long absTicks, float longitude)
		{
			return GenDate.Twelfth(absTicks, longitude).GetQuadrum();
		}

		// Token: 0x06009ED5 RID: 40661 RVA: 0x002E9168 File Offset: 0x002E7368
		public static int Year(long absTicks, float longitude)
		{
			long num = absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude);
			return 5500 + Mathf.FloorToInt((float)num / 3600000f);
		}

		// Token: 0x06009ED6 RID: 40662 RVA: 0x00069B6C File Offset: 0x00067D6C
		public static int DayOfSeason(long absTicks, float longitude)
		{
			return (GenDate.DayOfYear(absTicks, longitude) - (int)(SeasonUtility.FirstSeason.GetFirstTwelfth(0f) * RimWorld.Twelfth.Sixth)) % 15;
		}

		// Token: 0x06009ED7 RID: 40663 RVA: 0x00069B8A File Offset: 0x00067D8A
		public static int DayOfQuadrum(long absTicks, float longitude)
		{
			return (GenDate.DayOfYear(absTicks, longitude) - (int)(QuadrumUtility.FirstQuadrum.GetFirstTwelfth() * RimWorld.Twelfth.Sixth)) % 15;
		}

		// Token: 0x06009ED8 RID: 40664 RVA: 0x00069BA3 File Offset: 0x00067DA3
		public static int DayTick(long absTicks, float longitude)
		{
			return (int)GenMath.PositiveMod(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 60000L);
		}

		// Token: 0x06009ED9 RID: 40665 RVA: 0x002E9194 File Offset: 0x002E7394
		public static float DayPercent(long absTicks, float longitude)
		{
			int num = GenDate.DayTick(absTicks, longitude);
			if (num == 0)
			{
				num = 1;
			}
			return (float)num / 60000f;
		}

		// Token: 0x06009EDA RID: 40666 RVA: 0x00069BB9 File Offset: 0x00067DB9
		public static float YearPercent(long absTicks, float longitude)
		{
			return (float)((int)GenMath.PositiveMod(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 3600000L)) / 3600000f;
		}

		// Token: 0x06009EDB RID: 40667 RVA: 0x00069AE3 File Offset: 0x00067CE3
		public static int HourInteger(long absTicks, float longitude)
		{
			return GenMath.PositiveModRemap(absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude), 2500, 24);
		}

		// Token: 0x06009EDC RID: 40668 RVA: 0x00069BD6 File Offset: 0x00067DD6
		public static float HourFloat(long absTicks, float longitude)
		{
			return GenDate.DayPercent(absTicks, longitude) * 24f;
		}

		// Token: 0x06009EDD RID: 40669 RVA: 0x002E91B8 File Offset: 0x002E73B8
		public static string DateFullStringAt(long absTicks, Vector2 location)
		{
			int num = GenDate.DayOfSeason(absTicks, location.x) + 1;
			string value = Find.ActiveLanguageWorker.OrdinalNumber(num, Gender.None);
			return "FullDate".Translate(value, GenDate.Quadrum(absTicks, location.x).Label(), GenDate.Year(absTicks, location.x), num);
		}

		// Token: 0x06009EDE RID: 40670 RVA: 0x00069BE5 File Offset: 0x00067DE5
		public static string DateFullStringWithHourAt(long absTicks, Vector2 location)
		{
			return GenDate.DateFullStringAt(absTicks, location) + ", " + GenDate.HourInteger(absTicks, location.x) + "LetterHour".Translate();
		}

		// Token: 0x06009EDF RID: 40671 RVA: 0x002E9224 File Offset: 0x002E7424
		public static string DateReadoutStringAt(long absTicks, Vector2 location)
		{
			int num = GenDate.DayOfSeason(absTicks, location.x) + 1;
			string value = Find.ActiveLanguageWorker.OrdinalNumber(num, Gender.None);
			return "DateReadout".Translate(value, GenDate.Quadrum(absTicks, location.x).Label(), GenDate.Year(absTicks, location.x), num);
		}

		// Token: 0x06009EE0 RID: 40672 RVA: 0x002E9290 File Offset: 0x002E7490
		public static string DateShortStringAt(long absTicks, Vector2 location)
		{
			int value = GenDate.DayOfSeason(absTicks, location.x) + 1;
			return "ShortDate".Translate(value, GenDate.Quadrum(absTicks, location.x).LabelShort(), GenDate.Year(absTicks, location.x), value);
		}

		// Token: 0x06009EE1 RID: 40673 RVA: 0x002E92F0 File Offset: 0x002E74F0
		public static string SeasonDateStringAt(long absTicks, Vector2 longLat)
		{
			int num = GenDate.DayOfSeason(absTicks, longLat.x) + 1;
			string value = Find.ActiveLanguageWorker.OrdinalNumber(num, Gender.None);
			return "SeasonFullDate".Translate(value, GenDate.Season(absTicks, longLat).Label(), num);
		}

		// Token: 0x06009EE2 RID: 40674 RVA: 0x00069C1D File Offset: 0x00067E1D
		public static string SeasonDateStringAt(Twelfth twelfth, Vector2 longLat)
		{
			return GenDate.SeasonDateStringAt((long)((int)twelfth * 300000 + 1), longLat);
		}

		// Token: 0x06009EE3 RID: 40675 RVA: 0x002E9348 File Offset: 0x002E7548
		public static string QuadrumDateStringAt(long absTicks, float longitude)
		{
			int num = GenDate.DayOfQuadrum(absTicks, longitude) + 1;
			string value = Find.ActiveLanguageWorker.OrdinalNumber(num, Gender.None);
			return "SeasonFullDate".Translate(value, GenDate.Quadrum(absTicks, longitude).Label(), num);
		}

		// Token: 0x06009EE4 RID: 40676 RVA: 0x00069C2F File Offset: 0x00067E2F
		public static string QuadrumDateStringAt(Quadrum quadrum)
		{
			return GenDate.QuadrumDateStringAt((long)((int)quadrum * 900000 + 1), 0f);
		}

		// Token: 0x06009EE5 RID: 40677 RVA: 0x00069C45 File Offset: 0x00067E45
		public static string QuadrumDateStringAt(Twelfth twelfth)
		{
			return GenDate.QuadrumDateStringAt((long)((int)twelfth * 300000 + 1), 0f);
		}

		// Token: 0x06009EE6 RID: 40678 RVA: 0x00069C5B File Offset: 0x00067E5B
		public static float TicksToDays(this int numTicks)
		{
			return (float)numTicks / 60000f;
		}

		// Token: 0x06009EE7 RID: 40679 RVA: 0x002E9398 File Offset: 0x002E7598
		public static string ToStringTicksToDays(this int numTicks, string format = "F1")
		{
			string text = numTicks.TicksToDays().ToString(format);
			if (text == "1")
			{
				return "Period1Day".Translate();
			}
			return text + " " + "DaysLower".Translate();
		}

		// Token: 0x06009EE8 RID: 40680 RVA: 0x002E93F4 File Offset: 0x002E75F4
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

		// Token: 0x06009EE9 RID: 40681 RVA: 0x002E966C File Offset: 0x002E786C
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

		// Token: 0x06009EEA RID: 40682 RVA: 0x002E98D0 File Offset: 0x002E7AD0
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

		// Token: 0x06009EEB RID: 40683 RVA: 0x00069C65 File Offset: 0x00067E65
		public static void TicksToPeriod(this int numTicks, out int years, out int quadrums, out int days, out float hoursFloat)
		{
			((long)numTicks).TicksToPeriod(out years, out quadrums, out days, out hoursFloat);
		}

		// Token: 0x06009EEC RID: 40684 RVA: 0x002E9920 File Offset: 0x002E7B20
		public static void TicksToPeriod(this long numTicks, out int years, out int quadrums, out int days, out float hoursFloat)
		{
			if (numTicks < 0L)
			{
				Log.ErrorOnce("Tried to calculate period for negative ticks", 12841103, false);
			}
			years = (int)(numTicks / 3600000L);
			long num = numTicks - (long)years * 3600000L;
			quadrums = (int)(num / 900000L);
			num -= (long)quadrums * 900000L;
			days = (int)(num / 60000L);
			num -= (long)days * 60000L;
			hoursFloat = (float)num / 2500f;
		}

		// Token: 0x06009EED RID: 40685 RVA: 0x002E9998 File Offset: 0x002E7B98
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

		// Token: 0x06009EEE RID: 40686 RVA: 0x00069C73 File Offset: 0x00067E73
		public static int TimeZoneAt(float longitude)
		{
			return Mathf.RoundToInt(GenDate.TimeZoneFloatAt(longitude));
		}

		// Token: 0x06009EEF RID: 40687 RVA: 0x00069C80 File Offset: 0x00067E80
		public static float TimeZoneFloatAt(float longitude)
		{
			return longitude / 15f;
		}

		// Token: 0x0400653E RID: 25918
		public const int TicksPerDay = 60000;

		// Token: 0x0400653F RID: 25919
		public const int HoursPerDay = 24;

		// Token: 0x04006540 RID: 25920
		public const int DaysPerTwelfth = 5;

		// Token: 0x04006541 RID: 25921
		public const int TwelfthsPerYear = 12;

		// Token: 0x04006542 RID: 25922
		public const int GameStartHourOfDay = 6;

		// Token: 0x04006543 RID: 25923
		public const int TicksPerTwelfth = 300000;

		// Token: 0x04006544 RID: 25924
		public const int TicksPerSeason = 900000;

		// Token: 0x04006545 RID: 25925
		public const int TicksPerQuadrum = 900000;

		// Token: 0x04006546 RID: 25926
		public const int TicksPerYear = 3600000;

		// Token: 0x04006547 RID: 25927
		public const int DaysPerYear = 60;

		// Token: 0x04006548 RID: 25928
		public const int DaysPerSeason = 15;

		// Token: 0x04006549 RID: 25929
		public const int DaysPerQuadrum = 15;

		// Token: 0x0400654A RID: 25930
		public const int TicksPerHour = 2500;

		// Token: 0x0400654B RID: 25931
		public const float TimeZoneWidth = 15f;

		// Token: 0x0400654C RID: 25932
		public const int DefaultStartingYear = 5500;
	}
}
