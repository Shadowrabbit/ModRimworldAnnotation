using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000183 RID: 387
	public class ScatterableDef : Def
	{
		// Token: 0x060009B7 RID: 2487 RVA: 0x00099F34 File Offset: 0x00098134
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.defName == "UnnamedDef")
			{
				this.defName = "Scatterable_" + this.texturePath;
			}
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.mat = MaterialPool.MatFrom(this.texturePath, ShaderDatabase.Transparent);
			});
		}

		// Token: 0x0400084F RID: 2127
		[NoTranslate]
		public string texturePath;

		// Token: 0x04000850 RID: 2128
		public float minSize;

		// Token: 0x04000851 RID: 2129
		public float maxSize;

		// Token: 0x04000852 RID: 2130
		public float selectionWeight = 100f;

		// Token: 0x04000853 RID: 2131
		[NoTranslate]
		public string scatterType = "";

		// Token: 0x04000854 RID: 2132
		public Material mat;
	}
}
