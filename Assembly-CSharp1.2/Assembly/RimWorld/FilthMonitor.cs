using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CB3 RID: 7347
	internal static class FilthMonitor
	{
		// Token: 0x06009FC8 RID: 40904 RVA: 0x002EB074 File Offset: 0x002E9274
		public static void FilthMonitorTick()
		{
			if (!DebugViewSettings.logFilthSummary)
			{
				return;
			}
			if (FilthMonitor.lastUpdate + 2500 <= Find.TickManager.TicksAbs)
			{
				int num = PawnsFinder.AllMaps_Spawned.Count((Pawn pawn) => pawn.Faction == Faction.OfPlayer);
				int num2 = PawnsFinder.AllMaps_Spawned.Count((Pawn pawn) => pawn.Faction == Faction.OfPlayer && pawn.RaceProps.Humanlike);
				int num3 = PawnsFinder.AllMaps_Spawned.Count((Pawn pawn) => pawn.Faction == Faction.OfPlayer && !pawn.RaceProps.Humanlike);
				Log.Message(string.Format("Filth data, per day:\n  {0} filth spawned per pawn\n  {1} filth human-generated per human\n  {2} filth animal-generated per animal\n  {3} filth accumulated per pawn\n  {4} filth dropped per pawn", new object[]
				{
					(float)FilthMonitor.filthSpawned / (float)num / 2500f * 60000f,
					(float)FilthMonitor.filthHumanGenerated / (float)num2 / 2500f * 60000f,
					(float)FilthMonitor.filthAnimalGenerated / (float)num3 / 2500f * 60000f,
					(float)FilthMonitor.filthAccumulated / (float)num / 2500f * 60000f,
					(float)FilthMonitor.filthDropped / (float)num / 2500f * 60000f
				}), false);
				FilthMonitor.filthSpawned = 0;
				FilthMonitor.filthAnimalGenerated = 0;
				FilthMonitor.filthHumanGenerated = 0;
				FilthMonitor.filthAccumulated = 0;
				FilthMonitor.filthDropped = 0;
				FilthMonitor.lastUpdate = Find.TickManager.TicksAbs;
			}
		}

		// Token: 0x06009FC9 RID: 40905 RVA: 0x0006A8A1 File Offset: 0x00068AA1
		public static void Notify_FilthAccumulated()
		{
			if (!DebugViewSettings.logFilthSummary)
			{
				return;
			}
			FilthMonitor.filthAccumulated++;
		}

		// Token: 0x06009FCA RID: 40906 RVA: 0x0006A8B7 File Offset: 0x00068AB7
		public static void Notify_FilthDropped()
		{
			if (!DebugViewSettings.logFilthSummary)
			{
				return;
			}
			FilthMonitor.filthDropped++;
		}

		// Token: 0x06009FCB RID: 40907 RVA: 0x0006A8CD File Offset: 0x00068ACD
		public static void Notify_FilthAnimalGenerated()
		{
			if (!DebugViewSettings.logFilthSummary)
			{
				return;
			}
			FilthMonitor.filthAnimalGenerated++;
		}

		// Token: 0x06009FCC RID: 40908 RVA: 0x0006A8E3 File Offset: 0x00068AE3
		public static void Notify_FilthHumanGenerated()
		{
			if (!DebugViewSettings.logFilthSummary)
			{
				return;
			}
			FilthMonitor.filthHumanGenerated++;
		}

		// Token: 0x06009FCD RID: 40909 RVA: 0x0006A8F9 File Offset: 0x00068AF9
		public static void Notify_FilthSpawned()
		{
			if (!DebugViewSettings.logFilthSummary)
			{
				return;
			}
			FilthMonitor.filthSpawned++;
		}

		// Token: 0x04006C94 RID: 27796
		private static int lastUpdate;

		// Token: 0x04006C95 RID: 27797
		private static int filthAccumulated;

		// Token: 0x04006C96 RID: 27798
		private static int filthDropped;

		// Token: 0x04006C97 RID: 27799
		private static int filthAnimalGenerated;

		// Token: 0x04006C98 RID: 27800
		private static int filthHumanGenerated;

		// Token: 0x04006C99 RID: 27801
		private static int filthSpawned;

		// Token: 0x04006C9A RID: 27802
		private const int SampleDuration = 2500;
	}
}
