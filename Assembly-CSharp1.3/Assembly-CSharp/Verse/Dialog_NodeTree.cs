using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000458 RID: 1112
	public class Dialog_NodeTree : Window
	{
		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x0600219B RID: 8603 RVA: 0x000D2280 File Offset: 0x000D0480
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

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x0600219C RID: 8604 RVA: 0x000D22E4 File Offset: 0x000D04E4
		private bool InteractiveNow
		{
			get
			{
				return Time.realtimeSinceStartup >= this.makeInteractiveAtTime;
			}
		}

		// Token: 0x0600219D RID: 8605 RVA: 0x000D22F8 File Offset: 0x000D04F8
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
				this.makeInteractiveAtTime = RealTime.LastRealTime + 1f;
			}
			this.soundAppear = SoundDefOf.CommsWindow_Open;
			this.soundClose = SoundDefOf.CommsWindow_Close;
			if (radioMode)
			{
				this.soundAmbient = SoundDefOf.RadioComms_Ambience;
			}
		}

		// Token: 0x0600219E RID: 8606 RVA: 0x000D2379 File Offset: 0x000D0579
		public override void PreClose()
		{
			base.PreClose();
			this.curNode.PreClose();
		}

		// Token: 0x0600219F RID: 8607 RVA: 0x000D238C File Offset: 0x000D058C
		public override void PostClose()
		{
			base.PostClose();
			if (this.closeAction != null)
			{
				this.closeAction();
			}
		}

		// Token: 0x060021A0 RID: 8608 RVA: 0x000D23A8 File Offset: 0x000D05A8
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

		// Token: 0x060021A1 RID: 8609 RVA: 0x000D2408 File Offset: 0x000D0608
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

		// Token: 0x060021A2 RID: 8610 RVA: 0x000D248C File Offset: 0x000D068C
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

		// Token: 0x060021A3 RID: 8611 RVA: 0x000D2640 File Offset: 0x000D0840
		public void GotoNode(DiaNode node)
		{
			foreach (DiaOption diaOption in node.options)
			{
				diaOption.dialog = this;
			}
			this.curNode = node;
		}

		// Token: 0x040014F9 RID: 5369
		private Vector2 scrollPosition;

		// Token: 0x040014FA RID: 5370
		private Vector2 optsScrollPosition;

		// Token: 0x040014FB RID: 5371
		protected string title;

		// Token: 0x040014FC RID: 5372
		protected DiaNode curNode;

		// Token: 0x040014FD RID: 5373
		public Action closeAction;

		// Token: 0x040014FE RID: 5374
		private float makeInteractiveAtTime;

		// Token: 0x040014FF RID: 5375
		public Color screenFillColor = Color.clear;

		// Token: 0x04001500 RID: 5376
		protected float minOptionsAreaHeight;

		// Token: 0x04001501 RID: 5377
		private const float InteractivityDelay = 1f;

		// Token: 0x04001502 RID: 5378
		private const float TitleHeight = 36f;

		// Token: 0x04001503 RID: 5379
		protected const float OptHorMargin = 15f;

		// Token: 0x04001504 RID: 5380
		protected const float OptVerticalSpace = 7f;

		// Token: 0x04001505 RID: 5381
		private const int ResizeIfMoreOptionsThan = 5;

		// Token: 0x04001506 RID: 5382
		private const float MinSpaceLeftForTextAfterOptionsResizing = 100f;

		// Token: 0x04001507 RID: 5383
		private float optTotalHeight;
	}
}
