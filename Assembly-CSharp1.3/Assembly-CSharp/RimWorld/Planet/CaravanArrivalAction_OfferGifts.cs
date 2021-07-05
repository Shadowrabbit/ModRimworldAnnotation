using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001794 RID: 6036
	public class CaravanArrivalAction_OfferGifts : CaravanArrivalAction
	{
		// Token: 0x170016BA RID: 5818
		// (get) Token: 0x06008B7B RID: 35707 RVA: 0x00321651 File Offset: 0x0031F851
		public override string Label
		{
			get
			{
				return "OfferGifts".Translate();
			}
		}

		// Token: 0x170016BB RID: 5819
		// (get) Token: 0x06008B7C RID: 35708 RVA: 0x00321662 File Offset: 0x0031F862
		public override string ReportString
		{
			get
			{
				return "CaravanOfferingGifts".Translate(this.settlement.Label);
			}
		}

		// Token: 0x06008B7D RID: 35709 RVA: 0x0032126E File Offset: 0x0031F46E
		public CaravanArrivalAction_OfferGifts()
		{
		}

		// Token: 0x06008B7E RID: 35710 RVA: 0x00321683 File Offset: 0x0031F883
		public CaravanArrivalAction_OfferGifts(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x06008B7F RID: 35711 RVA: 0x00321694 File Offset: 0x0031F894
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
			return CaravanArrivalAction_OfferGifts.CanOfferGiftsTo(caravan, this.settlement);
		}

		// Token: 0x06008B80 RID: 35712 RVA: 0x003216E0 File Offset: 0x0031F8E0
		public override void Arrived(Caravan caravan)
		{
			CameraJumper.TryJumpAndSelect(caravan);
			Pawn playerNegotiator = BestCaravanPawnUtility.FindBestNegotiator(caravan, null, null);
			Find.WindowStack.Add(new Dialog_Trade(playerNegotiator, this.settlement, true));
		}

		// Token: 0x06008B81 RID: 35713 RVA: 0x00321718 File Offset: 0x0031F918
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x06008B82 RID: 35714 RVA: 0x00321734 File Offset: 0x0031F934
		public static FloatMenuAcceptanceReport CanOfferGiftsTo(Caravan caravan, Settlement settlement)
		{
			return settlement != null && settlement.Spawned && !settlement.HasMap && settlement.Faction != null && settlement.Faction != Faction.OfPlayer && !settlement.Faction.def.permanentEnemy && settlement.Faction.HostileTo(Faction.OfPlayer) && settlement.CanTradeNow && CaravanArrivalAction_OfferGifts.HasNegotiator(caravan);
		}

		// Token: 0x06008B83 RID: 35715 RVA: 0x003217A4 File Offset: 0x0031F9A4
		private static bool HasNegotiator(Caravan caravan)
		{
			Pawn pawn = BestCaravanPawnUtility.FindBestNegotiator(caravan, null, null);
			return pawn != null && !pawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled;
		}

		// Token: 0x06008B84 RID: 35716 RVA: 0x003217D8 File Offset: 0x0031F9D8
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Settlement settlement)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_OfferGifts>(() => CaravanArrivalAction_OfferGifts.CanOfferGiftsTo(caravan, settlement), () => new CaravanArrivalAction_OfferGifts(settlement), "OfferGifts".Translate(), caravan, settlement.Tile, settlement, null);
		}

		// Token: 0x040058DC RID: 22748
		private Settlement settlement;
	}
}
