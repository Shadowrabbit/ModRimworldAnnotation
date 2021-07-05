using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017D1 RID: 6097
	public class SettlementUtility
	{
		// Token: 0x06008DD4 RID: 36308 RVA: 0x0032F6A4 File Offset: 0x0032D8A4
		public static bool IsPlayerAttackingAnySettlementOf(Faction faction)
		{
			if (faction == Faction.OfPlayer)
			{
				return false;
			}
			if (!faction.HostileTo(Faction.OfPlayer))
			{
				return false;
			}
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Settlement settlement = maps[i].info.parent as Settlement;
				if (settlement != null && settlement.Faction == faction)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008DD5 RID: 36309 RVA: 0x0032F708 File Offset: 0x0032D908
		public static void Attack(Caravan caravan, Settlement settlement)
		{
			if (!settlement.HasMap)
			{
				LongEventHandler.QueueLongEvent(delegate()
				{
					SettlementUtility.AttackNow(caravan, settlement);
				}, "GeneratingMapForNewEncounter", false, null, true);
				return;
			}
			SettlementUtility.AttackNow(caravan, settlement);
		}

		// Token: 0x06008DD6 RID: 36310 RVA: 0x0032F764 File Offset: 0x0032D964
		private static void AttackNow(Caravan caravan, Settlement settlement)
		{
			bool flag = !settlement.HasMap;
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(settlement.Tile, null);
			TaggedString label = "LetterLabelCaravanEnteredEnemyBase".Translate();
			TaggedString text = "LetterCaravanEnteredEnemyBase".Translate(caravan.Label, settlement.Label.ApplyTag(TagType.Settlement, settlement.Faction.GetUniqueLoadID())).CapitalizeFirst();
			SettlementUtility.AffectRelationsOnAttacked(settlement, ref text);
			if (flag)
			{
				Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(orGenerateMap.mapPawns.AllPawns, ref label, ref text, "LetterRelatedPawnsSettlement".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
			}
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, caravan.PawnsListForReading, settlement.Faction, null, null, null);
			CaravanEnterMapUtility.Enter(caravan, orGenerateMap, CaravanEnterMode.Edge, CaravanDropInventoryMode.DoNotDrop, true, null);
			Find.GoodwillSituationManager.RecalculateAll(true);
		}

		// Token: 0x06008DD7 RID: 36311 RVA: 0x0032F854 File Offset: 0x0032DA54
		public static void AffectRelationsOnAttacked(MapParent mapParent, ref TaggedString letterText)
		{
			if (mapParent.Faction != null && mapParent.Faction != Faction.OfPlayer)
			{
				FactionRelationKind playerRelationKind = mapParent.Faction.PlayerRelationKind;
				Faction.OfPlayer.TryAffectGoodwillWith(mapParent.Faction, Faction.OfPlayer.GoodwillToMakeHostile(mapParent.Faction), false, false, HistoryEventDefOf.AttackedSettlement, null);
				mapParent.Faction.TryAppendRelationKindChangedInfo(ref letterText, playerRelationKind, mapParent.Faction.PlayerRelationKind, null);
			}
		}
	}
}
