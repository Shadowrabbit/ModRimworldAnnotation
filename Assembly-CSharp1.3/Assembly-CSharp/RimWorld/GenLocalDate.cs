using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013F4 RID: 5108
	public static class GenLocalDate
	{
		// Token: 0x170015B9 RID: 5561
		// (get) Token: 0x06007C9B RID: 31899 RVA: 0x002C323F File Offset: 0x002C143F
		private static int TicksAbs
		{
			get
			{
				return GenTicks.TicksAbs;
			}
		}

		// Token: 0x06007C9C RID: 31900 RVA: 0x002C3246 File Offset: 0x002C1446
		public static int DayOfYear(Map map)
		{
			return GenLocalDate.DayOfYear(map.Tile);
		}

		// Token: 0x06007C9D RID: 31901 RVA: 0x002C3253 File Offset: 0x002C1453
		public static int HourOfDay(Map map)
		{
			return GenLocalDate.HourOfDay(map.Tile);
		}

		// Token: 0x06007C9E RID: 31902 RVA: 0x002C3260 File Offset: 0x002C1460
		public static int DayOfTwelfth(Map map)
		{
			return GenLocalDate.DayOfTwelfth(map.Tile);
		}

		// Token: 0x06007C9F RID: 31903 RVA: 0x002C326D File Offset: 0x002C146D
		public static Twelfth Twelfth(Map map)
		{
			return GenLocalDate.Twelfth(map.Tile);
		}

		// Token: 0x06007CA0 RID: 31904 RVA: 0x002C327A File Offset: 0x002C147A
		public static Season Season(Map map)
		{
			return GenLocalDate.Season(map.Tile);
		}

		// Token: 0x06007CA1 RID: 31905 RVA: 0x002C3287 File Offset: 0x002C1487
		public static int Year(Map map)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return 5500;
			}
			return GenLocalDate.Year(map.Tile);
		}

		// Token: 0x06007CA2 RID: 31906 RVA: 0x002C32A2 File Offset: 0x002C14A2
		public static int DayOfSeason(Map map)
		{
			return GenLocalDate.DayOfSeason(map.Tile);
		}

		// Token: 0x06007CA3 RID: 31907 RVA: 0x002C32AF File Offset: 0x002C14AF
		public static int DayOfQuadrum(Map map)
		{
			return GenLocalDate.DayOfQuadrum(map.Tile);
		}

		// Token: 0x06007CA4 RID: 31908 RVA: 0x002C32BC File Offset: 0x002C14BC
		public static int DayTick(Map map)
		{
			return GenLocalDate.DayTick(map.Tile);
		}

		// Token: 0x06007CA5 RID: 31909 RVA: 0x002C32C9 File Offset: 0x002C14C9
		public static float DayPercent(Map map)
		{
			return GenLocalDate.DayPercent(map.Tile);
		}

		// Token: 0x06007CA6 RID: 31910 RVA: 0x002C32D6 File Offset: 0x002C14D6
		public static float YearPercent(Map map)
		{
			return GenLocalDate.YearPercent(map.Tile);
		}

		// Token: 0x06007CA7 RID: 31911 RVA: 0x002C32E3 File Offset: 0x002C14E3
		public static int HourInteger(Map map)
		{
			return GenLocalDate.HourInteger(map.Tile);
		}

		// Token: 0x06007CA8 RID: 31912 RVA: 0x002C32F0 File Offset: 0x002C14F0
		public static float HourFloat(Map map)
		{
			return GenLocalDate.HourFloat(map.Tile);
		}

		// Token: 0x06007CA9 RID: 31913 RVA: 0x002C32FD File Offset: 0x002C14FD
		public static int DayOfYear(Thing thing)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				return GenDate.DayOfYear((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
			}
			return 0;
		}

		// Token: 0x06007CAA RID: 31914 RVA: 0x002C331A File Offset: 0x002C151A
		public static int HourOfDay(Thing thing)
		{
			return GenDate.HourOfDay((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06007CAB RID: 31915 RVA: 0x002C332D File Offset: 0x002C152D
		public static int DayOfTwelfth(Thing thing)
		{
			return GenDate.DayOfTwelfth((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06007CAC RID: 31916 RVA: 0x002C3340 File Offset: 0x002C1540
		public static Twelfth Twelfth(Thing thing)
		{
			return GenDate.Twelfth((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06007CAD RID: 31917 RVA: 0x002C3353 File Offset: 0x002C1553
		public static Season Season(Thing thing)
		{
			return GenDate.Season((long)GenLocalDate.TicksAbs, GenLocalDate.LocationForDate(thing));
		}

		// Token: 0x06007CAE RID: 31918 RVA: 0x002C3366 File Offset: 0x002C1566
		public static int Year(Thing thing)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return 5500;
			}
			return GenDate.Year((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06007CAF RID: 31919 RVA: 0x002C3387 File Offset: 0x002C1587
		public static int DayOfSeason(Thing thing)
		{
			return GenDate.DayOfSeason((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06007CB0 RID: 31920 RVA: 0x002C339A File Offset: 0x002C159A
		public static int DayOfQuadrum(Thing thing)
		{
			return GenDate.DayOfQuadrum((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06007CB1 RID: 31921 RVA: 0x002C33AD File Offset: 0x002C15AD
		public static int DayTick(Thing thing)
		{
			return GenDate.DayTick((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06007CB2 RID: 31922 RVA: 0x002C33C0 File Offset: 0x002C15C0
		public static float DayPercent(Thing thing)
		{
			return GenDate.DayPercent((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06007CB3 RID: 31923 RVA: 0x002C33D3 File Offset: 0x002C15D3
		public static float YearPercent(Thing thing)
		{
			return GenDate.YearPercent((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06007CB4 RID: 31924 RVA: 0x002C33E6 File Offset: 0x002C15E6
		public static int HourInteger(Thing thing)
		{
			return GenDate.HourInteger((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06007CB5 RID: 31925 RVA: 0x002C33F9 File Offset: 0x002C15F9
		public static float HourFloat(Thing thing)
		{
			return GenDate.HourFloat((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x06007CB6 RID: 31926 RVA: 0x002C340C File Offset: 0x002C160C
		public static int DayOfYear(int tile)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				return GenDate.DayOfYear((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
			}
			return 0;
		}

		// Token: 0x06007CB7 RID: 31927 RVA: 0x002C3433 File Offset: 0x002C1633
		public static int HourOfDay(int tile)
		{
			return GenDate.HourOfDay((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06007CB8 RID: 31928 RVA: 0x002C3450 File Offset: 0x002C1650
		public static int DayOfTwelfth(int tile)
		{
			return GenDate.DayOfTwelfth((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06007CB9 RID: 31929 RVA: 0x002C346D File Offset: 0x002C166D
		public static Twelfth Twelfth(int tile)
		{
			return GenDate.Twelfth((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06007CBA RID: 31930 RVA: 0x002C348A File Offset: 0x002C168A
		public static Season Season(int tile)
		{
			return GenDate.Season((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile));
		}

		// Token: 0x06007CBB RID: 31931 RVA: 0x002C34A2 File Offset: 0x002C16A2
		public static int Year(int tile)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return 5500;
			}
			return GenDate.Year((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06007CBC RID: 31932 RVA: 0x002C34CD File Offset: 0x002C16CD
		public static int DayOfSeason(int tile)
		{
			return GenDate.DayOfSeason((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06007CBD RID: 31933 RVA: 0x002C34EA File Offset: 0x002C16EA
		public static int DayOfQuadrum(int tile)
		{
			return GenDate.DayOfQuadrum((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06007CBE RID: 31934 RVA: 0x002C3507 File Offset: 0x002C1707
		public static int DayTick(int tile)
		{
			return GenDate.DayTick((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06007CBF RID: 31935 RVA: 0x002C3524 File Offset: 0x002C1724
		public static float DayPercent(int tile)
		{
			return GenDate.DayPercent((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06007CC0 RID: 31936 RVA: 0x002C3541 File Offset: 0x002C1741
		public static float YearPercent(int tile)
		{
			return GenDate.YearPercent((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06007CC1 RID: 31937 RVA: 0x002C355E File Offset: 0x002C175E
		public static int HourInteger(int tile)
		{
			return GenDate.HourInteger((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06007CC2 RID: 31938 RVA: 0x002C357B File Offset: 0x002C177B
		public static float HourFloat(int tile)
		{
			return GenDate.HourFloat((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x06007CC3 RID: 31939 RVA: 0x002C3598 File Offset: 0x002C1798
		private static float LongitudeForDate(Thing thing)
		{
			return GenLocalDate.LocationForDate(thing).x;
		}

		// Token: 0x06007CC4 RID: 31940 RVA: 0x002C35A8 File Offset: 0x002C17A8
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
