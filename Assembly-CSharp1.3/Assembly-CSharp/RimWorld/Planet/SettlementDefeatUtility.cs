using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017D0 RID: 6096
	public static class SettlementDefeatUtility
	{
		// Token: 0x06008DD1 RID: 36305 RVA: 0x0032F390 File Offset: 0x0032D590
		public static void CheckDefeated(Settlement factionBase)
		{
			if (factionBase.Faction == Faction.OfPlayer)
			{
				return;
			}
			Map map = factionBase.Map;
			if (map == null || !SettlementDefeatUtility.IsDefeated(map, factionBase.Faction))
			{
				return;
			}
			IdeoUtility.Notify_PlayerRaidedSomeone(map.mapPawns.FreeColonistsSpawned);
			DestroyedSettlement destroyedSettlement = (DestroyedSettlement)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.DestroyedSettlement);
			destroyedSettlement.Tile = factionBase.Tile;
			destroyedSettlement.SetFaction(factionBase.Faction);
			Find.WorldObjects.Add(destroyedSettlement);
			TimedDetectionRaids component = destroyedSettlement.GetComponent<TimedDetectionRaids>();
			component.CopyFrom(factionBase.GetComponent<TimedDetectionRaids>());
			component.SetNotifiedSilently();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("LetterFactionBaseDefeated".Translate(factionBase.Label, component.DetectionCountdownTimeLeftString));
			if (!SettlementDefeatUtility.HasAnyOtherBase(factionBase))
			{
				factionBase.Faction.defeated = true;
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.Append("LetterFactionBaseDefeated_FactionDestroyed".Translate(factionBase.Faction.Name));
			}
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (!faction.Hidden && !faction.IsPlayer && faction != factionBase.Faction && faction.HostileTo(factionBase.Faction))
				{
					FactionRelationKind playerRelationKind = faction.PlayerRelationKind;
					Faction.OfPlayer.TryAffectGoodwillWith(faction, 20, false, false, HistoryEventDefOf.DestroyedEnemyBase, null);
					stringBuilder.AppendLine();
					stringBuilder.AppendLine();
					stringBuilder.Append("RelationsWith".Translate(faction.Name) + ": " + 20.ToStringWithSign());
					faction.TryAppendRelationKindChangedInfo(stringBuilder, playerRelationKind, faction.PlayerRelationKind, null);
				}
			}
			Find.LetterStack.ReceiveLetter("LetterLabelFactionBaseDefeated".Translate(), stringBuilder.ToString(), LetterDefOf.PositiveEvent, new GlobalTargetInfo(factionBase.Tile), factionBase.Faction, null, null, null);
			map.info.parent = destroyedSettlement;
			factionBase.Destroy();
			TaleRecorder.RecordTale(TaleDefOf.CaravanAssaultSuccessful, new object[]
			{
				map.mapPawns.FreeColonists.RandomElement<Pawn>()
			});
		}

		// Token: 0x06008DD2 RID: 36306 RVA: 0x0032F610 File Offset: 0x0032D810
		private static bool IsDefeated(Map map, Faction faction)
		{
			List<Pawn> list = map.mapPawns.SpawnedPawnsInFaction(faction);
			for (int i = 0; i < list.Count; i++)
			{
				Pawn pawn = list[i];
				if (pawn.RaceProps.Humanlike && GenHostility.IsActiveThreatToPlayer(pawn))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06008DD3 RID: 36307 RVA: 0x0032F65C File Offset: 0x0032D85C
		private static bool HasAnyOtherBase(Settlement defeatedFactionBase)
		{
			List<Settlement> settlements = Find.WorldObjects.Settlements;
			for (int i = 0; i < settlements.Count; i++)
			{
				Settlement settlement = settlements[i];
				if (settlement.Faction == defeatedFactionBase.Faction && settlement != defeatedFactionBase)
				{
					return true;
				}
			}
			return false;
		}
	}
}
