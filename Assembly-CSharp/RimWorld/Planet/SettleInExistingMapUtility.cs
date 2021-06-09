using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x02002119 RID: 8473
	public static class SettleInExistingMapUtility
	{
		// Token: 0x0600B3E0 RID: 46048 RVA: 0x00343AD4 File Offset: 0x00341CD4
		public static Command SettleCommand(Map map, bool requiresNoEnemies)
		{
			Command_Settle command_Settle = new Command_Settle();
			command_Settle.defaultLabel = "CommandSettle".Translate();
			command_Settle.defaultDesc = "CommandSettleDesc".Translate();
			command_Settle.icon = SettleUtility.SettleCommandTex;
			Action <>9__1;
			command_Settle.action = delegate()
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				int tile = map.Tile;
				Action settleAction;
				if ((settleAction = <>9__1) == null)
				{
					settleAction = (<>9__1 = delegate()
					{
						SettleInExistingMapUtility.Settle(map);
					});
				}
				SettlementProximityGoodwillUtility.CheckConfirmSettle(tile, settleAction);
			};
			if (SettleUtility.PlayerSettlementsCountLimitReached)
			{
				if (Prefs.MaxNumberOfPlayerSettlements > 1)
				{
					command_Settle.Disable("CommandSettleFailReachedMaximumNumberOfBases".Translate());
				}
				else
				{
					command_Settle.Disable("CommandSettleFailAlreadyHaveBase".Translate());
				}
			}
			if (!command_Settle.disabled)
			{
				if (map.mapPawns.FreeColonistsCount == 0)
				{
					command_Settle.Disable("CommandSettleFailNoColonists".Translate());
				}
				else if (requiresNoEnemies)
				{
					using (HashSet<IAttackTarget>.Enumerator enumerator = map.attackTargetsCache.TargetsHostileToColony.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (GenHostility.IsActiveThreatToPlayer(enumerator.Current))
							{
								command_Settle.Disable("CommandSettleFailEnemies".Translate());
								break;
							}
						}
					}
				}
			}
			return command_Settle;
		}

		// Token: 0x0600B3E1 RID: 46049 RVA: 0x00343C14 File Offset: 0x00341E14
		public static void Settle(Map map)
		{
			MapParent parent = map.Parent;
			Settlement settlement = SettleUtility.AddNewHome(map.Tile, Faction.OfPlayer);
			map.info.parent = settlement;
			if (parent != null)
			{
				parent.Destroy();
			}
			Messages.Message("MessageSettledInExistingMap".Translate(), settlement, MessageTypeDefOf.PositiveEvent, false);
			SettleInExistingMapUtility.tmpPlayerPawns.Clear();
			SettleInExistingMapUtility.tmpPlayerPawns.AddRange(from x in map.mapPawns.AllPawnsSpawned
			where x.Faction == Faction.OfPlayer || x.HostFaction == Faction.OfPlayer
			select x);
			CaravanEnterMapUtility.DropAllInventory(SettleInExistingMapUtility.tmpPlayerPawns);
			SettleInExistingMapUtility.tmpPlayerPawns.Clear();
			List<Pawn> prisonersOfColonySpawned = map.mapPawns.PrisonersOfColonySpawned;
			for (int i = 0; i < prisonersOfColonySpawned.Count; i++)
			{
				prisonersOfColonySpawned[i].guest.WaitInsteadOfEscapingForDefaultTicks();
			}
		}

		// Token: 0x04007BA5 RID: 31653
		private static List<Pawn> tmpPlayerPawns = new List<Pawn>();
	}
}
