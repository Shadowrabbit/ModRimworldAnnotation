using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000E67 RID: 3687
	public class ThinkNode_ConditionalCloseToDutyTarget : ThinkNode_Conditional
	{
		// Token: 0x06005304 RID: 21252 RVA: 0x0003A00D File Offset: 0x0003820D
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalCloseToDutyTarget thinkNode_ConditionalCloseToDutyTarget = (ThinkNode_ConditionalCloseToDutyTarget)base.DeepCopy(resolve);
			thinkNode_ConditionalCloseToDutyTarget.maxDistToDutyTarget = this.maxDistToDutyTarget;
			return thinkNode_ConditionalCloseToDutyTarget;
		}

		// Token: 0x06005305 RID: 21253 RVA: 0x001BFCA4 File Offset: 0x001BDEA4
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty.focus.IsValid && pawn.Position.InHorDistOf(pawn.mindState.duty.focus.Cell, this.maxDistToDutyTarget);
		}

		// Token: 0x040034FD RID: 13565
		private float maxDistToDutyTarget = 10f;
	}
}
