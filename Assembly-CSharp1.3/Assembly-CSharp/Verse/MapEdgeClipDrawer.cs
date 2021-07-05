using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200022D RID: 557
	[StaticConstructorOnStartup]
	public static class MapEdgeClipDrawer
	{
		// Token: 0x06000FC6 RID: 4038 RVA: 0x00059B74 File Offset: 0x00057D74
		public static void DrawClippers(Map map)
		{
			IntVec3 size = map.Size;
			Vector3 s = new Vector3(500f, 1f, (float)size.z);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(new Vector3(-250f, MapEdgeClipDrawer.ClipAltitude, (float)size.z / 2f), Quaternion.identity, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, MapEdgeClipDrawer.ClipMat, 0);
			matrix = default(Matrix4x4);
			matrix.SetTRS(new Vector3((float)size.x + 250f, MapEdgeClipDrawer.ClipAltitude, (float)size.z / 2f), Quaternion.identity, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, MapEdgeClipDrawer.ClipMat, 0);
			s = new Vector3(1000f, 1f, 500f);
			matrix = default(Matrix4x4);
			matrix.SetTRS(new Vector3((float)(size.x / 2), MapEdgeClipDrawer.ClipAltitude, (float)size.z + 250f), Quaternion.identity, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, MapEdgeClipDrawer.ClipMat, 0);
			matrix = default(Matrix4x4);
			matrix.SetTRS(new Vector3((float)(size.x / 2), MapEdgeClipDrawer.ClipAltitude, -250f), Quaternion.identity, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, MapEdgeClipDrawer.ClipMat, 0);
		}

		// Token: 0x04000C63 RID: 3171
		public static readonly Material ClipMat = SolidColorMaterials.NewSolidColorMaterial(new Color(0.1f, 0.1f, 0.1f), ShaderDatabase.MetaOverlay);

		// Token: 0x04000C64 RID: 3172
		private static readonly float ClipAltitude = AltitudeLayer.WorldClipper.AltitudeFor();

		// Token: 0x04000C65 RID: 3173
		private const float ClipWidth = 500f;
	}
}
