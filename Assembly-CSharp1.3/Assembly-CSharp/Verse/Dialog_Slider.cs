using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200044B RID: 1099
	public class Dialog_Slider : Window
	{
		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x0600214C RID: 8524 RVA: 0x000D03F8 File Offset: 0x000CE5F8
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(300f, 130f);
			}
		}

		// Token: 0x0600214D RID: 8525 RVA: 0x000D040C File Offset: 0x000CE60C
		public Dialog_Slider(Func<int, string> textGetter, int from, int to, Action<int> confirmAction, int startingValue = -2147483648, float roundTo = 1f)
		{
			this.textGetter = textGetter;
			this.from = from;
			this.to = to;
			this.confirmAction = confirmAction;
			this.roundTo = roundTo;
			this.forcePause = true;
			this.closeOnClickedOutside = true;
			if (startingValue == -2147483648)
			{
				this.curValue = from;
				return;
			}
			this.curValue = startingValue;
		}

		// Token: 0x0600214E RID: 8526 RVA: 0x000D0478 File Offset: 0x000CE678
		public Dialog_Slider(string text, int from, int to, Action<int> confirmAction, int startingValue = -2147483648, float roundTo = 1f) : this((int val) => string.Format(text, val), from, to, confirmAction, startingValue, roundTo)
		{
		}

		// Token: 0x0600214F RID: 8527 RVA: 0x000D04AC File Offset: 0x000CE6AC
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(inRect.x, inRect.y + 15f, inRect.width, 30f);
			this.curValue = (int)Widgets.HorizontalSlider(rect, (float)this.curValue, (float)this.from, (float)this.to, true, this.textGetter(this.curValue), null, null, this.roundTo);
			Text.Font = GameFont.Small;
			if (Widgets.ButtonText(new Rect(inRect.x, inRect.yMax - 30f, inRect.width / 2f, 30f), "CancelButton".Translate(), true, true, true))
			{
				this.Close(true);
			}
			if (Widgets.ButtonText(new Rect(inRect.x + inRect.width / 2f, inRect.yMax - 30f, inRect.width / 2f, 30f), "OK".Translate(), true, true, true))
			{
				this.Close(true);
				this.confirmAction(this.curValue);
			}
		}

		// Token: 0x040014A7 RID: 5287
		public Func<int, string> textGetter;

		// Token: 0x040014A8 RID: 5288
		public int from;

		// Token: 0x040014A9 RID: 5289
		public int to;

		// Token: 0x040014AA RID: 5290
		public float roundTo = 1f;

		// Token: 0x040014AB RID: 5291
		private Action<int> confirmAction;

		// Token: 0x040014AC RID: 5292
		private int curValue;

		// Token: 0x040014AD RID: 5293
		private const float BotAreaHeight = 30f;

		// Token: 0x040014AE RID: 5294
		private const float TopPadding = 15f;
	}
}
