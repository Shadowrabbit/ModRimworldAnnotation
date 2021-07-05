using System;

namespace Verse.AI.Group
{
	// Token: 0x020006A5 RID: 1701
	public class Trigger_PawnLostViolently : Trigger
	{
		// Token: 0x06002F56 RID: 12118 RVA: 0x00118B42 File Offset: 0x00116D42
		public Trigger_PawnLostViolently(bool allowRoofCollapse = true)
		{
			this.allowRoofCollapse = allowRoofCollapse;
		}

		// Token: 0x06002F57 RID: 12119 RVA: 0x00118B51 File Offset: 0x00116D51
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

		// Token: 0x04001CF1 RID: 7409
		public bool allowRoofCollapse;
	}
}
