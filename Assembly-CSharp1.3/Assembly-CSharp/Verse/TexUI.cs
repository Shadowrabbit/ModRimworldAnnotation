using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200043E RID: 1086
	[StaticConstructorOnStartup]
	public static class TexUI
	{
		// Token: 0x04001415 RID: 5141
		public static readonly Texture2D TitleBGTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.05f));

		// Token: 0x04001416 RID: 5142
		public static readonly Texture2D HighlightTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.1f));

		// Token: 0x04001417 RID: 5143
		public static readonly Texture2D HighlightSelectedTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 0.94f, 0.5f, 0.18f));

		// Token: 0x04001418 RID: 5144
		public static readonly Texture2D ArrowTexRight = ContentFinder<Texture2D>.Get("UI/Widgets/ArrowRight", true);

		// Token: 0x04001419 RID: 5145
		public static readonly Texture2D ArrowTexLeft = ContentFinder<Texture2D>.Get("UI/Widgets/ArrowLeft", true);

		// Token: 0x0400141A RID: 5146
		public static readonly Texture2D WinExpandWidget = ContentFinder<Texture2D>.Get("UI/Widgets/WinExpandWidget", true);

		// Token: 0x0400141B RID: 5147
		public static readonly Texture2D ArrowTex = ContentFinder<Texture2D>.Get("UI/Misc/AlertFlashArrow", true);

		// Token: 0x0400141C RID: 5148
		public static readonly Texture2D RotLeftTex = ContentFinder<Texture2D>.Get("UI/Widgets/RotLeft", true);

		// Token: 0x0400141D RID: 5149
		public static readonly Texture2D RotRightTex = ContentFinder<Texture2D>.Get("UI/Widgets/RotRight", true);

		// Token: 0x0400141E RID: 5150
		public static readonly Texture2D GuiltyTex = ContentFinder<Texture2D>.Get("UI/Icons/Guilty", true);

		// Token: 0x0400141F RID: 5151
		public static readonly Texture2D RectHighlight = ContentFinder<Texture2D>.Get("UI/Overlays/RoundedRectHighlight", true);

		// Token: 0x04001420 RID: 5152
		public static readonly Texture2D RectTextHighlight = ContentFinder<Texture2D>.Get("UI/Overlays/RectVertHighlight", true);

		// Token: 0x04001421 RID: 5153
		public static readonly Texture2D GrayBg = SolidColorMaterials.NewSolidColorTexture(new ColorInt(51, 63, 51, 200).ToColor);

		// Token: 0x04001422 RID: 5154
		public static readonly Color AvailResearchColor = new ColorInt(32, 32, 32, 255).ToColor;

		// Token: 0x04001423 RID: 5155
		public static readonly Color ActiveResearchColor = new ColorInt(0, 64, 64, 255).ToColor;

		// Token: 0x04001424 RID: 5156
		public static readonly Color FinishedResearchColor = new ColorInt(0, 64, 16, 255).ToColor;

		// Token: 0x04001425 RID: 5157
		public static readonly Color LockedResearchColor = new ColorInt(42, 42, 42, 255).ToColor;

		// Token: 0x04001426 RID: 5158
		public static readonly Color RelatedResearchColor = new ColorInt(0, 0, 0, 255).ToColor;

		// Token: 0x04001427 RID: 5159
		public static readonly Color HighlightBgResearchColor = new ColorInt(30, 30, 30, 255).ToColor;

		// Token: 0x04001428 RID: 5160
		public static readonly Color HighlightBorderResearchColor = new ColorInt(160, 160, 160, 255).ToColor;

		// Token: 0x04001429 RID: 5161
		public static readonly Color DefaultBorderResearchColor = new ColorInt(80, 80, 80, 255).ToColor;

		// Token: 0x0400142A RID: 5162
		public static readonly Color DefaultLineResearchColor = new ColorInt(60, 60, 60, 255).ToColor;

		// Token: 0x0400142B RID: 5163
		public static readonly Color HighlightLineResearchColor = new ColorInt(51, 205, 217, 255).ToColor;

		// Token: 0x0400142C RID: 5164
		public static readonly Color DependencyOutlineResearchColor = new ColorInt(217, 20, 51, 255).ToColor;

		// Token: 0x0400142D RID: 5165
		public static readonly Texture2D FastFillTex = Texture2D.whiteTexture;

		// Token: 0x0400142E RID: 5166
		public static readonly GUIStyle FastFillStyle = new GUIStyle
		{
			normal = new GUIStyleState
			{
				background = TexUI.FastFillTex
			}
		};

		// Token: 0x0400142F RID: 5167
		public static readonly Texture2D TextBGBlack = ContentFinder<Texture2D>.Get("UI/Widgets/TextBGBlack", true);

		// Token: 0x04001430 RID: 5168
		public static readonly Texture2D GrayTextBG = ContentFinder<Texture2D>.Get("UI/Overlays/GrayTextBG", true);

		// Token: 0x04001431 RID: 5169
		public static readonly Texture2D FloatMenuOptionBG = ContentFinder<Texture2D>.Get("UI/Widgets/FloatMenuOptionBG", true);

		// Token: 0x04001432 RID: 5170
		public static readonly Material GrayscaleGUI = MatLoader.LoadMat("Misc/GrayscaleGUI", -1);
	}
}
