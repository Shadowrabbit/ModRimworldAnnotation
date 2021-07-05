using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200091E RID: 2334
	public class ThinkNode_ConditionalCloseToDutyTarget : ThinkNode_Conditional
	{
		// Token: 0x06003C70 RID: 15472 RVA: 0x0014F712 File Offset: 0x0014D912
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalCloseToDutyTarget thinkNode_ConditionalCloseToDutyTarget = (ThinkNode_ConditionalCloseToDutyTarget)base.DeepCopy(resolve);
			thinkNode_ConditionalCloseToDutyTarget.maxDistToDutyTarget = this.maxDistToDutyTarget;
			return thinkNode_ConditionalCloseToDutyTarget;
		}

		// Token: 0x06003C71 RID: 15473 RVA: 0x0014F72C File Offset: 0x0014D92C
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty.focus.IsValid && pawn.Position.InHorDistOf(pawn.mindState.duty.focus.Cell, this.maxDistToDutyTarget);
		}

		// Token: 0x040020AF RID: 8367
		private float maxDistToDutyTarget = 10f;
	}
}
