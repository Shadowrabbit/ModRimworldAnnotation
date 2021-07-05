using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000361 RID: 865
	public abstract class Graphic_WithPropertyBlock : Graphic_Single
	{
		// Token: 0x0600188B RID: 6283 RVA: 0x0009133C File Offset: 0x0008F53C
		protected override void DrawMeshInt(Mesh mesh, Vector3 loc, Quaternion quat, Material mat)
		{
			Graphics.DrawMesh(MeshPool.plane10, Matrix4x4.TRS(loc, quat, new Vector3(this.drawSize.x, 1f, this.drawSize.y)), mat, 0, null, 0, this.propertyBlock);
		}

		// Token: 0x040010A5 RID: 4261
		protected MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
	}
}
