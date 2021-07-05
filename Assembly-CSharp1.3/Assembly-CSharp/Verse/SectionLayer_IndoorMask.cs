using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001D6 RID: 470
	internal class SectionLayer_IndoorMask : SectionLayer
	{
		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000D84 RID: 3460 RVA: 0x0004A158 File Offset: 0x00048358
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawShadows;
			}
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x0004ADF6 File Offset: 0x00048FF6
		public SectionLayer_IndoorMask(Section section) : base(section)
		{
			this.relevantChangeTypes = (MapMeshFlag.FogOfWar | MapMeshFlag.Roofs);
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x0004AE08 File Offset: 0x00049008
		private bool HideRainPrimary(IntVec3 c)
		{
			if (base.Map.fogGrid.IsFogged(c))
			{
				return false;
			}
			if (c.Roofed(base.Map))
			{
				Building edifice = c.GetEdifice(base.Map);
				if (edifice == null)
				{
					return true;
				}
				if (edifice.def.Fillage != FillCategory.Full)
				{
					return true;
				}
				if (edifice.def.size.x > 1 || edifice.def.size.z > 1)
				{
					return true;
				}
				if (edifice.def.holdsRoof)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x0004AE94 File Offset: 0x00049094
		public override void Regenerate()
		{
			if (!MatBases.SunShadow.shader.isSupported)
			{
				return;
			}
			LayerSubMesh subMesh = base.GetSubMesh(MatBases.IndoorMask);
			subMesh.Clear(MeshParts.All);
			Building[] innerArray = base.Map.edificeGrid.InnerArray;
			CellRect cellRect = new CellRect(this.section.botLeft.x, this.section.botLeft.z, 17, 17);
			cellRect.ClipInsideMap(base.Map);
			subMesh.verts.Capacity = cellRect.Area * 2;
			subMesh.tris.Capacity = cellRect.Area * 4;
			float y = AltitudeLayer.MetaOverlays.AltitudeFor();
			CellIndices cellIndices = base.Map.cellIndices;
			for (int i = cellRect.minX; i <= cellRect.maxX; i++)
			{
				int j = cellRect.minZ;
				while (j <= cellRect.maxZ)
				{
					IntVec3 intVec = new IntVec3(i, 0, j);
					if (this.HideRainPrimary(intVec))
					{
						goto IL_145;
					}
					bool flag = intVec.Roofed(base.Map);
					bool flag2 = false;
					if (flag)
					{
						for (int k = 0; k < 8; k++)
						{
							IntVec3 c = intVec + GenAdj.AdjacentCells[k];
							if (c.InBounds(base.Map) && this.HideRainPrimary(c))
							{
								flag2 = true;
								break;
							}
						}
					}
					if (flag && flag2)
					{
						goto IL_145;
					}
					IL_268:
					j++;
					continue;
					IL_145:
					Thing thing = innerArray[cellIndices.CellToIndex(i, j)];
					float num;
					if (thing != null && (thing.def.passability == Traversability.Impassable || thing.def.IsDoor))
					{
						num = 0f;
					}
					else
					{
						num = 0.16f;
					}
					subMesh.verts.Add(new Vector3((float)i - num, y, (float)j - num));
					subMesh.verts.Add(new Vector3((float)i - num, y, (float)(j + 1) + num));
					subMesh.verts.Add(new Vector3((float)(i + 1) + num, y, (float)(j + 1) + num));
					subMesh.verts.Add(new Vector3((float)(i + 1) + num, y, (float)j - num));
					int count = subMesh.verts.Count;
					subMesh.tris.Add(count - 4);
					subMesh.tris.Add(count - 3);
					subMesh.tris.Add(count - 2);
					subMesh.tris.Add(count - 4);
					subMesh.tris.Add(count - 2);
					subMesh.tris.Add(count - 1);
					goto IL_268;
				}
			}
			if (subMesh.verts.Count > 0)
			{
				subMesh.FinalizeMesh(MeshParts.Verts | MeshParts.Tris);
			}
		}
	}
}
