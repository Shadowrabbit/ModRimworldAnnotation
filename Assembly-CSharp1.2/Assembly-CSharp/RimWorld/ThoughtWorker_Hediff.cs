using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EB2 RID: 3762
	public class ThoughtWorker_Hediff : ThoughtWorker
	{
		// Token: 0x060053B5 RID: 21429 RVA: 0x001C1858 File Offset: 0x001BFA58
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			Hediff firstHediffOfDef = p.health.hediffSet.GetFirstHediffOfDef(this.def.hediff, false);
			if (firstHediffOfDef == null || firstHediffOfDef.def.stages == null)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(Mathf.Min(new int[]
			{
				firstHediffOfDef.CurStageIndex,
				firstHediffOfDef.def.stages.Count - 1,
				this.def.stages.Count - 1
			}));
		}
	}
}
