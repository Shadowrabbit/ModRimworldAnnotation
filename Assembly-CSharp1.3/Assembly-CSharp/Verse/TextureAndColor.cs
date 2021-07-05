using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002C3 RID: 707
	public struct TextureAndColor
	{
		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06001317 RID: 4887 RVA: 0x0006CA93 File Offset: 0x0006AC93
		public bool HasValue
		{
			get
			{
				return this.texture != null;
			}
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06001318 RID: 4888 RVA: 0x0006CAA1 File Offset: 0x0006ACA1
		public Texture2D Texture
		{
			get
			{
				return this.texture;
			}
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06001319 RID: 4889 RVA: 0x0006CAA9 File Offset: 0x0006ACA9
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x0600131A RID: 4890 RVA: 0x0006CAB1 File Offset: 0x0006ACB1
		public TextureAndColor(Texture2D texture, Color color)
		{
			this.texture = texture;
			this.color = color;
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x0600131B RID: 4891 RVA: 0x0006CAC1 File Offset: 0x0006ACC1
		public static TextureAndColor None
		{
			get
			{
				return new TextureAndColor(null, Color.white);
			}
		}

		// Token: 0x0600131C RID: 4892 RVA: 0x0006CACE File Offset: 0x0006ACCE
		public static implicit operator TextureAndColor(Texture2D texture)
		{
			return new TextureAndColor(texture, Color.white);
		}

		// Token: 0x04000E4D RID: 3661
		private Texture2D texture;

		// Token: 0x04000E4E RID: 3662
		private Color color;
	}
}
