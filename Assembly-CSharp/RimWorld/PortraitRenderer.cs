using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200144D RID: 5197
	public class PortraitRenderer : MonoBehaviour
	{
		// Token: 0x0600705C RID: 28764 RVA: 0x0022679C File Offset: 0x0022499C
		public void RenderPortrait(Pawn pawn, RenderTexture renderTexture, Vector3 cameraOffset, float cameraZoom)
		{
			Camera portraitCamera = Find.PortraitCamera;
			portraitCamera.targetTexture = renderTexture;
			Vector3 position = portraitCamera.transform.position;
			float orthographicSize = portraitCamera.orthographicSize;
			portraitCamera.transform.position += cameraOffset;
			portraitCamera.orthographicSize = 1f / cameraZoom;
			this.pawn = pawn;
			portraitCamera.Render();
			this.pawn = null;
			portraitCamera.transform.position = position;
			portraitCamera.orthographicSize = orthographicSize;
			portraitCamera.targetTexture = null;
		}

		// Token: 0x0600705D RID: 28765 RVA: 0x0004BC87 File Offset: 0x00049E87
		public void OnPostRender()
		{
			this.pawn.Drawer.renderer.RenderPortrait();
		}

		// Token: 0x04004A20 RID: 18976
		private Pawn pawn;
	}
}
