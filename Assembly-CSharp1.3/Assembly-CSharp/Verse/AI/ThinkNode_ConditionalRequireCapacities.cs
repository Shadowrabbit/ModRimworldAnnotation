using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000620 RID: 1568
	public class ThinkNode_ConditionalRequireCapacities : ThinkNode_Conditional
	{
		// Token: 0x06002D2C RID: 11564 RVA: 0x0010F28E File Offset: 0x0010D48E
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalRequireCapacities thinkNode_ConditionalRequireCapacities = (ThinkNode_ConditionalRequireCapacities)base.DeepCopy(resolve);
			thinkNode_ConditionalRequireCapacities.requiredCapacities = this.requiredCapacities;
			return thinkNode_ConditionalRequireCapacities;
		}

		// Token: 0x06002D2D RID: 11565 RVA: 0x0010F2A8 File Offset: 0x0010D4A8
		protected override bool Satisfied(Pawn pawn)
		{
			if (pawn.health != null && pawn.health.capacities != null)
			{
				foreach (PawnCapacityDef capacity in this.requiredCapacities)
				{
					if (!pawn.health.capacities.CapableOf(capacity))
					{
						return false;
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x04001BB1 RID: 7089
		public List<PawnCapacityDef> requiredCapacities;
	}
}
