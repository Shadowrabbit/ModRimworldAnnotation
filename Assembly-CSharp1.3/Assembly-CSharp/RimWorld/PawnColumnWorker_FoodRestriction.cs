using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001394 RID: 5012
	public class PawnColumnWorker_FoodRestriction : PawnColumnWorker
	{
		// Token: 0x060079E2 RID: 31202 RVA: 0x002B0A38 File Offset: 0x002AEC38
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
			if (Widgets.ButtonText(new Rect(rect.x, rect.y + (rect.height - 65f), Mathf.Min(rect.width, 360f), 32f), "ManageFoodRestrictions".Translate(), true, true, true))
			{
				Find.WindowStack.Add(new Dialog_ManageFoodRestrictions(null));
			}
		}

		// Token: 0x060079E3 RID: 31203 RVA: 0x002B0AB3 File Offset: 0x002AECB3
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.foodRestriction == null)
			{
				return;
			}
			this.DoAssignFoodRestrictionButtons(rect, pawn);
		}

		// Token: 0x060079E4 RID: 31204 RVA: 0x002B0AC6 File Offset: 0x002AECC6
		private IEnumerable<Widgets.DropdownMenuElement<FoodRestriction>> Button_GenerateMenu(Pawn pawn)
		{
			using (List<FoodRestriction>.Enumerator enumerator = Current.Game.foodRestrictionDatabase.AllFoodRestrictions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FoodRestriction foodRestriction = enumerator.Current;
					yield return new Widgets.DropdownMenuElement<FoodRestriction>
					{
						option = new FloatMenuOption(foodRestriction.label, delegate()
						{
							pawn.foodRestriction.CurrentFoodRestriction = foodRestriction;
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
						payload = foodRestriction
					};
				}
			}
			List<FoodRestriction>.Enumerator enumerator = default(List<FoodRestriction>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060079E5 RID: 31205 RVA: 0x002B07E7 File Offset: 0x002AE9E7
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), Mathf.CeilToInt(194f));
		}

		// Token: 0x060079E6 RID: 31206 RVA: 0x002B07FF File Offset: 0x002AE9FF
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(Mathf.CeilToInt(251f), this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x060079E7 RID: 31207 RVA: 0x002B031D File Offset: 0x002AE51D
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 65);
		}

		// Token: 0x060079E8 RID: 31208 RVA: 0x002B0AD8 File Offset: 0x002AECD8
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x060079E9 RID: 31209 RVA: 0x002B0AFB File Offset: 0x002AECFB
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.foodRestriction != null && pawn.foodRestriction.CurrentFoodRestriction != null)
			{
				return pawn.foodRestriction.CurrentFoodRestriction.id;
			}
			return int.MinValue;
		}

		// Token: 0x060079EA RID: 31210 RVA: 0x002B0B28 File Offset: 0x002AED28
		private void DoAssignFoodRestrictionButtons(Rect rect, Pawn pawn)
		{
			int num = Mathf.FloorToInt((rect.width - 4f) * 0.71428573f);
			int num2 = Mathf.FloorToInt((rect.width - 4f) * 0.2857143f);
			float num3 = rect.x;
			Rect rect2 = new Rect(num3, rect.y + 2f, (float)num, rect.height - 4f);
			Widgets.Dropdown<Pawn, FoodRestriction>(rect2, pawn, (Pawn p) => p.foodRestriction.CurrentFoodRestriction, new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<FoodRestriction>>>(this.Button_GenerateMenu), pawn.foodRestriction.CurrentFoodRestriction.label.Truncate(rect2.width, null), null, pawn.foodRestriction.CurrentFoodRestriction.label, null, null, true);
			num3 += (float)num;
			num3 += 4f;
			if (Widgets.ButtonText(new Rect(num3, rect.y + 2f, (float)num2, rect.height - 4f), "AssignTabEdit".Translate(), true, true, true))
			{
				Find.WindowStack.Add(new Dialog_ManageFoodRestrictions(pawn.foodRestriction.CurrentFoodRestriction));
			}
			num3 += (float)num2;
		}

		// Token: 0x0400438C RID: 17292
		private const int TopAreaHeight = 65;

		// Token: 0x0400438D RID: 17293
		public const int ManageFoodRestrictionsButtonHeight = 32;
	}
}
