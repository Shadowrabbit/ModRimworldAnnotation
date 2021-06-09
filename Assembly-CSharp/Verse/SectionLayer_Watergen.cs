using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002A7 RID: 679
	internal class SectionLayer_Watergen : SectionLayer_Terrain
	{
		// Token: 0x0600116C RID: 4460 RVA: 0x00012B34 File Offset: 0x00010D34
		public SectionLayer_Watergen(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Terrain;
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x00012B45 File Offset: 0x00010D45
		public override Material GetMaterialFor(TerrainDef terrain)
		{
			return terrain.waterDepthMaterial;
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x000C2358 File Offset: 0x000C0558
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
					Graphics.DrawMesh(layerSubMesh.mesh, Vector3.zero, Quaternion.identity, layerSubMesh.material, SubcameraDefOf.WaterDepth.LayerId);
				}
			}
		}
	}
}
