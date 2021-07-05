using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017B2 RID: 6066
	[StaticConstructorOnStartup]
	public class Caravan_GotoMoteRenderer
	{
		// Token: 0x06008CA4 RID: 36004 RVA: 0x00328098 File Offset: 0x00326298
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

		// Token: 0x06008CA5 RID: 36005 RVA: 0x0032815C File Offset: 0x0032635C
		public void OrderedToTile(int tile)
		{
			this.tile = tile;
			this.lastOrderedToTileTime = Time.time;
		}

		// Token: 0x0400592F RID: 22831
		private int tile;

		// Token: 0x04005930 RID: 22832
		private float lastOrderedToTileTime = -0.51f;

		// Token: 0x04005931 RID: 22833
		private static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

		// Token: 0x04005932 RID: 22834
		private static Material cachedMaterial;

		// Token: 0x04005933 RID: 22835
		public static readonly Material FeedbackGoto = MaterialPool.MatFrom("Things/Mote/FeedbackGoto", ShaderDatabase.WorldOverlayTransparent, WorldMaterials.DynamicObjectRenderQueue);

		// Token: 0x04005934 RID: 22836
		private const float Duration = 0.5f;

		// Token: 0x04005935 RID: 22837
		private const float BaseSize = 0.8f;
	}
}
