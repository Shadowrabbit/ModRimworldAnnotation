using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000400 RID: 1024
	public struct TextureAndColor
	{
		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x060018E8 RID: 6376 RVA: 0x00017AEA File Offset: 0x00015CEA
		public bool HasValue
		{
			get
			{
				return this.texture != null;
			}
		}

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x060018E9 RID: 6377 RVA: 0x00017AF8 File Offset: 0x00015CF8
		public Texture2D Texture
		{
			get
			{
				return this.texture;
			}
		}

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x060018EA RID: 6378 RVA: 0x00017B00 File Offset: 0x00015D00
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x060018EB RID: 6379 RVA: 0x00017B08 File Offset: 0x00015D08
		public TextureAndColor(Texture2D texture, Color color)
		{
			this.texture = texture;
			this.color = color;
		}

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x060018EC RID: 6380 RVA: 0x00017B18 File Offset: 0x00015D18
		public static TextureAndColor None
		{
			get
			{
				return new TextureAndColor(null, Color.white);
			}
		}

		// Token: 0x060018ED RID: 6381 RVA: 0x00017B25 File Offset: 0x00015D25
		public static implicit operator TextureAndColor(Texture2D texture)
		{
			return new TextureAndColor(texture, Color.white);
		}

		// Token: 0x040012BD RID: 4797
		private Texture2D texture;

		// Token: 0x040012BE RID: 4798
		private Color color;
	}
}
