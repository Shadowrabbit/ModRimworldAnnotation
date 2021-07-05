using System;
using RimWorld.SketchGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012A7 RID: 4775
	public class GenStep_ScatterRuinsSimple : GenStep_Scatterer
	{
		// Token: 0x17001000 RID: 4096
		// (get) Token: 0x060067BF RID: 26559 RVA: 0x00046BA4 File Offset: 0x00044DA4
		public override int SeedPart
		{
			get
			{
				return 1348417666;
			}
		}

		// Token: 0x060067C0 RID: 26560 RVA: 0x00046BAB File Offset: 0x00044DAB
		protected override bool TryFindScatterCell(Map map, out IntVec3 result)
		{
			this.randomSize = Mathf.RoundToInt(Rand.ByCurve(GenStep_ScatterRuinsSimple.RuinSizeChanceCurve));
			return base.TryFindScatterCell(map, out result);
		}

		// Token: 0x060067C1 RID: 26561 RVA: 0x001FFA68 File Offset: 0x001FDC68
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			if (!c.SupportsStructureType(map, TerrainAffordanceDefOf.Heavy))
			{
				return false;
			}
			CellRect rect = new CellRect(c.x, c.z, this.randomSize, this.randomSize).ClipInsideMap(map);
			return this.CanPlaceAncientBuildingInRange(rect, map);
		}

		// Token: 0x060067C2 RID: 26562 RVA: 0x001FFAC8 File Offset: 0x001FDCC8
		protected bool CanPlaceAncientBuildingInRange(CellRect rect, Map map)
		{
			foreach (IntVec3 c in rect.Cells)
			{
				if (c.InBounds(map))
				{
					TerrainDef terrainDef = map.terrainGrid.TerrainAt(c);
					if (terrainDef.HasTag("River") || terrainDef.HasTag("Road"))
					{
						return false;
					}
					if (!GenConstruct.CanBuildOnTerrain(ThingDefOf.Wall, c, map, Rot4.North, null, null))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060067C3 RID: 26563 RVA: 0x001FFB60 File Offset: 0x001FDD60
		protected override void ScatterAt(IntVec3 c, Map map, GenStepParams parms, int stackCount = 1)
		{
			CellRect rect = new CellRect(c.x, c.z, this.randomSize, this.randomSize).ClipInsideMap(map);
			if (this.CanPlaceAncientBuildingInRange(rect, map))
			{
				ResolveParams parms2 = default(ResolveParams);
				parms2.sketch = new Sketch();
				parms2.monumentSize = new IntVec2?(new IntVec2(rect.Width, rect.Height));
				SketchGen.Generate(SketchResolverDefOf.MonumentRuin, parms2).Spawn(map, rect.CenterCell, null, Sketch.SpawnPosType.Unchanged, Sketch.SpawnMode.Normal, false, false, null, false, false, delegate(SketchEntity entity, IntVec3 cell)
				{
					bool result = false;
					foreach (IntVec3 b in entity.OccupiedRect.AdjacentCells)
					{
						IntVec3 c2 = cell + b;
						if (c2.InBounds(map))
						{
							Building edifice = c2.GetEdifice(map);
							if (edifice == null || !edifice.def.building.isNaturalRock)
							{
								result = true;
								break;
							}
						}
					}
					return result;
				}, null);
			}
		}

		// Token: 0x04004514 RID: 17684
		private static readonly SimpleCurve RuinSizeChanceCurve = new SimpleCurve
		{
			{
				new CurvePoint(6f, 0f),
				true
			},
			{
				new CurvePoint(6.001f, 4f),
				true
			},
			{
				new CurvePoint(10f, 1f),
				true
			},
			{
				new CurvePoint(30f, 0f),
				true
			}
		};

		// Token: 0x04004515 RID: 17685
		private int randomSize;
	}
}
