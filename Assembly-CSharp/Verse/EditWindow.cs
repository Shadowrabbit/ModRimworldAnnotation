using System;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x020007AA RID: 1962
	public abstract class EditWindow : Window
	{
		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x0600315C RID: 12636 RVA: 0x00026EF7 File Offset: 0x000250F7
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 500f);
			}
		}

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x0600315D RID: 12637 RVA: 0x00026F08 File Offset: 0x00025108
		protected override float Margin
		{
			get
			{
				return 8f;
			}
		}

		// Token: 0x0600315E RID: 12638 RVA: 0x00145A40 File Offset: 0x00143C40
		public EditWindow()
		{
			this.resizeable = true;
			this.draggable = true;
			this.preventCameraMotion = false;
			this.doCloseX = true;
			this.windowRect.x = 5f;
			this.windowRect.y = 5f;
		}

		// Token: 0x0600315F RID: 12639 RVA: 0x00145A90 File Offset: 0x00143C90
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

		// Token: 0x04002221 RID: 8737
		private const float SuperimposeAvoidThreshold = 8f;

		// Token: 0x04002222 RID: 8738
		private const float SuperimposeAvoidOffset = 16f;

		// Token: 0x04002223 RID: 8739
		private const float SuperimposeAvoidOffsetMinEdge = 200f;
	}
}
