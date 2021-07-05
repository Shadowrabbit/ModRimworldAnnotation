using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001482 RID: 5250
	internal static class FilthMonitor
	{
		// Token: 0x06007D85 RID: 32133 RVA: 0x002C5908 File Offset: 0x002C3B08
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
				}));
				FilthMonitor.filthSpawned = 0;
				FilthMonitor.filthAnimalGenerated = 0;
				FilthMonitor.filthHumanGenerated = 0;
				FilthMonitor.filthAccumulated = 0;
				FilthMonitor.filthDropped = 0;
				FilthMonitor.lastUpdate = Find.TickManager.TicksAbs;
			}
		}

		// Token: 0x06007D86 RID: 32134 RVA: 0x002C5A88 File Offset: 0x002C3C88
		public static void Notify_FilthAccumulated()
		{
			if (!DebugViewSettings.logFilthSummary)
			{
				return;
			}
			FilthMonitor.filthAccumulated++;
		}

		// Token: 0x06007D87 RID: 32135 RVA: 0x002C5A9E File Offset: 0x002C3C9E
		public static void Notify_FilthDropped()
		{
			if (!DebugViewSettings.logFilthSummary)
			{
				return;
			}
			FilthMonitor.filthDropped++;
		}

		// Token: 0x06007D88 RID: 32136 RVA: 0x002C5AB4 File Offset: 0x002C3CB4
		public static void Notify_FilthAnimalGenerated()
		{
			if (!DebugViewSettings.logFilthSummary)
			{
				return;
			}
			FilthMonitor.filthAnimalGenerated++;
		}

		// Token: 0x06007D89 RID: 32137 RVA: 0x002C5ACA File Offset: 0x002C3CCA
		public static void Notify_FilthHumanGenerated()
		{
			if (!DebugViewSettings.logFilthSummary)
			{
				return;
			}
			FilthMonitor.filthHumanGenerated++;
		}

		// Token: 0x06007D8A RID: 32138 RVA: 0x002C5AE0 File Offset: 0x002C3CE0
		public static void Notify_FilthSpawned()
		{
			if (!DebugViewSettings.logFilthSummary)
			{
				return;
			}
			FilthMonitor.filthSpawned++;
		}

		// Token: 0x04004E50 RID: 20048
		private static int lastUpdate;

		// Token: 0x04004E51 RID: 20049
		private static int filthAccumulated;

		// Token: 0x04004E52 RID: 20050
		private static int filthDropped;

		// Token: 0x04004E53 RID: 20051
		private static int filthAnimalGenerated;

		// Token: 0x04004E54 RID: 20052
		private static int filthHumanGenerated;

		// Token: 0x04004E55 RID: 20053
		private static int filthSpawned;

		// Token: 0x04004E56 RID: 20054
		private const int SampleDuration = 2500;
	}
}
