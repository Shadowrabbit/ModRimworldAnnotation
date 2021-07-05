using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200045D RID: 1117
	public class WindowResizer
	{
		// Token: 0x060021C6 RID: 8646 RVA: 0x000D2FB4 File Offset: 0x000D11B4
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

		// Token: 0x0400153B RID: 5435
		public Vector2 minWindowSize = new Vector2(150f, 150f);

		// Token: 0x0400153C RID: 5436
		private bool isResizing;

		// Token: 0x0400153D RID: 5437
		private Rect resizeStart;

		// Token: 0x0400153E RID: 5438
		private const float ResizeButtonSize = 24f;
	}
}
