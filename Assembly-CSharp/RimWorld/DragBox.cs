using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AEC RID: 6892
	public class DragBox
	{
		// Token: 0x170017DE RID: 6110
		// (get) Token: 0x060097C9 RID: 38857 RVA: 0x000651B9 File Offset: 0x000633B9
		public float LeftX
		{
			get
			{
				return Math.Min(this.start.x, UI.MouseMapPosition().x);
			}
		}

		// Token: 0x170017DF RID: 6111
		// (get) Token: 0x060097CA RID: 38858 RVA: 0x000651D5 File Offset: 0x000633D5
		public float RightX
		{
			get
			{
				return Math.Max(this.start.x, UI.MouseMapPosition().x);
			}
		}

		// Token: 0x170017E0 RID: 6112
		// (get) Token: 0x060097CB RID: 38859 RVA: 0x000651F1 File Offset: 0x000633F1
		public float BotZ
		{
			get
			{
				return Math.Min(this.start.z, UI.MouseMapPosition().z);
			}
		}

		// Token: 0x170017E1 RID: 6113
		// (get) Token: 0x060097CC RID: 38860 RVA: 0x0006520D File Offset: 0x0006340D
		public float TopZ
		{
			get
			{
				return Math.Max(this.start.z, UI.MouseMapPosition().z);
			}
		}

		// Token: 0x170017E2 RID: 6114
		// (get) Token: 0x060097CD RID: 38861 RVA: 0x002C8E54 File Offset: 0x002C7054
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

		// Token: 0x170017E3 RID: 6115
		// (get) Token: 0x060097CE RID: 38862 RVA: 0x002C8F0C File Offset: 0x002C710C
		public bool IsValid
		{
			get
			{
				return (this.start - UI.MouseMapPosition()).magnitude > 0.5f;
			}
		}

		// Token: 0x170017E4 RID: 6116
		// (get) Token: 0x060097CF RID: 38863 RVA: 0x00065229 File Offset: 0x00063429
		public bool IsValidAndActive
		{
			get
			{
				return this.active && this.IsValid;
			}
		}

		// Token: 0x060097D0 RID: 38864 RVA: 0x0006523B File Offset: 0x0006343B
		public void DragBoxOnGUI()
		{
			if (this.IsValidAndActive)
			{
				Widgets.DrawBox(this.ScreenRect, 2);
			}
		}

		// Token: 0x060097D1 RID: 38865 RVA: 0x002C8F38 File Offset: 0x002C7138
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

		// Token: 0x060097D2 RID: 38866 RVA: 0x002C8FBC File Offset: 0x002C71BC
		public bool Contains(Vector3 v)
		{
			return v.x + 0.5f > this.LeftX && v.x - 0.5f < this.RightX && v.z + 0.5f > this.BotZ && v.z - 0.5f < this.TopZ;
		}

		// Token: 0x040060FC RID: 24828
		public bool active;

		// Token: 0x040060FD RID: 24829
		public Vector3 start;

		// Token: 0x040060FE RID: 24830
		private const float DragBoxMinDiagonal = 0.5f;
	}
}
