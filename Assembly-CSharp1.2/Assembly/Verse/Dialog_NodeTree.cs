using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020007A1 RID: 1953
	public class Dialog_NodeTree : Window
	{
		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06003139 RID: 12601 RVA: 0x00144AC8 File Offset: 0x00142CC8
		public override Vector2 InitialSize
		{
			get
			{
				int num = 480;
				if (this.curNode.options.Count > 5)
				{
					Text.Font = GameFont.Small;
					num += (this.curNode.options.Count - 5) * (int)(Text.LineHeight + 7f);
				}
				return new Vector2(620f, (float)Mathf.Min(num, UI.screenHeight));
			}
		}

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x0600313A RID: 12602 RVA: 0x00026D7F File Offset: 0x00024F7F
		private bool InteractiveNow
		{
			get
			{
				return Time.realtimeSinceStartup >= this.makeInteractiveAtTime;
			}
		}

		// Token: 0x0600313B RID: 12603 RVA: 0x00144B2C File Offset: 0x00142D2C
		public Dialog_NodeTree(DiaNode nodeRoot, bool delayInteractivity = false, bool radioMode = false, string title = null)
		{
			this.title = title;
			this.GotoNode(nodeRoot);
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			if (delayInteractivity)
			{
				this.makeInteractiveAtTime = RealTime.LastRealTime + 0.5f;
			}
			this.soundAppear = SoundDefOf.CommsWindow_Open;
			this.soundClose = SoundDefOf.CommsWindow_Close;
			if (radioMode)
			{
				this.soundAmbient = SoundDefOf.RadioComms_Ambience;
			}
		}

		// Token: 0x0600313C RID: 12604 RVA: 0x00026D91 File Offset: 0x00024F91
		public override void PreClose()
		{
			base.PreClose();
			this.curNode.PreClose();
		}

		// Token: 0x0600313D RID: 12605 RVA: 0x00026DA4 File Offset: 0x00024FA4
		public override void PostClose()
		{
			base.PostClose();
			if (this.closeAction != null)
			{
				this.closeAction();
			}
		}

		// Token: 0x0600313E RID: 12606 RVA: 0x00144BB0 File Offset: 0x00142DB0
		public override void WindowOnGUI()
		{
			if (this.screenFillColor != Color.clear)
			{
				GUI.color = this.screenFillColor;
				GUI.DrawTexture(new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight), BaseContent.WhiteTex);
				GUI.color = Color.white;
			}
			base.WindowOnGUI();
		}

		// Token: 0x0600313F RID: 12607 RVA: 0x00144C10 File Offset: 0x00142E10
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = inRect.AtZero();
			if (this.title != null)
			{
				Text.Font = GameFont.Small;
				Rect rect2 = rect;
				rect2.height = 36f;
				rect.yMin += 53f;
				Widgets.DrawTitleBG(rect2);
				rect2.xMin += 9f;
				rect2.yMin += 5f;
				Widgets.Label(rect2, this.title);
			}
			this.DrawNode(rect);
		}

		// Token: 0x06003140 RID: 12608 RVA: 0x00144C94 File Offset: 0x00142E94
		protected void DrawNode(Rect rect)
		{
			GUI.BeginGroup(rect);
			Text.Font = GameFont.Small;
			float num = Mathf.Min(this.optTotalHeight, rect.height - 100f - this.Margin * 2f);
			Rect outRect = new Rect(0f, 0f, rect.width, rect.height - Mathf.Max(num, this.minOptionsAreaHeight));
			float width = rect.width - 16f;
			Rect rect2 = new Rect(0f, 0f, width, Text.CalcHeight(this.curNode.text, width));
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, rect2, true);
			Widgets.Label(rect2, this.curNode.text.Resolve());
			Widgets.EndScrollView();
			Widgets.BeginScrollView(new Rect(0f, rect.height - num, rect.width, num), ref this.optsScrollPosition, new Rect(0f, 0f, rect.width - 16f, this.optTotalHeight), true);
			float num2 = 0f;
			float num3 = 0f;
			for (int i = 0; i < this.curNode.options.Count; i++)
			{
				Rect rect3 = new Rect(15f, num2, rect.width - 30f, 999f);
				float num4 = this.curNode.options[i].OptOnGUI(rect3, this.InteractiveNow);
				num2 += num4 + 7f;
				num3 += num4 + 7f;
			}
			if (Event.current.type == EventType.Layout)
			{
				this.optTotalHeight = num3;
			}
			Widgets.EndScrollView();
			GUI.EndGroup();
		}

		// Token: 0x06003141 RID: 12609 RVA: 0x00144E48 File Offset: 0x00143048
		public void GotoNode(DiaNode node)
		{
			foreach (DiaOption diaOption in node.options)
			{
				diaOption.dialog = this;
			}
			this.curNode = node;
		}

		// Token: 0x040021F0 RID: 8688
		private Vector2 scrollPosition;

		// Token: 0x040021F1 RID: 8689
		private Vector2 optsScrollPosition;

		// Token: 0x040021F2 RID: 8690
		protected string title;

		// Token: 0x040021F3 RID: 8691
		protected DiaNode curNode;

		// Token: 0x040021F4 RID: 8692
		public Action closeAction;

		// Token: 0x040021F5 RID: 8693
		private float makeInteractiveAtTime;

		// Token: 0x040021F6 RID: 8694
		public Color screenFillColor = Color.clear;

		// Token: 0x040021F7 RID: 8695
		protected float minOptionsAreaHeight;

		// Token: 0x040021F8 RID: 8696
		private const float InteractivityDelay = 0.5f;

		// Token: 0x040021F9 RID: 8697
		private const float TitleHeight = 36f;

		// Token: 0x040021FA RID: 8698
		protected const float OptHorMargin = 15f;

		// Token: 0x040021FB RID: 8699
		protected const float OptVerticalSpace = 7f;

		// Token: 0x040021FC RID: 8700
		private const int ResizeIfMoreOptionsThan = 5;

		// Token: 0x040021FD RID: 8701
		private const float MinSpaceLeftForTextAfterOptionsResizing = 100f;

		// Token: 0x040021FE RID: 8702
		private float optTotalHeight;
	}
}
