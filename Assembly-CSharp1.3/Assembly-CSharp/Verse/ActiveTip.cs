using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000424 RID: 1060
	[StaticConstructorOnStartup]
	public class ActiveTip
	{
		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x06001FE4 RID: 8164 RVA: 0x000C5960 File Offset: 0x000C3B60
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
						goto IL_3E;
					}
					catch (Exception ex)
					{
						Log.Error(ex.ToString());
						text = "Error getting tip text.";
						goto IL_3E;
					}
				}
				text = this.signal.text;
				IL_3E:
				return text.TrimEnd(Array.Empty<char>());
			}
		}

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x06001FE5 RID: 8165 RVA: 0x000C59C8 File Offset: 0x000C3BC8
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
				return new Rect(0f, 0f, vector.x, vector.y).ContractedBy(-4f).RoundedCeil();
			}
		}

		// Token: 0x06001FE6 RID: 8166 RVA: 0x000C5A39 File Offset: 0x000C3C39
		public ActiveTip(TipSignal signal)
		{
			this.signal = signal;
		}

		// Token: 0x06001FE7 RID: 8167 RVA: 0x000C5A48 File Offset: 0x000C3C48
		public ActiveTip(ActiveTip cloneSource)
		{
			this.signal = cloneSource.signal;
			this.firstTriggerTime = cloneSource.firstTriggerTime;
			this.lastTriggerFrame = cloneSource.lastTriggerFrame;
		}

		// Token: 0x06001FE8 RID: 8168 RVA: 0x000C5A74 File Offset: 0x000C3C74
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
				}, false, false, 1f, null);
			}
			else
			{
				Widgets.DrawShadowAround(bgRect);
				Widgets.DrawWindowBackground(bgRect);
				this.DrawInner(bgRect, finalText);
			}
			return bgRect.height;
		}

		// Token: 0x06001FE9 RID: 8169 RVA: 0x000C5B30 File Offset: 0x000C3D30
		private void DrawInner(Rect bgRect, string label)
		{
			Widgets.DrawAtlas(bgRect, ActiveTip.TooltipBGAtlas);
			Text.Font = GameFont.Small;
			Widgets.Label(bgRect.ContractedBy(4f), label);
		}

		// Token: 0x04001353 RID: 4947
		public TipSignal signal;

		// Token: 0x04001354 RID: 4948
		public double firstTriggerTime;

		// Token: 0x04001355 RID: 4949
		public int lastTriggerFrame;

		// Token: 0x04001356 RID: 4950
		private const int TipMargin = 4;

		// Token: 0x04001357 RID: 4951
		private const float MaxWidth = 260f;

		// Token: 0x04001358 RID: 4952
		public static readonly Texture2D TooltipBGAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/TooltipBG", true);
	}
}
