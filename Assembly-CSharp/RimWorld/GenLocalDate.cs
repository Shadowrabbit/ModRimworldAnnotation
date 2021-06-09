using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C35 RID: 7221
	public static class GenLocalDate
	{
		// Token: 0x170018C7 RID: 6343
		// (get) Token: 0x06009EF0 RID: 40688 RVA: 0x00069C89 File Offset: 0x00067E89
		private static int TicksAbs
		{
			get
			{
				return GenTicks.TicksAbs;
			}
		}

		// Token: 0x06009EF1 RID: 40689 RVA: 0x00069C90 File Offset: 0x00067E90
		public static int DayOfYear(Map map)
		{
			return GenLocalDate.DayOfYear(map.Tile);
		}

		// Token: 0x06009EF2 RID: 40690 RVA: 0x00069C9D File Offset: 0x00067E9D
		public static int HourOfDay(Map map)
		{
			return GenLocalDate.HourOfDay(map.Tile);
		}

		// Token: 0x06009EF3 RID: 40691 RVA: 0x00069CAA File Offset: 0x00067EAA
		public static int DayOfTwelfth(Map map)
		{
			return GenLocalDate.DayOfTwelfth(map.Tile);
		}

		// Token: 0x06009EF4 RID: 40692 RVA: 0x00069CB7 File Offset: 0x00067EB7
		public static Twelfth Twelfth(Map map)
		{
			return GenLocalDate.Twelfth(map.Tile);
		}

		// Token: 0x06009EF5 RID: 40693 RVA: 0x00069CC4 File Offset: 0x00067EC4
		public static Season Season(Map map)
		{
			return GenLocalDate.Season(map.Tile);
		}

		// Token: 0x06009EF6 RID: 40694 RVA: 0x00069CD1 File Offset: 0x00067ED1
		public static int Year(Map map)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return 5500;
			}
			return GenLocalDate.Year(map.Tile);
		}

		// Token: 0x06009EF7 RID: 40695 RVA: 0x00069CEC File Offset: 0x00067EEC
		public static int DayOfSeason(Map map)
		{
			return GenLocalDate.DayOfSeason(map.Tile);
		}

		// Token: 0x06009EF8 RID: 40696 RVA: 0x00069CF9 File Offset: 0x00067EF9
		public static int DayOfQuadrum(Map map)
		{
			return GenLocalDate.DayOfQuadrum(map.Tile);
		}

		// Token: 0x06009EF9 RID: 40697 RVA: 0x00069D06 File Offset: 0x00067F06
		public static int DayTick(Map map)
		{
			return GenLocalDate.DayTick(map.Tile);
		}

		// Token: 0x06009EFA RID: 40698 RVA: 0x00069D13 File Offset: 0x00067F13
		public static float DayPercent(Map map)
		{
			return GenLocalDate.DayPercent(map.Tile);
		}

		// Token: 0x06009EFB RID: 40699 RVA: 0x00069D20 File Offset: 0x00067F20
		public static float YearPercent(Map map)
		{
			return GenLocalDate.YearPercent(map.Tile);
		}

		// Token: 0x06009EFC RID: 40700 RVA: 0x00069D2D File Offset: 0x00067F2D
		public static int HourInteger(Map map)
		{
			return GenLocalDate.HourInteger(map.Tile);
		}

		// Token: 0x06009EFD RID: 40701 RVA: 0x00069D3A File Offset: 0x00067F3A
		public static float HourFloat(Map map)
		{
			return GenLocalDate.HourFloat(map.Tile);
		}

		// Token: 0x06009EFE RID: 40702 RVA: 0x00069D47 File Offset: 0x00067F47
		public static int DayOfYear(Thing thing)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				return GenDate.DayOfYear((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
			}
			return 0;
		}

		// Token: 0x06009EFF RID: 40703 RVA: 0x00069D64 File Offset: 0x00067F64
		public static int HourOfDay(Thing thing)
		{
			return GenDate.HourOfDay((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06009F00 RID: 40704 RVA: 0x00069D77 File Offset: 0x00067F77
		public static int DayOfTwelfth(Thing thing)
		{
			return GenDate.DayOfTwelfth((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06009F01 RID: 40705 RVA: 0x00069D8A File Offset: 0x00067F8A
		public static Twelfth Twelfth(Thing thing)
		{
			return GenDate.Twelfth((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06009F02 RID: 40706 RVA: 0x00069D9D File Offset: 0x00067F9D
		public static Season Season(Thing thing)
		{
			return GenDate.Season((long)GenLocalDate.TicksAbs, GenLocalDate.LocationForDate(thing));
		}

		// Token: 0x06009F03 RID: 40707 RVA: 0x00069DB0 File Offset: 0x00067FB0
		public static int Year(Thing thing)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return 5500;
			}
			return GenDate.Year((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06009F04 RID: 40708 RVA: 0x00069DD1 File Offset: 0x00067FD1
		public static int DayOfSeason(Thing thing)
		{
			return GenDate.DayOfSeason((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06009F05 RID: 40709 RVA: 0x00069DE4 File Offset: 0x00067FE4
		public static int DayOfQuadrum(Thing thing)
		{
			return GenDate.DayOfQuadrum((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06009F06 RID: 40710 RVA: 0x00069DF7 File Offset: 0x00067FF7
		public static int DayTick(Thing thing)
		{
			return GenDate.DayTick((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06009F07 RID: 40711 RVA: 0x00069E0A File Offset: 0x0006800A
		public static float DayPercent(Thing thing)
		{
			return GenDate.DayPercent((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06009F08 RID: 40712 RVA: 0x00069E1D File Offset: 0x0006801D
		public static float YearPercent(Thing thing)
		{
			return GenDate.YearPercent((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06009F09 RID: 40713 RVA: 0x00069E30 File Offset: 0x00068030
		public static int HourInteger(Thing thing)
		{
			return GenDate.HourInteger((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06009F0A RID: 40714 RVA: 0x00069E43 File Offset: 0x00068043
		public static float HourFloat(Thing thing)
		{
			return GenDate.HourFloat((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06009F0B RID: 40715 RVA: 0x00069E56 File Offset: 0x00068056
		public static int DayOfYear(int tile)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				return GenDate.DayOfYear((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
			}
			return 0;
		}

		// Token: 0x06009F0C RID: 40716 RVA: 0x00069E7D File Offset: 0x0006807D
		public static int HourOfDay(int tile)
		{
			return GenDate.HourOfDay((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06009F0D RID: 40717 RVA: 0x00069E9A File Offset: 0x0006809A
		public static int DayOfTwelfth(int tile)
		{
			return GenDate.DayOfTwelfth((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06009F0E RID: 40718 RVA: 0x00069EB7 File Offset: 0x000680B7
		public static Twelfth Twelfth(int tile)
		{
			return GenDate.Twelfth((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06009F0F RID: 40719 RVA: 0x00069ED4 File Offset: 0x000680D4
		public static Season Season(int tile)
		{
			return GenDate.Season((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile));
		}

		// Token: 0x06009F10 RID: 40720 RVA: 0x00069EEC File Offset: 0x000680EC
		public static int Year(int tile)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return 5500;
			}
			return GenDate.Year((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06009F11 RID: 40721 RVA: 0x00069F17 File Offset: 0x00068117
		public static int DayOfSeason(int tile)
		{
			return GenDate.DayOfSeason((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06009F12 RID: 40722 RVA: 0x00069F34 File Offset: 0x00068134
		public static int DayOfQuadrum(int tile)
		{
			return GenDate.DayOfQuadrum((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06009F13 RID: 40723 RVA: 0x00069F51 File Offset: 0x00068151
		public static int DayTick(int tile)
		{
			return GenDate.DayTick((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06009F14 RID: 40724 RVA: 0x00069F6E File Offset: 0x0006816E
		public static float DayPercent(int tile)
		{
			return GenDate.DayPercent((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06009F15 RID: 40725 RVA: 0x00069F8B File Offset: 0x0006818B
		public static float YearPercent(int tile)
		{
			return GenDate.YearPercent((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06009F16 RID: 40726 RVA: 0x00069FA8 File Offset: 0x000681A8
		public static int HourInteger(int tile)
		{
			return GenDate.HourInteger((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06009F17 RID: 40727 RVA: 0x00069FC5 File Offset: 0x000681C5
		public static float HourFloat(int tile)
		{
			return GenDate.HourFloat((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06009F18 RID: 40728 RVA: 0x00069FE2 File Offset: 0x000681E2
		private static float LongitudeForDate(Thing thing)
		{
			return GenLocalDate.LocationForDate(thing).x;
		}

		// Token: 0x06009F19 RID: 40729 RVA: 0x002E9A90 File Offset: 0x002E7C90
		private static Vector2 LocationForDate(Thing thing)
		{
			int tile = thing.Tile;
			if (tile >= 0)
			{
				return Find.WorldGrid.LongLatOf(tile);
			}
			return Vector2.zero;
		}
	}
}
