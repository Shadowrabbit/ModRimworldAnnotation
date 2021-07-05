using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001798 RID: 6040
	public class CaravanArrivalAction_VisitSettlement : CaravanArrivalAction
	{
		// Token: 0x170016C2 RID: 5826
		// (get) Token: 0x06008BA2 RID: 35746 RVA: 0x00321EC1 File Offset: 0x003200C1
		public override string Label
		{
			get
			{
				return "VisitSettlement".Translate(this.settlement.Label);
			}
		}

		// Token: 0x170016C3 RID: 5827
		// (get) Token: 0x06008BA3 RID: 35747 RVA: 0x00321EE2 File Offset: 0x003200E2
		public override string ReportString
		{
			get
			{
				return "CaravanVisiting".Translate(this.settlement.Label);
			}
		}

		// Token: 0x06008BA4 RID: 35748 RVA: 0x0032126E File Offset: 0x0031F46E
		public CaravanArrivalAction_VisitSettlement()
		{
		}

		// Token: 0x06008BA5 RID: 35749 RVA: 0x00321F03 File Offset: 0x00320103
		public CaravanArrivalAction_VisitSettlement(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x06008BA6 RID: 35750 RVA: 0x00321F14 File Offset: 0x00320114
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

		// Token: 0x06008BA7 RID: 35751 RVA: 0x00321F60 File Offset: 0x00320160
		public override void Arrived(Caravan caravan)
		{
			if (caravan.IsPlayerControlled)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelCaravanEnteredMap".Translate(this.settlement), "LetterCaravanEnteredMap".Translate(caravan.Label, this.settlement).CapitalizeFirst(), LetterDefOf.NeutralEvent, caravan, null, null, null, null);
			}
		}

		// Token: 0x06008BA8 RID: 35752 RVA: 0x00321FCB File Offset: 0x003201CB
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x06008BA9 RID: 35753 RVA: 0x00321FE4 File Offset: 0x003201E4
		public static FloatMenuAcceptanceReport CanVisit(Caravan caravan, Settlement settlement)
		{
			return settlement != null && settlement.Spawned && settlement.Visitable;
		}

		// Token: 0x06008BAA RID: 35754 RVA: 0x00322000 File Offset: 0x00320200
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Settlement settlement)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitSettlement>(() => CaravanArrivalAction_VisitSettlement.CanVisit(caravan, settlement), () => new CaravanArrivalAction_VisitSettlement(settlement), "VisitSettlement".Translate(settlement.Label), caravan, settlement.Tile, settlement, null);
		}

		// Token: 0x040058E0 RID: 22752
		private Settlement settlement;
	}
}
