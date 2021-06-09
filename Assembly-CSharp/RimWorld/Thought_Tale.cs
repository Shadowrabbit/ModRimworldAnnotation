using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EA7 RID: 3751
	public class Thought_Tale : Thought_SituationalSocial
	{
		// Token: 0x0600539D RID: 21405 RVA: 0x001C1430 File Offset: 0x001BF630
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
