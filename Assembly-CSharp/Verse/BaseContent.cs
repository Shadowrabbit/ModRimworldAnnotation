using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200006E RID: 110
	[StaticConstructorOnStartup]
	public static class BaseContent
	{
		// Token: 0x0600045C RID: 1116 RVA: 0x00009E00 File Offset: 0x00008000
		public static bool NullOrBad(this Material mat)
		{
			return mat == null || mat == BaseContent.BadMat;
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00009E18 File Offset: 0x00008018
		public static bool NullOrBad(this Texture2D tex)
		{
			return tex == null || tex == BaseContent.BadTex;
		}

		// Token: 0x040001E1 RID: 481
		public static readonly string BadTexPath = "UI/Misc/BadTexture";

		// Token: 0x040001E2 RID: 482
		public static readonly string PlaceholderImagePath = "PlaceholderImage";

		// Token: 0x040001E3 RID: 483
		public static readonly Material BadMat = MaterialPool.MatFrom(BaseContent.BadTexPath, ShaderDatabase.Cutout);

		// Token: 0x040001E4 RID: 484
		public static readonly Texture2D BadTex = ContentFinder<Texture2D>.Get(BaseContent.BadTexPath, true);

		// Token: 0x040001E5 RID: 485
		public static readonly Graphic BadGraphic = GraphicDatabase.Get<Graphic_Single>(BaseContent.BadTexPath);

		// Token: 0x040001E6 RID: 486
		public static readonly Texture2D BlackTex = SolidColorMaterials.NewSolidColorTexture(Color.black);

		// Token: 0x040001E7 RID: 487
		public static readonly Texture2D GreyTex = SolidColorMaterials.NewSolidColorTexture(Color.grey);

		// Token: 0x040001E8 RID: 488
		public static readonly Texture2D WhiteTex = SolidColorMaterials.NewSolidColorTexture(Color.white);

		// Token: 0x040001E9 RID: 489
		public static readonly Texture2D ClearTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);

		// Token: 0x040001EA RID: 490
		public static readonly Texture2D YellowTex = SolidColorMaterials.NewSolidColorTexture(Color.yellow);

		// Token: 0x040001EB RID: 491
		public static readonly Material BlackMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.black, false);

		// Token: 0x040001EC RID: 492
		public static readonly Material WhiteMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.white, false);

		// Token: 0x040001ED RID: 493
		public static readonly Material ClearMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.clear, false);
	}
}
