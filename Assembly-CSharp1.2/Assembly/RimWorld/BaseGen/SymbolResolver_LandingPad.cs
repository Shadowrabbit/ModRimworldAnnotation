using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E8C RID: 7820
	public class SymbolResolver_LandingPad : SymbolResolver
	{
		// Token: 0x0600A83E RID: 43070 RVA: 0x00310010 File Offset: 0x0030E210
		public override void Resolve(ResolveParams rp)
		{
			TerrainDef floorDef = rp.floorDef ?? BaseGenUtility.RandomBasicFloorDef(rp.faction, false);
			ResolveParams resolveParams = rp;
			resolveParams.singleThingDef = ThingDefOf.Shuttle;
			resolveParams.rect = CellRect.SingleCell(rp.rect.CenterCell);
			resolveParams.postThingSpawn = delegate(Thing x)
			{
				x.TryGetComp<CompShuttle>().leaveAfterTicks = SymbolResolver_LandingPad.ShuttleLeaveAfterTicksRange.RandomInRange;
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

		// Token: 0x04007225 RID: 29221
		private static readonly IntRange ShuttleLeaveAfterTicksRange = new IntRange(300, 3600);
	}
}
