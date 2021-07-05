using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200030D RID: 781
	public class GraphicMeshSet
	{
		// Token: 0x0600167B RID: 5755 RVA: 0x00083170 File Offset: 0x00081370
		public GraphicMeshSet(Mesh normalMesh, Mesh leftMesh)
		{
			Mesh[] array = this.meshes;
			int num = 0;
			Mesh[] array2 = this.meshes;
			int num2 = 1;
			this.meshes[2] = normalMesh;
			array[num] = (array2[num2] = normalMesh);
			this.meshes[3] = leftMesh;
		}

		// Token: 0x0600167C RID: 5756 RVA: 0x000831B8 File Offset: 0x000813B8
		public GraphicMeshSet(float size)
		{
			this.meshes[0] = (this.meshes[1] = (this.meshes[2] = MeshMakerPlanes.NewPlaneMesh(size, false, true)));
			this.meshes[3] = MeshMakerPlanes.NewPlaneMesh(size, true, true);
		}

		// Token: 0x0600167D RID: 5757 RVA: 0x00083210 File Offset: 0x00081410
		public GraphicMeshSet(float width, float height)
		{
			Vector2 size = new Vector2(width, height);
			this.meshes[0] = (this.meshes[1] = (this.meshes[2] = MeshMakerPlanes.NewPlaneMesh(size, false, true, false)));
			this.meshes[3] = MeshMakerPlanes.NewPlaneMesh(size, true, true, false);
		}

		// Token: 0x0600167E RID: 5758 RVA: 0x00083270 File Offset: 0x00081470
		public Mesh MeshAt(Rot4 rot)
		{
			return this.meshes[rot.AsInt];
		}

		// Token: 0x04000F98 RID: 3992
		private Mesh[] meshes = new Mesh[4];
	}
}
