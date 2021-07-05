using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A8 RID: 2472
	public class ThoughtWorker_Hediff : ThoughtWorker
	{
		// Token: 0x06003DDE RID: 15838 RVA: 0x00153988 File Offset: 0x00151B88
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
