using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000746 RID: 1862
	public class Listing_Tree : Listing_Lines
	{
		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x06002EE5 RID: 12005 RVA: 0x00024CDE File Offset: 0x00022EDE
		protected virtual float LabelWidth
		{
			get
			{
				return base.ColumnWidth - 26f;
			}
		}

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x06002EE6 RID: 12006 RVA: 0x00024CEC File Offset: 0x00022EEC
		protected float EditAreaWidth
		{
			get
			{
				return base.ColumnWidth - this.LabelWidth;
			}
		}

		// Token: 0x06002EE7 RID: 12007 RVA: 0x00024CFB File Offset: 0x00022EFB
		public override void Begin(Rect rect)
		{
			base.Begin(rect);
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
		}

		// Token: 0x06002EE8 RID: 12008 RVA: 0x00024D10 File Offset: 0x00022F10
		public override void End()
		{
			base.End();
			Text.WordWrap = true;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06002EE9 RID: 12009 RVA: 0x00024D24 File Offset: 0x00022F24
		protected float XAtIndentLevel(int indentLevel)
		{
			return (float)indentLevel * this.nestIndentWidth;
		}

		// Token: 0x06002EEA RID: 12010 RVA: 0x001394E0 File Offset: 0x001376E0
		protected void LabelLeft(string label, string tipText, int indentLevel, float widthOffset = 0f)
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
			rect.width = this.LabelWidth - rect.xMin + widthOffset;
			rect.yMax += 5f;
			rect.yMin -= 5f;
			Widgets.Label(rect, label.Truncate(rect.width, null));
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06002EEB RID: 12011 RVA: 0x001395AC File Offset: 0x001377AC
		protected bool OpenCloseWidget(TreeNode node, int indentLevel, int openMask)
		{
			if (!node.Openable)
			{
				return false;
			}
			float x = this.XAtIndentLevel(indentLevel);
			float y = this.curY + this.lineHeight / 2f - 9f;
			Rect butRect = new Rect(x, y, 18f, 18f);
			Texture2D tex = node.IsOpen(openMask) ? TexButton.Collapse : TexButton.Reveal;
			if (Widgets.ButtonImage(butRect, tex, true))
			{
				bool flag = node.IsOpen(openMask);
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

		// Token: 0x06002EEC RID: 12012 RVA: 0x00139648 File Offset: 0x00137848
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

		// Token: 0x06002EED RID: 12013 RVA: 0x001396BC File Offset: 0x001378BC
		public bool ButtonText(string label)
		{
			Text.WordWrap = true;
			float num = Text.CalcHeight(label, base.ColumnWidth);
			bool result = Widgets.ButtonText(new Rect(0f, this.curY, base.ColumnWidth, num), label, true, true, true);
			this.curY += num + 0f;
			Text.WordWrap = false;
			return result;
		}

		// Token: 0x06002EEE RID: 12014 RVA: 0x00024D2F File Offset: 0x00022F2F
		public WidgetRow StartWidgetsRow(int indentLevel)
		{
			WidgetRow result = new WidgetRow(this.LabelWidth, this.curY, UIDirection.RightThenUp, 99999f, 4f);
			this.curY += 24f;
			return result;
		}

		// Token: 0x04001FDA RID: 8154
		public float nestIndentWidth = 11f;

		// Token: 0x04001FDB RID: 8155
		protected const float OpenCloseWidgetSize = 18f;
	}
}
