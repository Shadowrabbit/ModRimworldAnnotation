using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001D8 RID: 472
	internal class SectionLayer_Snow : SectionLayer
	{
		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000D8E RID: 3470 RVA: 0x0004B9D5 File Offset: 0x00049BD5
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawSnow;
			}
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x0004B9DC File Offset: 0x00049BDC
		public SectionLayer_Snow(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Snow;
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x0004B9FC File Offset: 0x00049BFC
		private bool Filled(int index)
		{
			Building building = base.Map.edificeGrid[index];
			return building != null && building.def.Fillage == FillCategory.Full;
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x0004BA30 File Offset: 0x00049C30
		public override void Regenerate()
		{
			LayerSubMesh subMesh = base.GetSubMesh(MatBases.Snow);
			if (subMesh.mesh.vertexCount == 0)
			{
				SectionLayerGeometryMaker_Solid.MakeBaseGeometry(this.section, subMesh, AltitudeLayer.Terrain);
			}
			subMesh.Clear(MeshParts.Colors);
			float[] depthGridDirect_Unsafe = base.Map.snowGrid.DepthGridDirect_Unsafe;
			CellRect cellRect = this.section.CellRect;
			int num = base.Map.Size.z - 1;
			int num2 = base.Map.Size.x - 1;
			bool flag = false;
			CellIndices cellIndices = base.Map.cellIndices;
			for (int i = cellRect.minX; i <= cellRect.maxX; i++)
			{
				for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
				{
					float num3 = depthGridDirect_Unsafe[cellIndices.CellToIndex(i, j)];
					int num4 = cellIndices.CellToIndex(i, j - 1);
					float num5 = (j > 0) ? depthGridDirect_Unsafe[num4] : num3;
					num4 = cellIndices.CellToIndex(i - 1, j - 1);
					float num6 = (j > 0 && i > 0) ? depthGridDirect_Unsafe[num4] : num3;
					num4 = cellIndices.CellToIndex(i - 1, j);
					float num7 = (i > 0) ? depthGridDirect_Unsafe[num4] : num3;
					num4 = cellIndices.CellToIndex(i - 1, j + 1);
					float num8 = (j < num && i > 0) ? depthGridDirect_Unsafe[num4] : num3;
					num4 = cellIndices.CellToIndex(i, j + 1);
					float num9 = (j < num) ? depthGridDirect_Unsafe[num4] : num3;
					num4 = cellIndices.CellToIndex(i + 1, j + 1);
					float num10 = (j < num && i < num2) ? depthGridDirect_Unsafe[num4] : num3;
					num4 = cellIndices.CellToIndex(i + 1, j);
					float num11 = (i < num2) ? depthGridDirect_Unsafe[num4] : num3;
					num4 = cellIndices.CellToIndex(i + 1, j - 1);
					float num12 = (j > 0 && i < num2) ? depthGridDirect_Unsafe[num4] : num3;
					this.vertDepth[0] = (num5 + num6 + num7 + num3) / 4f;
					this.vertDepth[1] = (num7 + num3) / 2f;
					this.vertDepth[2] = (num7 + num8 + num9 + num3) / 4f;
					this.vertDepth[3] = (num9 + num3) / 2f;
					this.vertDepth[4] = (num9 + num10 + num11 + num3) / 4f;
					this.vertDepth[5] = (num11 + num3) / 2f;
					this.vertDepth[6] = (num11 + num12 + num5 + num3) / 4f;
					this.vertDepth[7] = (num5 + num3) / 2f;
					this.vertDepth[8] = num3;
					for (int k = 0; k < 9; k++)
					{
						if (this.vertDepth[k] > 0.01f)
						{
							flag = true;
						}
						subMesh.colors.Add(SectionLayer_Snow.SnowDepthColor(this.vertDepth[k]));
					}
				}
			}
			if (flag)
			{
				subMesh.disabled = false;
				subMesh.FinalizeMesh(MeshParts.Colors);
				return;
			}
			subMesh.disabled = true;
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x0004BD2D File Offset: 0x00049F2D
		private static Color32 SnowDepthColor(float snowDepth)
		{
			return Color32.Lerp(SectionLayer_Snow.ColorClear, SectionLayer_Snow.ColorWhite, snowDepth);
		}

		// Token: 0x04000B23 RID: 2851
		private float[] vertDepth = new float[9];

		// Token: 0x04000B24 RID: 2852
		private static readonly Color32 ColorClear = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);

		// Token: 0x04000B25 RID: 2853
		private static readonly Color32 ColorWhite = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
	}
}
