﻿using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001DA RID: 474
	internal class SectionLayer_Terrain : SectionLayer
	{
		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000D98 RID: 3480 RVA: 0x0004C340 File Offset: 0x0004A540
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawTerrain;
			}
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x0004C347 File Offset: 0x0004A547
		public SectionLayer_Terrain(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Terrain;
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x0004C358 File Offset: 0x0004A558
		public virtual Material GetMaterialFor(TerrainDef terrain)
		{
			return terrain.DrawMatSingle;
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x0004C360 File Offset: 0x0004A560
		public bool AllowRenderingFor(TerrainDef terrain)
		{
			return DebugViewSettings.drawTerrainWater || !terrain.HasTag("Water");
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x0004C37C File Offset: 0x0004A57C
		public override void Regenerate()
		{
			base.ClearSubMeshes(MeshParts.All);
			TerrainGrid terrainGrid = base.Map.terrainGrid;
			CellRect cellRect = this.section.CellRect;
			TerrainDef[] array = new TerrainDef[8];
			HashSet<TerrainDef> hashSet = new HashSet<TerrainDef>();
			bool[] array2 = new bool[8];
			foreach (IntVec3 intVec in cellRect)
			{
				hashSet.Clear();
				TerrainDef terrainDef = terrainGrid.TerrainAt(intVec);
				LayerSubMesh subMesh = base.GetSubMesh(this.GetMaterialFor(terrainDef));
				if (subMesh != null && this.AllowRenderingFor(terrainDef))
				{
					int count = subMesh.verts.Count;
					subMesh.verts.Add(new Vector3((float)intVec.x, 0f, (float)intVec.z));
					subMesh.verts.Add(new Vector3((float)intVec.x, 0f, (float)(intVec.z + 1)));
					subMesh.verts.Add(new Vector3((float)(intVec.x + 1), 0f, (float)(intVec.z + 1)));
					subMesh.verts.Add(new Vector3((float)(intVec.x + 1), 0f, (float)intVec.z));
					subMesh.colors.Add(SectionLayer_Terrain.ColorWhite);
					subMesh.colors.Add(SectionLayer_Terrain.ColorWhite);
					subMesh.colors.Add(SectionLayer_Terrain.ColorWhite);
					subMesh.colors.Add(SectionLayer_Terrain.ColorWhite);
					subMesh.tris.Add(count);
					subMesh.tris.Add(count + 1);
					subMesh.tris.Add(count + 2);
					subMesh.tris.Add(count);
					subMesh.tris.Add(count + 2);
					subMesh.tris.Add(count + 3);
				}
				for (int i = 0; i < 8; i++)
				{
					IntVec3 c = intVec + GenAdj.AdjacentCellsAroundBottom[i];
					if (!c.InBounds(base.Map))
					{
						array[i] = terrainDef;
					}
					else
					{
						TerrainDef terrainDef2 = terrainGrid.TerrainAt(c);
						Thing edifice = c.GetEdifice(base.Map);
						if (edifice != null && edifice.def.coversFloor)
						{
							terrainDef2 = TerrainDefOf.Underwall;
						}
						array[i] = terrainDef2;
						if (terrainDef2 != terrainDef && terrainDef2.edgeType != TerrainDef.TerrainEdgeType.Hard && terrainDef2.renderPrecedence >= terrainDef.renderPrecedence && !hashSet.Contains(terrainDef2))
						{
							hashSet.Add(terrainDef2);
						}
					}
				}
				foreach (TerrainDef terrainDef3 in hashSet)
				{
					LayerSubMesh subMesh2 = base.GetSubMesh(this.GetMaterialFor(terrainDef3));
					if (subMesh2 != null && this.AllowRenderingFor(terrainDef3))
					{
						int count = subMesh2.verts.Count;
						subMesh2.verts.Add(new Vector3((float)intVec.x + 0.5f, 0f, (float)intVec.z));
						subMesh2.verts.Add(new Vector3((float)intVec.x, 0f, (float)intVec.z));
						subMesh2.verts.Add(new Vector3((float)intVec.x, 0f, (float)intVec.z + 0.5f));
						subMesh2.verts.Add(new Vector3((float)intVec.x, 0f, (float)(intVec.z + 1)));
						subMesh2.verts.Add(new Vector3((float)intVec.x + 0.5f, 0f, (float)(intVec.z + 1)));
						subMesh2.verts.Add(new Vector3((float)(intVec.x + 1), 0f, (float)(intVec.z + 1)));
						subMesh2.verts.Add(new Vector3((float)(intVec.x + 1), 0f, (float)intVec.z + 0.5f));
						subMesh2.verts.Add(new Vector3((float)(intVec.x + 1), 0f, (float)intVec.z));
						subMesh2.verts.Add(new Vector3((float)intVec.x + 0.5f, 0f, (float)intVec.z + 0.5f));
						for (int j = 0; j < 8; j++)
						{
							array2[j] = false;
						}
						for (int k = 0; k < 8; k++)
						{
							if (k % 2 == 0)
							{
								if (array[k] == terrainDef3)
								{
									array2[(k - 1 + 8) % 8] = true;
									array2[k] = true;
									array2[(k + 1) % 8] = true;
								}
							}
							else if (array[k] == terrainDef3)
							{
								array2[k] = true;
							}
						}
						for (int l = 0; l < 8; l++)
						{
							if (array2[l])
							{
								subMesh2.colors.Add(SectionLayer_Terrain.ColorWhite);
							}
							else
							{
								subMesh2.colors.Add(SectionLayer_Terrain.ColorClear);
							}
						}
						subMesh2.colors.Add(SectionLayer_Terrain.ColorClear);
						for (int m = 0; m < 8; m++)
						{
							subMesh2.tris.Add(count + m);
							subMesh2.tris.Add(count + (m + 1) % 8);
							subMesh2.tris.Add(count + 8);
						}
					}
				}
			}
			base.FinalizeMesh(MeshParts.All);
		}

		// Token: 0x04000B27 RID: 2855
		private static readonly Color32 ColorWhite = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		// Token: 0x04000B28 RID: 2856
		private static readonly Color32 ColorClear = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
	}
}
