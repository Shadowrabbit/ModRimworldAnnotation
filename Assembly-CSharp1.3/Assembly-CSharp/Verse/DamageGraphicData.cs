using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200009D RID: 157
	public class DamageGraphicData
	{
		// Token: 0x06000531 RID: 1329 RVA: 0x0001A949 File Offset: 0x00018B49
		public void ResolveReferencesSpecial()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (this.scratches != null)
				{
					this.scratchMats = new List<Material>();
					for (int i = 0; i < this.scratches.Count; i++)
					{
						this.scratchMats[i] = MaterialPool.MatFrom(this.scratches[i], ShaderDatabase.Transparent);
						GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)this.scratchMats[i].mainTexture, null);
					}
				}
				if (this.cornerTL != null)
				{
					this.cornerTLMat = MaterialPool.MatFrom(this.cornerTL, ShaderDatabase.Transparent);
					GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)this.cornerTLMat.mainTexture, null);
				}
				if (this.cornerTR != null)
				{
					this.cornerTRMat = MaterialPool.MatFrom(this.cornerTR, ShaderDatabase.Transparent);
					GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)this.cornerTRMat.mainTexture, null);
				}
				if (this.cornerBL != null)
				{
					this.cornerBLMat = MaterialPool.MatFrom(this.cornerBL, ShaderDatabase.Transparent);
					GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)this.cornerBLMat.mainTexture, null);
				}
				if (this.cornerBR != null)
				{
					this.cornerBRMat = MaterialPool.MatFrom(this.cornerBR, ShaderDatabase.Transparent);
					GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)this.cornerBRMat.mainTexture, null);
				}
				if (this.edgeTop != null)
				{
					this.edgeTopMat = MaterialPool.MatFrom(this.edgeTop, ShaderDatabase.Transparent);
					GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)this.edgeTopMat.mainTexture, null);
				}
				if (this.edgeBot != null)
				{
					this.edgeBotMat = MaterialPool.MatFrom(this.edgeBot, ShaderDatabase.Transparent);
					GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)this.edgeBotMat.mainTexture, null);
				}
				if (this.edgeLeft != null)
				{
					this.edgeLeftMat = MaterialPool.MatFrom(this.edgeLeft, ShaderDatabase.Transparent);
					GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)this.edgeLeftMat.mainTexture, null);
				}
				if (this.edgeRight != null)
				{
					this.edgeRightMat = MaterialPool.MatFrom(this.edgeRight, ShaderDatabase.Transparent);
					GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)this.edgeRightMat.mainTexture, null);
				}
			});
		}

		// Token: 0x04000268 RID: 616
		public bool enabled = true;

		// Token: 0x04000269 RID: 617
		public Rect rectN;

		// Token: 0x0400026A RID: 618
		public Rect rectE;

		// Token: 0x0400026B RID: 619
		public Rect rectS;

		// Token: 0x0400026C RID: 620
		public Rect rectW;

		// Token: 0x0400026D RID: 621
		public Rect rect;

		// Token: 0x0400026E RID: 622
		[NoTranslate]
		public List<string> scratches;

		// Token: 0x0400026F RID: 623
		[NoTranslate]
		public string cornerTL;

		// Token: 0x04000270 RID: 624
		[NoTranslate]
		public string cornerTR;

		// Token: 0x04000271 RID: 625
		[NoTranslate]
		public string cornerBL;

		// Token: 0x04000272 RID: 626
		[NoTranslate]
		public string cornerBR;

		// Token: 0x04000273 RID: 627
		[NoTranslate]
		public string edgeLeft;

		// Token: 0x04000274 RID: 628
		[NoTranslate]
		public string edgeRight;

		// Token: 0x04000275 RID: 629
		[NoTranslate]
		public string edgeTop;

		// Token: 0x04000276 RID: 630
		[NoTranslate]
		public string edgeBot;

		// Token: 0x04000277 RID: 631
		[Unsaved(false)]
		public List<Material> scratchMats;

		// Token: 0x04000278 RID: 632
		[Unsaved(false)]
		public Material cornerTLMat;

		// Token: 0x04000279 RID: 633
		[Unsaved(false)]
		public Material cornerTRMat;

		// Token: 0x0400027A RID: 634
		[Unsaved(false)]
		public Material cornerBLMat;

		// Token: 0x0400027B RID: 635
		[Unsaved(false)]
		public Material cornerBRMat;

		// Token: 0x0400027C RID: 636
		[Unsaved(false)]
		public Material edgeLeftMat;

		// Token: 0x0400027D RID: 637
		[Unsaved(false)]
		public Material edgeRightMat;

		// Token: 0x0400027E RID: 638
		[Unsaved(false)]
		public Material edgeTopMat;

		// Token: 0x0400027F RID: 639
		[Unsaved(false)]
		public Material edgeBotMat;
	}
}
