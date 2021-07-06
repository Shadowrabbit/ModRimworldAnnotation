using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000786 RID: 1926
	public class WidgetRow
	{
		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x0600303E RID: 12350 RVA: 0x00025FAE File Offset: 0x000241AE
		public float FinalX
		{
			get
			{
				return this.curX;
			}
		}

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x0600303F RID: 12351 RVA: 0x00025FB6 File Offset: 0x000241B6
		public float FinalY
		{
			get
			{
				return this.curY;
			}
		}

		// Token: 0x06003040 RID: 12352 RVA: 0x00025FBE File Offset: 0x000241BE
		public WidgetRow()
		{
		}

		// Token: 0x06003041 RID: 12353 RVA: 0x00025FD8 File Offset: 0x000241D8
		public WidgetRow(float x, float y, UIDirection growDirection = UIDirection.RightThenUp, float maxWidth = 99999f, float gap = 4f)
		{
			this.Init(x, y, growDirection, maxWidth, gap);
		}

		// Token: 0x06003042 RID: 12354 RVA: 0x00025FFF File Offset: 0x000241FF
		public void Init(float x, float y, UIDirection growDirection = UIDirection.RightThenUp, float maxWidth = 99999f, float gap = 4f)
		{
			this.growDirection = growDirection;
			this.startX = x;
			this.curX = x;
			this.curY = y;
			this.maxWidth = maxWidth;
			this.gap = gap;
		}

		// Token: 0x06003043 RID: 12355 RVA: 0x0002602D File Offset: 0x0002422D
		private float LeftX(float elementWidth)
		{
			if (this.growDirection == UIDirection.RightThenUp || this.growDirection == UIDirection.RightThenDown)
			{
				return this.curX;
			}
			return this.curX - elementWidth;
		}

		// Token: 0x06003044 RID: 12356 RVA: 0x0013F004 File Offset: 0x0013D204
		private void IncrementPosition(float amount)
		{
			if (this.growDirection == UIDirection.RightThenUp || this.growDirection == UIDirection.RightThenDown)
			{
				this.curX += amount;
			}
			else
			{
				this.curX -= amount;
			}
			if (Mathf.Abs(this.curX - this.startX) > this.maxWidth)
			{
				this.IncrementY();
			}
		}

		// Token: 0x06003045 RID: 12357 RVA: 0x0013F064 File Offset: 0x0013D264
		private void IncrementY()
		{
			if (this.growDirection == UIDirection.RightThenUp || this.growDirection == UIDirection.LeftThenUp)
			{
				this.curY -= 24f + this.gap;
			}
			else
			{
				this.curY += 24f + this.gap;
			}
			this.curX = this.startX;
		}

		// Token: 0x06003046 RID: 12358 RVA: 0x00026050 File Offset: 0x00024250
		private void IncrementYIfWillExceedMaxWidth(float width)
		{
			if (Mathf.Abs(this.curX - this.startX) + Mathf.Abs(width) > this.maxWidth)
			{
				this.IncrementY();
			}
		}

		// Token: 0x06003047 RID: 12359 RVA: 0x00026079 File Offset: 0x00024279
		public void Gap(float width)
		{
			if (this.curX != this.startX)
			{
				this.IncrementPosition(width);
			}
		}

		// Token: 0x06003048 RID: 12360 RVA: 0x0013F0C4 File Offset: 0x0013D2C4
		public bool ButtonIcon(Texture2D tex, string tooltip = null, Color? mouseoverColor = null, bool doMouseoverSound = true)
		{
			this.IncrementYIfWillExceedMaxWidth(24f);
			Rect rect = new Rect(this.LeftX(24f), this.curY, 24f, 24f);
			if (doMouseoverSound)
			{
				MouseoverSounds.DoRegion(rect);
			}
			bool result = Widgets.ButtonImage(rect, tex, Color.white, mouseoverColor ?? GenUI.MouseoverColor, true);
			this.IncrementPosition(24f + this.gap);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			return result;
		}

		// Token: 0x06003049 RID: 12361 RVA: 0x00026090 File Offset: 0x00024290
		public void GapButtonIcon()
		{
			if (this.curY != this.startX)
			{
				this.IncrementPosition(24f + this.gap);
			}
		}

		// Token: 0x0600304A RID: 12362 RVA: 0x0013F154 File Offset: 0x0013D354
		public void ToggleableIcon(ref bool toggleable, Texture2D tex, string tooltip, SoundDef mouseoverSound = null, string tutorTag = null)
		{
			this.IncrementYIfWillExceedMaxWidth(24f);
			Rect rect = new Rect(this.LeftX(24f), this.curY, 24f, 24f);
			bool flag = Widgets.ButtonImage(rect, tex, true);
			this.IncrementPosition(24f + this.gap);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			Rect position = new Rect(rect.x + rect.width / 2f, rect.y, rect.height / 2f, rect.height / 2f);
			Texture2D image = toggleable ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex;
			GUI.DrawTexture(position, image);
			if (mouseoverSound != null)
			{
				MouseoverSounds.DoRegion(rect, mouseoverSound);
			}
			if (flag)
			{
				toggleable = !toggleable;
				if (toggleable)
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
			}
			if (tutorTag != null)
			{
				UIHighlighter.HighlightOpportunity(rect, tutorTag);
			}
		}

		// Token: 0x0600304B RID: 12363 RVA: 0x0013F24C File Offset: 0x0013D44C
		public Rect Icon(Texture2D tex, string tooltip = null)
		{
			this.IncrementYIfWillExceedMaxWidth(24f);
			Rect rect = new Rect(this.LeftX(24f), this.curY, 24f, 24f);
			GUI.DrawTexture(rect, tex);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			this.IncrementPosition(24f + this.gap);
			return rect;
		}

		// Token: 0x0600304C RID: 12364 RVA: 0x0013F2B4 File Offset: 0x0013D4B4
		public bool ButtonText(string label, string tooltip = null, bool drawBackground = true, bool doMouseoverSound = true)
		{
			Vector2 vector = Text.CalcSize(label);
			vector.x += 16f;
			vector.y += 2f;
			this.IncrementYIfWillExceedMaxWidth(vector.x);
			Rect rect = new Rect(this.LeftX(vector.x), this.curY, vector.x, vector.y);
			bool result = Widgets.ButtonText(rect, label, drawBackground, doMouseoverSound, true);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			this.IncrementPosition(rect.width + this.gap);
			return result;
		}

		// Token: 0x0600304D RID: 12365 RVA: 0x0013F34C File Offset: 0x0013D54C
		public Rect Label(string text, float width = -1f)
		{
			if (width < 0f)
			{
				width = Text.CalcSize(text).x;
			}
			this.IncrementYIfWillExceedMaxWidth(width);
			Rect rect = new Rect(this.LeftX(width), this.curY, width, 24f);
			this.IncrementPosition(2f);
			Widgets.Label(rect, text);
			this.IncrementPosition(2f);
			this.IncrementPosition(rect.width);
			return rect;
		}

		// Token: 0x0600304E RID: 12366 RVA: 0x0013F3BC File Offset: 0x0013D5BC
		public Rect FillableBar(float width, float height, float fillPct, string label, Texture2D fillTex, Texture2D bgTex = null)
		{
			this.IncrementYIfWillExceedMaxWidth(width);
			Rect rect = new Rect(this.LeftX(width), this.curY, width, height);
			Widgets.FillableBar(rect, fillPct, fillTex, bgTex, false);
			if (!label.NullOrEmpty())
			{
				Rect rect2 = rect;
				rect2.xMin += 2f;
				rect2.xMax -= 2f;
				if (Text.Anchor >= TextAnchor.UpperLeft)
				{
					rect2.height += 14f;
				}
				Text.Font = GameFont.Tiny;
				Text.WordWrap = false;
				Widgets.Label(rect2, label);
				Text.WordWrap = true;
			}
			this.IncrementPosition(width);
			return rect;
		}

		// Token: 0x0400212F RID: 8495
		private float startX;

		// Token: 0x04002130 RID: 8496
		private float curX;

		// Token: 0x04002131 RID: 8497
		private float curY;

		// Token: 0x04002132 RID: 8498
		private float maxWidth = 99999f;

		// Token: 0x04002133 RID: 8499
		private float gap;

		// Token: 0x04002134 RID: 8500
		private UIDirection growDirection = UIDirection.RightThenUp;

		// Token: 0x04002135 RID: 8501
		public const float IconSize = 24f;

		// Token: 0x04002136 RID: 8502
		public const float DefaultGap = 4f;

		// Token: 0x04002137 RID: 8503
		private const float DefaultMaxWidth = 99999f;

		// Token: 0x04002138 RID: 8504
		public const float LabelGap = 2f;

		// Token: 0x04002139 RID: 8505
		public const float ButtonExtraSpace = 16f;
	}
}
