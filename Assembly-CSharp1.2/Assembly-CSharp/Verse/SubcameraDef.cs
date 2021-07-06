using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200018A RID: 394
	public class SubcameraDef : Def
	{
		// Token: 0x170001DC RID: 476
		// (get) Token: 0x060009D5 RID: 2517 RVA: 0x0000DA80 File Offset: 0x0000BC80
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

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x060009D6 RID: 2518 RVA: 0x0009A6C4 File Offset: 0x000988C4
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

		// Token: 0x0400087A RID: 2170
		[NoTranslate]
		public string layer;

		// Token: 0x0400087B RID: 2171
		public int depth;

		// Token: 0x0400087C RID: 2172
		public RenderTextureFormat format;

		// Token: 0x0400087D RID: 2173
		[Unsaved(false)]
		private int layerCached = -1;
	}
}
