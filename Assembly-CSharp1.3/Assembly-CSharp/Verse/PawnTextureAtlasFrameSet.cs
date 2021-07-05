using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002FD RID: 765
	public class PawnTextureAtlasFrameSet
	{
		// Token: 0x0600162A RID: 5674 RVA: 0x000811F1 File Offset: 0x0007F3F1
		public int GetIndex(Rot4 rotation, PawnDrawMode drawMode)
		{
			if (drawMode == PawnDrawMode.BodyAndHead)
			{
				return rotation.AsInt;
			}
			return 4 + rotation.AsInt;
		}

		// Token: 0x04000F5D RID: 3933
		public RenderTexture atlas;

		// Token: 0x04000F5E RID: 3934
		public Rect[] uvRects = new Rect[8];

		// Token: 0x04000F5F RID: 3935
		public Mesh[] meshes = new Mesh[8];

		// Token: 0x04000F60 RID: 3936
		public bool[] isDirty = new bool[]
		{
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true
		};
	}
}
