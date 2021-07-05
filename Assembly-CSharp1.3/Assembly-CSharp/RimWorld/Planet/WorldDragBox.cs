using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001815 RID: 6165
	public class WorldDragBox
	{
		// Token: 0x170017B5 RID: 6069
		// (get) Token: 0x06009067 RID: 36967 RVA: 0x0033CCC3 File Offset: 0x0033AEC3
		public float LeftX
		{
			get
			{
				return Math.Min(this.start.x, UI.MousePositionOnUIInverted.x);
			}
		}

		// Token: 0x170017B6 RID: 6070
		// (get) Token: 0x06009068 RID: 36968 RVA: 0x0033CCDF File Offset: 0x0033AEDF
		public float RightX
		{
			get
			{
				return Math.Max(this.start.x, UI.MousePositionOnUIInverted.x);
			}
		}

		// Token: 0x170017B7 RID: 6071
		// (get) Token: 0x06009069 RID: 36969 RVA: 0x0033CCFB File Offset: 0x0033AEFB
		public float BotZ
		{
			get
			{
				return Math.Min(this.start.y, UI.MousePositionOnUIInverted.y);
			}
		}

		// Token: 0x170017B8 RID: 6072
		// (get) Token: 0x0600906A RID: 36970 RVA: 0x0033CD17 File Offset: 0x0033AF17
		public float TopZ
		{
			get
			{
				return Math.Max(this.start.y, UI.MousePositionOnUIInverted.y);
			}
		}

		// Token: 0x170017B9 RID: 6073
		// (get) Token: 0x0600906B RID: 36971 RVA: 0x0033CD33 File Offset: 0x0033AF33
		public Rect ScreenRect
		{
			get
			{
				return new Rect(this.LeftX, this.BotZ, this.RightX - this.LeftX, this.TopZ - this.BotZ);
			}
		}

		// Token: 0x170017BA RID: 6074
		// (get) Token: 0x0600906C RID: 36972 RVA: 0x0033CD60 File Offset: 0x0033AF60
		public float Diagonal
		{
			get
			{
				return (this.start - new Vector2(UI.MousePositionOnUIInverted.x, UI.MousePositionOnUIInverted.y)).magnitude;
			}
		}

		// Token: 0x170017BB RID: 6075
		// (get) Token: 0x0600906D RID: 36973 RVA: 0x0033CD99 File Offset: 0x0033AF99
		public bool IsValid
		{
			get
			{
				return this.Diagonal > 7f;
			}
		}

		// Token: 0x170017BC RID: 6076
		// (get) Token: 0x0600906E RID: 36974 RVA: 0x0033CDA8 File Offset: 0x0033AFA8
		public bool IsValidAndActive
		{
			get
			{
				return this.active && this.IsValid;
			}
		}

		// Token: 0x0600906F RID: 36975 RVA: 0x0033CDBA File Offset: 0x0033AFBA
		public void DragBoxOnGUI()
		{
			if (this.IsValidAndActive)
			{
				Widgets.DrawBox(this.ScreenRect, 2, null);
			}
		}

		// Token: 0x06009070 RID: 36976 RVA: 0x0033CDD1 File Offset: 0x0033AFD1
		public bool Contains(WorldObject o)
		{
			return this.Contains(o.ScreenPos());
		}

		// Token: 0x06009071 RID: 36977 RVA: 0x0033CDE0 File Offset: 0x0033AFE0
		public bool Contains(Vector2 screenPoint)
		{
			return screenPoint.x + 0.5f > this.LeftX && screenPoint.x - 0.5f < this.RightX && screenPoint.y + 0.5f > this.BotZ && screenPoint.y - 0.5f < this.TopZ;
		}

		// Token: 0x04005AD6 RID: 23254
		public bool active;

		// Token: 0x04005AD7 RID: 23255
		public Vector2 start;

		// Token: 0x04005AD8 RID: 23256
		private const float DragBoxMinDiagonal = 7f;
	}
}
