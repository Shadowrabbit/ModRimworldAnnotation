using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000048 RID: 72
	public static class ShaderUtility
	{
		// Token: 0x060003BC RID: 956 RVA: 0x000149DE File Offset: 0x00012BDE
		public static bool SupportsMaskTex(this Shader shader)
		{
			return shader == ShaderDatabase.CutoutComplex || shader == ShaderDatabase.CutoutSkinOverlay || shader == ShaderDatabase.Wound;
		}

		// Token: 0x060003BD RID: 957 RVA: 0x00014A07 File Offset: 0x00012C07
		public static Shader GetSkinShader(bool skinColorOverriden)
		{
			if (skinColorOverriden)
			{
				return ShaderDatabase.CutoutSkinColorOverride;
			}
			return ShaderDatabase.CutoutSkin;
		}
	}
}
