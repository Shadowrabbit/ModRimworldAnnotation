using System;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000459 RID: 1113
	public abstract class EditWindow : Window
	{
		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x060021A4 RID: 8612 RVA: 0x000D2698 File Offset: 0x000D0898
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 500f);
			}
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x060021A5 RID: 8613 RVA: 0x000D26A9 File Offset: 0x000D08A9
		protected override float Margin
		{
			get
			{
				return 8f;
			}
		}

		// Token: 0x060021A6 RID: 8614 RVA: 0x000D26B0 File Offset: 0x000D08B0
		public EditWindow()
		{
			this.resizeable = true;
			this.draggable = true;
			this.preventCameraMotion = false;
			this.doCloseX = true;
			this.windowRect.x = 5f;
			this.windowRect.y = 5f;
		}

		// Token: 0x060021A7 RID: 8615 RVA: 0x000D2700 File Offset: 0x000D0900
		public override void PostOpen()
		{
			while (this.windowRect.x <= (float)UI.screenWidth - 200f && this.windowRect.y <= (float)UI.screenHeight - 200f)
			{
				bool flag = false;
				foreach (EditWindow editWindow in (from di in Find.WindowStack.Windows
				where di is EditWindow
				select di).Cast<EditWindow>())
				{
					if (editWindow != this && Mathf.Abs(editWindow.windowRect.x - this.windowRect.x) < 8f && Mathf.Abs(editWindow.windowRect.y - this.windowRect.y) < 8f)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					break;
				}
				this.windowRect.x = this.windowRect.x + 16f;
				this.windowRect.y = this.windowRect.y + 16f;
			}
		}

		// Token: 0x04001508 RID: 5384
		private const float SuperimposeAvoidThreshold = 8f;

		// Token: 0x04001509 RID: 5385
		private const float SuperimposeAvoidOffset = 16f;

		// Token: 0x0400150A RID: 5386
		private const float SuperimposeAvoidOffsetMinEdge = 200f;
	}
}
