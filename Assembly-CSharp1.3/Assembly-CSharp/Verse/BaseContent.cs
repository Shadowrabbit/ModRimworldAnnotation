using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000037 RID: 55
	[StaticConstructorOnStartup]
	public static class BaseContent
	{
		// Token: 0x0600032B RID: 811 RVA: 0x0001140C File Offset: 0x0000F60C
		public static bool NullOrBad(this Material mat)
		{
			return mat == null || mat == BaseContent.BadMat;
		}

		// Token: 0x0600032C RID: 812 RVA: 0x00011424 File Offset: 0x0000F624
		public static bool NullOrBad(this Texture2D tex)
		{
			return tex == null || tex == BaseContent.BadTex;
		}

		// Token: 0x0400009E RID: 158
		public static readonly string BadTexPath = "UI/Misc/BadTexture";

		// Token: 0x0400009F RID: 159
		public static readonly string PlaceholderImagePath = "PlaceholderImage";

		// Token: 0x040000A0 RID: 160
		public static readonly string PlaceholderGearImagePath = "PlaceholderImage_Gear";

		// Token: 0x040000A1 RID: 161
		public static readonly Material BadMat = MaterialPool.MatFrom(BaseContent.BadTexPath, ShaderDatabase.Cutout);

		// Token: 0x040000A2 RID: 162
		public static readonly Texture2D BadTex = ContentFinder<Texture2D>.Get(BaseContent.BadTexPath, true);

		// Token: 0x040000A3 RID: 163
		public static readonly Graphic BadGraphic = GraphicDatabase.Get<Graphic_Single>(BaseContent.BadTexPath);

		// Token: 0x040000A4 RID: 164
		public static readonly Texture2D BlackTex = SolidColorMaterials.NewSolidColorTexture(Color.black);

		// Token: 0x040000A5 RID: 165
		public static readonly Texture2D GreyTex = SolidColorMaterials.NewSolidColorTexture(Color.grey);

		// Token: 0x040000A6 RID: 166
		public static readonly Texture2D WhiteTex = SolidColorMaterials.NewSolidColorTexture(Color.white);

		// Token: 0x040000A7 RID: 167
		public static readonly Texture2D ClearTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);

		// Token: 0x040000A8 RID: 168
		public static readonly Texture2D YellowTex = SolidColorMaterials.NewSolidColorTexture(Color.yellow);

		// Token: 0x040000A9 RID: 169
		public static readonly Material BlackMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.black, false);

		// Token: 0x040000AA RID: 170
		public static readonly Material WhiteMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.white, false);

		// Token: 0x040000AB RID: 171
		public static readonly Material ClearMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.clear, false);
	}
}
