using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001CA RID: 458
	public class LayerSubMesh
	{
		// Token: 0x06000D45 RID: 3397 RVA: 0x000478A0 File Offset: 0x00045AA0
		public LayerSubMesh(Mesh mesh, Material material, Bounds? bounds = null)
		{
			this.mesh = mesh;
			this.material = material;
			this.bounds = bounds;
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x000478F4 File Offset: 0x00045AF4
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

		// Token: 0x06000D47 RID: 3399 RVA: 0x00047948 File Offset: 0x00045B48
		public void FinalizeMesh(MeshParts parts)
		{
			if (this.finalized)
			{
				Log.Warning("Finalizing mesh which is already finalized. Did you forget to call Clear()?");
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
					Log.Error("Cannot cook Verts for " + this.material.ToString() + ": no ingredients data. If you want to not render this submesh, disable it.");
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
					Log.Error("Cannot cook Tris for " + this.material.ToString() + ": no ingredients data.");
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
			if (this.bounds != null)
			{
				this.mesh.bounds = this.bounds.Value;
			}
			this.finalized = true;
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x00047A7A File Offset: 0x00045C7A
		public override string ToString()
		{
			return "LayerSubMesh(" + this.material.ToString() + ")";
		}

		// Token: 0x04000AE7 RID: 2791
		public bool finalized;

		// Token: 0x04000AE8 RID: 2792
		public bool disabled;

		// Token: 0x04000AE9 RID: 2793
		private Bounds? bounds;

		// Token: 0x04000AEA RID: 2794
		public Material material;

		// Token: 0x04000AEB RID: 2795
		public Mesh mesh;

		// Token: 0x04000AEC RID: 2796
		public List<Vector3> verts = new List<Vector3>();

		// Token: 0x04000AED RID: 2797
		public List<int> tris = new List<int>();

		// Token: 0x04000AEE RID: 2798
		public List<Color32> colors = new List<Color32>();

		// Token: 0x04000AEF RID: 2799
		public List<Vector3> uvs = new List<Vector3>();
	}
}
