using System;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001D7 RID: 471
	public class SectionLayer_LightingOverlay : SectionLayer
	{
		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000D88 RID: 3464 RVA: 0x0004B144 File Offset: 0x00049344
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawLightingOverlay;
			}
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x0004B14B File Offset: 0x0004934B
		public SectionLayer_LightingOverlay(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.GroundGlow;
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x0004B15C File Offset: 0x0004935C
		public string GlowReportAt(IntVec3 c)
		{
			Color32[] colors = base.GetSubMesh(MatBases.LightOverlay).mesh.colors32;
			int num;
			int num2;
			int num3;
			int num4;
			int num5;
			this.CalculateVertexIndices(c.x, c.z, out num, out num2, out num3, out num4, out num5);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("BL=" + colors[num]);
			stringBuilder.Append("\nTL=" + colors[num2]);
			stringBuilder.Append("\nTR=" + colors[num3]);
			stringBuilder.Append("\nBR=" + colors[num4]);
			stringBuilder.Append("\nCenter=" + colors[num5]);
			return stringBuilder.ToString();
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x0004B238 File Offset: 0x00049438
		public override void Regenerate()
		{
			LayerSubMesh subMesh = base.GetSubMesh(MatBases.LightOverlay);
			if (subMesh.verts.Count == 0)
			{
				this.MakeBaseGeometry(subMesh);
			}
			Color32[] array = new Color32[subMesh.verts.Count];
			int maxX = this.sectRect.maxX;
			int maxZ = this.sectRect.maxZ;
			int width = this.sectRect.Width;
			Map map = base.Map;
			int x = map.Size.x;
			Thing[] innerArray = map.edificeGrid.InnerArray;
			Thing[] array2 = innerArray;
			int num = array2.Length;
			RoofGrid roofGrid = map.roofGrid;
			CellIndices cellIndices = map.cellIndices;
			int num2;
			int num3;
			int num4;
			int num5;
			int num6;
			this.CalculateVertexIndices(this.sectRect.minX, this.sectRect.minZ, out num2, out num3, out num4, out num5, out num6);
			int num7 = cellIndices.CellToIndex(new IntVec3(this.sectRect.minX, 0, this.sectRect.minZ));
			int[] array3 = new int[4];
			array3[0] = -map.Size.x - 1;
			array3[1] = -map.Size.x;
			array3[2] = -1;
			int[] array4 = array3;
			int[] array5 = new int[4];
			array5[0] = -1;
			array5[1] = -1;
			int[] array6 = array5;
			for (int i = this.sectRect.minZ; i <= maxZ + 1; i++)
			{
				int num8 = num7 / x;
				int j = this.sectRect.minX;
				while (j <= maxX + 1)
				{
					ColorInt colorInt = new ColorInt(0, 0, 0, 0);
					int num9 = 0;
					bool flag = false;
					for (int k = 0; k < 4; k++)
					{
						int num10 = num7 + array4[k];
						if (num10 >= 0 && num10 < num && num10 / x == num8 + array6[k])
						{
							Thing thing = array2[num10];
							RoofDef roofDef = roofGrid.RoofAt(num10);
							if (roofDef != null && (roofDef.isThickRoof || thing == null || !thing.def.holdsRoof || thing.def.altitudeLayer == AltitudeLayer.DoorMoveable))
							{
								flag = true;
							}
							if (thing == null || !thing.def.blockLight)
							{
								colorInt += this.glowGrid[num10];
								num9++;
							}
						}
					}
					if (num9 > 0)
					{
						array[num2] = (colorInt / num9).ToColor32;
					}
					else
					{
						array[num2] = new Color32(0, 0, 0, 0);
					}
					if (flag && array[num2].a < 100)
					{
						array[num2].a = 100;
					}
					j++;
					num2++;
					num7++;
				}
				int num11 = maxX + 2 - this.sectRect.minX;
				num2 -= num11;
				num7 -= num11;
				num2 += width + 1;
				num7 += map.Size.x;
			}
			int num12;
			int num13;
			int num14;
			int num15;
			int num16;
			this.CalculateVertexIndices(this.sectRect.minX, this.sectRect.minZ, out num12, out num13, out num14, out num15, out num16);
			int num17 = cellIndices.CellToIndex(this.sectRect.minX, this.sectRect.minZ);
			for (int l = this.sectRect.minZ; l <= maxZ; l++)
			{
				int m = this.sectRect.minX;
				while (m <= maxX)
				{
					ColorInt colorInt2 = default(ColorInt) + array[num12];
					colorInt2 += array[num12 + 1];
					colorInt2 += array[num12 + width + 1];
					colorInt2 += array[num12 + width + 2];
					array[num16] = new Color32((byte)(colorInt2.r / 4), (byte)(colorInt2.g / 4), (byte)(colorInt2.b / 4), (byte)(colorInt2.a / 4));
					if (array[num16].a < 100 && roofGrid.Roofed(num17))
					{
						Thing thing2 = array2[num17];
						if (thing2 == null || !thing2.def.holdsRoof)
						{
							array[num16].a = 100;
						}
					}
					m++;
					num12++;
					num16++;
					num17++;
				}
				num12++;
				num17 -= width;
				num17 += map.Size.x;
			}
			subMesh.mesh.colors32 = array;
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x0004B6A0 File Offset: 0x000498A0
		private void MakeBaseGeometry(LayerSubMesh sm)
		{
			this.glowGrid = base.Map.glowGrid.glowGrid;
			this.sectRect = new CellRect(this.section.botLeft.x, this.section.botLeft.z, 17, 17);
			this.sectRect.ClipInsideMap(base.Map);
			int capacity = (this.sectRect.Width + 1) * (this.sectRect.Height + 1) + this.sectRect.Area;
			float y = AltitudeLayer.LightingOverlay.AltitudeFor();
			sm.verts.Capacity = capacity;
			for (int i = this.sectRect.minZ; i <= this.sectRect.maxZ + 1; i++)
			{
				for (int j = this.sectRect.minX; j <= this.sectRect.maxX + 1; j++)
				{
					sm.verts.Add(new Vector3((float)j, y, (float)i));
				}
			}
			this.firstCenterInd = sm.verts.Count;
			for (int k = this.sectRect.minZ; k <= this.sectRect.maxZ; k++)
			{
				for (int l = this.sectRect.minX; l <= this.sectRect.maxX; l++)
				{
					sm.verts.Add(new Vector3((float)l + 0.5f, y, (float)k + 0.5f));
				}
			}
			sm.tris.Capacity = this.sectRect.Area * 4 * 3;
			for (int m = this.sectRect.minZ; m <= this.sectRect.maxZ; m++)
			{
				for (int n = this.sectRect.minX; n <= this.sectRect.maxX; n++)
				{
					int item;
					int item2;
					int item3;
					int item4;
					int item5;
					this.CalculateVertexIndices(n, m, out item, out item2, out item3, out item4, out item5);
					sm.tris.Add(item);
					sm.tris.Add(item5);
					sm.tris.Add(item4);
					sm.tris.Add(item);
					sm.tris.Add(item2);
					sm.tris.Add(item5);
					sm.tris.Add(item2);
					sm.tris.Add(item3);
					sm.tris.Add(item5);
					sm.tris.Add(item3);
					sm.tris.Add(item4);
					sm.tris.Add(item5);
				}
			}
			sm.FinalizeMesh(MeshParts.Verts | MeshParts.Tris);
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x0004B93C File Offset: 0x00049B3C
		private void CalculateVertexIndices(int worldX, int worldZ, out int botLeft, out int topLeft, out int topRight, out int botRight, out int center)
		{
			int num = worldX - this.sectRect.minX;
			int num2 = worldZ - this.sectRect.minZ;
			botLeft = num2 * (this.sectRect.Width + 1) + num;
			topLeft = (num2 + 1) * (this.sectRect.Width + 1) + num;
			topRight = (num2 + 1) * (this.sectRect.Width + 1) + (num + 1);
			botRight = num2 * (this.sectRect.Width + 1) + (num + 1);
			center = this.firstCenterInd + (num2 * this.sectRect.Width + num);
		}

		// Token: 0x04000B1F RID: 2847
		private Color32[] glowGrid;

		// Token: 0x04000B20 RID: 2848
		private int firstCenterInd;

		// Token: 0x04000B21 RID: 2849
		private CellRect sectRect;

		// Token: 0x04000B22 RID: 2850
		private const byte RoofedAreaMinSkyCover = 100;
	}
}
