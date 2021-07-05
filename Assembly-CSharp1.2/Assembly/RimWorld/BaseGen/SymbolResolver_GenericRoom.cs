using System;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E85 RID: 7813
	public class SymbolResolver_GenericRoom : SymbolResolver
	{
		// Token: 0x0600A82A RID: 43050 RVA: 0x0030FBC4 File Offset: 0x0030DDC4
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("doors", rp, null);
			if (!this.interior.NullOrEmpty())
			{
				ResolveParams resolveParams = rp;
				resolveParams.rect = rp.rect.ContractedBy(1);
				BaseGen.symbolStack.Push(this.interior, resolveParams, null);
			}
			ResolveParams resolveParams2 = rp;
			if (this.useRandomCarpet)
			{
				resolveParams2.floorDef = (from x in DefDatabase<TerrainDef>.AllDefsListForReading
				where x.IsCarpet
				select x).RandomElement<TerrainDef>();
			}
			resolveParams2.noRoof = new bool?(!this.allowRoof);
			BaseGen.symbolStack.Push("emptyRoom", resolveParams2, null);
		}

		// Token: 0x04007219 RID: 29209
		public string interior;

		// Token: 0x0400721A RID: 29210
		public bool useRandomCarpet;

		// Token: 0x0400721B RID: 29211
		public bool allowRoof = true;
	}
}
