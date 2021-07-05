using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020007AF RID: 1967
	public class WindowResizer
	{
		// Token: 0x0600317C RID: 12668 RVA: 0x001460DC File Offset: 0x001442DC
		public Rect DoResizeControl(Rect winRect)
		{
			Vector2 mousePosition = Event.current.mousePosition;
			Rect rect = new Rect(winRect.width - 24f, winRect.height - 24f, 24f, 24f);
			if (Event.current.type == EventType.MouseDown && Mouse.IsOver(rect))
			{
				this.isResizing = true;
				this.resizeStart = new Rect(mousePosition.x, mousePosition.y, winRect.width, winRect.height);
			}
			if (this.isResizing)
			{
				winRect.width = this.resizeStart.width + (mousePosition.x - this.resizeStart.x);
				winRect.height = this.resizeStart.height + (mousePosition.y - this.resizeStart.y);
				if (winRect.width < this.minWindowSize.x)
				{
					winRect.width = this.minWindowSize.x;
				}
				if (winRect.height < this.minWindowSize.y)
				{
					winRect.height = this.minWindowSize.y;
				}
				winRect.xMax = Mathf.Min((float)UI.screenWidth, winRect.xMax);
				winRect.yMax = Mathf.Min((float)UI.screenHeight, winRect.yMax);
				if (Event.current.type == EventType.MouseUp)
				{
					this.isResizing = false;
				}
			}
			Widgets.ButtonImage(rect, TexUI.WinExpandWidget, true);
			return new Rect(winRect.x, winRect.y, (float)((int)winRect.width), (float)((int)winRect.height));
		}

		// Token: 0x0400224E RID: 8782
		public Vector2 minWindowSize = new Vector2(150f, 150f);

		// Token: 0x0400224F RID: 8783
		private bool isResizing;

		// Token: 0x04002250 RID: 8784
		private Rect resizeStart;

		// Token: 0x04002251 RID: 8785
		private const float ResizeButtonSize = 24f;
	}
}
