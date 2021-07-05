using System;
using System.Collections.Generic;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CA9 RID: 3241
	public class GenStep_ScatterShrines : GenStep_ScatterRuinsSimple
	{
		// Token: 0x17000D09 RID: 3337
		// (get) Token: 0x06004B92 RID: 19346 RVA: 0x00191A93 File Offset: 0x0018FC93
		public override int SeedPart
		{
			get
			{
				return 1801222485;
			}
		}

		// Token: 0x06004B93 RID: 19347 RVA: 0x00191A9C File Offset: 0x0018FC9C
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice.def.building.isNaturalRock;
		}

		// Token: 0x06004B94 RID: 19348 RVA: 0x00191AD8 File Offset: 0x0018FCD8
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
			if (Find.Storyteller.difficulty.peacefulTemples)
			{
				resolveParams.podContentsType = new PodContentsType?(PodContentsType.AncientFriendly);
			}
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("ancientTemple", resolveParams, null);
			BaseGen.Generate();
		}

		// Token: 0x04002DC0 RID: 11712
		private static readonly IntRange SizeRange = new IntRange(15, 20);
	}
}
