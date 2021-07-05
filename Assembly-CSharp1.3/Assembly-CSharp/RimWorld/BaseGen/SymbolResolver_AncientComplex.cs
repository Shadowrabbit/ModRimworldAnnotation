using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001599 RID: 5529
	public class SymbolResolver_AncientComplex : SymbolResolver
	{
		// Token: 0x0600829B RID: 33435 RVA: 0x002E67D8 File Offset: 0x002E49D8
		public override void Resolve(ResolveParams rp)
		{
			ResolveParams resolveParams = rp;
			resolveParams.filthDensity = new FloatRange?(SymbolResolver_AncientComplex.FilthDensity_DriedBlood);
			resolveParams.filthDef = ThingDefOf.Filth_DriedBlood;
			BaseGen.symbolStack.Push("filth", resolveParams, null);
			resolveParams.ignoreDoorways = new bool?(true);
			resolveParams.filthDensity = new FloatRange?(SymbolResolver_AncientComplex.FilthDensity_MoldyUniform);
			resolveParams.filthDef = ThingDefOf.Filth_MoldyUniform;
			BaseGen.symbolStack.Push("filth", resolveParams, null);
			resolveParams.filthDensity = new FloatRange?(SymbolResolver_AncientComplex.FilthDensity_ScatteredDocuments);
			resolveParams.filthDef = ThingDefOf.Filth_ScatteredDocuments;
			BaseGen.symbolStack.Push("filth", resolveParams, null);
			ResolveParams resolveParams2 = rp;
			resolveParams2.desiccatedCorpsePawnKind = PawnKindDefOf.AncientSoldier;
			resolveParams2.desiccatedCorpseRandomAgeRange = new IntRange?(SymbolResolver_AncientComplex.CorpseRandomAgeRange);
			BaseGen.symbolStack.Push("desiccatedCorpses", resolveParams2, null);
			ResolveParams resolveParams3 = rp;
			resolveParams3.floorDef = TerrainDefOf.PackedDirt;
			BaseGen.symbolStack.Push("outdoorsPath", resolveParams3, null);
			BaseGen.symbolStack.Push("ancientComplexDefences", rp, null);
			BaseGen.symbolStack.Push("ensureCanReachMapEdge", rp, null);
			ComplexSketch complexSketch = rp.ancientComplexSketch;
			if (complexSketch == null)
			{
				complexSketch = ComplexDefOf.AncientComplex.Worker.GenerateSketch(new IntVec2(rp.rect.Width, rp.rect.Height), null);
			}
			ResolveParams resolveParams4 = rp;
			resolveParams4.ancientComplexSketch = complexSketch;
			BaseGen.symbolStack.Push("ancientComplexSketch", resolveParams4, null);
			ResolveParams resolveParams5 = rp;
			resolveParams5.floorDef = TerrainDefOf.Concrete;
			resolveParams5.allowBridgeOnAnyImpassableTerrain = new bool?(true);
			resolveParams5.floorOnlyIfTerrainSupports = new bool?(false);
			foreach (ComplexRoom complexRoom in complexSketch.layout.Rooms)
			{
				foreach (CellRect cellRect in complexRoom.rects)
				{
					resolveParams5.rect = cellRect.MovedBy(rp.rect.BottomLeft);
					BaseGen.symbolStack.Push("floor", resolveParams5, null);
					BaseGen.symbolStack.Push("clear", resolveParams5, null);
				}
			}
			ResolveParams resolveParams6 = rp;
			resolveParams6.rect = rp.rect.ExpandedBy(5);
			resolveParams6.floorDef = TerrainDefOf.Gravel;
			resolveParams6.chanceToSkipFloor = new float?(0.05f);
			resolveParams6.floorOnlyIfTerrainSupports = new bool?(true);
			BaseGen.symbolStack.Push("floor", resolveParams6, null);
			foreach (IntVec3 c in resolveParams6.rect)
			{
				Building edifice = c.GetEdifice(BaseGen.globalSettings.map);
				if (edifice != null && edifice.def.destroyable && edifice.def.IsBuildingArtificial)
				{
					edifice.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x040051C6 RID: 20934
		private static readonly FloatRange FilthDensity_DriedBlood = new FloatRange(0.01f, 0.025f);

		// Token: 0x040051C7 RID: 20935
		private static readonly FloatRange FilthDensity_MoldyUniform = new FloatRange(0.005f, 0.01f);

		// Token: 0x040051C8 RID: 20936
		private static readonly FloatRange FilthDensity_ScatteredDocuments = new FloatRange(0.005f, 0.015f);

		// Token: 0x040051C9 RID: 20937
		private static readonly IntRange CorpseRandomAgeRange = new IntRange(1080000000, 1260000000);
	}
}
