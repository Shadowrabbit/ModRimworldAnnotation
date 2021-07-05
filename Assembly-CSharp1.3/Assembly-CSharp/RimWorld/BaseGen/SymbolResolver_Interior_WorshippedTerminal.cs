using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001604 RID: 5636
	public class SymbolResolver_Interior_WorshippedTerminal : SymbolResolver
	{
		// Token: 0x060083FF RID: 33791 RVA: 0x002F405C File Offset: 0x002F225C
		public override void Resolve(ResolveParams rp)
		{
			IntVec3 topRight = rp.rect.TopRight;
			topRight.x -= rp.rect.Width / 2;
			topRight.z--;
			List<IntVec3> list = new List<IntVec3>();
			list.Add(topRight + Rot4.East.FacingCell);
			list.Add(topRight + Rot4.West.FacingCell);
			CellRect rect = rp.rect;
			rect.maxZ -= 3;
			ResolveParams resolveParams = rp;
			if (rp.sitePart != null)
			{
				resolveParams.singleThingToSpawn = rp.sitePart.things.FirstOrDefault((Thing t) => t.def == ThingDefOf.AncientTerminal);
			}
			if (resolveParams.singleThingToSpawn == null)
			{
				resolveParams.singleThingToSpawn = ThingMaker.MakeThing(ThingDefOf.AncientTerminal, null);
			}
			resolveParams.rect = CellRect.CenteredOn(topRight, 1, 1);
			resolveParams.thingRot = new Rot4?(Rot4.South);
			BaseGen.symbolStack.Push("thing", resolveParams, null);
			if (rect.Height > 1)
			{
				ResolveParams resolveParams2 = rp;
				resolveParams2.rect = rect;
				resolveParams2.singleThingDef = ThingDefOf.SteleLarge;
				BaseGen.symbolStack.Push("thing", resolveParams2, null);
				if (Rand.Chance(0.5f))
				{
					BaseGen.symbolStack.Push("thing", resolveParams2, null);
				}
			}
		}
	}
}
