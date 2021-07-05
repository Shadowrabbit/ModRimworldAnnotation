using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000181 RID: 385
	public class CellBoolDrawer
	{
		// Token: 0x06000AFB RID: 2811 RVA: 0x0003B7E8 File Offset: 0x000399E8
		private CellBoolDrawer(int mapSizeX, int mapSizeZ, float opacity = 0.33f)
		{
			this.mapSizeX = mapSizeX;
			this.mapSizeZ = mapSizeZ;
			this.opacity = opacity;
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x0003B838 File Offset: 0x00039A38
		public CellBoolDrawer(ICellBoolGiver giver, int mapSizeX, int mapSizeZ, float opacity = 0.33f) : this(mapSizeX, mapSizeZ, opacity)
		{
			this.colorGetter = (() => giver.Color);
			this.extraColorGetter = new Func<int, Color>(giver.GetCellExtraColor);
			this.cellBoolGetter = new Func<int, bool>(giver.GetCellBool);
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x0003B89E File Offset: 0x00039A9E
		public CellBoolDrawer(ICellBoolGiver giver, int mapSizeX, int mapSizeZ, int renderQueue, float opacity = 0.33f) : this(giver, mapSizeX, mapSizeZ, opacity)
		{
			this.renderQueue = renderQueue;
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x0003B8B3 File Offset: 0x00039AB3
		public CellBoolDrawer(Func<int, bool> cellBoolGetter, Func<Color> colorGetter, Func<int, Color> extraColorGetter, int mapSizeX, int mapSizeZ, float opacity = 0.33f) : this(mapSizeX, mapSizeZ, opacity)
		{
			this.colorGetter = colorGetter;
			this.extraColorGetter = extraColorGetter;
			this.cellBoolGetter = cellBoolGetter;
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x0003B8D6 File Offset: 0x00039AD6
		public CellBoolDrawer(Func<int, bool> cellBoolGetter, Func<Color> colorGetter, Func<int, Color> extraColorGetter, int mapSizeX, int mapSizeZ, int renderQueue, float opacity = 0.33f) : this(cellBoolGetter, colorGetter, extraColorGetter, mapSizeX, mapSizeZ, opacity)
		{
			this.renderQueue = renderQueue;
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x0003B8EF File Offset: 0x00039AEF
		public void MarkForDraw()
		{
			this.wantDraw = true;
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x0003B8F8 File Offset: 0x00039AF8
		public void CellBoolDrawerUpdate()
		{
			if (this.wantDraw)
			{
				this.ActuallyDraw();
				this.wantDraw = false;
			}
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x0003B910 File Offset: 0x00039B10
		private void ActuallyDraw()
		{
			if (this.dirty)
			{
				this.RegenerateMesh();
			}
			for (int i = 0; i < this.meshes.Count; i++)
			{
				Graphics.DrawMesh(this.meshes[i], Vector3.zero, Quaternion.identity, this.material, 0);
			}
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x0003B963 File Offset: 0x00039B63
		public void SetDirty()
		{
			this.dirty = true;
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x0003B96C File Offset: 0x00039B6C
		public void RegenerateMesh()
		{
			for (int i = 0; i < this.meshes.Count; i++)
			{
				this.meshes[i].Clear();
			}
			int num = 0;
			int num2 = 0;
			if (this.meshes.Count < num + 1)
			{
				Mesh mesh = new Mesh();
				mesh.name = "CellBoolDrawer";
				this.meshes.Add(mesh);
			}
			Mesh mesh2 = this.meshes[num];
			CellRect cellRect = new CellRect(0, 0, this.mapSizeX, this.mapSizeZ);
			float y = AltitudeLayer.MapDataOverlay.AltitudeFor();
			bool careAboutVertexColors = false;
			for (int j = cellRect.minX; j <= cellRect.maxX; j++)
			{
				for (int k = cellRect.minZ; k <= cellRect.maxZ; k++)
				{
					int arg = CellIndicesUtility.CellToIndex(j, k, this.mapSizeX);
					if (this.cellBoolGetter(arg))
					{
						CellBoolDrawer.verts.Add(new Vector3((float)j, y, (float)k));
						CellBoolDrawer.verts.Add(new Vector3((float)j, y, (float)(k + 1)));
						CellBoolDrawer.verts.Add(new Vector3((float)(j + 1), y, (float)(k + 1)));
						CellBoolDrawer.verts.Add(new Vector3((float)(j + 1), y, (float)k));
						Color color = this.extraColorGetter(arg);
						CellBoolDrawer.colors.Add(color);
						CellBoolDrawer.colors.Add(color);
						CellBoolDrawer.colors.Add(color);
						CellBoolDrawer.colors.Add(color);
						if (color != Color.white)
						{
							careAboutVertexColors = true;
						}
						int count = CellBoolDrawer.verts.Count;
						CellBoolDrawer.tris.Add(count - 4);
						CellBoolDrawer.tris.Add(count - 3);
						CellBoolDrawer.tris.Add(count - 2);
						CellBoolDrawer.tris.Add(count - 4);
						CellBoolDrawer.tris.Add(count - 2);
						CellBoolDrawer.tris.Add(count - 1);
						num2++;
						if (num2 >= 16383)
						{
							this.FinalizeWorkingDataIntoMesh(mesh2);
							num++;
							if (this.meshes.Count < num + 1)
							{
								Mesh mesh3 = new Mesh();
								mesh3.name = "CellBoolDrawer";
								this.meshes.Add(mesh3);
							}
							mesh2 = this.meshes[num];
							num2 = 0;
						}
					}
				}
			}
			this.FinalizeWorkingDataIntoMesh(mesh2);
			this.CreateMaterialIfNeeded(careAboutVertexColors);
			this.dirty = false;
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x0003BBEC File Offset: 0x00039DEC
		private void FinalizeWorkingDataIntoMesh(Mesh mesh)
		{
			if (CellBoolDrawer.verts.Count > 0)
			{
				mesh.SetVertices(CellBoolDrawer.verts);
				CellBoolDrawer.verts.Clear();
				mesh.SetTriangles(CellBoolDrawer.tris, 0);
				CellBoolDrawer.tris.Clear();
				mesh.SetColors(CellBoolDrawer.colors);
				CellBoolDrawer.colors.Clear();
			}
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x0003BC48 File Offset: 0x00039E48
		private void CreateMaterialIfNeeded(bool careAboutVertexColors)
		{
			if (this.material == null || this.materialCaresAboutVertexColors != careAboutVertexColors)
			{
				Color color = this.colorGetter();
				this.material = SolidColorMaterials.SimpleSolidColorMaterial(new Color(color.r, color.g, color.b, this.opacity * color.a), careAboutVertexColors);
				this.materialCaresAboutVertexColors = careAboutVertexColors;
				this.material.renderQueue = this.renderQueue;
			}
		}

		// Token: 0x0400091A RID: 2330
		private bool wantDraw;

		// Token: 0x0400091B RID: 2331
		private Material material;

		// Token: 0x0400091C RID: 2332
		private bool materialCaresAboutVertexColors;

		// Token: 0x0400091D RID: 2333
		private bool dirty = true;

		// Token: 0x0400091E RID: 2334
		private List<Mesh> meshes = new List<Mesh>();

		// Token: 0x0400091F RID: 2335
		private int mapSizeX;

		// Token: 0x04000920 RID: 2336
		private int mapSizeZ;

		// Token: 0x04000921 RID: 2337
		private float opacity = 0.33f;

		// Token: 0x04000922 RID: 2338
		private int renderQueue = 3600;

		// Token: 0x04000923 RID: 2339
		private Func<Color> colorGetter;

		// Token: 0x04000924 RID: 2340
		private Func<int, Color> extraColorGetter;

		// Token: 0x04000925 RID: 2341
		private Func<int, bool> cellBoolGetter;

		// Token: 0x04000926 RID: 2342
		private static List<Vector3> verts = new List<Vector3>();

		// Token: 0x04000927 RID: 2343
		private static List<int> tris = new List<int>();

		// Token: 0x04000928 RID: 2344
		private static List<Color> colors = new List<Color>();

		// Token: 0x04000929 RID: 2345
		private const float DefaultOpacity = 0.33f;

		// Token: 0x0400092A RID: 2346
		private const int MaxCellsPerMesh = 16383;
	}
}
