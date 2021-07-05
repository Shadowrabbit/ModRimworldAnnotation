using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EA8 RID: 3752
	public class ThoughtWorker_HediffWithTarget : ThoughtWorker_Hediff
	{
		// Token: 0x0600539F RID: 21407 RVA: 0x001C14D0 File Offset: 0x001BF6D0
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
