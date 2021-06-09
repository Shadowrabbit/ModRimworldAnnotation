using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020020C4 RID: 8388
	public class CaravanArrivalAction_VisitSettlement : CaravanArrivalAction
	{
		// Token: 0x17001A40 RID: 6720
		// (get) Token: 0x0600B1DD RID: 45533 RVA: 0x00073952 File Offset: 0x00071B52
		public override string Label
		{
			get
			{
				return "VisitSettlement".Translate(this.settlement.Label);
			}
		}

		// Token: 0x17001A41 RID: 6721
		// (get) Token: 0x0600B1DE RID: 45534 RVA: 0x00073973 File Offset: 0x00071B73
		public override string ReportString
		{
			get
			{
				return "CaravanVisiting".Translate(this.settlement.Label);
			}
		}

		// Token: 0x0600B1DF RID: 45535 RVA: 0x00073602 File Offset: 0x00071802
		public CaravanArrivalAction_VisitSettlement()
		{
		}

		// Token: 0x0600B1E0 RID: 45536 RVA: 0x00073994 File Offset: 0x00071B94
		public CaravanArrivalAction_VisitSettlement(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x0600B1E1 RID: 45537 RVA: 0x00339164 File Offset: 0x00337364
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
			return CaravanArrivalAction_VisitSettlement.CanVisit(caravan, this.settlement);
		}

		// Token: 0x0600B1E2 RID: 45538 RVA: 0x003391B0 File Offset: 0x003373B0
		public override void Arrived(Caravan caravan)
		{
			if (caravan.IsPlayerControlled)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelCaravanEnteredMap".Translate(this.settlement), "LetterCaravanEnteredMap".Translate(caravan.Label, this.settlement).CapitalizeFirst(), LetterDefOf.NeutralEvent, caravan, null, null, null, null);
			}
		}

		// Token: 0x0600B1E3 RID: 45539 RVA: 0x000739A3 File Offset: 0x00071BA3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x0600B1E4 RID: 45540 RVA: 0x000739BC File Offset: 0x00071BBC
		public static FloatMenuAcceptanceReport CanVisit(Caravan caravan, Settlement settlement)
		{
			return settlement != null && settlement.Spawned && settlement.Visitable;
		}

		// Token: 0x0600B1E5 RID: 45541 RVA: 0x0033921C File Offset: 0x0033741C
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Settlement settlement)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitSettlement>(() => CaravanArrivalAction_VisitSettlement.CanVisit(caravan, settlement), () => new CaravanArrivalAction_VisitSettlement(settlement), "VisitSettlement".Translate(settlement.Label), caravan, settlement.Tile, settlement, null);
		}

		// Token: 0x04007A69 RID: 31337
		private Settlement settlement;
	}
}
