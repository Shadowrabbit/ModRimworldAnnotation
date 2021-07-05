using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020008F4 RID: 2292
	public class ShipJob_WaitSendable : ShipJob_Wait
	{
		// Token: 0x17000AC7 RID: 2759
		// (get) Token: 0x06003C0F RID: 15375 RVA: 0x0001276E File Offset: 0x0001096E
		protected override bool ShouldEnd
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000AC8 RID: 2760
		// (get) Token: 0x06003C10 RID: 15376 RVA: 0x0014EBBD File Offset: 0x0014CDBD
		public override bool HasDestination
		{
			get
			{
				return this.destination != null;
			}
		}

		// Token: 0x06003C11 RID: 15377 RVA: 0x0014EBC8 File Offset: 0x0014CDC8
		public override IEnumerable<Gizmo> GetJobGizmos()
		{
			yield break;
		}

		// Token: 0x06003C12 RID: 15378 RVA: 0x0014EBD4 File Offset: 0x0014CDD4
		protected override void SendAway()
		{
			ShipJob_FlyAway shipJob_FlyAway = (ShipJob_FlyAway)ShipJobMaker.MakeShipJob(ShipJobDefOf.FlyAway);
			shipJob_FlyAway.destinationTile = this.destination.Tile;
			shipJob_FlyAway.arrivalAction = new TransportPodsArrivalAction_TransportShip(this.destination, this.transportShip);
			shipJob_FlyAway.dropMode = TransportShipDropMode.None;
			this.transportShip.SetNextJob(shipJob_FlyAway);
			this.transportShip.TryGetNextJob();
		}

		// Token: 0x06003C13 RID: 15379 RVA: 0x0014EC37 File Offset: 0x0014CE37
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.destination, "destination", false);
		}

		// Token: 0x040020A6 RID: 8358
		public MapParent destination;
	}
}
