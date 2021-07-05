using System;

namespace RimWorld.BaseGen
{
	// Token: 0x0200159B RID: 5531
	public class SymbolResolver_AncientComplex_Sketch : SymbolResolver
	{
		// Token: 0x060082A4 RID: 33444 RVA: 0x002E70F0 File Offset: 0x002E52F0
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && rp.ancientComplexSketch != null;
		}

		// Token: 0x060082A5 RID: 33445 RVA: 0x002E7106 File Offset: 0x002E5306
		public override void Resolve(ResolveParams rp)
		{
			rp.ancientComplexSketch.complexDef.Worker.Spawn(rp.ancientComplexSketch, BaseGen.globalSettings.map, rp.rect.BottomLeft, rp.threatPoints, null);
		}
	}
}
