using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000758 RID: 1880
	public static class Text
	{
		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x06002F69 RID: 12137 RVA: 0x00025295 File Offset: 0x00023495
		// (set) Token: 0x06002F6A RID: 12138 RVA: 0x0002529C File Offset: 0x0002349C
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

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x06002F6B RID: 12139 RVA: 0x000252C6 File Offset: 0x000234C6
		// (set) Token: 0x06002F6C RID: 12140 RVA: 0x000252CD File Offset: 0x000234CD
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

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x06002F6D RID: 12141 RVA: 0x000252D5 File Offset: 0x000234D5
		// (set) Token: 0x06002F6E RID: 12142 RVA: 0x000252DC File Offset: 0x000234DC
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

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x06002F6F RID: 12143 RVA: 0x000252E4 File Offset: 0x000234E4
		public static float LineHeight
		{
			get
			{
				return Text.lineHeights[(int)Text.Font];
			}
		}

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x06002F70 RID: 12144 RVA: 0x000252F1 File Offset: 0x000234F1
		public static float SpaceBetweenLines
		{
			get
			{
				return Text.spaceBetweenLines[(int)Text.Font];
			}
		}

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x06002F71 RID: 12145 RVA: 0x0013AF74 File Offset: 0x00139174
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

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x06002F72 RID: 12146 RVA: 0x0013AFD8 File Offset: 0x001391D8
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

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x06002F73 RID: 12147 RVA: 0x0013B01C File Offset: 0x0013921C
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

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x06002F74 RID: 12148 RVA: 0x0013B060 File Offset: 0x00139260
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

		// Token: 0x06002F75 RID: 12149 RVA: 0x0013B0A4 File Offset: 0x001392A4
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

		// Token: 0x06002F76 RID: 12150 RVA: 0x000252FE File Offset: 0x000234FE
		public static float CalcHeight(string text, float width)
		{
			Text.tmpTextGUIContent.text = text.StripTags();
			return Text.CurFontStyle.CalcHeight(Text.tmpTextGUIContent, width);
		}

		// Token: 0x06002F77 RID: 12151 RVA: 0x00025320 File Offset: 0x00023520
		public static Vector2 CalcSize(string text)
		{
			Text.tmpTextGUIContent.text = text.StripTags();
			return Text.CurFontStyle.CalcSize(Text.tmpTextGUIContent);
		}

		// Token: 0x06002F78 RID: 12152 RVA: 0x0013B3CC File Offset: 0x001395CC
		public static void StartOfOnGUI()
		{
			if (!Text.WordWrap)
			{
				Log.ErrorOnce("Word wrap was false at end of frame.", 764362, false);
				Text.WordWrap = true;
			}
			if (Text.Anchor != TextAnchor.UpperLeft)
			{
				Log.ErrorOnce("Alignment was " + Text.Anchor + " at end of frame.", 15558, false);
				Text.Anchor = TextAnchor.UpperLeft;
			}
			Text.Font = GameFont.Small;
		}

		// Token: 0x04002020 RID: 8224
		private static GameFont fontInt = GameFont.Small;

		// Token: 0x04002021 RID: 8225
		private static TextAnchor anchorInt = TextAnchor.UpperLeft;

		// Token: 0x04002022 RID: 8226
		private static bool wordWrapInt = true;

		// Token: 0x04002023 RID: 8227
		private static Font[] fonts = new Font[3];

		// Token: 0x04002024 RID: 8228
		public static readonly GUIStyle[] fontStyles = new GUIStyle[3];

		// Token: 0x04002025 RID: 8229
		public static readonly GUIStyle[] textFieldStyles = new GUIStyle[3];

		// Token: 0x04002026 RID: 8230
		public static readonly GUIStyle[] textAreaStyles = new GUIStyle[3];

		// Token: 0x04002027 RID: 8231
		public static readonly GUIStyle[] textAreaReadOnlyStyles = new GUIStyle[3];

		// Token: 0x04002028 RID: 8232
		private static readonly float[] lineHeights = new float[3];

		// Token: 0x04002029 RID: 8233
		private static readonly float[] spaceBetweenLines = new float[3];

		// Token: 0x0400202A RID: 8234
		private static GUIContent tmpTextGUIContent = new GUIContent();

		// Token: 0x0400202B RID: 8235
		private const int NumFonts = 3;

		// Token: 0x0400202C RID: 8236
		public const float SmallFontHeight = 22f;
	}
}
