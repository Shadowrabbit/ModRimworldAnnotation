using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001E2 RID: 482
	internal class SectionLayer_Watergen : SectionLayer_Terrain
	{
		// Token: 0x06000DB2 RID: 3506 RVA: 0x0004D7D4 File Offset: 0x0004B9D4
		public SectionLayer_Watergen(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Terrain;
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x0004D7E5 File Offset: 0x0004B9E5
		public override Material GetMaterialFor(TerrainDef terrain)
		{
			return terrain.waterDepthMaterial;
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x0004D7F0 File Offset: 0x0004B9F0
		public override void DrawLayer()
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
					Graphics.DrawMesh(layerSubMesh.mesh, Matrix4x4.identity, layerSubMesh.material, SubcameraDefOf.WaterDepth.LayerId);
				}
			}
		}
	}
}
