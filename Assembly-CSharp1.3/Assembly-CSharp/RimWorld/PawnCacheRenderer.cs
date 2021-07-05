using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DD7 RID: 3543
	public class PawnCacheRenderer : MonoBehaviour
	{
		// Token: 0x06005240 RID: 21056 RVA: 0x001BC24C File Offset: 0x001BA44C
		public void RenderPawn(Pawn pawn, RenderTexture renderTexture, Vector3 cameraOffset, float cameraZoom, float angle, Rot4 rotation, bool renderHead = true, bool renderBody = true, bool renderHeadgear = true, bool renderClothes = true, bool portrait = false, Vector3 positionOffset = default(Vector3), Dictionary<Apparel, Color> overrideApparelColor = null, bool stylingStation = false)
		{
			this.rotation = rotation;
			this.renderHead = renderHead;
			this.renderBody = renderBody;
			this.renderHeadgear = renderHeadgear;
			this.renderClothes = renderClothes;
			this.portrait = portrait;
			this.angle = angle;
			this.positionOffset = positionOffset;
			this.overrideApparelColor = overrideApparelColor;
			this.stylingStation = stylingStation;
			Camera pawnCacheCamera = Find.PawnCacheCamera;
			Vector3 position = pawnCacheCamera.transform.position;
			float orthographicSize = pawnCacheCamera.orthographicSize;
			pawnCacheCamera.transform.position += cameraOffset;
			pawnCacheCamera.orthographicSize = 1f / cameraZoom;
			this.pawn = pawn;
			pawnCacheCamera.SetTargetBuffers(renderTexture.colorBuffer, renderTexture.depthBuffer);
			pawnCacheCamera.Render();
			this.pawn = null;
			pawnCacheCamera.transform.position = position;
			pawnCacheCamera.orthographicSize = orthographicSize;
			pawnCacheCamera.targetTexture = null;
		}

		// Token: 0x06005241 RID: 21057 RVA: 0x001BC328 File Offset: 0x001BA528
		public void OnPostRender()
		{
			this.pawn.Drawer.renderer.RenderCache(this.rotation, this.angle, this.positionOffset, this.renderHead, this.renderBody, this.portrait, this.renderHeadgear, this.renderClothes, this.overrideApparelColor, this.stylingStation);
		}

		// Token: 0x04003099 RID: 12441
		private Pawn pawn;

		// Token: 0x0400309A RID: 12442
		private Rot4 rotation;

		// Token: 0x0400309B RID: 12443
		private bool renderHead;

		// Token: 0x0400309C RID: 12444
		private bool renderBody;

		// Token: 0x0400309D RID: 12445
		private bool renderHeadgear;

		// Token: 0x0400309E RID: 12446
		private bool renderClothes;

		// Token: 0x0400309F RID: 12447
		private bool portrait;

		// Token: 0x040030A0 RID: 12448
		private float angle;

		// Token: 0x040030A1 RID: 12449
		private Vector3 positionOffset;

		// Token: 0x040030A2 RID: 12450
		private Dictionary<Apparel, Color> overrideApparelColor;

		// Token: 0x040030A3 RID: 12451
		private bool stylingStation;
	}
}
