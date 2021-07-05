using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E90 RID: 3728
	public class Thought_WantToSleepWithSpouseOrLover : Thought_Situational
	{
		// Token: 0x17000CC3 RID: 3267
		// (get) Token: 0x0600536E RID: 21358 RVA: 0x001C0A40 File Offset: 0x001BEC40
		public override string LabelCap
		{
			get
			{
				DirectPawnRelation directPawnRelation = LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(this.pawn, false);
				string text = base.CurStage.label.Formatted(directPawnRelation.otherPawn.LabelShort, directPawnRelation.otherPawn).CapitalizeFirst();
				if (this.def.Worker != null)
				{
					text = this.def.Worker.PostProcessLabel(this.pawn, text);
				}
				return text;
			}
		}

		// Token: 0x17000CC4 RID: 3268
		// (get) Token: 0x0600536F RID: 21359 RVA: 0x0003A350 File Offset: 0x00038550
		protected override float BaseMoodOffset
		{
			get
			{
				return Mathf.Min(-0.05f * (float)this.pawn.relations.OpinionOf(LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(this.pawn, false).otherPawn), -1f);
			}
		}
	}
}
