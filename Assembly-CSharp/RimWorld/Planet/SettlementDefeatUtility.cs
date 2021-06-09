using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002152 RID: 8530
	public static class SettlementDefeatUtility
	{
		// Token: 0x0600B5B0 RID: 46512 RVA: 0x00348FF4 File Offset: 0x003471F4
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
					if (faction.TryAffectGoodwillWith(Faction.OfPlayer, 20, false, false, null, null))
					{
						stringBuilder.AppendLine();
						stringBuilder.AppendLine();
						stringBuilder.Append("RelationsWith".Translate(faction.Name) + ": " + 20.ToStringWithSign());
						faction.TryAppendRelationKindChangedInfo(stringBuilder, playerRelationKind, faction.PlayerRelationKind, null);
					}
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

		// Token: 0x0600B5B1 RID: 46513 RVA: 0x0034925C File Offset: 0x0034745C
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

		// Token: 0x0600B5B2 RID: 46514 RVA: 0x003492A8 File Offset: 0x003474A8
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
