using System;
using RimWorld.Planet;

namespace RimWorld.QuestGen
{
	// Token: 0x02001659 RID: 5721
	public class QuestNode_AddShipJob_FlyAway : QuestNode_AddShipJob
	{
		// Token: 0x17001614 RID: 5652
		// (get) Token: 0x06008574 RID: 34164 RVA: 0x002FE6F9 File Offset: 0x002FC8F9
		protected override ShipJobDef DefaultShipJobDef
		{
			get
			{
				return ShipJobDefOf.FlyAway;
			}
		}

		// Token: 0x06008575 RID: 34165 RVA: 0x002FE700 File Offset: 0x002FC900
		protected override void AddJobVars(ShipJob shipJob, Slate slate)
		{
			ShipJob_FlyAway shipJob_FlyAway;
			if ((shipJob_FlyAway = (shipJob as ShipJob_FlyAway)) != null)
			{
				MapParent value = this.destination.GetValue(slate);
				if (value != null)
				{
					shipJob_FlyAway.destinationTile = value.Tile;
				}
				shipJob_FlyAway.dropMode = (this.unsatisfiedDropMode.GetValue(slate) ?? TransportShipDropMode.NonRequired);
			}
		}

		// Token: 0x04005354 RID: 21332
		public SlateRef<MapParent> destination;

		// Token: 0x04005355 RID: 21333
		public SlateRef<TransportShipDropMode?> unsatisfiedDropMode;
	}
}
