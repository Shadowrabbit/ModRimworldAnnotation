using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002A9 RID: 681
	public static class SectionLayerGeometryMaker_Solid
	{
		// Token: 0x06001173 RID: 4467 RVA: 0x000C2560 File Offset: 0x000C0760
		public static void MakeBaseGeometry(Section section, LayerSubMesh sm, AltitudeLayer altitudeLayer)
		{
			sm.Clear(MeshParts.Verts | MeshParts.Tris);
			CellRect cellRect = new CellRect(section.botLeft.x, section.botLeft.z, 17, 17);
			cellRect.ClipInsideMap(section.map);
			float y = altitudeLayer.AltitudeFor();
			sm.verts.Capacity = cellRect.Area * 9;
			for (int i = cellRect.minX; i <= cellRect.maxX; i++)
			{
				for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
				{
					sm.verts.Add(new Vector3((float)i, y, (float)j));
					sm.verts.Add(new Vector3((float)i, y, (float)j + 0.5f));
					sm.verts.Add(new Vector3((float)i, y, (float)(j + 1)));
					sm.verts.Add(new Vector3((float)i + 0.5f, y, (float)(j + 1)));
					sm.verts.Add(new Vector3((float)(i + 1), y, (float)(j + 1)));
					sm.verts.Add(new Vector3((float)(i + 1), y, (float)j + 0.5f));
					sm.verts.Add(new Vector3((float)(i + 1), y, (float)j));
					sm.verts.Add(new Vector3((float)i + 0.5f, y, (float)j));
					sm.verts.Add(new Vector3((float)i + 0.5f, y, (float)j + 0.5f));
				}
			}
			int num = cellRect.Area * 8 * 3;
			sm.tris.Capacity = num;
			int num2 = 0;
			while (sm.tris.Count < num)
			{
				sm.tris.Add(num2 + 7);
				sm.tris.Add(num2);
				sm.tris.Add(num2 + 1);
				sm.tris.Add(num2 + 1);
				sm.tris.Add(num2 + 2);
				sm.tris.Add(num2 + 3);
				sm.tris.Add(num2 + 3);
				sm.tris.Add(num2 + 4);
				sm.tris.Add(num2 + 5);
				sm.tris.Add(num2 + 5);
				sm.tris.Add(num2 + 6);
				sm.tris.Add(num2 + 7);
				sm.tris.Add(num2 + 7);
				sm.tris.Add(num2 + 1);
				sm.tris.Add(num2 + 8);
				sm.tris.Add(num2 + 1);
				sm.tris.Add(num2 + 3);
				sm.tris.Add(num2 + 8);
				sm.tris.Add(num2 + 3);
				sm.tris.Add(num2 + 5);
				sm.tris.Add(num2 + 8);
				sm.tris.Add(num2 + 5);
				sm.tris.Add(num2 + 7);
				sm.tris.Add(num2 + 8);
				num2 += 9;
			}
			sm.FinalizeMesh(MeshParts.Verts | MeshParts.Tris);
		}
	}
}
