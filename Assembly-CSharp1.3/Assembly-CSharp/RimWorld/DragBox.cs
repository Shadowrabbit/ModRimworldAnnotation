using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200133B RID: 4923
	public class DragBox
	{
		// Token: 0x170014DD RID: 5341
		// (get) Token: 0x06007724 RID: 30500 RVA: 0x0029D5B0 File Offset: 0x0029B7B0
		public float LeftX
		{
			get
			{
				return Math.Min(this.start.x, UI.MouseMapPosition().x);
			}
		}

		// Token: 0x170014DE RID: 5342
		// (get) Token: 0x06007725 RID: 30501 RVA: 0x0029D5CC File Offset: 0x0029B7CC
		public float RightX
		{
			get
			{
				return Math.Max(this.start.x, UI.MouseMapPosition().x);
			}
		}

		// Token: 0x170014DF RID: 5343
		// (get) Token: 0x06007726 RID: 30502 RVA: 0x0029D5E8 File Offset: 0x0029B7E8
		public float BotZ
		{
			get
			{
				return Math.Min(this.start.z, UI.MouseMapPosition().z);
			}
		}

		// Token: 0x170014E0 RID: 5344
		// (get) Token: 0x06007727 RID: 30503 RVA: 0x0029D604 File Offset: 0x0029B804
		public float TopZ
		{
			get
			{
				return Math.Max(this.start.z, UI.MouseMapPosition().z);
			}
		}

		// Token: 0x170014E1 RID: 5345
		// (get) Token: 0x06007728 RID: 30504 RVA: 0x0029D620 File Offset: 0x0029B820
		public Rect ScreenRect
		{
			get
			{
				Vector2 vector = this.start.MapToUIPosition();
				Vector2 mousePosition = Event.current.mousePosition;
				if (mousePosition.x < vector.x)
				{
					float x = mousePosition.x;
					mousePosition.x = vector.x;
					vector.x = x;
				}
				if (mousePosition.y < vector.y)
				{
					float y = mousePosition.y;
					mousePosition.y = vector.y;
					vector.y = y;
				}
				return new Rect
				{
					xMin = vector.x,
					xMax = mousePosition.x,
					yMin = vector.y,
					yMax = mousePosition.y
				};
			}
		}

		// Token: 0x170014E2 RID: 5346
		// (get) Token: 0x06007729 RID: 30505 RVA: 0x0029D6D8 File Offset: 0x0029B8D8
		public bool IsValid
		{
			get
			{
				return (this.start - UI.MouseMapPosition()).magnitude > 0.5f;
			}
		}

		// Token: 0x170014E3 RID: 5347
		// (get) Token: 0x0600772A RID: 30506 RVA: 0x0029D704 File Offset: 0x0029B904
		public bool IsValidAndActive
		{
			get
			{
				return this.active && this.IsValid;
			}
		}

		// Token: 0x0600772B RID: 30507 RVA: 0x0029D716 File Offset: 0x0029B916
		public void DragBoxOnGUI()
		{
			if (this.IsValidAndActive)
			{
				Widgets.DrawBox(this.ScreenRect, 2, null);
			}
		}

		// Token: 0x0600772C RID: 30508 RVA: 0x0029D730 File Offset: 0x0029B930
		public bool Contains(Thing t)
		{
			if (t is Pawn)
			{
				return this.Contains((t as Pawn).Drawer.DrawPos);
			}
			foreach (IntVec3 intVec in t.OccupiedRect())
			{
				if (this.Contains(intVec.ToVector3Shifted()))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600772D RID: 30509 RVA: 0x0029D7B4 File Offset: 0x0029B9B4
		public bool Contains(Vector3 v)
		{
			return v.x + 0.5f > this.LeftX && v.x - 0.5f < this.RightX && v.z + 0.5f > this.BotZ && v.z - 0.5f < this.TopZ;
		}

		// Token: 0x04004239 RID: 16953
		public bool active;

		// Token: 0x0400423A RID: 16954
		public Vector3 start;

		// Token: 0x0400423B RID: 16955
		private const float DragBoxMinDiagonal = 0.5f;
	}
}
