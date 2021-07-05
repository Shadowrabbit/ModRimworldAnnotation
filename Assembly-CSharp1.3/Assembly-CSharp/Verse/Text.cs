using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000422 RID: 1058
	public static class Text
	{
		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x06001FD1 RID: 8145 RVA: 0x000C5331 File Offset: 0x000C3531
		// (set) Token: 0x06001FD2 RID: 8146 RVA: 0x000C5338 File Offset: 0x000C3538
		public static GameFont Font
		{
			get
			{
				return Text.fontInt;
			}
			set
			{
				if (value == GameFont.Tiny && !LongEventHandler.AnyEventNowOrWaiting && !LanguageDatabase.activeLanguage.info.canBeTiny)
				{
					Text.fontInt = GameFont.Small;
					return;
				}
				Text.fontInt = value;
			}
		}

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x06001FD3 RID: 8147 RVA: 0x000C5362 File Offset: 0x000C3562
		// (set) Token: 0x06001FD4 RID: 8148 RVA: 0x000C5369 File Offset: 0x000C3569
		public static TextAnchor Anchor
		{
			get
			{
				return Text.anchorInt;
			}
			set
			{
				Text.anchorInt = value;
			}
		}

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x06001FD5 RID: 8149 RVA: 0x000C5371 File Offset: 0x000C3571
		// (set) Token: 0x06001FD6 RID: 8150 RVA: 0x000C5378 File Offset: 0x000C3578
		public static bool WordWrap
		{
			get
			{
				return Text.wordWrapInt;
			}
			set
			{
				Text.wordWrapInt = value;
			}
		}

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x06001FD7 RID: 8151 RVA: 0x000C5380 File Offset: 0x000C3580
		public static float LineHeight
		{
			get
			{
				return Text.lineHeights[(int)Text.Font];
			}
		}

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x06001FD8 RID: 8152 RVA: 0x000C538D File Offset: 0x000C358D
		public static float SpaceBetweenLines
		{
			get
			{
				return Text.spaceBetweenLines[(int)Text.Font];
			}
		}

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x06001FD9 RID: 8153 RVA: 0x000C539C File Offset: 0x000C359C
		public static GUIStyle CurFontStyle
		{
			get
			{
				GUIStyle guistyle;
				switch (Text.fontInt)
				{
				case GameFont.Tiny:
					guistyle = Text.fontStyles[0];
					break;
				case GameFont.Small:
					guistyle = Text.fontStyles[1];
					break;
				case GameFont.Medium:
					guistyle = Text.fontStyles[2];
					break;
				default:
					throw new NotImplementedException();
				}
				guistyle.alignment = Text.anchorInt;
				guistyle.wordWrap = Text.wordWrapInt;
				return guistyle;
			}
		}

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x06001FDA RID: 8154 RVA: 0x000C5400 File Offset: 0x000C3600
		public static GUIStyle CurTextFieldStyle
		{
			get
			{
				switch (Text.fontInt)
				{
				case GameFont.Tiny:
					return Text.textFieldStyles[0];
				case GameFont.Small:
					return Text.textFieldStyles[1];
				case GameFont.Medium:
					return Text.textFieldStyles[2];
				default:
					throw new NotImplementedException();
				}
			}
		}

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x06001FDB RID: 8155 RVA: 0x000C5444 File Offset: 0x000C3644
		public static GUIStyle CurTextAreaStyle
		{
			get
			{
				switch (Text.fontInt)
				{
				case GameFont.Tiny:
					return Text.textAreaStyles[0];
				case GameFont.Small:
					return Text.textAreaStyles[1];
				case GameFont.Medium:
					return Text.textAreaStyles[2];
				default:
					throw new NotImplementedException();
				}
			}
		}

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x06001FDC RID: 8156 RVA: 0x000C5488 File Offset: 0x000C3688
		public static GUIStyle CurTextAreaReadOnlyStyle
		{
			get
			{
				switch (Text.fontInt)
				{
				case GameFont.Tiny:
					return Text.textAreaReadOnlyStyles[0];
				case GameFont.Small:
					return Text.textAreaReadOnlyStyles[1];
				case GameFont.Medium:
					return Text.textAreaReadOnlyStyles[2];
				default:
					throw new NotImplementedException();
				}
			}
		}

		// Token: 0x06001FDD RID: 8157 RVA: 0x000C54CC File Offset: 0x000C36CC
		static Text()
		{
			Text.fonts[0] = (Font)Resources.Load("Fonts/Calibri_tiny");
			Text.fonts[1] = (Font)Resources.Load("Fonts/Arial_small");
			Text.fonts[2] = (Font)Resources.Load("Fonts/Arial_medium");
			Text.fontStyles[0] = new GUIStyle(GUI.skin.label);
			Text.fontStyles[0].font = Text.fonts[0];
			Text.fontStyles[1] = new GUIStyle(GUI.skin.label);
			Text.fontStyles[1].font = Text.fonts[1];
			Text.fontStyles[1].contentOffset = new Vector2(0f, -1f);
			Text.fontStyles[2] = new GUIStyle(GUI.skin.label);
			Text.fontStyles[2].font = Text.fonts[2];
			for (int i = 0; i < Text.textFieldStyles.Length; i++)
			{
				Text.textFieldStyles[i] = new GUIStyle(GUI.skin.textField);
				Text.textFieldStyles[i].alignment = TextAnchor.MiddleLeft;
			}
			Text.textFieldStyles[0].font = Text.fonts[0];
			Text.textFieldStyles[1].font = Text.fonts[1];
			Text.textFieldStyles[2].font = Text.fonts[2];
			for (int j = 0; j < Text.textAreaStyles.Length; j++)
			{
				Text.textAreaStyles[j] = new GUIStyle(Text.textFieldStyles[j]);
				Text.textAreaStyles[j].alignment = TextAnchor.UpperLeft;
				Text.textAreaStyles[j].wordWrap = true;
			}
			for (int k = 0; k < Text.textAreaReadOnlyStyles.Length; k++)
			{
				Text.textAreaReadOnlyStyles[k] = new GUIStyle(Text.textAreaStyles[k]);
				GUIStyle guistyle = Text.textAreaReadOnlyStyles[k];
				guistyle.normal.background = null;
				guistyle.active.background = null;
				guistyle.onHover.background = null;
				guistyle.hover.background = null;
				guistyle.onFocused.background = null;
				guistyle.focused.background = null;
			}
			GUI.skin.settings.doubleClickSelectsWord = true;
			int num = 0;
			foreach (object obj in Enum.GetValues(typeof(GameFont)))
			{
				Text.Font = (GameFont)obj;
				Text.lineHeights[num] = Text.CalcHeight("W", 999f);
				Text.spaceBetweenLines[num] = Text.CalcHeight("W\nW", 999f) - Text.CalcHeight("W", 999f) * 2f;
				num++;
			}
			Text.Font = GameFont.Small;
		}

		// Token: 0x06001FDE RID: 8158 RVA: 0x000C57F4 File Offset: 0x000C39F4
		public static float LineHeightOf(GameFont font)
		{
			return Text.lineHeights[(int)font];
		}

		// Token: 0x06001FDF RID: 8159 RVA: 0x000C57FD File Offset: 0x000C39FD
		public static float CalcHeight(string text, float width)
		{
			Text.tmpTextGUIContent.text = text.StripTags();
			return Text.CurFontStyle.CalcHeight(Text.tmpTextGUIContent, width);
		}

		// Token: 0x06001FE0 RID: 8160 RVA: 0x000C581F File Offset: 0x000C3A1F
		public static Vector2 CalcSize(string text)
		{
			Text.tmpTextGUIContent.text = text.StripTags();
			return Text.CurFontStyle.CalcSize(Text.tmpTextGUIContent);
		}

		// Token: 0x06001FE1 RID: 8161 RVA: 0x000C5840 File Offset: 0x000C3A40
		public static void StartOfOnGUI()
		{
			if (!Text.WordWrap)
			{
				Log.ErrorOnce("Word wrap was false at end of frame.", 764362);
				Text.WordWrap = true;
			}
			if (Text.Anchor != TextAnchor.UpperLeft)
			{
				Log.ErrorOnce("Alignment was " + Text.Anchor + " at end of frame.", 15558);
				Text.Anchor = TextAnchor.UpperLeft;
			}
			Text.Font = GameFont.Small;
		}

		// Token: 0x04001346 RID: 4934
		private static GameFont fontInt = GameFont.Small;

		// Token: 0x04001347 RID: 4935
		private static TextAnchor anchorInt = TextAnchor.UpperLeft;

		// Token: 0x04001348 RID: 4936
		private static bool wordWrapInt = true;

		// Token: 0x04001349 RID: 4937
		private static Font[] fonts = new Font[3];

		// Token: 0x0400134A RID: 4938
		public static readonly GUIStyle[] fontStyles = new GUIStyle[3];

		// Token: 0x0400134B RID: 4939
		public static readonly GUIStyle[] textFieldStyles = new GUIStyle[3];

		// Token: 0x0400134C RID: 4940
		public static readonly GUIStyle[] textAreaStyles = new GUIStyle[3];

		// Token: 0x0400134D RID: 4941
		public static readonly GUIStyle[] textAreaReadOnlyStyles = new GUIStyle[3];

		// Token: 0x0400134E RID: 4942
		private static readonly float[] lineHeights = new float[3];

		// Token: 0x0400134F RID: 4943
		private static readonly float[] spaceBetweenLines = new float[3];

		// Token: 0x04001350 RID: 4944
		private static GUIContent tmpTextGUIContent = new GUIContent();

		// Token: 0x04001351 RID: 4945
		private const int NumFonts = 3;

		// Token: 0x04001352 RID: 4946
		public const float SmallFontHeight = 22f;
	}
}
