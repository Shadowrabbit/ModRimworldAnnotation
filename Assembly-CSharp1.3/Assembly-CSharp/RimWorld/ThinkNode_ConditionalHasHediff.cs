using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000931 RID: 2353
	public class ThinkNode_ConditionalHasHediff : ThinkNode_Conditional
	{
		// Token: 0x06003C99 RID: 15513 RVA: 0x0014FCD7 File Offset: 0x0014DED7
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalHasHediff thinkNode_ConditionalHasHediff = (ThinkNode_ConditionalHasHediff)base.DeepCopy(resolve);
			thinkNode_ConditionalHasHediff.hediff = this.hediff;
			thinkNode_ConditionalHasHediff.severityRange = this.severityRange;
			return thinkNode_ConditionalHasHediff;
		}

		// Token: 0x06003C9A RID: 15514 RVA: 0x0014FD00 File Offset: 0x0014DF00
		protected override bool Satisfied(Pawn pawn)
		{
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(this.hediff, false);
			return firstHediffOfDef != null && firstHediffOfDef.Severity >= this.severityRange.RandomInRange;
		}

		// Token: 0x040020B3 RID: 8371
		public HediffDef hediff;

		// Token: 0x040020B4 RID: 8372
		public FloatRange severityRange = FloatRange.Zero;
	}
}
