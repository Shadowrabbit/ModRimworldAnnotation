using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200075A RID: 1882
	[StaticConstructorOnStartup]
	public class ActiveTip
	{
		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x06002F7B RID: 12155 RVA: 0x0013B4F0 File Offset: 0x001396F0
		private string FinalText
		{
			get
			{
				string text;
				if (this.signal.textGetter != null)
				{
					try
					{
						text = this.signal.textGetter();
						goto IL_3F;
					}
					catch (Exception ex)
					{
						Log.Error(ex.ToString(), false);
						text = "Error getting tip text.";
						goto IL_3F;
					}
				}
				text = this.signal.text;
				IL_3F:
				return text.TrimEnd(Array.Empty<char>());
			}
		}

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x06002F7C RID: 12156 RVA: 0x0013B558 File Offset: 0x00139758
		public Rect TipRect
		{
			get
			{
				string finalText = this.FinalText;
				Vector2 vector = Text.CalcSize(finalText);
				if (vector.x > 260f)
				{
					vector.x = 260f;
					vector.y = Text.CalcHeight(finalText, vector.x);
				}
				return new Rect(0f, 0f, vector.x, vector.y).ContractedBy(-4f);
			}
		}

		// Token: 0x06002F7D RID: 12157 RVA: 0x00025341 File Offset: 0x00023541
		public ActiveTip(TipSignal signal)
		{
			this.signal = signal;
		}

		// Token: 0x06002F7E RID: 12158 RVA: 0x00025350 File Offset: 0x00023550
		public ActiveTip(ActiveTip cloneSource)
		{
			this.signal = cloneSource.signal;
			this.firstTriggerTime = cloneSource.firstTriggerTime;
			this.lastTriggerFrame = cloneSource.lastTriggerFrame;
		}

		// Token: 0x06002F7F RID: 12159 RVA: 0x0013B5C4 File Offset: 0x001397C4
		public float DrawTooltip(Vector2 pos)
		{
			Text.Font = GameFont.Small;
			string finalText = this.FinalText;
			Rect bgRect = this.TipRect;
			bgRect.position = pos;
			if (!LongEventHandler.AnyEventWhichDoesntUseStandardWindowNowOrWaiting)
			{
				Find.WindowStack.ImmediateWindow(153 * this.signal.uniqueId + 62346, bgRect, WindowLayer.Super, delegate
				{
					this.DrawInner(bgRect.AtZero(), finalText);
				}, false, false, 1f);
			}
			else
			{
				Widgets.DrawShadowAround(bgRect);
				Widgets.DrawWindowBackground(bgRect);
				this.DrawInner(bgRect, finalText);
			}
			return bgRect.height;
		}

		// Token: 0x06002F80 RID: 12160 RVA: 0x0002537C File Offset: 0x0002357C
		private void DrawInner(Rect bgRect, string label)
		{
			Widgets.DrawAtlas(bgRect, ActiveTip.TooltipBGAtlas);
			Text.Font = GameFont.Small;
			Widgets.Label(bgRect.ContractedBy(4f), label);
		}

		// Token: 0x0400202D RID: 8237
		public TipSignal signal;

		// Token: 0x0400202E RID: 8238
		public double firstTriggerTime;

		// Token: 0x0400202F RID: 8239
		public int lastTriggerFrame;

		// Token: 0x04002030 RID: 8240
		private const int TipMargin = 4;

		// Token: 0x04002031 RID: 8241
		private const float MaxWidth = 260f;

		// Token: 0x04002032 RID: 8242
		public static readonly Texture2D TooltipBGAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/TooltipBG", true);
	}
}
