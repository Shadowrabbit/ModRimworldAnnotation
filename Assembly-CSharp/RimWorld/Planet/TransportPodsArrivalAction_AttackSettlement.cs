using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200216F RID: 8559
	public class TransportPodsArrivalAction_AttackSettlement : TransportPodsArrivalAction
	{
		// Token: 0x0600B652 RID: 46674 RVA: 0x0004B7F4 File Offset: 0x000499F4
		public TransportPodsArrivalAction_AttackSettlement()
		{
		}

		// Token: 0x0600B653 RID: 46675 RVA: 0x00076460 File Offset: 0x00074660
		public TransportPodsArrivalAction_AttackSettlement(Settlement settlement, PawnsArrivalModeDef arrivalMode)
		{
			this.settlement = settlement;
			this.arrivalMode = arrivalMode;
		}

		// Token: 0x0600B654 RID: 46676 RVA: 0x00076476 File Offset: 0x00074676
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
			Scribe_Defs.Look<PawnsArrivalModeDef>(ref this.arrivalMode, "arrivalMode");
		}

		// Token: 0x0600B655 RID: 46677 RVA: 0x0034B574 File Offset: 0x00349774
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

		// Token: 0x0600B656 RID: 46678 RVA: 0x0007649F File Offset: 0x0007469F
		public override bool ShouldUseLongEvent(List<ActiveDropPodInfo> pods, int tile)
		{
			return !this.settlement.HasMap;
		}

		// Token: 0x0600B657 RID: 46679 RVA: 0x0034B5C0 File Offset: 0x003497C0
		public override void Arrived(List<ActiveDropPodInfo> pods, int tile)
		{
			Thing lookTarget = TransportPodsArrivalActionUtility.GetLookTarget(pods);
			bool flag = !this.settlement.HasMap;
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(this.settlement.Tile, null);
			TaggedString label = "LetterLabelCaravanEnteredEnemyBase".Translate();
			TaggedString text = "LetterTransportPodsLandedInEnemyBase".Translate(this.settlement.Label).CapitalizeFirst();
			SettlementUtility.AffectRelationsOnAttacked_NewTmp(this.settlement, ref text);
			if (flag)
			{
				Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(orGenerateMap.mapPawns.AllPawns, ref label, ref text, "LetterRelatedPawnsInMapWherePlayerLanded".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
			}
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, lookTarget, this.settlement.Faction, null, null, null);
			this.arrivalMode.Worker.TravelingTransportPodsArrived(pods, orGenerateMap);
		}

		// Token: 0x0600B658 RID: 46680 RVA: 0x0034B6AC File Offset: 0x003498AC
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

		// Token: 0x0600B659 RID: 46681 RVA: 0x000764AF File Offset: 0x000746AF
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(CompLaunchable representative, IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			if (representative.parent.TryGetComp<CompShuttle>() != null)
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
					arrivalActionGetter = (<>9__1 = (() => new TransportPodsArrivalAction_AttackSettlement(settlement, PawnsArrivalModeDefOf.Shuttle)));
				}
				foreach (FloatMenuOption floatMenuOption in TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_AttackSettlement>(acceptanceReportGetter, arrivalActionGetter, "AttackShuttle".Translate(settlement.Label), representative, settlement.Tile, null))
				{
					yield return floatMenuOption;
				}
				IEnumerator<FloatMenuOption> enumerator = null;
			}
			else
			{
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
					arrivalActionGetter2 = (<>9__3 = (() => new TransportPodsArrivalAction_AttackSettlement(settlement, PawnsArrivalModeDefOf.EdgeDrop)));
				}
				foreach (FloatMenuOption floatMenuOption2 in TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_AttackSettlement>(acceptanceReportGetter2, arrivalActionGetter2, "AttackAndDropAtEdge".Translate(settlement.Label), representative, settlement.Tile, null))
				{
					yield return floatMenuOption2;
				}
				IEnumerator<FloatMenuOption> enumerator = null;
				Func<FloatMenuAcceptanceReport> <>9__4;
				Func<FloatMenuAcceptanceReport> acceptanceReportGetter3;
				if ((acceptanceReportGetter3 = <>9__4) == null)
				{
					acceptanceReportGetter3 = (<>9__4 = (() => TransportPodsArrivalAction_AttackSettlement.CanAttack(pods, settlement)));
				}
				Func<TransportPodsArrivalAction_AttackSettlement> <>9__5;
				Func<TransportPodsArrivalAction_AttackSettlement> arrivalActionGetter3;
				if ((arrivalActionGetter3 = <>9__5) == null)
				{
					arrivalActionGetter3 = (<>9__5 = (() => new TransportPodsArrivalAction_AttackSettlement(settlement, PawnsArrivalModeDefOf.CenterDrop)));
				}
				foreach (FloatMenuOption floatMenuOption3 in TransportPodsArrivalActionUtility.GetFloatMenuOptions<TransportPodsArrivalAction_AttackSettlement>(acceptanceReportGetter3, arrivalActionGetter3, "AttackAndDropInCenter".Translate(settlement.Label), representative, settlement.Tile, null))
				{
					yield return floatMenuOption3;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x04007CDA RID: 31962
		private Settlement settlement;

		// Token: 0x04007CDB RID: 31963
		private PawnsArrivalModeDef arrivalMode;
	}
}
