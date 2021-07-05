using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004D0 RID: 1232
	public static class MaterialUtility
	{
		// Token: 0x06002564 RID: 9572 RVA: 0x000E94DB File Offset: 0x000E76DB
		public static Texture2D GetMaskTexture(this Material mat)
		{
			if (!mat.HasProperty(ShaderPropertyIDs.MaskTex))
			{
				return null;
			}
			return (Texture2D)mat.GetTexture(ShaderPropertyIDs.MaskTex);
		}

		// Token: 0x06002565 RID: 9573 RVA: 0x000E94FC File Offset: 0x000E76FC
		public static Color GetColorTwo(this Material mat)
		{
			if (!mat.HasProperty(ShaderPropertyIDs.ColorTwo))
			{
				return Color.white;
			}
			return mat.GetColor(ShaderPropertyIDs.ColorTwo);
		}
	}
}
