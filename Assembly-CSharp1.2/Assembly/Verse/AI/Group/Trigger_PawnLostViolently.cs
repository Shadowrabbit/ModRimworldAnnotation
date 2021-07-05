using System;

namespace Verse.AI.Group
{
	// Token: 0x02000B07 RID: 2823
	public class Trigger_PawnLostViolently : Trigger
	{
		// Token: 0x06004229 RID: 16937 RVA: 0x000314E4 File Offset: 0x0002F6E4
		public Trigger_PawnLostViolently(bool allowRoofCollapse = true)
		{
			this.allowRoofCollapse = allowRoofCollapse;
		}

		// Token: 0x0600422A RID: 16938 RVA: 0x000314F3 File Offset: 0x0002F6F3
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.PawnLost)
			{
				if (signal.condition == PawnLostCondition.MadePrisoner)
				{
					return true;
				}
				if (signal.condition == PawnLostCondition.IncappedOrKilled && (signal.dinfo.Category != DamageInfo.SourceCategory.Collapse || this.allowRoofCollapse))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04002D67 RID: 11623
		public bool allowRoofCollapse;
	}
}
