using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200028B RID: 651
	public class LayerSubMesh
	{
		// Token: 0x060010F5 RID: 4341 RVA: 0x000BCB9C File Offset: 0x000BAD9C
		public LayerSubMesh(Mesh mesh, Material material)
		{
			this.mesh = mesh;
			this.material = material;
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x000BCBEC File Offset: 0x000BADEC
		public void Clear(MeshParts parts)
		{
			if ((parts & MeshParts.Verts) != MeshParts.None)
			{
				this.verts.Clear();
			}
			if ((parts & MeshParts.Tris) != MeshParts.None)
			{
				this.tris.Clear();
			}
			if ((parts & MeshParts.Colors) != MeshParts.None)
			{
				this.colors.Clear();
			}
			if ((parts & MeshParts.UVs) != MeshParts.None)
			{
				this.uvs.Clear();
			}
			this.finalized = false;
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x000BCC40 File Offset: 0x000BAE40
		public void FinalizeMesh(MeshParts parts)
		{
			if (this.finalized)
			{
				Log.Warning("Finalizing mesh which is already finalized. Did you forget to call Clear()?", false);
			}
			if ((parts & MeshParts.Verts) != MeshParts.None || (parts & MeshParts.Tris) != MeshParts.None)
			{
				this.mesh.Clear();
			}
			if ((parts & MeshParts.Verts) != MeshParts.None)
			{
				if (this.verts.Count > 0)
				{
					this.mesh.SetVertices(this.verts);
				}
				else
				{
					Log.Error("Cannot cook Verts for " + this.material.ToString() + ": no ingredients data. If you want to not render this submesh, disable it.", false);
				}
			}
			if ((parts & MeshParts.Tris) != MeshParts.None)
			{
				if (this.tris.Count > 0)
				{
					this.mesh.SetTriangles(this.tris, 0);
				}
				else
				{
					Log.Error("Cannot cook Tris for " + this.material.ToString() + ": no ingredients data.", false);
				}
			}
			if ((parts & MeshParts.Colors) != MeshParts.None && this.colors.Count > 0)
			{
				this.mesh.SetColors(this.colors);
			}
			if ((parts & MeshParts.UVs) != MeshParts.None && this.uvs.Count > 0)
			{
				this.mesh.SetUVs(0, this.uvs);
			}
			this.finalized = true;
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x00012780 File Offset: 0x00010980
		public override string ToString()
		{
			return "LayerSubMesh(" + this.material.ToString() + ")";
		}

		// Token: 0x04000DC6 RID: 3526
		public bool finalized;

		// Token: 0x04000DC7 RID: 3527
		public bool disabled;

		// Token: 0x04000DC8 RID: 3528
		public Material material;

		// Token: 0x04000DC9 RID: 3529
		public Mesh mesh;

		// Token: 0x04000DCA RID: 3530
		public List<Vector3> verts = new List<Vector3>();

		// Token: 0x04000DCB RID: 3531
		public List<int> tris = new List<int>();

		// Token: 0x04000DCC RID: 3532
		public List<Color32> colors = new List<Color32>();

		// Token: 0x04000DCD RID: 3533
		public List<Vector3> uvs = new List<Vector3>();
	}
}
