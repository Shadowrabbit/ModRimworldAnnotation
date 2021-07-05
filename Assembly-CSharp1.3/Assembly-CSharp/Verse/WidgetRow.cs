using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000441 RID: 1089
	public class WidgetRow
	{
		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x06002090 RID: 8336 RVA: 0x000CA7CD File Offset: 0x000C89CD
		public float FinalX
		{
			get
			{
				return this.curX;
			}
		}

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x06002091 RID: 8337 RVA: 0x000CA7D5 File Offset: 0x000C89D5
		public float FinalY
		{
			get
			{
				return this.curY;
			}
		}

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x06002092 RID: 8338 RVA: 0x000CA7DD File Offset: 0x000C89DD
		// (set) Token: 0x06002093 RID: 8339 RVA: 0x000CA7E5 File Offset: 0x000C89E5
		public float CellGap
		{
			get
			{
				return this.gap;
			}
			set
			{
				this.gap = value;
			}
		}

		// Token: 0x06002094 RID: 8340 RVA: 0x000CA7EE File Offset: 0x000C89EE
		public WidgetRow()
		{
		}

		// Token: 0x06002095 RID: 8341 RVA: 0x000CA808 File Offset: 0x000C8A08
		public WidgetRow(float x, float y, UIDirection growDirection = UIDirection.RightThenUp, float maxWidth = 99999f, float gap = 4f)
		{
			this.Init(x, y, growDirection, maxWidth, gap);
		}

		// Token: 0x06002096 RID: 8342 RVA: 0x000CA82F File Offset: 0x000C8A2F
		public void Init(float x, float y, UIDirection growDirection = UIDirection.RightThenUp, float maxWidth = 99999f, float gap = 4f)
		{
			this.growDirection = growDirection;
			this.startX = x;
			this.curX = x;
			this.curY = y;
			this.maxWidth = maxWidth;
			this.gap = gap;
		}

		// Token: 0x06002097 RID: 8343 RVA: 0x000CA85D File Offset: 0x000C8A5D
		private float LeftX(float elementWidth)
		{
			if (this.growDirection == UIDirection.RightThenUp || this.growDirection == UIDirection.RightThenDown)
			{
				return this.curX;
			}
			return this.curX - elementWidth;
		}

		// Token: 0x06002098 RID: 8344 RVA: 0x000CA880 File Offset: 0x000C8A80
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

		// Token: 0x06002099 RID: 8345 RVA: 0x000CA8E0 File Offset: 0x000C8AE0
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

		// Token: 0x0600209A RID: 8346 RVA: 0x000CA93E File Offset: 0x000C8B3E
		private void IncrementYIfWillExceedMaxWidth(float width)
		{
			if (Mathf.Abs(this.curX - this.startX) + Mathf.Abs(width) > this.maxWidth)
			{
				this.IncrementY();
			}
		}

		// Token: 0x0600209B RID: 8347 RVA: 0x000CA967 File Offset: 0x000C8B67
		public void Gap(float width)
		{
			if (this.curX != this.startX)
			{
				this.IncrementPosition(width);
			}
		}

		// Token: 0x0600209C RID: 8348 RVA: 0x000CA980 File Offset: 0x000C8B80
		public bool ButtonIcon(Texture2D tex, string tooltip = null, Color? mouseoverColor = null, Color? backgroundColor = null, Color? mouseoverBackgroundColor = null, bool doMouseoverSound = true)
		{
			this.IncrementYIfWillExceedMaxWidth(24f);
			Rect rect = new Rect(this.LeftX(24f), this.curY, 24f, 24f);
			if (doMouseoverSound)
			{
				MouseoverSounds.DoRegion(rect);
			}
			if (mouseoverBackgroundColor != null && Mouse.IsOver(rect))
			{
				Widgets.DrawRectFast(rect, mouseoverBackgroundColor.Value, null);
			}
			else if (backgroundColor != null && !Mouse.IsOver(rect))
			{
				Widgets.DrawRectFast(rect, backgroundColor.Value, null);
			}
			bool result = Widgets.ButtonImage(rect, tex, Color.white, mouseoverColor ?? GenUI.MouseoverColor, true);
			this.IncrementPosition(24f + this.gap);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			return result;
		}

		// Token: 0x0600209D RID: 8349 RVA: 0x000CAA50 File Offset: 0x000C8C50
		public bool ButtonIconWithBG(Texture2D texture, float width = -1f, string tooltip = null, bool doMouseoverSound = true)
		{
			if (width < 0f)
			{
				width = 24f;
			}
			width += 16f;
			this.IncrementYIfWillExceedMaxWidth(width);
			Rect rect = new Rect(this.LeftX(width), this.curY, width, 26f);
			if (doMouseoverSound)
			{
				MouseoverSounds.DoRegion(rect);
			}
			bool result = Widgets.ButtonImageWithBG(rect, texture, new Vector2?(Vector2.one * 24f));
			this.IncrementPosition(width + this.gap);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			return result;
		}

		// Token: 0x0600209E RID: 8350 RVA: 0x000CAAE0 File Offset: 0x000C8CE0
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

		// Token: 0x0600209F RID: 8351 RVA: 0x000CABD8 File Offset: 0x000C8DD8
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

		// Token: 0x060020A0 RID: 8352 RVA: 0x000CAC40 File Offset: 0x000C8E40
		public Rect DefIcon(ThingDef def, string tooltip = null)
		{
			this.IncrementYIfWillExceedMaxWidth(24f);
			Rect rect = new Rect(this.LeftX(24f), this.curY, 24f, 24f);
			Widgets.DefIcon(rect, def, null, 1f, null, false, null);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			this.IncrementPosition(24f + this.gap);
			return rect;
		}

		// Token: 0x060020A1 RID: 8353 RVA: 0x000CACBC File Offset: 0x000C8EBC
		public bool ButtonText(string label, string tooltip = null, bool drawBackground = true, bool doMouseoverSound = true, bool active = true, float? fixedWidth = null)
		{
			Rect rect = this.ButtonRect(label, fixedWidth);
			bool result = Widgets.ButtonText(rect, label, drawBackground, doMouseoverSound, active);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			return result;
		}

		// Token: 0x060020A2 RID: 8354 RVA: 0x000CACF4 File Offset: 0x000C8EF4
		public Rect ButtonRect(string label, float? fixedWidth = null)
		{
			Vector2 vector = (fixedWidth != null) ? new Vector2(fixedWidth.Value, 24f) : Text.CalcSize(label);
			vector.x += 16f;
			vector.y += 2f;
			this.IncrementYIfWillExceedMaxWidth(vector.x);
			Rect result = new Rect(this.LeftX(vector.x), this.curY, vector.x, vector.y);
			this.IncrementPosition(result.width + this.gap);
			return result;
		}

		// Token: 0x060020A3 RID: 8355 RVA: 0x000CAD8C File Offset: 0x000C8F8C
		public Rect Label(string text, float width = -1f, string tooltip = null, float height = -1f)
		{
			if (height < 0f)
			{
				height = 24f;
			}
			if (width < 0f)
			{
				width = Text.CalcSize(text).x;
			}
			this.IncrementYIfWillExceedMaxWidth(width + 2f);
			this.IncrementPosition(2f);
			Rect rect = new Rect(this.LeftX(width), this.curY, width, height);
			Widgets.Label(rect, text);
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			this.IncrementPosition(2f);
			this.IncrementPosition(rect.width);
			return rect;
		}

		// Token: 0x060020A4 RID: 8356 RVA: 0x000CAE24 File Offset: 0x000C9024
		public Rect TextFieldNumeric<T>(ref int val, ref string buffer, float width = -1f) where T : struct
		{
			if (width < 0f)
			{
				width = Text.CalcSize(val.ToString()).x;
			}
			this.IncrementYIfWillExceedMaxWidth(width + 2f);
			this.IncrementPosition(2f);
			Rect rect = new Rect(this.LeftX(width), this.curY, width, 24f);
			Widgets.TextFieldNumeric<int>(rect, ref val, ref buffer, 0f, 1E+09f);
			this.IncrementPosition(2f);
			this.IncrementPosition(rect.width);
			return rect;
		}

		// Token: 0x060020A5 RID: 8357 RVA: 0x000CAEA8 File Offset: 0x000C90A8
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

		// Token: 0x0400143D RID: 5181
		private float startX;

		// Token: 0x0400143E RID: 5182
		private float curX;

		// Token: 0x0400143F RID: 5183
		private float curY;

		// Token: 0x04001440 RID: 5184
		private float maxWidth = 99999f;

		// Token: 0x04001441 RID: 5185
		private float gap;

		// Token: 0x04001442 RID: 5186
		private UIDirection growDirection = UIDirection.RightThenUp;

		// Token: 0x04001443 RID: 5187
		public const float IconSize = 24f;

		// Token: 0x04001444 RID: 5188
		public const float DefaultGap = 4f;

		// Token: 0x04001445 RID: 5189
		private const float DefaultMaxWidth = 99999f;

		// Token: 0x04001446 RID: 5190
		public const float LabelGap = 2f;

		// Token: 0x04001447 RID: 5191
		public const float ButtonExtraSpace = 16f;
	}
}
