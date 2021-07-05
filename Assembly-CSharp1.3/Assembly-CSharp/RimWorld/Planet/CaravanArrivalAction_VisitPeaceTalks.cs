using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001797 RID: 6039
	public class CaravanArrivalAction_VisitPeaceTalks : CaravanArrivalAction
	{
		// Token: 0x170016C0 RID: 5824
		// (get) Token: 0x06008B99 RID: 35737 RVA: 0x00321D75 File Offset: 0x0031FF75
		public override string Label
		{
			get
			{
				return "VisitPeaceTalks".Translate(this.peaceTalks.Label);
			}
		}

		// Token: 0x170016C1 RID: 5825
		// (get) Token: 0x06008B9A RID: 35738 RVA: 0x00321D96 File Offset: 0x0031FF96
		public override string ReportString
		{
			get
			{
				return "CaravanVisiting".Translate(this.peaceTalks.Label);
			}
		}

		// Token: 0x06008B9B RID: 35739 RVA: 0x0032126E File Offset: 0x0031F46E
		public CaravanArrivalAction_VisitPeaceTalks()
		{
		}

		// Token: 0x06008B9C RID: 35740 RVA: 0x00321DB7 File Offset: 0x0031FFB7
		public CaravanArrivalAction_VisitPeaceTalks(PeaceTalks peaceTalks)
		{
			this.peaceTalks = peaceTalks;
		}

		// Token: 0x06008B9D RID: 35741 RVA: 0x00321DC8 File Offset: 0x0031FFC8
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

		// Token: 0x06008B9E RID: 35742 RVA: 0x00321E11 File Offset: 0x00320011
		public override void Arrived(Caravan caravan)
		{
			this.peaceTalks.Notify_CaravanArrived(caravan);
		}

		// Token: 0x06008B9F RID: 35743 RVA: 0x00321E1F File Offset: 0x0032001F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<PeaceTalks>(ref this.peaceTalks, "peaceTalks", false);
		}

		// Token: 0x06008BA0 RID: 35744 RVA: 0x00321E38 File Offset: 0x00320038
		public static FloatMenuAcceptanceReport CanVisit(Caravan caravan, PeaceTalks peaceTalks)
		{
			return peaceTalks != null && peaceTalks.Spawned;
		}

		// Token: 0x06008BA1 RID: 35745 RVA: 0x00321E4C File Offset: 0x0032004C
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, PeaceTalks peaceTalks)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitPeaceTalks>(() => CaravanArrivalAction_VisitPeaceTalks.CanVisit(caravan, peaceTalks), () => new CaravanArrivalAction_VisitPeaceTalks(peaceTalks), "VisitPeaceTalks".Translate(peaceTalks.Label), caravan, peaceTalks.Tile, peaceTalks, null);
		}

		// Token: 0x040058DF RID: 22751
		private PeaceTalks peaceTalks;
	}
}
