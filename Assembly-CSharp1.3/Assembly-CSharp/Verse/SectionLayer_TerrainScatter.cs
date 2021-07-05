using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001DB RID: 475
	public class SectionLayer_TerrainScatter : SectionLayer
	{
		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000D9E RID: 3486 RVA: 0x0004C340 File Offset: 0x0004A540
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawTerrain;
			}
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x0004C972 File Offset: 0x0004AB72
		public SectionLayer_TerrainScatter(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Terrain;
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x0004C990 File Offset: 0x0004AB90
		public override void Regenerate()
		{
			base.ClearSubMeshes(MeshParts.All);
			this.scats.RemoveAll((SectionLayer_TerrainScatter.Scatterable scat) => !scat.IsOnValidTerrain);
			int num = 0;
			TerrainDef[] topGrid = base.Map.terrainGrid.topGrid;
			CellRect cellRect = this.section.CellRect;
			CellIndices cellIndices = base.Map.cellIndices;
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					if (topGrid[cellIndices.CellToIndex(j, i)].scatterType != null)
					{
						num++;
					}
				}
			}
			num /= 40;
			int num2 = 0;
			while (this.scats.Count < num && num2 < 200)
			{
				num2++;
				IntVec3 randomCell = this.section.CellRect.RandomCell;
				string terrScatType = base.Map.terrainGrid.TerrainAt(randomCell).scatterType;
				ScatterableDef def2;
				if (terrScatType != null && !randomCell.Filled(base.Map) && (from def in DefDatabase<ScatterableDef>.AllDefs
				where def.scatterType == terrScatType
				select def).TryRandomElement(out def2))
				{
					Vector3 loc = new Vector3((float)randomCell.x + Rand.Value, (float)randomCell.y, (float)randomCell.z + Rand.Value);
					SectionLayer_TerrainScatter.Scatterable scatterable = new SectionLayer_TerrainScatter.Scatterable(def2, loc, base.Map);
					this.scats.Add(scatterable);
					scatterable.PrintOnto(this);
				}
			}
			for (int k = 0; k < this.scats.Count; k++)
			{
				this.scats[k].PrintOnto(this);
			}
			base.FinalizeMesh(MeshParts.All);
		}

		// Token: 0x04000B29 RID: 2857
		private List<SectionLayer_TerrainScatter.Scatterable> scats = new List<SectionLayer_TerrainScatter.Scatterable>();

		// Token: 0x02001973 RID: 6515
		private class Scatterable
		{
			// Token: 0x06009883 RID: 39043 RVA: 0x0035F344 File Offset: 0x0035D544
			public Scatterable(ScatterableDef def, Vector3 loc, Map map)
			{
				this.def = def;
				this.loc = loc;
				this.map = map;
				this.size = Rand.Range(def.minSize, def.maxSize);
				this.rotation = Rand.Range(0f, 360f);
			}

			// Token: 0x06009884 RID: 39044 RVA: 0x0035F398 File Offset: 0x0035D598
			public void PrintOnto(SectionLayer layer)
			{
				Material mat = this.def.mat;
				Vector2[] uvs;
				Color32 color;
				Graphic.TryGetTextureAtlasReplacementInfo(mat, TextureAtlasGroup.Terrain, false, false, out mat, out uvs, out color);
				Printer_Plane.PrintPlane(layer, this.loc, Vector2.one * this.size, mat, this.rotation, false, uvs, null, 0.01f, 0f);
			}

			// Token: 0x1700192A RID: 6442
			// (get) Token: 0x06009885 RID: 39045 RVA: 0x0035F3F4 File Offset: 0x0035D5F4
			public bool IsOnValidTerrain
			{
				get
				{
					IntVec3 c = this.loc.ToIntVec3();
					TerrainDef terrainDef = this.map.terrainGrid.TerrainAt(c);
					return this.def.scatterType == terrainDef.scatterType && !c.Filled(this.map);
				}
			}

			// Token: 0x040061AD RID: 25005
			private Map map;

			// Token: 0x040061AE RID: 25006
			public ScatterableDef def;

			// Token: 0x040061AF RID: 25007
			public Vector3 loc;

			// Token: 0x040061B0 RID: 25008
			public float size;

			// Token: 0x040061B1 RID: 25009
			public float rotation;
		}
	}
}
