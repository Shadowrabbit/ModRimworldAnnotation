using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000100 RID: 256
	public class DamageGraphicData
	{
		// Token: 0x06000736 RID: 1846 RVA: 0x0000BDDE File Offset: 0x00009FDE
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
					}
				}
				if (this.cornerTL != null)
				{
					this.cornerTLMat = MaterialPool.MatFrom(this.cornerTL, ShaderDatabase.Transparent);
				}
				if (this.cornerTR != null)
				{
					this.cornerTRMat = MaterialPool.MatFrom(this.cornerTR, ShaderDatabase.Transparent);
				}
				if (this.cornerBL != null)
				{
					this.cornerBLMat = MaterialPool.MatFrom(this.cornerBL, ShaderDatabase.Transparent);
				}
				if (this.cornerBR != null)
				{
					this.cornerBRMat = MaterialPool.MatFrom(this.cornerBR, ShaderDatabase.Transparent);
				}
				if (this.edgeTop != null)
				{
					this.edgeTopMat = MaterialPool.MatFrom(this.edgeTop, ShaderDatabase.Transparent);
				}
				if (this.edgeBot != null)
				{
					this.edgeBotMat = MaterialPool.MatFrom(this.edgeBot, ShaderDatabase.Transparent);
				}
				if (this.edgeLeft != null)
				{
					this.edgeLeftMat = MaterialPool.MatFrom(this.edgeLeft, ShaderDatabase.Transparent);
				}
				if (this.edgeRight != null)
				{
					this.edgeRightMat = MaterialPool.MatFrom(this.edgeRight, ShaderDatabase.Transparent);
				}
			});
		}

		// Token: 0x0400043B RID: 1083
		public bool enabled = true;

		// Token: 0x0400043C RID: 1084
		public Rect rectN;

		// Token: 0x0400043D RID: 1085
		public Rect rectE;

		// Token: 0x0400043E RID: 1086
		public Rect rectS;

		// Token: 0x0400043F RID: 1087
		public Rect rectW;

		// Token: 0x04000440 RID: 1088
		public Rect rect;

		// Token: 0x04000441 RID: 1089
		[NoTranslate]
		public List<string> scratches;

		// Token: 0x04000442 RID: 1090
		[NoTranslate]
		public string cornerTL;

		// Token: 0x04000443 RID: 1091
		[NoTranslate]
		public string cornerTR;

		// Token: 0x04000444 RID: 1092
		[NoTranslate]
		public string cornerBL;

		// Token: 0x04000445 RID: 1093
		[NoTranslate]
		public string cornerBR;

		// Token: 0x04000446 RID: 1094
		[NoTranslate]
		public string edgeLeft;

		// Token: 0x04000447 RID: 1095
		[NoTranslate]
		public string edgeRight;

		// Token: 0x04000448 RID: 1096
		[NoTranslate]
		public string edgeTop;

		// Token: 0x04000449 RID: 1097
		[NoTranslate]
		public string edgeBot;

		// Token: 0x0400044A RID: 1098
		[Unsaved(false)]
		public List<Material> scratchMats;

		// Token: 0x0400044B RID: 1099
		[Unsaved(false)]
		public Material cornerTLMat;

		// Token: 0x0400044C RID: 1100
		[Unsaved(false)]
		public Material cornerTRMat;

		// Token: 0x0400044D RID: 1101
		[Unsaved(false)]
		public Material cornerBLMat;

		// Token: 0x0400044E RID: 1102
		[Unsaved(false)]
		public Material cornerBRMat;

		// Token: 0x0400044F RID: 1103
		[Unsaved(false)]
		public Material edgeLeftMat;

		// Token: 0x04000450 RID: 1104
		[Unsaved(false)]
		public Material edgeRightMat;

		// Token: 0x04000451 RID: 1105
		[Unsaved(false)]
		public Material edgeTopMat;

		// Token: 0x04000452 RID: 1106
		[Unsaved(false)]
		public Material edgeBotMat;
	}
}
