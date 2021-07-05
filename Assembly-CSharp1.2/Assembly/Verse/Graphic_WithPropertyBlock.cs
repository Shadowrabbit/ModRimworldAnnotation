using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004E8 RID: 1256
	public abstract class Graphic_WithPropertyBlock : Graphic_Single
	{
		// Token: 0x06001F5C RID: 8028 RVA: 0x0001BA69 File Offset: 0x00019C69
		protected override void DrawMeshInt(Mesh mesh, Vector3 loc, Quaternion quat, Material mat)
		{
			Graphics.DrawMesh(MeshPool.plane10, Matrix4x4.TRS(loc, quat, new Vector3(this.drawSize.x, 1f, this.drawSize.y)), mat, 0, null, 0, this.propertyBlock);
		}

		// Token: 0x04001601 RID: 5633
		protected MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
	}
}
