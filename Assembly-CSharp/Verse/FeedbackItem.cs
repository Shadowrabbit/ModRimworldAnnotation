using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200070A RID: 1802
	public abstract class FeedbackItem
	{
		// Token: 0x06002DA3 RID: 11683 RVA: 0x00134A6C File Offset: 0x00132C6C
		public FeedbackItem(Vector2 ScreenPos)
		{
			this.uniqueID = FeedbackItem.freeUniqueID++;
			this.CurScreenPos = ScreenPos;
			this.CurScreenPos.y = this.CurScreenPos.y - 15f;
		}

		// Token: 0x06002DA4 RID: 11684 RVA: 0x00023F97 File Offset: 0x00022197
		public void Update()
		{
			this.TimeLeft -= Time.deltaTime;
			this.CurScreenPos += this.FloatPerSecond * Time.deltaTime;
		}

		// Token: 0x06002DA5 RID: 11685
		public abstract void FeedbackOnGUI();

		// Token: 0x06002DA6 RID: 11686 RVA: 0x00134AD0 File Offset: 0x00132CD0
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
			}, false, false, 1f);
		}

		// Token: 0x04001F17 RID: 7959
		protected Vector2 FloatPerSecond = new Vector2(20f, -20f);

		// Token: 0x04001F18 RID: 7960
		private int uniqueID;

		// Token: 0x04001F19 RID: 7961
		public float TimeLeft = 2f;

		// Token: 0x04001F1A RID: 7962
		protected Vector2 CurScreenPos;

		// Token: 0x04001F1B RID: 7963
		private static int freeUniqueID;
	}
}
