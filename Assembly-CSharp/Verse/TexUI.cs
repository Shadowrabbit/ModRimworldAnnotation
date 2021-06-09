using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000783 RID: 1923
	[StaticConstructorOnStartup]
	public static class TexUI
	{
		// Token: 0x0400210A RID: 8458
		public static readonly Texture2D TitleBGTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.05f));

		// Token: 0x0400210B RID: 8459
		public static readonly Texture2D HighlightTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.1f));

		// Token: 0x0400210C RID: 8460
		public static readonly Texture2D HighlightSelectedTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 0.94f, 0.5f, 0.18f));

		// Token: 0x0400210D RID: 8461
		public static readonly Texture2D ArrowTexRight = ContentFinder<Texture2D>.Get("UI/Widgets/ArrowRight", true);

		// Token: 0x0400210E RID: 8462
		public static readonly Texture2D ArrowTexLeft = ContentFinder<Texture2D>.Get("UI/Widgets/ArrowLeft", true);

		// Token: 0x0400210F RID: 8463
		public static readonly Texture2D WinExpandWidget = ContentFinder<Texture2D>.Get("UI/Widgets/WinExpandWidget", true);

		// Token: 0x04002110 RID: 8464
		public static readonly Texture2D ArrowTex = ContentFinder<Texture2D>.Get("UI/Misc/AlertFlashArrow", true);

		// Token: 0x04002111 RID: 8465
		public static readonly Texture2D RotLeftTex = ContentFinder<Texture2D>.Get("UI/Widgets/RotLeft", true);

		// Token: 0x04002112 RID: 8466
		public static readonly Texture2D RotRightTex = ContentFinder<Texture2D>.Get("UI/Widgets/RotRight", true);

		// Token: 0x04002113 RID: 8467
		public static readonly Texture2D GrayBg = SolidColorMaterials.NewSolidColorTexture(new ColorInt(51, 63, 51, 200).ToColor);

		// Token: 0x04002114 RID: 8468
		public static readonly Color AvailResearchColor = new ColorInt(32, 32, 32, 255).ToColor;

		// Token: 0x04002115 RID: 8469
		public static readonly Color ActiveResearchColor = new ColorInt(0, 64, 64, 255).ToColor;

		// Token: 0x04002116 RID: 8470
		public static readonly Color FinishedResearchColor = new ColorInt(0, 64, 16, 255).ToColor;

		// Token: 0x04002117 RID: 8471
		public static readonly Color LockedResearchColor = new ColorInt(42, 42, 42, 255).ToColor;

		// Token: 0x04002118 RID: 8472
		public static readonly Color RelatedResearchColor = new ColorInt(0, 0, 0, 255).ToColor;

		// Token: 0x04002119 RID: 8473
		public static readonly Color HighlightBgResearchColor = new ColorInt(30, 30, 30, 255).ToColor;

		// Token: 0x0400211A RID: 8474
		public static readonly Color HighlightBorderResearchColor = new ColorInt(160, 160, 160, 255).ToColor;

		// Token: 0x0400211B RID: 8475
		public static readonly Color DefaultBorderResearchColor = new ColorInt(80, 80, 80, 255).ToColor;

		// Token: 0x0400211C RID: 8476
		public static readonly Color DefaultLineResearchColor = new ColorInt(60, 60, 60, 255).ToColor;

		// Token: 0x0400211D RID: 8477
		public static readonly Color HighlightLineResearchColor = new ColorInt(51, 205, 217, 255).ToColor;

		// Token: 0x0400211E RID: 8478
		public static readonly Color DependencyOutlineResearchColor = new ColorInt(217, 20, 51, 255).ToColor;

		// Token: 0x0400211F RID: 8479
		public static readonly Texture2D FastFillTex = Texture2D.whiteTexture;

		// Token: 0x04002120 RID: 8480
		public static readonly GUIStyle FastFillStyle = new GUIStyle
		{
			normal = new GUIStyleState
			{
				background = TexUI.FastFillTex
			}
		};

		// Token: 0x04002121 RID: 8481
		public static readonly Texture2D TextBGBlack = ContentFinder<Texture2D>.Get("UI/Widgets/TextBGBlack", true);

		// Token: 0x04002122 RID: 8482
		public static readonly Texture2D GrayTextBG = ContentFinder<Texture2D>.Get("UI/Overlays/GrayTextBG", true);

		// Token: 0x04002123 RID: 8483
		public static readonly Texture2D FloatMenuOptionBG = ContentFinder<Texture2D>.Get("UI/Widgets/FloatMenuOptionBG", true);

		// Token: 0x04002124 RID: 8484
		public static readonly Material GrayscaleGUI = MatLoader.LoadMat("Misc/GrayscaleGUI", -1);
	}
}
