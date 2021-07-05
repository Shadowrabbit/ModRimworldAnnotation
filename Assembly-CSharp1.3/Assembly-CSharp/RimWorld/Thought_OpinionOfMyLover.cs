using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200097F RID: 2431
	public class Thought_OpinionOfMyLover : Thought_Situational
	{
		// Token: 0x17000AD6 RID: 2774
		// (get) Token: 0x06003D81 RID: 15745 RVA: 0x00152554 File Offset: 0x00150754
		public override string LabelCap
		{
			get
			{
				DirectPawnRelation directPawnRelation = LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(this.pawn, false);
				string text = base.CurStage.label.Formatted(directPawnRelation.def.GetGenderSpecificLabel(directPawnRelation.otherPawn), directPawnRelation.otherPawn.LabelShort, directPawnRelation.otherPawn).CapitalizeFirst();
				if (this.def.Worker != null)
				{
					text = this.def.Worker.PostProcessLabel(this.pawn, text);
				}
				return text;
			}
		}

		// Token: 0x17000AD7 RID: 2775
		// (get) Token: 0x06003D82 RID: 15746 RVA: 0x001525E4 File Offset: 0x001507E4
		protected override float BaseMoodOffset
		{
			get
			{
				float num = 0.1f * (float)this.pawn.relations.OpinionOf(LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(this.pawn, false).otherPawn);
				if (num < 0f)
				{
					return Mathf.Min(num, -1f);
				}
				return Mathf.Max(num, 1f);
			}
		}
	}
}
