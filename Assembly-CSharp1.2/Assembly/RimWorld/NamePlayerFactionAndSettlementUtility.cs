using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015AB RID: 5547
	public static class NamePlayerFactionAndSettlementUtility
	{
		// Token: 0x06007870 RID: 30832 RVA: 0x000511B1 File Offset: 0x0004F3B1
		public static bool CanNameFactionNow()
		{
			return NamePlayerFactionAndSettlementUtility.CanNameFaction(Find.TickManager.TicksGame);
		}

		// Token: 0x06007871 RID: 30833 RVA: 0x000511C2 File Offset: 0x0004F3C2
		public static bool CanNameSettlementNow(Settlement factionBase)
		{
			return NamePlayerFactionAndSettlementUtility.CanNameSettlement(factionBase, Find.TickManager.TicksGame - factionBase.creationGameTicks);
		}

		// Token: 0x06007872 RID: 30834 RVA: 0x000511DB File Offset: 0x0004F3DB
		public static bool CanNameFactionSoon()
		{
			return NamePlayerFactionAndSettlementUtility.CanNameFaction(Find.TickManager.TicksGame + 30000);
		}

		// Token: 0x06007873 RID: 30835 RVA: 0x000511F2 File Offset: 0x0004F3F2
		public static bool CanNameSettlementSoon(Settlement factionBase)
		{
			return NamePlayerFactionAndSettlementUtility.CanNameSettlement(factionBase, Find.TickManager.TicksGame - factionBase.creationGameTicks + 30000);
		}

		// Token: 0x06007874 RID: 30836 RVA: 0x00051211 File Offset: 0x0004F411
		private static bool CanNameFaction(int ticksPassed)
		{
			return !Faction.OfPlayer.HasName && (float)ticksPassed / 60000f >= 4.3f && NamePlayerFactionAndSettlementUtility.CanNameAnythingNow();
		}

		// Token: 0x06007875 RID: 30837 RVA: 0x0024A2F4 File Offset: 0x002484F4
		private static bool CanNameSettlement(Settlement factionBase, int ticksPassed)
		{
			return factionBase.Faction == Faction.OfPlayer && !factionBase.namedByPlayer && (float)ticksPassed / 60000f >= 4.3f && factionBase.HasMap && factionBase.Map.dangerWatcher.DangerRating != StoryDanger.High && factionBase.Map.mapPawns.FreeColonistsSpawnedCount != 0 && NamePlayerFactionAndSettlementUtility.CanNameAnythingNow();
		}

		// Token: 0x06007876 RID: 30838 RVA: 0x0024A35C File Offset: 0x0024855C
		private static bool CanNameAnythingNow()
		{
			if (Find.AnyPlayerHomeMap == null || Find.CurrentMap == null || !Find.CurrentMap.IsPlayerHome || Find.GameEnder.gameEnding)
			{
				return false;
			}
			bool flag = false;
			bool flag2 = false;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome)
				{
					if (maps[i].mapPawns.FreeColonistsSpawnedCount >= 2)
					{
						flag = true;
					}
					if (!maps[i].attackTargetsCache.TargetsHostileToColony.Any((IAttackTarget x) => GenHostility.IsActiveThreatToPlayer(x)))
					{
						flag2 = true;
					}
				}
			}
			return flag && flag2;
		}

		// Token: 0x04004F5D RID: 20317
		private const float MinDaysPassedToNameFaction = 4.3f;

		// Token: 0x04004F5E RID: 20318
		private const float MinDaysPassedToNameSettlement = 4.3f;

		// Token: 0x04004F5F RID: 20319
		private const int SoonTicks = 30000;
	}
}
