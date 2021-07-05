using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001392 RID: 5010
	public class PawnColumnWorker_DrugPolicy : PawnColumnWorker
	{
		// Token: 0x060079D8 RID: 31192 RVA: 0x002B0744 File Offset: 0x002AE944
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

		// Token: 0x060079D9 RID: 31193 RVA: 0x002B07D5 File Offset: 0x002AE9D5
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.drugs == null)
			{
				return;
			}
			DrugPolicyUIUtility.DoAssignDrugPolicyButtons(rect, pawn);
		}

		// Token: 0x060079DA RID: 31194 RVA: 0x002B07E7 File Offset: 0x002AE9E7
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), Mathf.CeilToInt(194f));
		}

		// Token: 0x060079DB RID: 31195 RVA: 0x002B07FF File Offset: 0x002AE9FF
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(Mathf.CeilToInt(251f), this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x060079DC RID: 31196 RVA: 0x002B031D File Offset: 0x002AE51D
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 65);
		}

		// Token: 0x060079DD RID: 31197 RVA: 0x002B0820 File Offset: 0x002AEA20
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x060079DE RID: 31198 RVA: 0x002B0843 File Offset: 0x002AEA43
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.drugs != null && pawn.drugs.CurrentPolicy != null)
			{
				return pawn.drugs.CurrentPolicy.uniqueId;
			}
			return int.MinValue;
		}

		// Token: 0x04004389 RID: 17289
		private const int TopAreaHeight = 65;

		// Token: 0x0400438A RID: 17290
		public const int ManageDrugPoliciesButtonHeight = 32;
	}
}
