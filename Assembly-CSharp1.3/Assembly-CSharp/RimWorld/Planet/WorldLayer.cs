using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001759 RID: 5977
	[StaticConstructorOnStartup]
	public class WorldLayer
	{
		// Token: 0x17001678 RID: 5752
		// (get) Token: 0x060089F3 RID: 35315 RVA: 0x00318BE3 File Offset: 0x00316DE3
		public virtual bool ShouldRegenerate
		{
			get
			{
				return this.dirty;
			}
		}

		// Token: 0x17001679 RID: 5753
		// (get) Token: 0x060089F4 RID: 35316 RVA: 0x00318BEB File Offset: 0x00316DEB
		protected virtual int Layer
		{
			get
			{
				return WorldCameraManager.WorldLayer;
			}
		}

		// Token: 0x1700167A RID: 5754
		// (get) Token: 0x060089F5 RID: 35317 RVA: 0x00318BF2 File Offset: 0x00316DF2
		protected virtual Quaternion Rotation
		{
			get
			{
				return Quaternion.identity;
			}
		}

		// Token: 0x1700167B RID: 5755
		// (get) Token: 0x060089F6 RID: 35318 RVA: 0x0001F15E File Offset: 0x0001D35E
		protected virtual float Alpha
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x1700167C RID: 5756
		// (get) Token: 0x060089F7 RID: 35319 RVA: 0x00318BE3 File Offset: 0x00316DE3
		public bool Dirty
		{
			get
			{
				return this.dirty;
			}
		}

		// Token: 0x060089F8 RID: 35320 RVA: 0x00318BFC File Offset: 0x00316DFC
		protected LayerSubMesh GetSubMesh(Material material)
		{
			int num;
			return this.GetSubMesh(material, out num);
		}

		// Token: 0x060089F9 RID: 35321 RVA: 0x00318C14 File Offset: 0x00316E14
		protected LayerSubMesh GetSubMesh(Material material, out int subMeshIndex)
		{
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				LayerSubMesh layerSubMesh = this.subMeshes[i];
				if (layerSubMesh.material == material && layerSubMesh.verts.Count < 40000)
				{
					subMeshIndex = i;
					return layerSubMesh;
				}
			}
			Mesh mesh = new Mesh();
			if (UnityData.isEditor)
			{
				mesh.name = "WorldLayerSubMesh_" + base.GetType().Name + "_" + Find.World.info.seedString;
			}
			LayerSubMesh layerSubMesh2 = new LayerSubMesh(mesh, material, null);
			subMeshIndex = this.subMeshes.Count;
			this.subMeshes.Add(layerSubMesh2);
			return layerSubMesh2;
		}

		// Token: 0x060089FA RID: 35322 RVA: 0x00318CD4 File Offset: 0x00316ED4
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

		// Token: 0x060089FB RID: 35323 RVA: 0x00318D22 File Offset: 0x00316F22
		public void RegenerateNow()
		{
			this.dirty = false;
			this.Regenerate().ExecuteEnumerable();
		}

		// Token: 0x060089FC RID: 35324 RVA: 0x00318D38 File Offset: 0x00316F38
		public void Render()
		{
			if (this.ShouldRegenerate)
			{
				this.RegenerateNow();
			}
			int layer = this.Layer;
			Quaternion rotation = this.Rotation;
			float alpha = this.Alpha;
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				if (this.subMeshes[i].finalized)
				{
					if (alpha != 1f)
					{
						Color color = this.subMeshes[i].material.color;
						WorldLayer.propertyBlock.SetColor(ShaderPropertyIDs.Color, new Color(color.r, color.g, color.b, color.a * alpha));
						Graphics.DrawMesh(this.subMeshes[i].mesh, Vector3.zero, rotation, this.subMeshes[i].material, layer, null, 0, WorldLayer.propertyBlock);
					}
					else
					{
						Graphics.DrawMesh(this.subMeshes[i].mesh, Vector3.zero, rotation, this.subMeshes[i].material, layer);
					}
				}
			}
		}

		// Token: 0x060089FD RID: 35325 RVA: 0x00318E54 File Offset: 0x00317054
		public virtual IEnumerable Regenerate()
		{
			this.dirty = false;
			this.ClearSubMeshes(MeshParts.All);
			yield break;
		}

		// Token: 0x060089FE RID: 35326 RVA: 0x00318E64 File Offset: 0x00317064
		public void SetDirty()
		{
			this.dirty = true;
		}

		// Token: 0x060089FF RID: 35327 RVA: 0x00318E70 File Offset: 0x00317070
		private void ClearSubMeshes(MeshParts parts)
		{
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				this.subMeshes[i].Clear(parts);
			}
		}

		// Token: 0x040057B6 RID: 22454
		protected List<LayerSubMesh> subMeshes = new List<LayerSubMesh>();

		// Token: 0x040057B7 RID: 22455
		private bool dirty = true;

		// Token: 0x040057B8 RID: 22456
		private static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

		// Token: 0x040057B9 RID: 22457
		private const int MaxVerticesPerMesh = 40000;
	}
}
