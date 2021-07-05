using System;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000A9C RID: 2716
	[StaticConstructorOnStartup]
	public abstract class PawnColumnWorker
	{
		// Token: 0x17000B59 RID: 2905
		// (get) Token: 0x06004097 RID: 16535 RVA: 0x0001A4C7 File Offset: 0x000186C7
		protected virtual Color DefaultHeaderColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x17000B5A RID: 2906
		// (get) Token: 0x06004098 RID: 16536 RVA: 0x000126F5 File Offset: 0x000108F5
		protected virtual GameFont DefaultHeaderFont
		{
			get
			{
				return GameFont.Small;
			}
		}

		// Token: 0x06004099 RID: 16537 RVA: 0x0015D498 File Offset: 0x0015B698
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

		// Token: 0x0600409A RID: 16538
		public abstract void DoCell(Rect rect, Pawn pawn, PawnTable table);

		// Token: 0x0600409B RID: 16539 RVA: 0x0015D664 File Offset: 0x0015B864
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

		// Token: 0x0600409C RID: 16540 RVA: 0x0015D6DE File Offset: 0x0015B8DE
		public virtual int GetMaxWidth(PawnTable table)
		{
			return 1000000;
		}

		// Token: 0x0600409D RID: 16541 RVA: 0x0015D6E5 File Offset: 0x0015B8E5
		public virtual int GetOptimalWidth(PawnTable table)
		{
			return this.GetMinWidth(table);
		}

		// Token: 0x0600409E RID: 16542 RVA: 0x0015D6EE File Offset: 0x0015B8EE
		public virtual int GetMinCellHeight(Pawn pawn)
		{
			return 30;
		}

		// Token: 0x0600409F RID: 16543 RVA: 0x0015D6F4 File Offset: 0x0015B8F4
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

		// Token: 0x060040A0 RID: 16544 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual int Compare(Pawn a, Pawn b)
		{
			return 0;
		}

		// Token: 0x060040A1 RID: 16545 RVA: 0x0015D770 File Offset: 0x0015B970
		protected virtual Rect GetInteractableHeaderRect(Rect headerRect, PawnTable table)
		{
			float num = Mathf.Min(25f, headerRect.height);
			return new Rect(headerRect.x, headerRect.yMax - num, headerRect.width, num);
		}

		// Token: 0x060040A2 RID: 16546 RVA: 0x0015D7AC File Offset: 0x0015B9AC
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

		// Token: 0x060040A3 RID: 16547 RVA: 0x0015D8A8 File Offset: 0x0015BAA8
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

		// Token: 0x04002586 RID: 9606
		public PawnColumnDef def;

		// Token: 0x04002587 RID: 9607
		protected const int DefaultCellHeight = 30;

		// Token: 0x04002588 RID: 9608
		private static readonly Texture2D SortingIcon = ContentFinder<Texture2D>.Get("UI/Icons/Sorting", true);

		// Token: 0x04002589 RID: 9609
		private static readonly Texture2D SortingDescendingIcon = ContentFinder<Texture2D>.Get("UI/Icons/SortingDescending", true);

		// Token: 0x0400258A RID: 9610
		private const int IconMargin = 2;
	}
}
