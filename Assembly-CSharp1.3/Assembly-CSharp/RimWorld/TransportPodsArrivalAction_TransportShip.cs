using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DD4 RID: 3540
	public class TransportPodsArrivalAction_TransportShip : TransportPodsArrivalAction
	{
		// Token: 0x06005218 RID: 21016 RVA: 0x001BAF56 File Offset: 0x001B9156
		public TransportPodsArrivalAction_TransportShip()
		{
		}

		// Token: 0x06005219 RID: 21017 RVA: 0x001BAF69 File Offset: 0x001B9169
		public TransportPodsArrivalAction_TransportShip(MapParent mapParent, TransportShip transportShip)
		{
			this.mapParent = mapParent;
			this.transportShip = transportShip;
		}

		// Token: 0x0600521A RID: 21018 RVA: 0x001BAF8A File Offset: 0x001B918A
		public override bool ShouldUseLongEvent(List<ActiveDropPodInfo> pods, int tile)
		{
			return !this.mapParent.HasMap;
		}

		// Token: 0x0600521B RID: 21019 RVA: 0x001BAF9C File Offset: 0x001B919C
		public override void Arrived(List<ActiveDropPodInfo> pods, int tile)
		{
			if (this.transportShip == null || this.transportShip.Disposed)
			{
				Log.Error("Trying to arrive in a null or disposed transport ship.");
				return;
			}
			bool flag = !this.mapParent.HasMap;
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(tile, null);
			if (!this.cell.IsValid)
			{
				this.cell = DropCellFinder.GetBestShuttleLandingSpot(orGenerateMap, Faction.OfPlayer);
			}
			LookTargets lookTargets = new LookTargets(this.cell, orGenerateMap);
			if (!this.cell.IsValid)
			{
				Log.Error("Could not find cell for transport ship arrival.");
				return;
			}
			Settlement settlement;
			if ((settlement = (orGenerateMap.Parent as Settlement)) != null && settlement.Faction != Faction.OfPlayer)
			{
				TaggedString label = "LetterLabelCaravanEnteredEnemyBase".Translate();
				TaggedString text = "LetterShuttleLandedInEnemyBase".Translate(settlement.Label).CapitalizeFirst();
				SettlementUtility.AffectRelationsOnAttacked(settlement, ref text);
				if (flag)
				{
					Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
					PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(orGenerateMap.mapPawns.AllPawns, ref label, ref text, "LetterRelatedPawnsInMapWherePlayerLanded".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
				}
				Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, lookTargets, settlement.Faction, null, null, null);
			}
			for (int i = 0; i < pods.Count; i++)
			{
				this.transportShip.TransporterComp.innerContainer.TryAddRangeOrTransfer(pods[i].innerContainer, true, true);
			}
			this.transportShip.ArriveAt(this.cell, this.mapParent);
			Messages.Message("MessageShuttleArrived".Translate(), lookTargets, MessageTypeDefOf.TaskCompletion, true);
		}

		// Token: 0x0600521C RID: 21020 RVA: 0x001BB148 File Offset: 0x001B9348
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<TransportShip>(ref this.transportShip, "transportShip", false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<IntVec3>(ref this.cell, "cell", default(IntVec3), false);
		}

		// Token: 0x04003077 RID: 12407
		public MapParent mapParent;

		// Token: 0x04003078 RID: 12408
		public TransportShip transportShip;

		// Token: 0x04003079 RID: 12409
		public IntVec3 cell = IntVec3.Invalid;
	}
}
