using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020021E4 RID: 8676
	public class WorldDragBox
	{
		// Token: 0x17001B95 RID: 7061
		// (get) Token: 0x0600B9B8 RID: 47544 RVA: 0x00078437 File Offset: 0x00076637
		public float LeftX
		{
			get
			{
				return Math.Min(this.start.x, UI.MousePositionOnUIInverted.x);
			}
		}

		// Token: 0x17001B96 RID: 7062
		// (get) Token: 0x0600B9B9 RID: 47545 RVA: 0x00078453 File Offset: 0x00076653
		public float RightX
		{
			get
			{
				return Math.Max(this.start.x, UI.MousePositionOnUIInverted.x);
			}
		}

		// Token: 0x17001B97 RID: 7063
		// (get) Token: 0x0600B9BA RID: 47546 RVA: 0x0007846F File Offset: 0x0007666F
		public float BotZ
		{
			get
			{
				return Math.Min(this.start.y, UI.MousePositionOnUIInverted.y);
			}
		}

		// Token: 0x17001B98 RID: 7064
		// (get) Token: 0x0600B9BB RID: 47547 RVA: 0x0007848B File Offset: 0x0007668B
		public float TopZ
		{
			get
			{
				return Math.Max(this.start.y, UI.MousePositionOnUIInverted.y);
			}
		}

		// Token: 0x17001B99 RID: 7065
		// (get) Token: 0x0600B9BC RID: 47548 RVA: 0x000784A7 File Offset: 0x000766A7
		public Rect ScreenRect
		{
			get
			{
				return new Rect(this.LeftX, this.BotZ, this.RightX - this.LeftX, this.TopZ - this.BotZ);
			}
		}

		// Token: 0x17001B9A RID: 7066
		// (get) Token: 0x0600B9BD RID: 47549 RVA: 0x00357394 File Offset: 0x00355594
		public float Diagonal
		{
			get
			{
				return (this.start - new Vector2(UI.MousePositionOnUIInverted.x, UI.MousePositionOnUIInverted.y)).magnitude;
			}
		}

		// Token: 0x17001B9B RID: 7067
		// (get) Token: 0x0600B9BE RID: 47550 RVA: 0x000784D4 File Offset: 0x000766D4
		public bool IsValid
		{
			get
			{
				return this.Diagonal > 7f;
			}
		}

		// Token: 0x17001B9C RID: 7068
		// (get) Token: 0x0600B9BF RID: 47551 RVA: 0x000784E3 File Offset: 0x000766E3
		public bool IsValidAndActive
		{
			get
			{
				return this.active && this.IsValid;
			}
		}

		// Token: 0x0600B9C0 RID: 47552 RVA: 0x000784F5 File Offset: 0x000766F5
		public void DragBoxOnGUI()
		{
			if (this.IsValidAndActive)
			{
				Widgets.DrawBox(this.ScreenRect, 2);
			}
		}

		// Token: 0x0600B9C1 RID: 47553 RVA: 0x0007850B File Offset: 0x0007670B
		public bool Contains(WorldObject o)
		{
			return this.Contains(o.ScreenPos());
		}

		// Token: 0x0600B9C2 RID: 47554 RVA: 0x003573D0 File Offset: 0x003555D0
		public bool Contains(Vector2 screenPoint)
		{
			return screenPoint.x + 0.5f > this.LeftX && screenPoint.x - 0.5f < this.RightX && screenPoint.y + 0.5f > this.BotZ && screenPoint.y - 0.5f < this.TopZ;
		}

		// Token: 0x04007EEA RID: 32490
		public bool active;

		// Token: 0x04007EEB RID: 32491
		public Vector2 start;

		// Token: 0x04007EEC RID: 32492
		private const float DragBoxMinDiagonal = 7f;
	}
}
