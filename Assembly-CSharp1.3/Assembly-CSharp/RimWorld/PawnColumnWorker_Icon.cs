using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001385 RID: 4997
	public abstract class PawnColumnWorker_Icon : PawnColumnWorker
	{
		// Token: 0x17001561 RID: 5473
		// (get) Token: 0x06007988 RID: 31112 RVA: 0x002AFD37 File Offset: 0x002ADF37
		protected virtual int Width
		{
			get
			{
				return 26;
			}
		}

		// Token: 0x17001562 RID: 5474
		// (get) Token: 0x06007989 RID: 31113 RVA: 0x0009007E File Offset: 0x0008E27E
		protected virtual int Padding
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x0600798A RID: 31114 RVA: 0x002AFD3C File Offset: 0x002ADF3C
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

		// Token: 0x0600798B RID: 31115 RVA: 0x002AFE38 File Offset: 0x002AE038
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), this.Width);
		}

		// Token: 0x0600798C RID: 31116 RVA: 0x002AF523 File Offset: 0x002AD723
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x0600798D RID: 31117 RVA: 0x002AFE4C File Offset: 0x002AE04C
		public override int GetMinCellHeight(Pawn pawn)
		{
			return Mathf.Max(base.GetMinCellHeight(pawn), Mathf.CeilToInt(this.GetIconSize(pawn).y));
		}

		// Token: 0x0600798E RID: 31118 RVA: 0x002AFE6C File Offset: 0x002AE06C
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x0600798F RID: 31119 RVA: 0x002AFE90 File Offset: 0x002AE090
		private int GetValueToCompare(Pawn pawn)
		{
			Texture2D iconFor = this.GetIconFor(pawn);
			if (!(iconFor != null))
			{
				return int.MinValue;
			}
			return iconFor.GetInstanceID();
		}

		// Token: 0x06007990 RID: 31120
		protected abstract Texture2D GetIconFor(Pawn pawn);

		// Token: 0x06007991 RID: 31121 RVA: 0x00002688 File Offset: 0x00000888
		protected virtual string GetIconTip(Pawn pawn)
		{
			return null;
		}

		// Token: 0x06007992 RID: 31122 RVA: 0x0001A4C7 File Offset: 0x000186C7
		protected virtual Color GetIconColor(Pawn pawn)
		{
			return Color.white;
		}

		// Token: 0x06007993 RID: 31123 RVA: 0x0000313F File Offset: 0x0000133F
		protected virtual void ClickedIcon(Pawn pawn)
		{
		}

		// Token: 0x06007994 RID: 31124 RVA: 0x0000313F File Offset: 0x0000133F
		protected virtual void PaintedIcon(Pawn pawn)
		{
		}

		// Token: 0x06007995 RID: 31125 RVA: 0x002AFEBA File Offset: 0x002AE0BA
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
