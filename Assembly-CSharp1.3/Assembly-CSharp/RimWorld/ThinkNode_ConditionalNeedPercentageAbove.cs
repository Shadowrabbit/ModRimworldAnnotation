using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000919 RID: 2329
	public class ThinkNode_ConditionalNeedPercentageAbove : ThinkNode_Conditional
	{
		// Token: 0x06003C65 RID: 15461 RVA: 0x0014F60D File Offset: 0x0014D80D
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalNeedPercentageAbove thinkNode_ConditionalNeedPercentageAbove = (ThinkNode_ConditionalNeedPercentageAbove)base.DeepCopy(resolve);
			thinkNode_ConditionalNeedPercentageAbove.need = this.need;
			thinkNode_ConditionalNeedPercentageAbove.threshold = this.threshold;
			return thinkNode_ConditionalNeedPercentageAbove;
		}

		// Token: 0x06003C66 RID: 15462 RVA: 0x0014F634 File Offset: 0x0014D834
		protected override bool Satisfied(Pawn pawn)
		{
			Need need = pawn.needs.TryGetNeed(this.need);
			return need != null && need.CurInstantLevelPercentage > this.threshold;
		}

		// Token: 0x040020AD RID: 8365
		private NeedDef need;

		// Token: 0x040020AE RID: 8366
		private float threshold;
	}
}
