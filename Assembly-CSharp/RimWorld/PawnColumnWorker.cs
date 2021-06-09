using System;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000FBF RID: 4031
	[StaticConstructorOnStartup]
	public abstract class PawnColumnWorker
	{
		// Token: 0x17000D9B RID: 3483
		// (get) Token: 0x0600581A RID: 22554 RVA: 0x0000BBC0 File Offset: 0x00009DC0
		protected virtual Color DefaultHeaderColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x17000D9C RID: 3484
		// (get) Token: 0x0600581B RID: 22555 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected virtual GameFont DefaultHeaderFont
		{
			get
			{
				return GameFont.Small;
			}
		}

		// Token: 0x0600581C RID: 22556 RVA: 0x001CF558 File Offset: 0x001CD758
		public virtual void DoHeader(Rect rect, PawnTable table)
		{
			if (!this.def.label.NullOrEmpty())
			{
				Text.Font = this.DefaultHeaderFont;
				GUI.color = this.DefaultHeaderColor;
				Text.Anchor = TextAnchor.LowerCenter;
				Rect rect2 = rect;
				rect2.y += 3f;
				Widgets.Label(rect2, this.def.LabelCap.Resolve().Truncate(rect.width, null));
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
				Text.Font = GameFont.Small;
			}
			else if (this.def.HeaderIcon != null)
			{
				Vector2 headerIconSize = this.def.HeaderIconSize;
				int num = (int)((rect.width - headerIconSize.x) / 2f);
				GUI.DrawTexture(new Rect(rect.x + (float)num, rect.yMax - headerIconSize.y, headerIconSize.x, headerIconSize.y).ContractedBy(2f), this.def.HeaderIcon);
			}
			if (table.SortingBy == this.def)
			{
				Texture2D texture2D = table.SortingDescending ? PawnColumnWorker.SortingDescendingIcon : PawnColumnWorker.SortingIcon;
				GUI.DrawTexture(new Rect(rect.xMax - (float)texture2D.width - 1f, rect.yMax - (float)texture2D.height - 1f, (float)texture2D.width, (float)texture2D.height), texture2D);
			}
			if (this.def.HeaderInteractable)
			{
				Rect interactableHeaderRect = this.GetInteractableHeaderRect(rect, table);
				if (Mouse.IsOver(interactableHeaderRect))
				{
					Widgets.DrawHighlight(interactableHeaderRect);
					string headerTip = this.GetHeaderTip(table);
					if (!headerTip.NullOrEmpty())
					{
						TooltipHandler.TipRegion(interactableHeaderRect, headerTip);
					}
				}
				if (Widgets.ButtonInvisible(interactableHeaderRect, true))
				{
					this.HeaderClicked(rect, table);
				}
			}
		}

		// Token: 0x0600581D RID: 22557
		public abstract void DoCell(Rect rect, Pawn pawn, PawnTable table);

		// Token: 0x0600581E RID: 22558 RVA: 0x001CF724 File Offset: 0x001CD924
		public virtual int GetMinWidth(PawnTable table)
		{
			if (!this.def.label.NullOrEmpty())
			{
				Text.Font = this.DefaultHeaderFont;
				int result = Mathf.CeilToInt(Text.CalcSize(this.def.LabelCap).x);
				Text.Font = GameFont.Small;
				return result;
			}
			if (this.def.HeaderIcon != null)
			{
				return Mathf.CeilToInt(this.def.HeaderIconSize.x);
			}
			return 1;
		}

		// Token: 0x0600581F RID: 22559 RVA: 0x0003D325 File Offset: 0x0003B525
		public virtual int GetMaxWidth(PawnTable table)
		{
			return 1000000;
		}

		// Token: 0x06005820 RID: 22560 RVA: 0x0003D32C File Offset: 0x0003B52C
		public virtual int GetOptimalWidth(PawnTable table)
		{
			return this.GetMinWidth(table);
		}

		// Token: 0x06005821 RID: 22561 RVA: 0x0003D335 File Offset: 0x0003B535
		public virtual int GetMinCellHeight(Pawn pawn)
		{
			return 30;
		}

		// Token: 0x06005822 RID: 22562 RVA: 0x001CF7A0 File Offset: 0x001CD9A0
		public virtual int GetMinHeaderHeight(PawnTable table)
		{
			if (!this.def.label.NullOrEmpty())
			{
				Text.Font = this.DefaultHeaderFont;
				int result = Mathf.CeilToInt(Text.CalcSize(this.def.LabelCap).y);
				Text.Font = GameFont.Small;
				return result;
			}
			if (this.def.HeaderIcon != null)
			{
				return Mathf.CeilToInt(this.def.HeaderIconSize.y);
			}
			return 0;
		}

		// Token: 0x06005823 RID: 22563 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual int Compare(Pawn a, Pawn b)
		{
			return 0;
		}

		// Token: 0x06005824 RID: 22564 RVA: 0x001CF81C File Offset: 0x001CDA1C
		protected virtual Rect GetInteractableHeaderRect(Rect headerRect, PawnTable table)
		{
			float num = Mathf.Min(25f, headerRect.height);
			return new Rect(headerRect.x, headerRect.yMax - num, headerRect.width, num);
		}

		// Token: 0x06005825 RID: 22565 RVA: 0x001CF858 File Offset: 0x001CDA58
		protected virtual void HeaderClicked(Rect headerRect, PawnTable table)
		{
			if (this.def.sortable && !Event.current.shift)
			{
				if (Event.current.button == 0)
				{
					if (table.SortingBy != this.def)
					{
						table.SortBy(this.def, true);
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						return;
					}
					if (table.SortingDescending)
					{
						table.SortBy(this.def, false);
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						return;
					}
					table.SortBy(null, false);
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
					return;
				}
				else if (Event.current.button == 1)
				{
					if (table.SortingBy != this.def)
					{
						table.SortBy(this.def, false);
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						return;
					}
					if (table.SortingDescending)
					{
						table.SortBy(null, false);
						SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
						return;
					}
					table.SortBy(this.def, true);
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
			}
		}

		// Token: 0x06005826 RID: 22566 RVA: 0x001CF954 File Offset: 0x001CDB54
		protected virtual string GetHeaderTip(PawnTable table)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!this.def.headerTip.NullOrEmpty())
			{
				stringBuilder.Append(this.def.headerTip);
			}
			if (this.def.sortable)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("ClickToSortByThisColumn".Translate());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04003A2D RID: 14893
		public PawnColumnDef def;

		// Token: 0x04003A2E RID: 14894
		protected const int DefaultCellHeight = 30;

		// Token: 0x04003A2F RID: 14895
		private static readonly Texture2D SortingIcon = ContentFinder<Texture2D>.Get("UI/Icons/Sorting", true);

		// Token: 0x04003A30 RID: 14896
		private static readonly Texture2D SortingDescendingIcon = ContentFinder<Texture2D>.Get("UI/Icons/SortingDescending", true);

		// Token: 0x04003A31 RID: 14897
		private const int IconMargin = 2;
	}
}
