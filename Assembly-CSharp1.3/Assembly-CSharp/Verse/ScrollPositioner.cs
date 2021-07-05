using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200041F RID: 1055
	public class ScrollPositioner
	{
		// Token: 0x06001FB3 RID: 8115 RVA: 0x000C4F4B File Offset: 0x000C314B
		public void Arm(bool armed = true)
		{
			this.armed = armed;
		}

		// Token: 0x06001FB4 RID: 8116 RVA: 0x000C4F54 File Offset: 0x000C3154
		public void ClearInterestRects()
		{
			this.interestRect = null;
		}

		// Token: 0x06001FB5 RID: 8117 RVA: 0x000C4F62 File Offset: 0x000C3162
		public void RegisterInterestRect(Rect rect)
		{
			if (this.interestRect != null)
			{
				this.interestRect = new Rect?(rect.Union(this.interestRect.Value));
				return;
			}
			this.interestRect = new Rect?(rect);
		}

		// Token: 0x06001FB6 RID: 8118 RVA: 0x000C4F9A File Offset: 0x000C319A
		public void ScrollHorizontally(ref Vector2 scrollPos, Vector2 outRectSize)
		{
			this.Scroll(ref scrollPos, outRectSize, true, false);
		}

		// Token: 0x06001FB7 RID: 8119 RVA: 0x000C4FA6 File Offset: 0x000C31A6
		public void ScrollVertically(ref Vector2 scrollPos, Vector2 outRectSize)
		{
			this.Scroll(ref scrollPos, outRectSize, false, true);
		}

		// Token: 0x06001FB8 RID: 8120 RVA: 0x000C4FB4 File Offset: 0x000C31B4
		public void Scroll(ref Vector2 scrollPos, Vector2 outRectSize, bool scrollHorizontally = true, bool scrollVertically = true)
		{
			if (Event.current.type != EventType.Layout)
			{
				return;
			}
			if (!this.armed)
			{
				return;
			}
			this.armed = false;
			if (this.interestRect == null)
			{
				return;
			}
			if (scrollHorizontally)
			{
				this.ScrollInDimension(ref scrollPos.x, outRectSize.x, this.interestRect.Value.xMin, this.interestRect.Value.xMax);
			}
			if (scrollVertically)
			{
				this.ScrollInDimension(ref scrollPos.y, outRectSize.y, this.interestRect.Value.yMin, this.interestRect.Value.yMax);
			}
		}

		// Token: 0x06001FB9 RID: 8121 RVA: 0x000C5064 File Offset: 0x000C3264
		private void ScrollInDimension(ref float scrollPos, float scrollViewSize, float v0, float v1)
		{
			float num = v1 - v0;
			if (num <= scrollViewSize)
			{
				scrollPos = v0 + num / 2f - scrollViewSize / 2f;
				return;
			}
			scrollPos = v0;
		}

		// Token: 0x0400133E RID: 4926
		private Rect? interestRect;

		// Token: 0x0400133F RID: 4927
		private bool armed;
	}
}
