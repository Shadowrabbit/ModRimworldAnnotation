using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200010C RID: 268
	public class SubcameraDef : Def
	{
		// Token: 0x17000155 RID: 341
		// (get) Token: 0x0600070C RID: 1804 RVA: 0x00021C14 File Offset: 0x0001FE14
		public int LayerId
		{
			get
			{
				if (this.layerCached == -1)
				{
					this.layerCached = LayerMask.NameToLayer(this.layer);
				}
				return this.layerCached;
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x0600070D RID: 1805 RVA: 0x00021C38 File Offset: 0x0001FE38
		public RenderTextureFormat BestFormat
		{
			get
			{
				if (SystemInfo.SupportsRenderTextureFormat(this.format))
				{
					return this.format;
				}
				if (this.format == RenderTextureFormat.R8 && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RG16))
				{
					return RenderTextureFormat.RG16;
				}
				if ((this.format == RenderTextureFormat.R8 || this.format == RenderTextureFormat.RG16) && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB32))
				{
					return RenderTextureFormat.ARGB32;
				}
				if ((this.format == RenderTextureFormat.R8 || this.format == RenderTextureFormat.RHalf || this.format == RenderTextureFormat.RFloat) && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGFloat))
				{
					return RenderTextureFormat.RGFloat;
				}
				if ((this.format == RenderTextureFormat.R8 || this.format == RenderTextureFormat.RHalf || this.format == RenderTextureFormat.RFloat || this.format == RenderTextureFormat.RGFloat) && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBFloat))
				{
					return RenderTextureFormat.ARGBFloat;
				}
				return this.format;
			}
		}

		// Token: 0x04000656 RID: 1622
		[NoTranslate]
		public string layer;

		// Token: 0x04000657 RID: 1623
		public int depth;

		// Token: 0x04000658 RID: 1624
		public RenderTextureFormat format;

		// Token: 0x04000659 RID: 1625
		[Unsaved(false)]
		private int layerCached = -1;
	}
}
