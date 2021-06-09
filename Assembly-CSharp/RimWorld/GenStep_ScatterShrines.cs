using System;
using System.Collections.Generic;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x020012A9 RID: 4777
	public class GenStep_ScatterShrines : GenStep_ScatterRuinsSimple
	{
		// Token: 0x17001001 RID: 4097
		// (get) Token: 0x060067C8 RID: 26568 RVA: 0x00046BCA File Offset: 0x00044DCA
		public override int SeedPart
		{
			get
			{
				return 1801222485;
			}
		}

		// Token: 0x060067C9 RID: 26569 RVA: 0x001FFD20 File Offset: 0x001FDF20
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice.def.building.isNaturalRock;
		}

		// Token: 0x060067CA RID: 26570 RVA: 0x001FFD5C File Offset: 0x001FDF5C
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int stackCount = 1)
		{
			int randomInRange = GenStep_ScatterShrines.SizeRange.RandomInRange;
			int randomInRange2 = GenStep_ScatterShrines.SizeRange.RandomInRange;
			CellRect rect = new CellRect(loc.x, loc.z, randomInRange, randomInRange2);
			rect.ClipInsideMap(map);
			if (rect.Width != randomInRange || rect.Height != randomInRange2)
			{
				return;
			}
			foreach (IntVec3 c in rect.Cells)
			{
				List<Thing> list = map.thingGrid.ThingsListAt(c);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].def == ThingDefOf.AncientCryptosleepCasket)
					{
						return;
					}
				}
			}
			if (!base.CanPlaceAncientBuildingInRange(rect, map))
			{
				return;
			}
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = rect;
			resolveParams.disableSinglePawn = new bool?(true);
			resolveParams.disableHives = new bool?(true);
			resolveParams.makeWarningLetter = new bool?(true);
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("ancientTemple", resolveParams, null);
			BaseGen.Generate();
		}

		// Token: 0x04004517 RID: 17687
		private static readonly IntRange SizeRange = new IntRange(15, 20);
	}
}
