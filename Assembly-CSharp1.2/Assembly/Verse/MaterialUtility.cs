using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200087A RID: 2170
	public static class MaterialUtility
	{
		// Token: 0x060035F0 RID: 13808 RVA: 0x00029C96 File Offset: 0x00027E96
		public static Texture2D GetMaskTexture(this Material mat)
		{
			if (!mat.HasProperty(ShaderPropertyIDs.MaskTex))
			{
				return null;
			}
			return (Texture2D)mat.GetTexture(ShaderPropertyIDs.MaskTex);
		}

		// Token: 0x060035F1 RID: 13809 RVA: 0x00029CB7 File Offset: 0x00027EB7
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
