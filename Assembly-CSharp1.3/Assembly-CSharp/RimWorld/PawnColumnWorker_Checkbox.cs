using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001376 RID: 4982
	public abstract class PawnColumnWorker_Checkbox : PawnColumnWorker
	{
		// Token: 0x06007935 RID: 31029 RVA: 0x002AF436 File Offset: 0x002AD636
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
		}

		// Token: 0x06007936 RID: 31030 RVA: 0x002AF448 File Offset: 0x002AD648
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (!this.HasCheckbox(pawn))
			{
				return;
			}
			int num = (int)((rect.width - 24f) / 2f);
			int num2 = Mathf.Max(3, 0);
			Vector2 vector = new Vector2(rect.x + (float)num, rect.y + (float)num2);
			Rect rect2 = new Rect(vector.x, vector.y, 24f, 24f);
			bool value = this.GetValue(pawn);
			bool flag = value;
			Widgets.Checkbox(vector, ref value, 24f, false, this.def.paintable, null, null);
			if (Mouse.IsOver(rect2))
			{
				string tip = this.GetTip(pawn);
				if (!tip.NullOrEmpty())
				{
					TooltipHandler.TipRegion(rect2, tip);
				}
			}
			if (value != flag)
			{
				this.SetValue(pawn, value, table);
			}
		}

		// Token: 0x06007937 RID: 31031 RVA: 0x002AF513 File Offset: 0x002AD713
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 28);
		}

		// Token: 0x06007938 RID: 31032 RVA: 0x002AF523 File Offset: 0x002AD723
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x06007939 RID: 31033 RVA: 0x002AF538 File Offset: 0x002AD738
		public override int GetMinCellHeight(Pawn pawn)
		{
			return Mathf.Max(base.GetMinCellHeight(pawn), 24);
		}

		// Token: 0x0600793A RID: 31034 RVA: 0x002AF548 File Offset: 0x002AD748
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x0600793B RID: 31035 RVA: 0x002AF56B File Offset: 0x002AD76B
		private int GetValueToCompare(Pawn pawn)
		{
			if (!this.HasCheckbox(pawn))
			{
				return 0;
			}
			if (!this.GetValue(pawn))
			{
				return 1;
			}
			return 2;
		}

		// Token: 0x0600793C RID: 31036 RVA: 0x00002688 File Offset: 0x00000888
		protected virtual string GetTip(Pawn pawn)
		{
			return null;
		}

		// Token: 0x0600793D RID: 31037 RVA: 0x000126F5 File Offset: 0x000108F5
		protected virtual bool HasCheckbox(Pawn pawn)
		{
			return true;
		}

		// Token: 0x0600793E RID: 31038
		protected abstract bool GetValue(Pawn pawn);

		// Token: 0x0600793F RID: 31039
		protected abstract void SetValue(Pawn pawn, bool value, PawnTable table);

		// Token: 0x06007940 RID: 31040 RVA: 0x002AF584 File Offset: 0x002AD784
		protected override void HeaderClicked(Rect headerRect, PawnTable table)
		{
			base.HeaderClicked(headerRect, table);
			if (Event.current.shift)
			{
				List<Pawn> pawnsListForReading = table.PawnsListForReading;
				for (int i = 0; i < pawnsListForReading.Count; i++)
				{
					if (this.HasCheckbox(pawnsListForReading[i]))
					{
						if (Event.current.button == 0)
						{
							if (!this.GetValue(pawnsListForReading[i]))
							{
								this.SetValue(pawnsListForReading[i], true, table);
							}
						}
						else if (Event.current.button == 1 && this.GetValue(pawnsListForReading[i]))
						{
							this.SetValue(pawnsListForReading[i], false, table);
						}
					}
				}
				if (Event.current.button == 0)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
					return;
				}
				if (Event.current.button == 1)
				{
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
			}
		}

		// Token: 0x06007941 RID: 31041 RVA: 0x002AF656 File Offset: 0x002AD856
		protected override string GetHeaderTip(PawnTable table)
		{
			return base.GetHeaderTip(table) + "\n" + "CheckboxShiftClickTip".Translate();
		}

		// Token: 0x0400437D RID: 17277
		public const int HorizontalPadding = 2;
	}
}
