using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B70 RID: 7024
	public abstract class PawnColumnWorker_Icon : PawnColumnWorker
	{
		// Token: 0x17001866 RID: 6246
		// (get) Token: 0x06009AC4 RID: 39620 RVA: 0x00067044 File Offset: 0x00065244
		protected virtual int Width
		{
			get
			{
				return 26;
			}
		}

		// Token: 0x17001867 RID: 6247
		// (get) Token: 0x06009AC5 RID: 39621 RVA: 0x0001B6B4 File Offset: 0x000198B4
		protected virtual int Padding
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06009AC6 RID: 39622 RVA: 0x002D7EF0 File Offset: 0x002D60F0
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			Texture2D iconFor = this.GetIconFor(pawn);
			if (iconFor != null)
			{
				Vector2 iconSize = this.GetIconSize(pawn);
				int num = (int)((rect.width - iconSize.x) / 2f);
				int num2 = Mathf.Max((int)((30f - iconSize.y) / 2f), 0);
				Rect rect2 = new Rect(rect.x + (float)num, rect.y + (float)num2, iconSize.x, iconSize.y);
				GUI.color = this.GetIconColor(pawn);
				GUI.DrawTexture(rect2.ContractedBy((float)this.Padding), iconFor);
				GUI.color = Color.white;
				if (Mouse.IsOver(rect2))
				{
					string iconTip = this.GetIconTip(pawn);
					if (!iconTip.NullOrEmpty())
					{
						TooltipHandler.TipRegion(rect2, iconTip);
					}
				}
				if (Widgets.ButtonInvisible(rect2, false))
				{
					this.ClickedIcon(pawn);
				}
				if (Mouse.IsOver(rect2) && Input.GetMouseButton(0))
				{
					this.PaintedIcon(pawn);
				}
			}
		}

		// Token: 0x06009AC7 RID: 39623 RVA: 0x00067048 File Offset: 0x00065248
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), this.Width);
		}

		// Token: 0x06009AC8 RID: 39624 RVA: 0x00066D65 File Offset: 0x00064F65
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x06009AC9 RID: 39625 RVA: 0x0006705C File Offset: 0x0006525C
		public override int GetMinCellHeight(Pawn pawn)
		{
			return Mathf.Max(base.GetMinCellHeight(pawn), Mathf.CeilToInt(this.GetIconSize(pawn).y));
		}

		// Token: 0x06009ACA RID: 39626 RVA: 0x002D7FEC File Offset: 0x002D61EC
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06009ACB RID: 39627 RVA: 0x002D8010 File Offset: 0x002D6210
		private int GetValueToCompare(Pawn pawn)
		{
			Texture2D iconFor = this.GetIconFor(pawn);
			if (!(iconFor != null))
			{
				return int.MinValue;
			}
			return iconFor.GetInstanceID();
		}

		// Token: 0x06009ACC RID: 39628
		protected abstract Texture2D GetIconFor(Pawn pawn);

		// Token: 0x06009ACD RID: 39629 RVA: 0x0000C32E File Offset: 0x0000A52E
		protected virtual string GetIconTip(Pawn pawn)
		{
			return null;
		}

		// Token: 0x06009ACE RID: 39630 RVA: 0x0000BBC0 File Offset: 0x00009DC0
		protected virtual Color GetIconColor(Pawn pawn)
		{
			return Color.white;
		}

		// Token: 0x06009ACF RID: 39631 RVA: 0x00006A05 File Offset: 0x00004C05
		protected virtual void ClickedIcon(Pawn pawn)
		{
		}

		// Token: 0x06009AD0 RID: 39632 RVA: 0x00006A05 File Offset: 0x00004C05
		protected virtual void PaintedIcon(Pawn pawn)
		{
		}

		// Token: 0x06009AD1 RID: 39633 RVA: 0x0006707B File Offset: 0x0006527B
		protected virtual Vector2 GetIconSize(Pawn pawn)
		{
			if (this.GetIconFor(pawn) == null)
			{
				return Vector2.zero;
			}
			return new Vector2((float)this.Width, (float)this.Width);
		}
	}
}
