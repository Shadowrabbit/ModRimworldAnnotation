using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003EF RID: 1007
	public abstract class FeedbackItem
	{
		// Token: 0x06001E5B RID: 7771 RVA: 0x000BE048 File Offset: 0x000BC248
		public FeedbackItem(Vector2 ScreenPos)
		{
			this.uniqueID = FeedbackItem.freeUniqueID++;
			this.CurScreenPos = ScreenPos;
			this.CurScreenPos.y = this.CurScreenPos.y - 15f;
		}

		// Token: 0x06001E5C RID: 7772 RVA: 0x000BE0A9 File Offset: 0x000BC2A9
		public void Update()
		{
			this.TimeLeft -= Time.deltaTime;
			this.CurScreenPos += this.FloatPerSecond * Time.deltaTime;
		}

		// Token: 0x06001E5D RID: 7773
		public abstract void FeedbackOnGUI();

		// Token: 0x06001E5E RID: 7774 RVA: 0x000BE0E0 File Offset: 0x000BC2E0
		protected void DrawFloatingText(string str, Color TextColor)
		{
			float x = Text.CalcSize(str).x;
			Rect wordRect = new Rect(this.CurScreenPos.x - x / 2f, this.CurScreenPos.y, x, 20f);
			Find.WindowStack.ImmediateWindow(5983 * this.uniqueID + 495, wordRect, WindowLayer.Super, delegate
			{
				Rect rect = wordRect.AtZero();
				Text.Anchor = TextAnchor.UpperCenter;
				Text.Font = GameFont.Small;
				GUI.DrawTexture(rect, TexUI.GrayTextBG);
				GUI.color = TextColor;
				Widgets.Label(rect, str);
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
			}, false, false, 1f, null);
		}

		// Token: 0x04001271 RID: 4721
		protected Vector2 FloatPerSecond = new Vector2(20f, -20f);

		// Token: 0x04001272 RID: 4722
		private int uniqueID;

		// Token: 0x04001273 RID: 4723
		public float TimeLeft = 2f;

		// Token: 0x04001274 RID: 4724
		protected Vector2 CurScreenPos;

		// Token: 0x04001275 RID: 4725
		private static int freeUniqueID;
	}
}
