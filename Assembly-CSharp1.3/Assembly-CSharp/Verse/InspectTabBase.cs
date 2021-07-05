using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000057 RID: 87
	public abstract class InspectTabBase
	{
		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060003EB RID: 1003
		protected abstract float PaneTopY { get; }

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060003EC RID: 1004
		protected abstract bool StillValid { get; }

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool IsVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060003EE RID: 1006 RVA: 0x00015395 File Offset: 0x00013595
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

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060003EF RID: 1007 RVA: 0x000153CC File Offset: 0x000135CC
		protected Rect TabRect
		{
			get
			{
				this.UpdateSize();
				float y = this.PaneTopY - 30f - this.size.y;
				return new Rect(0f, y, this.size.x, this.size.y);
			}
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0001541C File Offset: 0x0001361C
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
					}), 49827);
				}
			}, true, false, 1f, new Action(this.Notify_ClickOutsideWindow));
			this.ExtraOnGUI();
		}

		// Token: 0x060003F1 RID: 1009
		protected abstract void CloseTab();

		// Token: 0x060003F2 RID: 1010
		protected abstract void FillTab();

		// Token: 0x060003F3 RID: 1011 RVA: 0x0000313F File Offset: 0x0000133F
		protected virtual void ExtraOnGUI()
		{
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0000313F File Offset: 0x0000133F
		protected virtual void UpdateSize()
		{
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void OnOpen()
		{
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void TabTick()
		{
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void TabUpdate()
		{
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_ClearingAllMapsMemory()
		{
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_ClickOutsideWindow()
		{
		}

		// Token: 0x0400012C RID: 300
		public string labelKey;

		// Token: 0x0400012D RID: 301
		protected Vector2 size;

		// Token: 0x0400012E RID: 302
		public string tutorTag;

		// Token: 0x0400012F RID: 303
		private string cachedTutorHighlightTagClosed;
	}
}
