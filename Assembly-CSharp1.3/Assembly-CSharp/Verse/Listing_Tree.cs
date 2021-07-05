using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000414 RID: 1044
	public class Listing_Tree : Listing_Lines
	{
		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x06001F53 RID: 8019 RVA: 0x000C3281 File Offset: 0x000C1481
		protected virtual float LabelWidth
		{
			get
			{
				return base.ColumnWidth - 26f;
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x06001F54 RID: 8020 RVA: 0x000C328F File Offset: 0x000C148F
		protected float EditAreaWidth
		{
			get
			{
				return base.ColumnWidth - this.LabelWidth;
			}
		}

		// Token: 0x06001F55 RID: 8021 RVA: 0x000C329E File Offset: 0x000C149E
		public override void Begin(Rect rect)
		{
			base.Begin(rect);
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
		}

		// Token: 0x06001F56 RID: 8022 RVA: 0x000C32B3 File Offset: 0x000C14B3
		public override void End()
		{
			base.End();
			Text.WordWrap = true;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06001F57 RID: 8023 RVA: 0x000C32C7 File Offset: 0x000C14C7
		protected float XAtIndentLevel(int indentLevel)
		{
			return (float)indentLevel * this.nestIndentWidth;
		}

		// Token: 0x06001F58 RID: 8024 RVA: 0x000C32D4 File Offset: 0x000C14D4
		protected void LabelLeft(string label, string tipText, int indentLevel, float widthOffset = 0f, Color? textColor = null)
		{
			Rect rect = new Rect(0f, this.curY, base.ColumnWidth, this.lineHeight)
			{
				xMin = this.XAtIndentLevel(indentLevel) + 18f
			};
			Widgets.DrawHighlightIfMouseover(rect);
			if (!tipText.NullOrEmpty())
			{
				if (Mouse.IsOver(rect))
				{
					GUI.DrawTexture(rect, TexUI.HighlightTex);
				}
				TooltipHandler.TipRegion(rect, tipText);
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			GUI.color = (textColor ?? Color.white);
			rect.width = this.LabelWidth - rect.xMin + widthOffset;
			rect.yMax += 5f;
			rect.yMin -= 5f;
			Widgets.Label(rect, label.Truncate(rect.width, null));
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
		}

		// Token: 0x06001F59 RID: 8025 RVA: 0x000C33C8 File Offset: 0x000C15C8
		protected bool OpenCloseWidget(TreeNode node, int indentLevel, int openMask)
		{
			if (!node.Openable)
			{
				return false;
			}
			float x = this.XAtIndentLevel(indentLevel);
			float y = this.curY + this.lineHeight / 2f - 9f;
			Rect butRect = new Rect(x, y, 18f, 18f);
			bool flag = this.IsOpen(node, openMask);
			Texture2D tex = flag ? TexButton.Collapse : TexButton.Reveal;
			if (Widgets.ButtonImage(butRect, tex, true))
			{
				if (flag)
				{
					SoundDefOf.TabClose.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
				}
				node.SetOpen(openMask, !flag);
				return true;
			}
			return false;
		}

		// Token: 0x06001F5A RID: 8026 RVA: 0x000C345D File Offset: 0x000C165D
		public virtual bool IsOpen(TreeNode node, int openMask)
		{
			return node.IsOpen(openMask);
		}

		// Token: 0x06001F5B RID: 8027 RVA: 0x000C346C File Offset: 0x000C166C
		public void InfoText(string text, int indentLevel)
		{
			Text.WordWrap = true;
			Rect rect = new Rect(0f, this.curY, base.ColumnWidth, 50f);
			rect.xMin = this.LabelWidth;
			rect.height = Text.CalcHeight(text, rect.width);
			Widgets.Label(rect, text);
			this.curY += rect.height;
			Text.WordWrap = false;
		}

		// Token: 0x06001F5C RID: 8028 RVA: 0x000C34E0 File Offset: 0x000C16E0
		public bool ButtonText(string label)
		{
			Text.WordWrap = true;
			float num = Text.CalcHeight(label, base.ColumnWidth);
			bool result = Widgets.ButtonText(new Rect(0f, this.curY, base.ColumnWidth, num), label, true, true, true);
			this.curY += num + 0f;
			Text.WordWrap = false;
			return result;
		}

		// Token: 0x06001F5D RID: 8029 RVA: 0x000C353A File Offset: 0x000C173A
		public WidgetRow StartWidgetsRow(int indentLevel)
		{
			WidgetRow result = new WidgetRow(this.LabelWidth, this.curY, UIDirection.RightThenUp, 99999f, 4f);
			this.curY += 24f;
			return result;
		}

		// Token: 0x04001309 RID: 4873
		public float nestIndentWidth = 11f;

		// Token: 0x0400130A RID: 4874
		protected const float OpenCloseWidgetSize = 18f;
	}
}
