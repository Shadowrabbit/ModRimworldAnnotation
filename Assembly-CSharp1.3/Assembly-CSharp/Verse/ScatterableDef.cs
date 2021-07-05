using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000102 RID: 258
	public class ScatterableDef : Def
	{
		// Token: 0x060006E8 RID: 1768 RVA: 0x0002138C File Offset: 0x0001F58C
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.defName == "UnnamedDef")
			{
				this.defName = "Scatterable_" + this.texturePath;
			}
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Terrain, ContentFinder<Texture2D>.Get(this.texturePath, true), null);
			});
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.mat = MaterialPool.MatFrom(this.texturePath, ShaderDatabase.Transparent);
			});
		}

		// Token: 0x04000623 RID: 1571
		[NoTranslate]
		public string texturePath;

		// Token: 0x04000624 RID: 1572
		public float minSize;

		// Token: 0x04000625 RID: 1573
		public float maxSize;

		// Token: 0x04000626 RID: 1574
		public float selectionWeight = 100f;

		// Token: 0x04000627 RID: 1575
		[NoTranslate]
		public string scatterType = "";

		// Token: 0x04000628 RID: 1576
		public Material mat;
	}
}
