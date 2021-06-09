using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020020BB RID: 8379
	public class CaravanArrivalAction_OfferGifts : CaravanArrivalAction
	{
		// Token: 0x17001A38 RID: 6712
		// (get) Token: 0x0600B1A8 RID: 45480 RVA: 0x000736F7 File Offset: 0x000718F7
		public override string Label
		{
			get
			{
				return "OfferGifts".Translate();
			}
		}

		// Token: 0x17001A39 RID: 6713
		// (get) Token: 0x0600B1A9 RID: 45481 RVA: 0x00073708 File Offset: 0x00071908
		public override string ReportString
		{
			get
			{
				return "CaravanOfferingGifts".Translate(this.settlement.Label);
			}
		}

		// Token: 0x0600B1AA RID: 45482 RVA: 0x00073602 File Offset: 0x00071802
		public CaravanArrivalAction_OfferGifts()
		{
		}

		// Token: 0x0600B1AB RID: 45483 RVA: 0x00073729 File Offset: 0x00071929
		public CaravanArrivalAction_OfferGifts(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x0600B1AC RID: 45484 RVA: 0x00338AB8 File Offset: 0x00336CB8
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

		// Token: 0x0600B1AD RID: 45485 RVA: 0x00338B04 File Offset: 0x00336D04
		public override void Arrived(Caravan caravan)
		{
			CameraJumper.TryJumpAndSelect(caravan);
			Pawn playerNegotiator = BestCaravanPawnUtility.FindBestNegotiator(caravan, null, null);
			Find.WindowStack.Add(new Dialog_Trade(playerNegotiator, this.settlement, true));
		}

		// Token: 0x0600B1AE RID: 45486 RVA: 0x00073738 File Offset: 0x00071938
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x0600B1AF RID: 45487 RVA: 0x00338B3C File Offset: 0x00336D3C
		public static FloatMenuAcceptanceReport CanOfferGiftsTo(Caravan caravan, Settlement settlement)
		{
			return settlement != null && settlement.Spawned && !settlement.HasMap && settlement.Faction != null && settlement.Faction != Faction.OfPlayer && !settlement.Faction.def.permanentEnemy && settlement.Faction.HostileTo(Faction.OfPlayer) && settlement.CanTradeNow && CaravanArrivalAction_OfferGifts.HasNegotiator(caravan);
		}

		// Token: 0x0600B1B0 RID: 45488 RVA: 0x00338BAC File Offset: 0x00336DAC
		private static bool HasNegotiator(Caravan caravan)
		{
			Pawn pawn = BestCaravanPawnUtility.FindBestNegotiator(caravan, null, null);
			return pawn != null && !pawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled;
		}

		// Token: 0x0600B1B1 RID: 45489 RVA: 0x00338BE0 File Offset: 0x00336DE0
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Settlement settlement)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_OfferGifts>(() => CaravanArrivalAction_OfferGifts.CanOfferGiftsTo(caravan, settlement), () => new CaravanArrivalAction_OfferGifts(settlement), "OfferGifts".Translate(), caravan, settlement.Tile, settlement, null);
		}

		// Token: 0x04007A5B RID: 31323
		private Settlement settlement;
	}
}
