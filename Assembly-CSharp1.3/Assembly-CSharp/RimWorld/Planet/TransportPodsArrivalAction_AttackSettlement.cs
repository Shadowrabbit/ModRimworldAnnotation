using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017E3 RID: 6115
	public class TransportPodsArrivalAction_AttackSettlement : TransportPodsArrivalAction
	{
		// Token: 0x06008E56 RID: 36438 RVA: 0x00331E93 File Offset: 0x00330093
		public TransportPodsArrivalAction_AttackSettlement()
		{
		}

		// Token: 0x06008E57 RID: 36439 RVA: 0x00331E9B File Offset: 0x0033009B
		public TransportPodsArrivalAction_AttackSettlement(Settlement settlement, PawnsArrivalModeDef arrivalMode)
		{
			this.settlement = settlement;
			this.arrivalMode = arrivalMode;
		}

		// Token: 0x06008E58 RID: 36440 RVA: 0x00331EB1 File Offset: 0x003300B1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
			Scribe_Defs.Look<PawnsArrivalModeDef>(ref this.arrivalMode, "arrivalMode");
		}

		// Token: 0x06008E59 RID: 36441 RVA: 0x00331EDC File Offset: 0x003300DC
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.settlement != null && this.settlement.Tile != destinationTile)
			{
				return false;
			}
			return TransportPodsArrivalAction_AttackSettlement.CanAttack(pods, this.settlement);
		}

		// Token: 0x06008E5A RID: 36442 RVA: 0x00331F25 File Offset: 0x00330125
		public override bool ShouldUseLongEvent(List<ActiveDropPodInfo> pods, int tile)
		{
			return !this.settlement.HasMap;
		}

		// Token: 0x06008E5B RID: 36443 RVA: 0x00331F38 File Offset: 0x00330138
		public override void Arrived(List<ActiveDropPodInfo> pods, int tile)
		{
			Thing lookTarget = TransportPodsArrivalActionUtility.GetLookTarget(pods);
			bool flag = !this.settlement.HasMap;
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(this.settlement.Tile, null);
			TaggedString label = "LetterLabelCaravanEnteredEnemyBase".Translate();
			TaggedString text = "LetterTransportPodsLandedInEnemyBase".Translate(this.settlement.Label).CapitalizeFirst();
			SettlementUtility.AffectRelationsOnAttacked(this.settlement, ref text);
			if (flag)
			{
				Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(orGenerateMap.mapPawns.AllPawns, ref label, ref text, "LetterRelatedPawnsInMapWherePlayerLanded".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
			}
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, lookTarget, this.settlement.Faction, null, null, null);
			this.arrivalMode.Worker.TravelingTransportPodsArrived(pods, orGenerateMap);
		}

		// Token: 0x06008E5C RID: 36444 RVA: 0x00332024 File Offset: 0x00330224
		public static FloatMenuAcceptanceReport CanAttack(IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			if (settlement == null || !settlement.Spawned || !settlement.Attackable)
			{
				return false;
			}
			if (!TransportPodsArrivalActionUtility.AnyNonDownedColonist(pods))
			{
				return false;
			}
			if (settlement.EnterCooldownBlocksEntering())
			{
				return FloatMenuAcceptanceReport.WithFailReasonAndMessage("EnterCooldownBlocksEntering".Translate(), "MessageEnterCooldownBlocksEntering".Translate(settlement.EnterCooldownTicksLeft().ToStringTicksToPeriod(true, false, true, true)));
			}
			return true;
		}

		// Token: 0x06008E5D RID: 36445 RVA: 0x003320A0 File Offset: 0x003302A0
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(CompLaunchable representative, IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			Func<FloatMenuAcceptanceReport> <>9__0;
			Func<FloatMenuAcceptanceReport> acceptanceReportGetter;
			if ((acceptanceReportGetter = <>9__0) == null)
			{
				acceptanceReportGetter = (<>9__0 = (() => TransportPodsArrivalAction_AttackSettlement.CanAttack(pods, settlement)));
			}
			Func<TransportPodsArrivalAction_AttackSettlement> <>9__1;
			Func<TransportPodsArrivalAction_AttackSettlement> arrivalActionGetter;
			if ((arrivalActionGetter = <>9__1) == null)
			{
				arrivalActionGetter = (<>9__1 = (() => new TransportPodsArrivalAction_AttackSettlement(settlement, PawnsArrivalModeDefOf.EdgeDrop)));
			}
			foreach (FloatMenuOption floatMenuOption in TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_AttackSettlement>(acceptanceReportGetter, arrivalActionGetter, "AttackAndDropAtEdge".Translate(settlement.Label), representative, settlement.Tile, null))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			Func<FloatMenuAcceptanceReport> <>9__2;
			Func<FloatMenuAcceptanceReport> acceptanceReportGetter2;
			if ((acceptanceReportGetter2 = <>9__2) == null)
			{
				acceptanceReportGetter2 = (<>9__2 = (() => TransportPodsArrivalAction_AttackSettlement.CanAttack(pods, settlement)));
			}
			Func<TransportPodsArrivalAction_AttackSettlement> <>9__3;
			Func<TransportPodsArrivalAction_AttackSettlement> arrivalActionGetter2;
			if ((arrivalActionGetter2 = <>9__3) == null)
			{
				arrivalActionGetter2 = (<>9__3 = (() => new TransportPodsArrivalAction_AttackSettlement(settlement, PawnsArrivalModeDefOf.CenterDrop)));
			}
			foreach (FloatMenuOption floatMenuOption2 in TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_AttackSettlement>(acceptanceReportGetter2, arrivalActionGetter2, "AttackAndDropInCenter".Translate(settlement.Label), representative, settlement.Tile, null))
			{
				yield return floatMenuOption2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x040059DC RID: 23004
		private Settlement settlement;

		// Token: 0x040059DD RID: 23005
		private PawnsArrivalModeDef arrivalMode;
	}
}
