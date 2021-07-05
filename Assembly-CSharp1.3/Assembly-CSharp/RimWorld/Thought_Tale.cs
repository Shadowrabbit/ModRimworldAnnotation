using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200099C RID: 2460
	public class Thought_Tale : Thought_SituationalSocial
	{
		// Token: 0x06003DC0 RID: 15808 RVA: 0x001533AE File Offset: 0x001515AE
		protected override ThoughtState CurrentStateInternal()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return false;
			}
			return this.def.Worker.CurrentSocialState(this.pawn, this.otherPawn);
		}

		// Token: 0x06003DC1 RID: 15809 RVA: 0x001533E8 File Offset: 0x001515E8
		public override float OpinionOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			Tale latestTale = Find.TaleManager.GetLatestTale(this.def.taleDef, this.otherPawn);
			if (latestTale != null)
			{
				float num = 1f;
				if (latestTale.def.type == TaleType.Expirable)
				{
					float value = (float)latestTale.AgeTicks / (latestTale.def.expireDays * 60000f);
					num = Mathf.InverseLerp(1f, this.def.lerpOpinionToZeroAfterDurationPct, value);
				}
				return base.CurStage.baseOpinionOffset * num;
			}
			return 0f;
		}
	}
}
