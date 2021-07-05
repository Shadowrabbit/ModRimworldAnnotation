using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020020D4 RID: 8404
	[StaticConstructorOnStartup]
	public class Caravan_GotoMoteRenderer
	{
		// Token: 0x0600B28B RID: 45707 RVA: 0x0033B884 File Offset: 0x00339A84
		public void RenderMote()
		{
			float num = (Time.time - this.lastOrderedToTileTime) / 0.5f;
			if (num > 1f)
			{
				return;
			}
			if (Caravan_GotoMoteRenderer.cachedMaterial == null)
			{
				Caravan_GotoMoteRenderer.cachedMaterial = MaterialPool.MatFrom((Texture2D)Caravan_GotoMoteRenderer.FeedbackGoto.mainTexture, Caravan_GotoMoteRenderer.FeedbackGoto.shader, Color.white, WorldMaterials.DynamicObjectRenderQueue);
			}
			WorldGrid worldGrid = Find.WorldGrid;
			Vector3 tileCenter = worldGrid.GetTileCenter(this.tile);
			Color value = new Color(1f, 1f, 1f, 1f - num);
			Caravan_GotoMoteRenderer.propertyBlock.SetColor(ShaderPropertyIDs.Color, value);
			WorldRendererUtility.DrawQuadTangentialToPlanet(tileCenter, 0.8f * worldGrid.averageTileSize, 0.018f, Caravan_GotoMoteRenderer.cachedMaterial, false, false, Caravan_GotoMoteRenderer.propertyBlock);
		}

		// Token: 0x0600B28C RID: 45708 RVA: 0x0007412F File Offset: 0x0007232F
		public void OrderedToTile(int tile)
		{
			this.tile = tile;
			this.lastOrderedToTileTime = Time.time;
		}

		// Token: 0x04007AC4 RID: 31428
		private int tile;

		// Token: 0x04007AC5 RID: 31429
		private float lastOrderedToTileTime = -0.51f;

		// Token: 0x04007AC6 RID: 31430
		private static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

		// Token: 0x04007AC7 RID: 31431
		private static Material cachedMaterial;

		// Token: 0x04007AC8 RID: 31432
		public static readonly Material FeedbackGoto = MaterialPool.MatFrom("Things/Mote/FeedbackGoto", ShaderDatabase.WorldOverlayTransparent, WorldMaterials.DynamicObjectRenderQueue);

		// Token: 0x04007AC9 RID: 31433
		private const float Duration = 0.5f;

		// Token: 0x04007ACA RID: 31434
		private const float BaseSize = 0.8f;
	}
}
