using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A7F RID: 2687
	public class ThinkNode_ConditionalRequireCapacities : ThinkNode_Conditional
	{
		// Token: 0x0600400D RID: 16397 RVA: 0x0002FFAD File Offset: 0x0002E1AD
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalRequireCapacities thinkNode_ConditionalRequireCapacities = (ThinkNode_ConditionalRequireCapacities)base.DeepCopy(resolve);
			thinkNode_ConditionalRequireCapacities.requiredCapacities = this.requiredCapacities;
			return thinkNode_ConditionalRequireCapacities;
		}

		// Token: 0x0600400E RID: 16398 RVA: 0x00181CE8 File Offset: 0x0017FEE8
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

		// Token: 0x04002C28 RID: 11304
		public List<PawnCapacityDef> requiredCapacities;
	}
}
