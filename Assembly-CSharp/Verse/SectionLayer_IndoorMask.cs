using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000298 RID: 664
	internal class SectionLayer_IndoorMask : SectionLayer
	{
		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06001137 RID: 4407 RVA: 0x000128A6 File Offset: 0x00010AA6
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawShadows;
			}
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x0001291F File Offset: 0x00010B1F
		public SectionLayer_IndoorMask(Section section) : base(section)
		{
			this.relevantChangeTypes = (MapMeshFlag.FogOfWar | MapMeshFlag.Roofs);
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x000BFB64 File Offset: 0x000BDD64
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
			}
			return false;
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x000BFBE0 File Offset: 0x000BDDE0
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
