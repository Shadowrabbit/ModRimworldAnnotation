using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000184 RID: 388
	public class ShaderTypeDef : Def
	{
		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x060009BA RID: 2490 RVA: 0x0000D958 File Offset: 0x0000BB58
		public Shader Shader
		{
			get
			{
				if (this.shaderInt == null)
				{
					this.shaderInt = ShaderDatabase.LoadShader(this.shaderPath);
				}
				return this.shaderInt;
			}
		}

		// Token: 0x04000855 RID: 2133
		[NoTranslate]
		public string shaderPath;

		// Token: 0x04000856 RID: 2134
		[Unsaved(false)]
		private Shader shaderInt;
	}
}
