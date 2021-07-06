using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200008C RID: 140
	public static class ShaderUtility
	{
		// Token: 0x0600050C RID: 1292 RVA: 0x0000A61B File Offset: 0x0000881B
		public static bool SupportsMaskTex(this Shader shader)
		{
			return shader == ShaderDatabase.CutoutComplex;
		}
	}
}
