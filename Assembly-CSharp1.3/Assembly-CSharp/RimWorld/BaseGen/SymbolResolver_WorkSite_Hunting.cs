using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015F2 RID: 5618
	public class SymbolResolver_WorkSite_Hunting : SymbolResolver_WorkSite
	{
		// Token: 0x060083D1 RID: 33745 RVA: 0x002F2638 File Offset: 0x002F0838
		public override void Resolve(ResolveParams rp)
		{
			SymbolResolver_WorkSite_Hunting.<>c__DisplayClass3_0 CS$<>8__locals1 = new SymbolResolver_WorkSite_Hunting.<>c__DisplayClass3_0();
			CS$<>8__locals1.rp = rp;
			CellRect rect = CS$<>8__locals1.rp.rect;
			CS$<>8__locals1.butcherTableRot = Rot4.Random;
			CellRect cellRect = rect.ContractedBy(1);
			switch (CS$<>8__locals1.butcherTableRot.AsByte)
			{
			case 0:
				CS$<>8__locals1.<Resolve>g__Place|0(new CellRect(cellRect.minX, cellRect.maxZ, cellRect.Width, 1));
				break;
			case 1:
				CS$<>8__locals1.<Resolve>g__Place|0(new CellRect(cellRect.maxX, cellRect.minZ, 1, cellRect.Height));
				break;
			case 2:
				CS$<>8__locals1.<Resolve>g__Place|0(new CellRect(cellRect.minX, cellRect.minZ, cellRect.Width, 1));
				break;
			case 3:
				CS$<>8__locals1.<Resolve>g__Place|0(new CellRect(cellRect.minX, cellRect.minZ, 1, cellRect.Height));
				break;
			}
			BiomeDef biome = BaseGen.globalSettings.map.Parent.Biome;
			List<Thing> list = new List<Thing>();
			CS$<>8__locals1.leather = null;
			if (!CS$<>8__locals1.rp.stockpileConcreteContents.NullOrEmpty<Thing>())
			{
				SymbolResolver_WorkSite_Hunting.<>c__DisplayClass3_0 CS$<>8__locals2 = CS$<>8__locals1;
				Thing thing = CS$<>8__locals1.rp.stockpileConcreteContents.FirstOrDefault((Thing t) => t.def.IsLeather);
				CS$<>8__locals2.leather = ((thing != null) ? thing.def : null);
			}
			if (CS$<>8__locals1.leather == null)
			{
				CS$<>8__locals1.leather = (from def in DefDatabase<ThingDef>.AllDefs
				where def.race != null && def.race.leatherDef != null
				select def).RandomElement<ThingDef>().race.leatherDef;
			}
			PawnKindDef pawnKindDef = (from def in biome.AllWildAnimals
			where def.RaceProps.leatherDef == CS$<>8__locals1.leather
			select def).RandomElementWithFallback(null);
			if (pawnKindDef == null)
			{
				pawnKindDef = (from def in DefDatabase<PawnKindDef>.AllDefs
				where def.RaceProps.leatherDef == CS$<>8__locals1.leather
				select def).RandomElement<PawnKindDef>();
			}
			int num = Rand.RangeInclusive(3, 6);
			for (int i = 0; i < num; i++)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(pawnKindDef, null);
				pawn.Kill(null, null);
				list.Add(pawn.Corpse);
			}
			BaseGen.symbolStack.Push("stockpile", new ResolveParams
			{
				rect = cellRect,
				stockpileConcreteContents = list
			}, null);
			BaseGen.symbolStack.Push("filth", new ResolveParams
			{
				rect = cellRect,
				filthDef = ThingDefOf.Filth_Blood,
				filthDensity = new FloatRange?(new FloatRange(0.33f, 1.25f))
			}, null);
			base.Resolve(CS$<>8__locals1.rp);
		}

		// Token: 0x04005239 RID: 21049
		private const int ButcherTableCount = 2;

		// Token: 0x0400523A RID: 21050
		private const int AnimalCorpseCountMin = 3;

		// Token: 0x0400523B RID: 21051
		private const int AnimalCorpseCountMax = 6;
	}
}
