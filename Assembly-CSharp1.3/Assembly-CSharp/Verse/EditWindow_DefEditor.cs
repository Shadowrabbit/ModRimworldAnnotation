using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003DE RID: 990
	internal class EditWindow_DefEditor : EditWindow
	{
		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06001DE6 RID: 7654 RVA: 0x0009A7CA File Offset: 0x000989CA
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(400f, 600f);
			}
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06001DE7 RID: 7655 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001DE8 RID: 7656 RVA: 0x000BAE77 File Offset: 0x000B9077
		public EditWindow_DefEditor(Def def)
		{
			this.def = def;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.optionalTitle = def.ToString();
		}

		// Token: 0x06001DE9 RID: 7657 RVA: 0x000BAEAC File Offset: 0x000B90AC
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

		// Token: 0x040011F0 RID: 4592
		public Def def;

		// Token: 0x040011F1 RID: 4593
		private float viewHeight;

		// Token: 0x040011F2 RID: 4594
		private Vector2 scrollPosition;

		// Token: 0x040011F3 RID: 4595
		private float labelColumnWidth = 140f;

		// Token: 0x040011F4 RID: 4596
		private const float TopAreaHeight = 16f;

		// Token: 0x040011F5 RID: 4597
		private const float ExtraScrollHeight = 200f;
	}
}
