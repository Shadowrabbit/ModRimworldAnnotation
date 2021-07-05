using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020013A6 RID: 5030
	public class QuickSearchWidget
	{
		// Token: 0x06007A76 RID: 31350 RVA: 0x002B3234 File Offset: 0x002B1434
		public QuickSearchWidget()
		{
			this.controlName = string.Format("QuickSearchWidget_{0}", QuickSearchWidget.instanceCounter++);
		}

		// Token: 0x06007A77 RID: 31351 RVA: 0x002B3274 File Offset: 0x002B1474
		public void OnGUI(Rect rect, Action onFilterChange = null)
		{
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
			{
				this.Unfocus();
				Event.current.Use();
			}
			if (OriginalEventUtility.EventType == EventType.MouseDown && !rect.Contains(Event.current.mousePosition))
			{
				this.Unfocus();
			}
			Color color = GUI.color;
			GUI.color = Color.white;
			float num = Mathf.Min(18f, rect.height);
			float num2 = num + 8f;
			float y = rect.y + (rect.height - num2) / 2f + 4f;
			Rect position = new Rect(rect.x + 4f, y, num, num);
			GUI.DrawTexture(position, TexButton.Search);
			GUI.SetNextControlName(this.controlName);
			Rect rect2 = new Rect(position.xMax + 4f, rect.y, rect.width - 2f * num2, rect.height);
			if (this.noResultsMatched && this.filter.Active)
			{
				GUI.color = ColorLibrary.RedReadable;
			}
			else if (!this.filter.Active && !this.CurrentlyFocused())
			{
				GUI.color = this.inactiveTextColor;
			}
			string text = Widgets.TextField(rect2, this.filter.Text, 15, null);
			GUI.color = Color.white;
			if (text != this.filter.Text)
			{
				this.filter.Text = text;
				if (onFilterChange != null)
				{
					onFilterChange();
				}
			}
			if (this.filter.Active && Widgets.ButtonImage(new Rect(rect2.xMax + 4f, y, num, num), TexButton.CloseXSmall, true))
			{
				this.filter.Text = "";
				SoundDefOf.CancelMode.PlayOneShotOnCamera(null);
				if (onFilterChange != null)
				{
					onFilterChange();
				}
			}
			GUI.color = color;
		}

		// Token: 0x06007A78 RID: 31352 RVA: 0x002B3455 File Offset: 0x002B1655
		public void Unfocus()
		{
			if (this.CurrentlyFocused())
			{
				UI.UnfocusCurrentControl();
			}
		}

		// Token: 0x06007A79 RID: 31353 RVA: 0x002B3464 File Offset: 0x002B1664
		private bool CurrentlyFocused()
		{
			return GUI.GetNameOfFocusedControl() == this.controlName;
		}

		// Token: 0x06007A7A RID: 31354 RVA: 0x002B3476 File Offset: 0x002B1676
		public void Reset()
		{
			this.filter.Text = "";
			this.noResultsMatched = false;
		}

		// Token: 0x06007A7B RID: 31355 RVA: 0x002B348F File Offset: 0x002B168F
		public static void DrawStrongHighlight(Rect rect)
		{
			Color color = GUI.color;
			GUI.color = QuickSearchWidget.HighlightStrongBgColor;
			Widgets.DrawAtlas(rect, TexUI.RectHighlight);
			GUI.color = color;
		}

		// Token: 0x06007A7C RID: 31356 RVA: 0x002B34B0 File Offset: 0x002B16B0
		public static void DrawTextHighlight(Rect rect, float expandBy = 4f)
		{
			rect.y -= expandBy;
			rect.height += expandBy * 2f;
			Color color = GUI.color;
			GUI.color = QuickSearchWidget.HighlightTextBgColor;
			GUI.DrawTexture(rect, TexUI.RectTextHighlight);
			GUI.color = color;
		}

		// Token: 0x040043B0 RID: 17328
		public const float WidgetHeight = 24f;

		// Token: 0x040043B1 RID: 17329
		public const float IconSize = 18f;

		// Token: 0x040043B2 RID: 17330
		public const float IconMargin = 4f;

		// Token: 0x040043B3 RID: 17331
		private const int MaxSearchTextLength = 15;

		// Token: 0x040043B4 RID: 17332
		public static readonly Color HighlightStrongBgColor = ColorLibrary.SkyBlue;

		// Token: 0x040043B5 RID: 17333
		public static readonly Color HighlightTextBgColor = QuickSearchWidget.HighlightStrongBgColor.ToTransparent(0.25f);

		// Token: 0x040043B6 RID: 17334
		private static int instanceCounter;

		// Token: 0x040043B7 RID: 17335
		public QuickSearchFilter filter = new QuickSearchFilter();

		// Token: 0x040043B8 RID: 17336
		public bool noResultsMatched;

		// Token: 0x040043B9 RID: 17337
		public Color inactiveTextColor = Color.white;

		// Token: 0x040043BA RID: 17338
		private readonly string controlName;
	}
}
