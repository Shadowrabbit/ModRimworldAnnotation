using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001B60 RID: 7008
	public abstract class PawnColumnWorker_Checkbox : PawnColumnWorker
	{
		// Token: 0x06009A72 RID: 39538 RVA: 0x00066D45 File Offset: 0x00064F45
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
		}

		// Token: 0x06009A73 RID: 39539 RVA: 0x002D7934 File Offset: 0x002D5B34
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
				this.SetValue(pawn, value);
			}
		}

		// Token: 0x06009A74 RID: 39540 RVA: 0x00066D55 File Offset: 0x00064F55
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 28);
		}

		// Token: 0x06009A75 RID: 39541 RVA: 0x00066D65 File Offset: 0x00064F65
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x06009A76 RID: 39542 RVA: 0x00066D7A File Offset: 0x00064F7A
		public override int GetMinCellHeight(Pawn pawn)
		{
			return Mathf.Max(base.GetMinCellHeight(pawn), 24);
		}

		// Token: 0x06009A77 RID: 39543 RVA: 0x002D7A00 File Offset: 0x002D5C00
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06009A78 RID: 39544 RVA: 0x00066D8A File Offset: 0x00064F8A
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

		// Token: 0x06009A79 RID: 39545 RVA: 0x0000C32E File Offset: 0x0000A52E
		protected virtual string GetTip(Pawn pawn)
		{
			return null;
		}

		// Token: 0x06009A7A RID: 39546 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected virtual bool HasCheckbox(Pawn pawn)
		{
			return true;
		}

		// Token: 0x06009A7B RID: 39547
		protected abstract bool GetValue(Pawn pawn);

		// Token: 0x06009A7C RID: 39548
		protected abstract void SetValue(Pawn pawn, bool value);

		// Token: 0x06009A7D RID: 39549 RVA: 0x002D7A24 File Offset: 0x002D5C24
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
								this.SetValue(pawnsListForReading[i], true);
							}
						}
						else if (Event.current.button == 1 && this.GetValue(pawnsListForReading[i]))
						{
							this.SetValue(pawnsListForReading[i], false);
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

		// Token: 0x06009A7E RID: 39550 RVA: 0x00066DA3 File Offset: 0x00064FA3
		protected override string GetHeaderTip(PawnTable table)
		{
			return base.GetHeaderTip(table) + "\n" + "CheckboxShiftClickTip".Translate();
		}

		// Token: 0x040062C4 RID: 25284
		public const int HorizontalPadding = 2;
	}
}
