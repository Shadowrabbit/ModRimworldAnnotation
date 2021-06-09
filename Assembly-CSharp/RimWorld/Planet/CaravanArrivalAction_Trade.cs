using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020020BD RID: 8381
	public class CaravanArrivalAction_Trade : CaravanArrivalAction
	{
		// Token: 0x17001A3A RID: 6714
		// (get) Token: 0x0600B1B5 RID: 45493 RVA: 0x00073771 File Offset: 0x00071971
		public override string Label
		{
			get
			{
				return "TradeWithSettlement".Translate(this.settlement.Label);
			}
		}

		// Token: 0x17001A3B RID: 6715
		// (get) Token: 0x0600B1B6 RID: 45494 RVA: 0x00073792 File Offset: 0x00071992
		public override string ReportString
		{
			get
			{
				return "CaravanTrading".Translate(this.settlement.Label);
			}
		}

		// Token: 0x0600B1B7 RID: 45495 RVA: 0x00073602 File Offset: 0x00071802
		public CaravanArrivalAction_Trade()
		{
		}

		// Token: 0x0600B1B8 RID: 45496 RVA: 0x000737B3 File Offset: 0x000719B3
		public CaravanArrivalAction_Trade(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x0600B1B9 RID: 45497 RVA: 0x00338C48 File Offset: 0x00336E48
		public override FloatMenuAcceptanceReport StillValid(Caravan caravan, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(caravan, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.settlement != null && this.settlement.Tile != destinationTile)
			{
				return false;
			}
			return CaravanArrivalAction_Trade.CanTradeWith(caravan, this.settlement);
		}

		// Token: 0x0600B1BA RID: 45498 RVA: 0x00338C94 File Offset: 0x00336E94
		public override void Arrived(Caravan caravan)
		{
			CameraJumper.TryJumpAndSelect(caravan);
			Pawn playerNegotiator = BestCaravanPawnUtility.FindBestNegotiator(caravan, this.settlement.Faction, this.settlement.TraderKind);
			Find.WindowStack.Add(new Dialog_Trade(playerNegotiator, this.settlement, false));
		}

		// Token: 0x0600B1BB RID: 45499 RVA: 0x000737C2 File Offset: 0x000719C2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x0600B1BC RID: 45500 RVA: 0x00338CE0 File Offset: 0x00336EE0
		public static FloatMenuAcceptanceReport CanTradeWith(Caravan caravan, Settlement settlement)
		{
			return settlement != null && settlement.Spawned && !settlement.HasMap && settlement.Faction != null && settlement.Faction != Faction.OfPlayer && !settlement.Faction.def.permanentEnemy && !settlement.Faction.HostileTo(Faction.OfPlayer) && settlement.CanTradeNow && CaravanArrivalAction_Trade.HasNegotiator(caravan, settlement);
		}

		// Token: 0x0600B1BD RID: 45501 RVA: 0x00338D50 File Offset: 0x00336F50
		private static bool HasNegotiator(Caravan caravan, Settlement settlement)
		{
			Pawn pawn = BestCaravanPawnUtility.FindBestNegotiator(caravan, settlement.Faction, settlement.TraderKind);
			return pawn != null && !pawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled;
		}

		// Token: 0x0600B1BE RID: 45502 RVA: 0x00338D90 File Offset: 0x00336F90
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Settlement settlement)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_Trade>(() => CaravanArrivalAction_Trade.CanTradeWith(caravan, settlement), () => new CaravanArrivalAction_Trade(settlement), "TradeWith".Translate(settlement.Label), caravan, settlement.Tile, settlement, null);
		}

		// Token: 0x04007A5E RID: 31326
		private Settlement settlement;
	}
}
