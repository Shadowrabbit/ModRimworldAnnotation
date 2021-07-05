using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000ECB RID: 3787
	public static class NamePlayerFactionAndSettlementUtility
	{
		// Token: 0x06005952 RID: 22866 RVA: 0x001E75C9 File Offset: 0x001E57C9
		public static bool CanNameFactionNow()
		{
			return NamePlayerFactionAndSettlementUtility.CanNameFaction(Find.TickManager.TicksGame);
		}

		// Token: 0x06005953 RID: 22867 RVA: 0x001E75DA File Offset: 0x001E57DA
		public static bool CanNameSettlementNow(Settlement factionBase)
		{
			return NamePlayerFactionAndSettlementUtility.CanNameSettlement(factionBase, Find.TickManager.TicksGame - factionBase.creationGameTicks);
		}

		// Token: 0x06005954 RID: 22868 RVA: 0x001E75F3 File Offset: 0x001E57F3
		public static bool CanNameFactionSoon()
		{
			return NamePlayerFactionAndSettlementUtility.CanNameFaction(Find.TickManager.TicksGame + 30000);
		}

		// Token: 0x06005955 RID: 22869 RVA: 0x001E760A File Offset: 0x001E580A
		public static bool CanNameSettlementSoon(Settlement factionBase)
		{
			return NamePlayerFactionAndSettlementUtility.CanNameSettlement(factionBase, Find.TickManager.TicksGame - factionBase.creationGameTicks + 30000);
		}

		// Token: 0x06005956 RID: 22870 RVA: 0x001E7629 File Offset: 0x001E5829
		private static bool CanNameFaction(int ticksPassed)
		{
			return !Faction.OfPlayer.HasName && (float)ticksPassed / 60000f >= 4.3f && NamePlayerFactionAndSettlementUtility.CanNameAnythingNow();
		}

		// Token: 0x06005957 RID: 22871 RVA: 0x001E7650 File Offset: 0x001E5850
		private static bool CanNameSettlement(Settlement factionBase, int ticksPassed)
		{
			return factionBase.Faction == Faction.OfPlayer && !factionBase.namedByPlayer && (float)ticksPassed / 60000f >= 4.3f && factionBase.HasMap && factionBase.Map.dangerWatcher.DangerRating != StoryDanger.High && factionBase.Map.mapPawns.FreeColonistsSpawnedCount != 0 && NamePlayerFactionAndSettlementUtility.CanNameAnythingNow();
		}

		// Token: 0x06005958 RID: 22872 RVA: 0x001E76B8 File Offset: 0x001E58B8
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

		// Token: 0x04003466 RID: 13414
		private const float MinDaysPassedToNameFaction = 4.3f;

		// Token: 0x04003467 RID: 13415
		private const float MinDaysPassedToNameSettlement = 4.3f;

		// Token: 0x04003468 RID: 13416
		private const int SoonTicks = 30000;
	}
}
