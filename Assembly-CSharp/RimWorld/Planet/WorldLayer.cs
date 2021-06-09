using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002046 RID: 8262
	[StaticConstructorOnStartup]
	public class WorldLayer
	{
		// Token: 0x170019D8 RID: 6616
		// (get) Token: 0x0600AF1C RID: 44828 RVA: 0x00072098 File Offset: 0x00070298
		public virtual bool ShouldRegenerate
		{
			get
			{
				return this.dirty;
			}
		}

		// Token: 0x170019D9 RID: 6617
		// (get) Token: 0x0600AF1D RID: 44829 RVA: 0x000720A0 File Offset: 0x000702A0
		protected virtual int Layer
		{
			get
			{
				return WorldCameraManager.WorldLayer;
			}
		}

		// Token: 0x170019DA RID: 6618
		// (get) Token: 0x0600AF1E RID: 44830 RVA: 0x000720A7 File Offset: 0x000702A7
		protected virtual Quaternion Rotation
		{
			get
			{
				return Quaternion.identity;
			}
		}

		// Token: 0x170019DB RID: 6619
		// (get) Token: 0x0600AF1F RID: 44831 RVA: 0x0000CE6C File Offset: 0x0000B06C
		protected virtual float Alpha
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x170019DC RID: 6620
		// (get) Token: 0x0600AF20 RID: 44832 RVA: 0x00072098 File Offset: 0x00070298
		public bool Dirty
		{
			get
			{
				return this.dirty;
			}
		}

		// Token: 0x0600AF21 RID: 44833 RVA: 0x0032E900 File Offset: 0x0032CB00
		protected LayerSubMesh GetSubMesh(Material material)
		{
			int num;
			return this.GetSubMesh(material, out num);
		}

		// Token: 0x0600AF22 RID: 44834 RVA: 0x0032E918 File Offset: 0x0032CB18
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
			LayerSubMesh layerSubMesh2 = new LayerSubMesh(mesh, material);
			subMeshIndex = this.subMeshes.Count;
			this.subMeshes.Add(layerSubMesh2);
			return layerSubMesh2;
		}

		// Token: 0x0600AF23 RID: 44835 RVA: 0x0032E9CC File Offset: 0x0032CBCC
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

		// Token: 0x0600AF24 RID: 44836 RVA: 0x000720AE File Offset: 0x000702AE
		public void RegenerateNow()
		{
			this.dirty = false;
			this.Regenerate().ExecuteEnumerable();
		}

		// Token: 0x0600AF25 RID: 44837 RVA: 0x0032EA1C File Offset: 0x0032CC1C
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

		// Token: 0x0600AF26 RID: 44838 RVA: 0x000720C2 File Offset: 0x000702C2
		public virtual IEnumerable Regenerate()
		{
			this.dirty = false;
			this.ClearSubMeshes(MeshParts.All);
			yield break;
		}

		// Token: 0x0600AF27 RID: 44839 RVA: 0x000720D2 File Offset: 0x000702D2
		public void SetDirty()
		{
			this.dirty = true;
		}

		// Token: 0x0600AF28 RID: 44840 RVA: 0x0032EB38 File Offset: 0x0032CD38
		private void ClearSubMeshes(MeshParts parts)
		{
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				this.subMeshes[i].Clear(parts);
			}
		}

		// Token: 0x04007863 RID: 30819
		protected List<LayerSubMesh> subMeshes = new List<LayerSubMesh>();

		// Token: 0x04007864 RID: 30820
		private bool dirty = true;

		// Token: 0x04007865 RID: 30821
		private static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

		// Token: 0x04007866 RID: 30822
		private const int MaxVerticesPerMesh = 40000;
	}
}
