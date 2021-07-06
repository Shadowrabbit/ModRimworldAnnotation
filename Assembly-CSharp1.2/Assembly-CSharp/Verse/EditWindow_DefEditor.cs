using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006EE RID: 1774
	internal class EditWindow_DefEditor : EditWindow
	{
		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x06002D1C RID: 11548 RVA: 0x0001D889 File Offset: 0x0001BA89
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(400f, 600f);
			}
		}

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x06002D1D RID: 11549 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002D1E RID: 11550 RVA: 0x00023A3B File Offset: 0x00021C3B
		public EditWindow_DefEditor(Def def)
		{
			this.def = def;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.optionalTitle = def.ToString();
		}

		// Token: 0x06002D1F RID: 11551 RVA: 0x00131E38 File Offset: 0x00130038
		public override void DoWindowContents(Rect inRect)
		{
			if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Escape))
			{
				UI.UnfocusCurrentControl();
			}
			Rect rect = new Rect(0f, 0f, inRect.width, 16f);
			this.labelColumnWidth = Widgets.HorizontalSlider(rect, this.labelColumnWidth, 0f, inRect.width, false, null, null, null, -1f);
			Rect outRect = inRect.AtZero();
			outRect.yMin += 16f;
			Rect rect2 = new Rect(0f, 0f, outRect.width - 16f, this.viewHeight);
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, rect2, true);
			Listing_TreeDefs listing_TreeDefs = new Listing_TreeDefs(this.labelColumnWidth);
			listing_TreeDefs.Begin(rect2);
			TreeNode_Editor node = EditTreeNodeDatabase.RootOf(this.def);
			listing_TreeDefs.ContentLines(node, 0);
			listing_TreeDefs.End();
			if (Event.current.type == EventType.Layout)
			{
				this.viewHeight = listing_TreeDefs.CurHeight + 200f;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x04001E87 RID: 7815
		public Def def;

		// Token: 0x04001E88 RID: 7816
		private float viewHeight;

		// Token: 0x04001E89 RID: 7817
		private Vector2 scrollPosition;

		// Token: 0x04001E8A RID: 7818
		private float labelColumnWidth = 140f;

		// Token: 0x04001E8B RID: 7819
		private const float TopAreaHeight = 16f;

		// Token: 0x04001E8C RID: 7820
		private const float ExtraScrollHeight = 200f;
	}
}
