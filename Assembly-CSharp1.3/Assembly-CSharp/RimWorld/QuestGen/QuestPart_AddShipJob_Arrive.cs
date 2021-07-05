using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200165D RID: 5725
	public class QuestPart_AddShipJob_Arrive : QuestPart_AddShipJob
	{
		// Token: 0x06008585 RID: 34181 RVA: 0x002FEB05 File Offset: 0x002FCD05
		public override ShipJob GetShipJob()
		{
			ShipJob_Arrive shipJob_Arrive = (ShipJob_Arrive)ShipJobMaker.MakeShipJob(this.shipJobDef);
			shipJob_Arrive.cell = this.cell;
			shipJob_Arrive.mapParent = this.mapParent;
			shipJob_Arrive.factionForArrival = this.factionForArrival;
			return shipJob_Arrive;
		}

		// Token: 0x06008586 RID: 34182 RVA: 0x002FEB3C File Offset: 0x002FCD3C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.cell, "cell", default(IntVec3), false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_References.Look<Faction>(ref this.factionForArrival, "factionForArrival", false);
		}

		// Token: 0x04005362 RID: 21346
		public IntVec3 cell;

		// Token: 0x04005363 RID: 21347
		public MapParent mapParent;

		// Token: 0x04005364 RID: 21348
		public Faction factionForArrival;
	}
}
