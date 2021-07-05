using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001B7B RID: 7035
	public class PawnColumnWorker_DrugPolicy : PawnColumnWorker
	{
		// Token: 0x06009B05 RID: 39685 RVA: 0x002D829C File Offset: 0x002D649C
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
			Rect rect2 = new Rect(rect.x, rect.y + (rect.height - 65f), Mathf.Min(rect.width, 360f), 32f);
			if (Widgets.ButtonText(rect2, "ManageDrugPolicies".Translate(), true, true, true))
			{
				Find.WindowStack.Add(new Dialog_ManageDrugPolicies(null));
			}
			UIHighlighter.HighlightOpportunity(rect2, "ManageDrugPolicies");
			UIHighlighter.HighlightOpportunity(rect2, "ButtonAssignDrugs");
		}

		// Token: 0x06009B06 RID: 39686 RVA: 0x000672B8 File Offset: 0x000654B8
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.drugs == null)
			{
				return;
			}
			DrugPolicyUIUtility.DoAssignDrugPolicyButtons(rect, pawn);
		}

		// Token: 0x06009B07 RID: 39687 RVA: 0x000672CA File Offset: 0x000654CA
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), Mathf.CeilToInt(194f));
		}

		// Token: 0x06009B08 RID: 39688 RVA: 0x000672E2 File Offset: 0x000654E2
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(Mathf.CeilToInt(251f), this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x06009B09 RID: 39689 RVA: 0x00067248 File Offset: 0x00065448
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 65);
		}

		// Token: 0x06009B0A RID: 39690 RVA: 0x002D8330 File Offset: 0x002D6530
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06009B0B RID: 39691 RVA: 0x00067301 File Offset: 0x00065501
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.drugs != null && pawn.drugs.CurrentPolicy != null)
			{
				return pawn.drugs.CurrentPolicy.uniqueId;
			}
			return int.MinValue;
		}

		// Token: 0x040062D3 RID: 25299
		private const int TopAreaHeight = 65;

		// Token: 0x040062D4 RID: 25300
		public const int ManageDrugPoliciesButtonHeight = 32;
	}
}
