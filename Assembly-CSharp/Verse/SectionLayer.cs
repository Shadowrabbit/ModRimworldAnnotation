using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000291 RID: 657
	public abstract class SectionLayer
	{
		// Token: 0x1700032B RID: 811
		// (get) Token: 0x0600110F RID: 4367 RVA: 0x000127E3 File Offset: 0x000109E3
		protected Map Map
		{
			get
			{
				return this.section.map;
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06001110 RID: 4368 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool Visible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x000127F0 File Offset: 0x000109F0
		public SectionLayer(Section section)
		{
			this.section = section;
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x000BD694 File Offset: 0x000BB894
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
			LayerSubMesh layerSubMesh = new LayerSubMesh(mesh, material);
			this.subMeshes.Add(layerSubMesh);
			return layerSubMesh;
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x000BD74C File Offset: 0x000BB94C
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

		// Token: 0x06001114 RID: 4372 RVA: 0x000BD79C File Offset: 0x000BB99C
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
					Graphics.DrawMesh(layerSubMesh.mesh, Vector3.zero, Quaternion.identity, layerSubMesh.material, 0);
				}
			}
		}

		// Token: 0x06001115 RID: 4373
		public abstract void Regenerate();

		// Token: 0x06001116 RID: 4374 RVA: 0x000BD804 File Offset: 0x000BBA04
		protected void ClearSubMeshes(MeshParts parts)
		{
			foreach (LayerSubMesh layerSubMesh in this.subMeshes)
			{
				layerSubMesh.Clear(parts);
			}
		}

		// Token: 0x04000DE5 RID: 3557
		protected Section section;

		// Token: 0x04000DE6 RID: 3558
		public MapMeshFlag relevantChangeTypes;

		// Token: 0x04000DE7 RID: 3559
		public List<LayerSubMesh> subMeshes = new List<LayerSubMesh>();
	}
}
