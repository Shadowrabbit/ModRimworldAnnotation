using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001B81 RID: 7041
	public class PawnColumnWorker_FoodRestriction : PawnColumnWorker
	{
		// Token: 0x06009B1F RID: 39711 RVA: 0x002D86C0 File Offset: 0x002D68C0
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
			if (Widgets.ButtonText(new Rect(rect.x, rect.y + (rect.height - 65f), Mathf.Min(rect.width, 360f), 32f), "ManageFoodRestrictions".Translate(), true, true, true))
			{
				Find.WindowStack.Add(new Dialog_ManageFoodRestrictions(null));
			}
		}

		// Token: 0x06009B20 RID: 39712 RVA: 0x000673D2 File Offset: 0x000655D2
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.foodRestriction == null)
			{
				return;
			}
			this.DoAssignFoodRestrictionButtons(rect, pawn);
		}

		// Token: 0x06009B21 RID: 39713 RVA: 0x000673E5 File Offset: 0x000655E5
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
						}, MenuOptionPriority.Default, null, null, 0f, null, null),
						payload = foodRestriction
					};
				}
			}
			List<FoodRestriction>.Enumerator enumerator = default(List<FoodRestriction>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06009B22 RID: 39714 RVA: 0x000672CA File Offset: 0x000654CA
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), Mathf.CeilToInt(194f));
		}

		// Token: 0x06009B23 RID: 39715 RVA: 0x000672E2 File Offset: 0x000654E2
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(Mathf.CeilToInt(251f), this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x06009B24 RID: 39716 RVA: 0x00067248 File Offset: 0x00065448
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 65);
		}

		// Token: 0x06009B25 RID: 39717 RVA: 0x002D873C File Offset: 0x002D693C
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06009B26 RID: 39718 RVA: 0x000673F5 File Offset: 0x000655F5
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.foodRestriction != null && pawn.foodRestriction.CurrentFoodRestriction != null)
			{
				return pawn.foodRestriction.CurrentFoodRestriction.id;
			}
			return int.MinValue;
		}

		// Token: 0x06009B27 RID: 39719 RVA: 0x002D8760 File Offset: 0x002D6960
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

		// Token: 0x040062E3 RID: 25315
		private const int TopAreaHeight = 65;

		// Token: 0x040062E4 RID: 25316
		public const int ManageFoodRestrictionsButtonHeight = 32;
	}
}
