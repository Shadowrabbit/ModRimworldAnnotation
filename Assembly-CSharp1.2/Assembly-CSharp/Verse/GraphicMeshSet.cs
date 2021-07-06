using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200047C RID: 1148
	public class GraphicMeshSet
	{
		// Token: 0x06001D00 RID: 7424 RVA: 0x000F2C5C File Offset: 0x000F0E5C
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

		// Token: 0x06001D01 RID: 7425 RVA: 0x000F2CA4 File Offset: 0x000F0EA4
		public GraphicMeshSet(float size)
		{
			this.meshes[0] = (this.meshes[1] = (this.meshes[2] = MeshMakerPlanes.NewPlaneMesh(size, false, true)));
			this.meshes[3] = MeshMakerPlanes.NewPlaneMesh(size, true, true);
		}

		// Token: 0x06001D02 RID: 7426 RVA: 0x000F2CFC File Offset: 0x000F0EFC
		public GraphicMeshSet(float width, float height)
		{
			Vector2 size = new Vector2(width, height);
			this.meshes[0] = (this.meshes[1] = (this.meshes[2] = MeshMakerPlanes.NewPlaneMesh(size, false, true, false)));
			this.meshes[3] = MeshMakerPlanes.NewPlaneMesh(size, true, true, false);
		}

		// Token: 0x06001D03 RID: 7427 RVA: 0x0001A2DA File Offset: 0x000184DA
		public Mesh MeshAt(Rot4 rot)
		{
			return this.meshes[rot.AsInt];
		}

		// Token: 0x040014AB RID: 5291
		private Mesh[] meshes = new Mesh[4];
	}
}
