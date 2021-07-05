using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C95 RID: 3221
	public class GenStep_ScatterAncientFences : GenStep_Scatterer
	{
		// Token: 0x17000CF5 RID: 3317
		// (get) Token: 0x06004B2D RID: 19245 RVA: 0x0018F3B6 File Offset: 0x0018D5B6
		public override int SeedPart
		{
			get
			{
				return 344678634;
			}
		}

		// Token: 0x06004B2E RID: 19246 RVA: 0x0018F3C0 File Offset: 0x0018D5C0
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckIdeology("Scatter ancient fences"))
			{
				return;
			}
			this.count = Rand.RangeInclusive(1, 3);
			GenStep_ScatterAncientFences.tmpUsedRects.Clear();
			base.Generate(map, parms);
			GenStep_ScatterAncientFences.tmpUsedRects.Clear();
			GenStep_ScatterAncientFences.tmpFenceCells.Clear();
		}

		// Token: 0x06004B2F RID: 19247 RVA: 0x0018F410 File Offset: 0x0018D610
		protected override bool CanScatterAt(IntVec3 loc, Map map)
		{
			if (!base.CanScatterAt(loc, map))
			{
				return false;
			}
			this.thingToScatter = (Rand.Bool ? ThingDefOf.AncientFence : ThingDefOf.AncientRazorWire);
			int randomInRange = GenStep_ScatterAncientFences.RectSizeRange.RandomInRange;
			CellRect item = CellRect.CenteredOn(loc, randomInRange, randomInRange);
			for (int i = 0; i < GenStep_ScatterAncientFences.tmpUsedRects.Count; i++)
			{
				if (item.Overlaps(GenStep_ScatterAncientFences.tmpUsedRects[i]))
				{
					return false;
				}
			}
			GenStep_ScatterAncientFences.tmpFenceCells.Clear();
			GenStep_ScatterAncientFences.tmpFenceCells.AddRange(item.EdgeCells);
			for (int j = 0; j < GenStep_ScatterAncientFences.tmpFenceCells.Count; j++)
			{
				if (!GenStep_ScatterAncientFences.tmpFenceCells[j].InBounds(map) || GenStep_ScatterAncientFences.tmpFenceCells[j].GetEdifice(map) != null || GenStep_ScatterAncientFences.tmpFenceCells[j].GetRoof(map) != null || GenStep_ScatterAncientFences.tmpFenceCells[j].Impassable(map))
				{
					return false;
				}
				TerrainDef terrain = GenStep_ScatterAncientFences.tmpFenceCells[j].GetTerrain(map);
				if (terrain.IsWater || terrain.IsRoad || !GenConstruct.CanBuildOnTerrain(this.thingToScatter, GenStep_ScatterAncientFences.tmpFenceCells[j], map, Rot4.North, null, null))
				{
					return false;
				}
			}
			GenStep_ScatterAncientFences.tmpUsedRects.Add(item);
			return true;
		}

		// Token: 0x06004B30 RID: 19248 RVA: 0x0018F568 File Offset: 0x0018D768
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			int num = Rand.Range(0, GenStep_ScatterAncientFences.tmpFenceCells.Count);
			int num2 = Mathf.RoundToInt((float)GenStep_ScatterAncientFences.tmpFenceCells.Count * GenStep_ScatterAncientFences.PerimeterCellsRange.RandomInRange);
			for (int i = 0; i < num2; i++)
			{
				if (!Rand.Chance(0.25f))
				{
					IntVec3 loc2 = GenStep_ScatterAncientFences.tmpFenceCells[(i + num) % GenStep_ScatterAncientFences.tmpFenceCells.Count];
					GenSpawn.Spawn(ThingMaker.MakeThing(this.thingToScatter, null), loc2, map, WipeMode.Vanish);
				}
			}
		}

		// Token: 0x04002D90 RID: 11664
		private static readonly FloatRange PerimeterCellsRange = new FloatRange(0.2f, 0.4f);

		// Token: 0x04002D91 RID: 11665
		private static readonly IntRange RectSizeRange = new IntRange(6, 12);

		// Token: 0x04002D92 RID: 11666
		private const float SkipChance = 0.25f;

		// Token: 0x04002D93 RID: 11667
		private CellRect rect;

		// Token: 0x04002D94 RID: 11668
		private ThingDef thingToScatter;

		// Token: 0x04002D95 RID: 11669
		private static List<IntVec3> tmpFenceCells = new List<IntVec3>();

		// Token: 0x04002D96 RID: 11670
		private static List<CellRect> tmpUsedRects = new List<CellRect>();
	}
}
