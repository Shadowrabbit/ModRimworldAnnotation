using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020020C2 RID: 8386
	public class CaravanArrivalAction_VisitPeaceTalks : CaravanArrivalAction
	{
		// Token: 0x17001A3E RID: 6718
		// (get) Token: 0x0600B1D1 RID: 45521 RVA: 0x000738A7 File Offset: 0x00071AA7
		public override string Label
		{
			get
			{
				return "VisitPeaceTalks".Translate(this.peaceTalks.Label);
			}
		}

		// Token: 0x17001A3F RID: 6719
		// (get) Token: 0x0600B1D2 RID: 45522 RVA: 0x000738C8 File Offset: 0x00071AC8
		public override string ReportString
		{
			get
			{
				return "CaravanVisiting".Translate(this.peaceTalks.Label);
			}
		}

		// Token: 0x0600B1D3 RID: 45523 RVA: 0x00073602 File Offset: 0x00071802
		public CaravanArrivalAction_VisitPeaceTalks()
		{
		}

		// Token: 0x0600B1D4 RID: 45524 RVA: 0x000738E9 File Offset: 0x00071AE9
		public CaravanArrivalAction_VisitPeaceTalks(PeaceTalks peaceTalks)
		{
			this.peaceTalks = peaceTalks;
		}

		// Token: 0x0600B1D5 RID: 45525 RVA: 0x003390A0 File Offset: 0x003372A0
		public override FloatMenuAcceptanceReport StillValid(Caravan caravan, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(caravan, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.peaceTalks != null && this.peaceTalks.Tile != destinationTile)
			{
				return false;
			}
			return CaravanArrivalAction_VisitPeaceTalks.CanVisit(caravan, this.peaceTalks);
		}

		// Token: 0x0600B1D6 RID: 45526 RVA: 0x000738F8 File Offset: 0x00071AF8
		public override void Arrived(Caravan caravan)
		{
			this.peaceTalks.Notify_CaravanArrived(caravan);
		}

		// Token: 0x0600B1D7 RID: 45527 RVA: 0x00073906 File Offset: 0x00071B06
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<PeaceTalks>(ref this.peaceTalks, "peaceTalks", false);
		}

		// Token: 0x0600B1D8 RID: 45528 RVA: 0x0007391F File Offset: 0x00071B1F
		public static FloatMenuAcceptanceReport CanVisit(Caravan caravan, PeaceTalks peaceTalks)
		{
			return peaceTalks != null && peaceTalks.Spawned;
		}

		// Token: 0x0600B1D9 RID: 45529 RVA: 0x003390EC File Offset: 0x003372EC
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, PeaceTalks peaceTalks)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitPeaceTalks>(() => CaravanArrivalAction_VisitPeaceTalks.CanVisit(caravan, peaceTalks), () => new CaravanArrivalAction_VisitPeaceTalks(peaceTalks), "VisitPeaceTalks".Translate(peaceTalks.Label), caravan, peaceTalks.Tile, peaceTalks, null);
		}

		// Token: 0x04007A66 RID: 31334
		private PeaceTalks peaceTalks;
	}
}
