using System;
using RimWorld.SketchGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CA8 RID: 3240
	public class GenStep_ScatterRuinsSimple : GenStep_Scatterer
	{
		// Token: 0x17000D08 RID: 3336
		// (get) Token: 0x06004B8B RID: 19339 RVA: 0x0019184A File Offset: 0x0018FA4A
		public override int SeedPart
		{
			get
			{
				return 1348417666;
			}
		}

		// Token: 0x06004B8C RID: 19340 RVA: 0x00191851 File Offset: 0x0018FA51
		protected override bool TryFindScatterCell(Map map, out IntVec3 result)
		{
			this.randomSize = Mathf.RoundToInt(Rand.ByCurve(GenStep_ScatterRuinsSimple.RuinSizeChanceCurve));
			return base.TryFindScatterCell(map, out result);
		}

		// Token: 0x06004B8D RID: 19341 RVA: 0x00191870 File Offset: 0x0018FA70
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

		// Token: 0x06004B8E RID: 19342 RVA: 0x001918D0 File Offset: 0x0018FAD0
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

		// Token: 0x06004B8F RID: 19343 RVA: 0x00191968 File Offset: 0x0018FB68
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

		// Token: 0x04002DBE RID: 11710
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

		// Token: 0x04002DBF RID: 11711
		private int randomSize;
	}
}
