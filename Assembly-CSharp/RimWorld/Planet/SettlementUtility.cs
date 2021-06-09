using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002153 RID: 8531
	public class SettlementUtility
	{
		// Token: 0x0600B5B3 RID: 46515 RVA: 0x003492F0 File Offset: 0x003474F0
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

		// Token: 0x0600B5B4 RID: 46516 RVA: 0x00349354 File Offset: 0x00347554
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

		// Token: 0x0600B5B5 RID: 46517 RVA: 0x003493B0 File Offset: 0x003475B0
		private static void AttackNow(Caravan caravan, Settlement settlement)
		{
			bool flag = !settlement.HasMap;
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(settlement.Tile, null);
			TaggedString label = "LetterLabelCaravanEnteredEnemyBase".Translate();
			TaggedString text = "LetterCaravanEnteredEnemyBase".Translate(caravan.Label, settlement.Label.ApplyTag(TagType.Settlement, settlement.Faction.GetUniqueLoadID())).CapitalizeFirst();
			SettlementUtility.AffectRelationsOnAttacked_NewTmp(settlement, ref text);
			if (flag)
			{
				Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(orGenerateMap.mapPawns.AllPawns, ref label, ref text, "LetterRelatedPawnsSettlement".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
			}
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, caravan.PawnsListForReading, settlement.Faction, null, null, null);
			CaravanEnterMapUtility.Enter(caravan, orGenerateMap, CaravanEnterMode.Edge, CaravanDropInventoryMode.DoNotDrop, true, null);
		}

		// Token: 0x0600B5B6 RID: 46518 RVA: 0x00076007 File Offset: 0x00074207
		[Obsolete("Only used for mod compatibility. Will be removed in a future version.")]
		public static void AffectRelationsOnAttacked(Settlement settlement, ref TaggedString letterText)
		{
			SettlementUtility.AffectRelationsOnAttacked_NewTmp(settlement, ref letterText);
		}

		// Token: 0x0600B5B7 RID: 46519 RVA: 0x00349498 File Offset: 0x00347698
		public static void AffectRelationsOnAttacked_NewTmp(MapParent mapParent, ref TaggedString letterText)
		{
			if (mapParent.Faction != null && mapParent.Faction != Faction.OfPlayer)
			{
				FactionRelationKind playerRelationKind = mapParent.Faction.PlayerRelationKind;
				if (!mapParent.Faction.HostileTo(Faction.OfPlayer))
				{
					mapParent.Faction.TrySetRelationKind(Faction.OfPlayer, FactionRelationKind.Hostile, false, null, null);
				}
				else if (mapParent.Faction.TryAffectGoodwillWith(Faction.OfPlayer, -50, false, false, null, null))
				{
					if (!letterText.NullOrEmpty())
					{
						letterText += "\n\n";
					}
					letterText += "RelationsWith".Translate(mapParent.Faction.Name.ApplyTag(mapParent.Faction)) + ": " + -50.ToStringWithSign();
				}
				mapParent.Faction.TryAppendRelationKindChangedInfo(ref letterText, playerRelationKind, mapParent.Faction.PlayerRelationKind, null);
			}
		}
	}
}
