using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200009E RID: 158
	public abstract class InspectTabBase
	{
		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000543 RID: 1347
		protected abstract float PaneTopY { get; }

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000544 RID: 1348
		protected abstract bool StillValid { get; }

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000545 RID: 1349 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool IsVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000546 RID: 1350 RVA: 0x0000A806 File Offset: 0x00008A06
		public string TutorHighlightTagClosed
		{
			get
			{
				if (this.tutorTag == null)
				{
					return null;
				}
				if (this.cachedTutorHighlightTagClosed == null)
				{
					this.cachedTutorHighlightTagClosed = "ITab-" + this.tutorTag + "-Closed";
				}
				return this.cachedTutorHighlightTagClosed;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000547 RID: 1351 RVA: 0x0008B384 File Offset: 0x00089584
		protected Rect TabRect
		{
			get
			{
				this.UpdateSize();
				float y = this.PaneTopY - 30f - this.size.y;
				return new Rect(0f, y, this.size.x, this.size.y);
			}
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x0008B3D4 File Offset: 0x000895D4
		public void DoTabGUI()
		{
			Rect rect = this.TabRect;
			Find.WindowStack.ImmediateWindow(235086, rect, WindowLayer.GameUI, delegate
			{
				if (!this.StillValid || !this.IsVisible)
				{
					return;
				}
				if (Widgets.CloseButtonFor(rect.AtZero()))
				{
					this.CloseTab();
				}
				try
				{
					this.FillTab();
				}
				catch (Exception ex)
				{
					Log.ErrorOnce(string.Concat(new object[]
					{
						"Exception filling tab ",
						this.GetType(),
						": ",
						ex
					}), 49827, false);
				}
			}, true, false, 1f);
			this.ExtraOnGUI();
		}

		// Token: 0x06000549 RID: 1353
		protected abstract void CloseTab();

		// Token: 0x0600054A RID: 1354
		protected abstract void FillTab();

		// Token: 0x0600054B RID: 1355 RVA: 0x00006A05 File Offset: 0x00004C05
		protected virtual void ExtraOnGUI()
		{
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00006A05 File Offset: 0x00004C05
		protected virtual void UpdateSize()
		{
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void OnOpen()
		{
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void TabTick()
		{
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void TabUpdate()
		{
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_ClearingAllMapsMemory()
		{
		}

		// Token: 0x04000297 RID: 663
		public string labelKey;

		// Token: 0x04000298 RID: 664
		protected Vector2 size;

		// Token: 0x04000299 RID: 665
		public string tutorTag;

		// Token: 0x0400029A RID: 666
		private string cachedTutorHighlightTagClosed;
	}
}
