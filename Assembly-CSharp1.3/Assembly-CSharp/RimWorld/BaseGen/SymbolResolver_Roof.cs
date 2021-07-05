using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015C6 RID: 5574
	public class SymbolResolver_Roof : SymbolResolver
	{
		// Token: 0x06008341 RID: 33601 RVA: 0x002EBE60 File Offset: 0x002EA060
		public override void Resolve(ResolveParams rp)
		{
			if (rp.noRoof != null && rp.noRoof.Value)
			{
				return;
			}
			RoofGrid roofGrid = BaseGen.globalSettings.map.roofGrid;
			RoofDef def = rp.roofDef ?? RoofDefOf.RoofConstructed;
			foreach (IntVec3 c in rp.rect)
			{
				if (!roofGrid.Roofed(c))
				{
					roofGrid.SetRoof(c, def);
				}
			}
		}
	}
}
