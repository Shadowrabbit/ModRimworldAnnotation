using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E89 RID: 3721
	public class Thought_OpinionOfMyLover : Thought_Situational
	{
		// Token: 0x17000CBE RID: 3262
		// (get) Token: 0x0600535B RID: 21339 RVA: 0x001C07BC File Offset: 0x001BE9BC
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

		// Token: 0x17000CBF RID: 3263
		// (get) Token: 0x0600535C RID: 21340 RVA: 0x001C084C File Offset: 0x001BEA4C
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
