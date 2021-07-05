using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001CF RID: 463
	public abstract class SectionLayer
	{
		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000D5D RID: 3421 RVA: 0x00048412 File Offset: 0x00046612
		protected Map Map
		{
			get
			{
				return this.section.map;
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000D5E RID: 3422 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool Visible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x0004841F File Offset: 0x0004661F
		public SectionLayer(Section section)
		{
			this.section = section;
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x0004843C File Offset: 0x0004663C
		public LayerSubMesh GetSubMesh(Material material)
		{
			if (material == null)
			{
				return null;
			}
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				if (this.subMeshes[i].material == material)
				{
					return this.subMeshes[i];
				}
			}
			Mesh mesh = new Mesh();
			if (UnityData.isEditor)
			{
				mesh.name = string.Concat(new object[]
				{
					"SectionLayerSubMesh_",
					base.GetType().Name,
					"_",
					this.Map.Tile
				});
			}
			Bounds value = new Bounds(this.section.botLeft.ToVector3(), Vector3.zero);
			value.Encapsulate(this.section.botLeft.ToVector3() + new Vector3(17f, 0f, 0f));
			value.Encapsulate(this.section.botLeft.ToVector3() + new Vector3(17f, 0f, 17f));
			value.Encapsulate(this.section.botLeft.ToVector3() + new Vector3(0f, 0f, 17f));
			LayerSubMesh layerSubMesh = new LayerSubMesh(mesh, material, new Bounds?(value));
			this.subMeshes.Add(layerSubMesh);
			return layerSubMesh;
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x000485A8 File Offset: 0x000467A8
		protected void FinalizeMesh(MeshParts tags)
		{
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				if (this.subMeshes[i].verts.Count > 0)
				{
					this.subMeshes[i].FinalizeMesh(tags);
				}
			}
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x000485F8 File Offset: 0x000467F8
		public virtual void DrawLayer()
		{
			if (!this.Visible)
			{
				return;
			}
			int count = this.subMeshes.Count;
			for (int i = 0; i < count; i++)
			{
				LayerSubMesh layerSubMesh = this.subMeshes[i];
				if (layerSubMesh.finalized && !layerSubMesh.disabled)
				{
					Graphics.DrawMesh(layerSubMesh.mesh, Matrix4x4.identity, layerSubMesh.material, 0);
				}
			}
		}

		// Token: 0x06000D63 RID: 3427
		public abstract void Regenerate();

		// Token: 0x06000D64 RID: 3428 RVA: 0x0004865C File Offset: 0x0004685C
		protected void ClearSubMeshes(MeshParts parts)
		{
			foreach (LayerSubMesh layerSubMesh in this.subMeshes)
			{
				layerSubMesh.Clear(parts);
			}
		}

		// Token: 0x04000B07 RID: 2823
		protected Section section;

		// Token: 0x04000B08 RID: 2824
		public MapMeshFlag relevantChangeTypes;

		// Token: 0x04000B09 RID: 2825
		public List<LayerSubMesh> subMeshes = new List<LayerSubMesh>();
	}
}
