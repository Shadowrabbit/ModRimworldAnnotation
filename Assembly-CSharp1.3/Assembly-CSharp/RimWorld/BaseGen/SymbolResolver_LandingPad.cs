using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015E5 RID: 5605
	public class SymbolResolver_LandingPad : SymbolResolver
	{
		// Token: 0x060083A0 RID: 33696 RVA: 0x002F0544 File Offset: 0x002EE744
		public override void Resolve(ResolveParams rp)
		{
			TerrainDef floorDef = rp.floorDef ?? Rand.Element<TerrainDef>(TerrainDefOf.PavedTile, TerrainDefOf.Concrete);
			ResolveParams resolveParams = rp;
			resolveParams.singleThingDef = ThingDefOf.Shuttle;
			resolveParams.rect = CellRect.SingleCell(rp.rect.CenterCell);
			resolveParams.postThingSpawn = delegate(Thing x)
			{
				TransportShip transportShip = TransportShipMaker.MakeTransportShip(TransportShipDefOf.Ship_Shuttle, null, x);
				ShipJob_WaitTime shipJob_WaitTime = (ShipJob_WaitTime)ShipJobMaker.MakeShipJob(ShipJobDefOf.WaitTime);
				shipJob_WaitTime.duration = SymbolResolver_LandingPad.ShuttleLeaveAfterTicksRange.RandomInRange;
				shipJob_WaitTime.showGizmos = false;
				transportShip.AddJob(shipJob_WaitTime);
				transportShip.AddJob(ShipJobDefOf.FlyAway);
				transportShip.Start();
			};
			BaseGen.symbolStack.Push("thing", resolveParams, null);
			foreach (IntVec3 c in rp.rect.Corners)
			{
				ResolveParams resolveParams2 = rp;
				resolveParams2.singleThingDef = ThingDefOf.ShipLandingBeacon;
				resolveParams2.rect = CellRect.SingleCell(c);
				BaseGen.symbolStack.Push("thing", resolveParams2, null);
			}
			ResolveParams resolveParams3 = rp;
			resolveParams3.floorDef = floorDef;
			BaseGen.symbolStack.Push("floor", resolveParams3, null);
			BaseGen.symbolStack.Push("clear", rp, null);
		}

		// Token: 0x0400522B RID: 21035
		private static readonly IntRange ShuttleLeaveAfterTicksRange = new IntRange(300, 3600);
	}
}
