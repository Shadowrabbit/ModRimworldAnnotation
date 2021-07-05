using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001662 RID: 5730
	public class QuestPart_AddShipJob_FlyAway : QuestPart_AddShipJob
	{
		// Token: 0x06008592 RID: 34194 RVA: 0x002FEDAE File Offset: 0x002FCFAE
		public override ShipJob GetShipJob()
		{
			ShipJob_FlyAway shipJob_FlyAway = (ShipJob_FlyAway)ShipJobMaker.MakeShipJob(this.shipJobDef);
			shipJob_FlyAway.destinationTile = this.destinationTile;
			shipJob_FlyAway.arrivalAction = this.arrivalAction;
			shipJob_FlyAway.dropMode = this.dropMode;
			return shipJob_FlyAway;
		}

		// Token: 0x06008593 RID: 34195 RVA: 0x002FEDE4 File Offset: 0x002FCFE4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.destinationTile, "destinationTile", 0, false);
			Scribe_Deep.Look<TransportPodsArrivalAction>(ref this.arrivalAction, "arrivalAction", Array.Empty<object>());
			Scribe_Values.Look<TransportShipDropMode>(ref this.dropMode, "dropMode", TransportShipDropMode.None, false);
		}

		// Token: 0x0400536B RID: 21355
		public int destinationTile = -1;

		// Token: 0x0400536C RID: 21356
		public TransportPodsArrivalAction arrivalAction;

		// Token: 0x0400536D RID: 21357
		public TransportShipDropMode dropMode = TransportShipDropMode.All;
	}
}
