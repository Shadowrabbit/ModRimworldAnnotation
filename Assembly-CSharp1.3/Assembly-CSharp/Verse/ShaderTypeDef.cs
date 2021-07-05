using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000103 RID: 259
	public class ShaderTypeDef : Def
	{
		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x00021435 File Offset: 0x0001F635
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

		// Token: 0x04000629 RID: 1577
		[NoTranslate]
		public string shaderPath;

		// Token: 0x0400062A RID: 1578
		[Unsaved(false)]
		private Shader shaderInt;
	}
}
