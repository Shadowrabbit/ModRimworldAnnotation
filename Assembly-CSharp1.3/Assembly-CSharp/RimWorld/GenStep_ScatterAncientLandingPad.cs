using System;
using RimWorld.BaseGen;
using RimWorld.SketchGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C96 RID: 3222
	public class GenStep_ScatterAncientLandingPad : GenStep_Scatterer
	{
		// Token: 0x17000CF6 RID: 3318
		// (get) Token: 0x06004B33 RID: 19251 RVA: 0x0018F624 File Offset: 0x0018D824
		public override int SeedPart
		{
			get
			{
				return 1872954345;
			}
		}

		// Token: 0x06004B34 RID: 19252 RVA: 0x0018F62B File Offset: 0x0018D82B
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckIdeology("Scatter ancient landing"))
			{
				return;
			}
			this.count = 1;
			this.allowInWaterBiome = false;
			this.randomSize = Mathf.RoundToInt(Rand.ByCurve(GenStep_ScatterAncientLandingPad.SizeChanceCurve));
			base.Generate(map, parms);
		}

		// Token: 0x06004B35 RID: 19253 RVA: 0x0018F668 File Offset: 0x0018D868
		protected override bool CanScatterAt(IntVec3 loc, Map map)
		{
			if (!base.CanScatterAt(loc, map))
			{
				return false;
			}
			CellRect rect = CellRect.CenteredOn(loc, this.randomSize, this.randomSize);
			return this.CanPlaceAt(rect, map);
		}

		// Token: 0x06004B36 RID: 19254 RVA: 0x0018F6A4 File Offset: 0x0018D8A4
		private bool CanPlaceAt(CellRect rect, Map map)
		{
			foreach (IntVec3 c in rect.ExpandedBy(2))
			{
				if (!c.InBounds(map))
				{
					return false;
				}
				TerrainDef terrainDef = map.terrainGrid.TerrainAt(c);
				if (terrainDef.IsWater || terrainDef.IsRoad)
				{
					return false;
				}
				if (c.GetEdifice(map) != null)
				{
					return false;
				}
				if (!c.SupportsStructureType(map, TerrainAffordanceDefOf.Heavy))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004B37 RID: 19255 RVA: 0x0018F74C File Offset: 0x0018D94C
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			RimWorld.BaseGen.ResolveParams resolveParams = default(RimWorld.BaseGen.ResolveParams);
			resolveParams.filthDensity = new FloatRange?(new FloatRange(0.025f, 0.05f));
			resolveParams.filthDef = ThingDefOf.Filth_Ash;
			resolveParams.rect = CellRect.CenteredOn(loc, this.randomSize, this.randomSize);
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("filth", resolveParams, null);
			BaseGen.Generate();
			RimWorld.SketchGen.ResolveParams parms2 = default(RimWorld.SketchGen.ResolveParams);
			parms2.landingPadSize = new IntVec2?(new IntVec2(this.randomSize, this.randomSize));
			parms2.sketch = new Sketch();
			SketchGen.Generate(SketchResolverDefOf.AncientLandingPad, parms2).Spawn(map, loc - new IntVec3(this.randomSize / 2, 0, this.randomSize / 2), null, Sketch.SpawnPosType.Unchanged, Sketch.SpawnMode.Normal, false, false, null, false, false, null, null);
		}

		// Token: 0x04002D97 RID: 11671
		private static readonly SimpleCurve SizeChanceCurve = new SimpleCurve
		{
			{
				new CurvePoint(8f, 0f),
				true
			},
			{
				new CurvePoint(12f, 4f),
				true
			},
			{
				new CurvePoint(18f, 0f),
				true
			}
		};

		// Token: 0x04002D98 RID: 11672
		private const int Gap = 2;

		// Token: 0x04002D99 RID: 11673
		private int randomSize;
	}
}
