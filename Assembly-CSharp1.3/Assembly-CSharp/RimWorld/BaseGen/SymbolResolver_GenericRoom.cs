using System;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015E0 RID: 5600
	public class SymbolResolver_GenericRoom : SymbolResolver
	{
		// Token: 0x06008390 RID: 33680 RVA: 0x002F0074 File Offset: 0x002EE274
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

		// Token: 0x04005225 RID: 21029
		public string interior;

		// Token: 0x04005226 RID: 21030
		public bool useRandomCarpet;

		// Token: 0x04005227 RID: 21031
		public bool allowRoof = true;
	}
}
