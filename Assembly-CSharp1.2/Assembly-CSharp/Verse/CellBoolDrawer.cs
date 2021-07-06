using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200023D RID: 573
	public class CellBoolDrawer
	{
		// Token: 0x06000EB0 RID: 3760 RVA: 0x000B38EC File Offset: 0x000B1AEC
		private CellBoolDrawer(int mapSizeX, int mapSizeZ, float opacity = 0.33f)
		{
			this.mapSizeX = mapSizeX;
			this.mapSizeZ = mapSizeZ;
			this.opacity = opacity;
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x000B393C File Offset: 0x000B1B3C
		public CellBoolDrawer(ICellBoolGiver giver, int mapSizeX, int mapSizeZ, float opacity = 0.33f) : this(mapSizeX, mapSizeZ, opacity)
		{
			this.colorGetter = (() => giver.Color);
			this.extraColorGetter = new Func<int, Color>(giver.GetCellExtraColor);
			this.cellBoolGetter = new Func<int, bool>(giver.GetCellBool);
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x000110CC File Offset: 0x0000F2CC
		public CellBoolDrawer(ICellBoolGiver giver, int mapSizeX, int mapSizeZ, int renderQueue, float opacity = 0.33f) : this(giver, mapSizeX, mapSizeZ, opacity)
		{
			this.renderQueue = renderQueue;
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x000110E1 File Offset: 0x0000F2E1
		public CellBoolDrawer(Func<int, bool> cellBoolGetter, Func<Color> colorGetter, Func<int, Color> extraColorGetter, int mapSizeX, int mapSizeZ, float opacity = 0.33f) : this(mapSizeX, mapSizeZ, opacity)
		{
			this.colorGetter = colorGetter;
			this.extraColorGetter = extraColorGetter;
			this.cellBoolGetter = cellBoolGetter;
		}

		// Token: 0x06000EB4 RID: 3764 RVA: 0x00011104 File Offset: 0x0000F304
		public CellBoolDrawer(Func<int, bool> cellBoolGetter, Func<Color> colorGetter, Func<int, Color> extraColorGetter, int mapSizeX, int mapSizeZ, int renderQueue, float opacity = 0.33f) : this(cellBoolGetter, colorGetter, extraColorGetter, mapSizeX, mapSizeZ, opacity)
		{
			this.renderQueue = renderQueue;
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x0001111D File Offset: 0x0000F31D
		public void MarkForDraw()
		{
			this.wantDraw = true;
		}

		// Token: 0x06000EB6 RID: 3766 RVA: 0x00011126 File Offset: 0x0000F326
		public void CellBoolDrawerUpdate()
		{
			if (this.wantDraw)
			{
				this.ActuallyDraw();
				this.wantDraw = false;
			}
		}

		// Token: 0x06000EB7 RID: 3767 RVA: 0x000B39A4 File Offset: 0x000B1BA4
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

		// Token: 0x06000EB8 RID: 3768 RVA: 0x0001113D File Offset: 0x0000F33D
		public void SetDirty()
		{
			this.dirty = true;
		}

		// Token: 0x06000EB9 RID: 3769 RVA: 0x000B39F8 File Offset: 0x000B1BF8
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

		// Token: 0x06000EBA RID: 3770 RVA: 0x000B3C78 File Offset: 0x000B1E78
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

		// Token: 0x06000EBB RID: 3771 RVA: 0x000B3CD4 File Offset: 0x000B1ED4
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

		// Token: 0x04000C12 RID: 3090
		private bool wantDraw;

		// Token: 0x04000C13 RID: 3091
		private Material material;

		// Token: 0x04000C14 RID: 3092
		private bool materialCaresAboutVertexColors;

		// Token: 0x04000C15 RID: 3093
		private bool dirty = true;

		// Token: 0x04000C16 RID: 3094
		private List<Mesh> meshes = new List<Mesh>();

		// Token: 0x04000C17 RID: 3095
		private int mapSizeX;

		// Token: 0x04000C18 RID: 3096
		private int mapSizeZ;

		// Token: 0x04000C19 RID: 3097
		private float opacity = 0.33f;

		// Token: 0x04000C1A RID: 3098
		private int renderQueue = 3600;

		// Token: 0x04000C1B RID: 3099
		private Func<Color> colorGetter;

		// Token: 0x04000C1C RID: 3100
		private Func<int, Color> extraColorGetter;

		// Token: 0x04000C1D RID: 3101
		private Func<int, bool> cellBoolGetter;

		// Token: 0x04000C1E RID: 3102
		private static List<Vector3> verts = new List<Vector3>();

		// Token: 0x04000C1F RID: 3103
		private static List<int> tris = new List<int>();

		// Token: 0x04000C20 RID: 3104
		private static List<Color> colors = new List<Color>();

		// Token: 0x04000C21 RID: 3105
		private const float DefaultOpacity = 0.33f;

		// Token: 0x04000C22 RID: 3106
		private const int MaxCellsPerMesh = 16383;
	}
}
