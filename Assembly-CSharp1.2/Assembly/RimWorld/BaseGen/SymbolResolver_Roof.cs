using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E67 RID: 7783
	public class SymbolResolver_Roof : SymbolResolver
	{
		// Token: 0x0600A7D0 RID: 42960 RVA: 0x0030D60C File Offset: 0x0030B80C
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
