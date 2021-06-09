using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200029D RID: 669
	public class SectionLayer_TerrainScatter : SectionLayer
	{
		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06001151 RID: 4433 RVA: 0x000129C8 File Offset: 0x00010BC8
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawTerrain;
			}
		}

		// Token: 0x06001152 RID: 4434 RVA: 0x00012A3B File Offset: 0x00010C3B
		public SectionLayer_TerrainScatter(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Terrain;
		}

		// Token: 0x06001153 RID: 4435 RVA: 0x000C15B0 File Offset: 0x000BF7B0
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

		// Token: 0x04000E0A RID: 3594
		private List<SectionLayer_TerrainScatter.Scatterable> scats = new List<SectionLayer_TerrainScatter.Scatterable>();

		// Token: 0x0200029E RID: 670
		private class Scatterable
		{
			// Token: 0x06001154 RID: 4436 RVA: 0x000C1790 File Offset: 0x000BF990
			public Scatterable(ScatterableDef def, Vector3 loc, Map map)
			{
				this.def = def;
				this.loc = loc;
				this.map = map;
				this.size = Rand.Range(def.minSize, def.maxSize);
				this.rotation = Rand.Range(0f, 360f);
			}

			// Token: 0x06001155 RID: 4437 RVA: 0x000C17E4 File Offset: 0x000BF9E4
			public void PrintOnto(SectionLayer layer)
			{
				Printer_Plane.PrintPlane(layer, this.loc, Vector2.one * this.size, this.def.mat, this.rotation, false, null, null, 0.01f, 0f);
			}

			// Token: 0x17000335 RID: 821
			// (get) Token: 0x06001156 RID: 4438 RVA: 0x000C182C File Offset: 0x000BFA2C
			public bool IsOnValidTerrain
			{
				get
				{
					IntVec3 c = this.loc.ToIntVec3();
					TerrainDef terrainDef = this.map.terrainGrid.TerrainAt(c);
					return this.def.scatterType == terrainDef.scatterType && !c.Filled(this.map);
				}
			}

			// Token: 0x04000E0B RID: 3595
			private Map map;

			// Token: 0x04000E0C RID: 3596
			public ScatterableDef def;

			// Token: 0x04000E0D RID: 3597
			public Vector3 loc;

			// Token: 0x04000E0E RID: 3598
			public float size;

			// Token: 0x04000E0F RID: 3599
			public float rotation;
		}
	}
}
