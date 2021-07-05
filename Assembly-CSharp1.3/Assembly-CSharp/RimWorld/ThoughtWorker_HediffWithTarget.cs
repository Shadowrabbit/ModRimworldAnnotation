using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200099D RID: 2461
	public class ThoughtWorker_HediffWithTarget : ThoughtWorker_Hediff
	{
		// Token: 0x06003DC3 RID: 15811 RVA: 0x00153488 File Offset: 0x00151688
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			HediffWithTarget hediffWithTarget = (HediffWithTarget)p.health.hediffSet.GetFirstHediffOfDef(this.def.hediff, false);
			if (hediffWithTarget == null || hediffWithTarget.target != other)
			{
				return ThoughtState.Inactive;
			}
			return this.CurrentStateInternal(p);
		}
	}
}
