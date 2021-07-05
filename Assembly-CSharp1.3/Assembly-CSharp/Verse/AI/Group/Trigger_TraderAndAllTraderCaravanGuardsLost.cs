using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020006B2 RID: 1714
	public class Trigger_TraderAndAllTraderCaravanGuardsLost : Trigger
	{
		// Token: 0x06002F71 RID: 12145 RVA: 0x001190A0 File Offset: 0x001172A0
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.PawnLost)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					TraderCaravanRole traderCaravanRole = lord.ownedPawns[i].GetTraderCaravanRole();
					if (traderCaravanRole == TraderCaravanRole.Trader || traderCaravanRole == TraderCaravanRole.Guard)
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}
	}
}
