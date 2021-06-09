using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020007A0 RID: 1952
	public abstract class Dialog_OptionLister : Window
	{
		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x06003131 RID: 12593 RVA: 0x00023846 File Offset: 0x00021A46
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)UI.screenWidth, (float)UI.screenHeight);
			}
		}

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06003132 RID: 12594 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003133 RID: 12595 RVA: 0x00026CFE File Offset: 0x00024EFE
		public Dialog_OptionLister()
		{
			this.doCloseX = true;
			this.onlyOneOfTypeAllowed = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06003134 RID: 12596 RVA: 0x00026D2D File Offset: 0x00024F2D
		public override void PostOpen()
		{
			base.PostOpen();
			if (this.focusOnFilterOnOpen)
			{
				this.focusFilter = true;
			}
		}

		// Token: 0x06003135 RID: 12597 RVA: 0x00144960 File Offset: 0x00142B60
		public override void DoWindowContents(Rect inRect)
		{
			GUI.SetNextControlName("DebugFilter");
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

		// Token: 0x06003136 RID: 12598 RVA: 0x00026D44 File Offset: 0x00024F44
		public override void PostClose()
		{
			base.PostClose();
			UI.UnfocusCurrentControl();
		}

		// Token: 0x06003137 RID: 12599
		protected abstract void DoListingItems();

		// Token: 0x06003138 RID: 12600 RVA: 0x00026D51 File Offset: 0x00024F51
		protected bool FilterAllows(string label)
		{
			return this.filter.NullOrEmpty() || label.NullOrEmpty() || label.IndexOf(this.filter, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x040021E9 RID: 8681
		protected Vector2 scrollPosition;

		// Token: 0x040021EA RID: 8682
		protected string filter = "";

		// Token: 0x040021EB RID: 8683
		protected float totalOptionsHeight;

		// Token: 0x040021EC RID: 8684
		protected Listing_Standard listing;

		// Token: 0x040021ED RID: 8685
		protected bool focusOnFilterOnOpen = true;

		// Token: 0x040021EE RID: 8686
		private bool focusFilter;

		// Token: 0x040021EF RID: 8687
		protected const string FilterControlName = "DebugFilter";
	}
}
