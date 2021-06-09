using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001441 RID: 5185
	public class TransportPodsArrivalAction_Shuttle : TransportPodsArrivalAction
	{
		// Token: 0x06006FD1 RID: 28625 RVA: 0x0004B7F4 File Offset: 0x000499F4
		public TransportPodsArrivalAction_Shuttle()
		{
		}

		// Token: 0x06006FD2 RID: 28626 RVA: 0x0004B7FC File Offset: 0x000499FC
		public TransportPodsArrivalAction_Shuttle(MapParent mapParent)
		{
			this.mapParent = mapParent;
		}

		// Token: 0x06006FD3 RID: 28627 RVA: 0x0004B80B File Offset: 0x00049A0B
		public override bool ShouldUseLongEvent(List<ActiveDropPodInfo> pods, int tile)
		{
			return !this.mapParent.HasMap;
		}

		// Token: 0x06006FD4 RID: 28628 RVA: 0x00223A44 File Offset: 0x00221C44
		public override void Arrived(List<ActiveDropPodInfo> pods, int tile)
		{
			Thing lookTarget = TransportPodsArrivalActionUtility.GetLookTarget(pods);
			bool flag = !this.mapParent.HasMap;
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(tile, null);
			Settlement settlement;
			if ((settlement = (orGenerateMap.Parent as Settlement)) != null && settlement.Faction != Faction.OfPlayer)
			{
				TaggedString label = "LetterLabelCaravanEnteredEnemyBase".Translate();
				TaggedString text = "LetterShuttleLandedInEnemyBase".Translate(settlement.Label).CapitalizeFirst();
				SettlementUtility.AffectRelationsOnAttacked_NewTmp(settlement, ref text);
				if (flag)
				{
					Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
					PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(orGenerateMap.mapPawns.AllPawns, ref label, ref text, "LetterRelatedPawnsInMapWherePlayerLanded".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
				}
				Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, lookTarget, settlement.Faction, null, null, null);
			}
			foreach (ActiveDropPodInfo activeDropPodInfo in pods)
			{
				activeDropPodInfo.missionShuttleHome = this.missionShuttleHome;
				activeDropPodInfo.missionShuttleTarget = this.missionShuttleTarget;
				activeDropPodInfo.sendAwayIfQuestFinished = this.sendAwayIfQuestFinished;
				activeDropPodInfo.questTags = this.questTags;
			}
			PawnsArrivalModeDefOf.Shuttle.Worker.TravelingTransportPodsArrived(pods, orGenerateMap);
			Messages.Message("MessageShuttleArrived".Translate(), lookTarget, MessageTypeDefOf.TaskCompletion, true);
		}

		// Token: 0x06006FD5 RID: 28629 RVA: 0x00223BC8 File Offset: 0x00221DC8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_References.Look<WorldObject>(ref this.missionShuttleTarget, "missionShuttleTarget", false);
			Scribe_References.Look<WorldObject>(ref this.missionShuttleHome, "missionShuttleHome", false);
			Scribe_References.Look<Quest>(ref this.sendAwayIfQuestFinished, "sendAwayIfQuestFinished", false);
			Scribe_Collections.Look<string>(ref this.questTags, "questTags", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x040049D8 RID: 18904
		public MapParent mapParent;

		// Token: 0x040049D9 RID: 18905
		public WorldObject missionShuttleTarget;

		// Token: 0x040049DA RID: 18906
		public WorldObject missionShuttleHome;

		// Token: 0x040049DB RID: 18907
		public Quest sendAwayIfQuestFinished;

		// Token: 0x040049DC RID: 18908
		public List<string> questTags;
	}
}
