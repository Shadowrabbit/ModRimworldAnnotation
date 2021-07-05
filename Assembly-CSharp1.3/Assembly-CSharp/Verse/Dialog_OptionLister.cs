using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200044F RID: 1103
	public abstract class Dialog_OptionLister : Window
	{
		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x06002179 RID: 8569 RVA: 0x000B9F29 File Offset: 0x000B8129
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)UI.screenWidth, (float)UI.screenHeight);
			}
		}

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x0600217A RID: 8570 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600217B RID: 8571 RVA: 0x000D1388 File Offset: 0x000CF588
		public Dialog_OptionLister()
		{
			this.doCloseX = true;
			this.onlyOneOfTypeAllowed = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x0600217C RID: 8572 RVA: 0x000D13B7 File Offset: 0x000CF5B7
		public override void PostOpen()
		{
			base.PostOpen();
			if (this.focusOnFilterOnOpen)
			{
				this.focusFilter = true;
			}
		}

		// Token: 0x0600217D RID: 8573 RVA: 0x000D13D0 File Offset: 0x000CF5D0
		public override void DoWindowContents(Rect inRect)
		{
			GUI.SetNextControlName("DebugFilter");
			if (Event.current.type == EventType.KeyDown && (KeyBindingDefOf.Dev_ToggleDebugSettingsMenu.KeyDownEvent || KeyBindingDefOf.Dev_ToggleDebugActionsMenu.KeyDownEvent))
			{
				return;
			}
			this.filter = Widgets.TextField(new Rect(0f, 0f, 200f, 30f), this.filter);
			if ((Event.current.type == EventType.KeyDown || Event.current.type == EventType.Repaint) && this.focusFilter)
			{
				GUI.FocusControl("DebugFilter");
				this.filter = "";
				this.focusFilter = false;
			}
			if (Event.current.type == EventType.Layout)
			{
				this.totalOptionsHeight = 0f;
			}
			Rect outRect = new Rect(inRect);
			outRect.yMin += 35f;
			int num = (int)(this.InitialSize.x / 200f);
			float num2 = (this.totalOptionsHeight + 24f * (float)(num - 1)) / (float)num;
			if (num2 < outRect.height)
			{
				num2 = outRect.height;
			}
			Rect rect = new Rect(0f, 0f, outRect.width - 16f, num2);
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, rect, true);
			this.listing = new Listing_Standard();
			this.listing.ColumnWidth = (rect.width - 17f * (float)(num - 1)) / (float)num;
			this.listing.Begin(rect);
			this.DoListingItems();
			this.listing.End();
			Widgets.EndScrollView();
		}

		// Token: 0x0600217E RID: 8574 RVA: 0x000D155C File Offset: 0x000CF75C
		public override void PostClose()
		{
			base.PostClose();
			UI.UnfocusCurrentControl();
		}

		// Token: 0x0600217F RID: 8575
		protected abstract void DoListingItems();

		// Token: 0x06002180 RID: 8576 RVA: 0x000D1569 File Offset: 0x000CF769
		protected bool FilterAllows(string label)
		{
			return this.filter.NullOrEmpty() || label.NullOrEmpty() || label.IndexOf(this.filter, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x040014D0 RID: 5328
		protected Vector2 scrollPosition;

		// Token: 0x040014D1 RID: 5329
		protected string filter = "";

		// Token: 0x040014D2 RID: 5330
		protected float totalOptionsHeight;

		// Token: 0x040014D3 RID: 5331
		protected Listing_Standard listing;

		// Token: 0x040014D4 RID: 5332
		protected bool focusOnFilterOnOpen = true;

		// Token: 0x040014D5 RID: 5333
		private bool focusFilter;

		// Token: 0x040014D6 RID: 5334
		protected const string FilterControlName = "DebugFilter";
	}
}
