using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000795 RID: 1941
	public class Dialog_Slider : Window
	{
		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x060030F2 RID: 12530 RVA: 0x0002696F File Offset: 0x00024B6F
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(300f, 130f);
			}
		}

		// Token: 0x060030F3 RID: 12531 RVA: 0x00143AB4 File Offset: 0x00141CB4
		public Dialog_Slider(Func<int, string> textGetter, int from, int to, Action<int> confirmAction, int startingValue = -2147483648)
		{
			this.textGetter = textGetter;
			this.from = from;
			this.to = to;
			this.confirmAction = confirmAction;
			this.forcePause = true;
			this.closeOnClickedOutside = true;
			if (startingValue == -2147483648)
			{
				this.curValue = from;
				return;
			}
			this.curValue = startingValue;
		}

		// Token: 0x060030F4 RID: 12532 RVA: 0x00143B0C File Offset: 0x00141D0C
		public Dialog_Slider(string text, int from, int to, Action<int> confirmAction, int startingValue = -2147483648) : this((int val) => string.Format(text, val), from, to, confirmAction, startingValue)
		{
		}

		// Token: 0x060030F5 RID: 12533 RVA: 0x00143B40 File Offset: 0x00141D40
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(inRect.x, inRect.y + 15f, inRect.width, 30f);
			this.curValue = (int)Widgets.HorizontalSlider(rect, (float)this.curValue, (float)this.from, (float)this.to, true, this.textGetter(this.curValue), null, null, 1f);
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

		// Token: 0x040021AC RID: 8620
		public Func<int, string> textGetter;

		// Token: 0x040021AD RID: 8621
		public int from;

		// Token: 0x040021AE RID: 8622
		public int to;

		// Token: 0x040021AF RID: 8623
		private Action<int> confirmAction;

		// Token: 0x040021B0 RID: 8624
		private int curValue;

		// Token: 0x040021B1 RID: 8625
		private const float BotAreaHeight = 30f;

		// Token: 0x040021B2 RID: 8626
		private const float TopPadding = 15f;
	}
}
