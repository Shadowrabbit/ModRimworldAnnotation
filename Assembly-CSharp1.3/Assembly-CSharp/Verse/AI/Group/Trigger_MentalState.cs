using System;

namespace Verse.AI.Group
{
	// Token: 0x020006A7 RID: 1703
	public class Trigger_MentalState : Trigger
	{
		// Token: 0x06002F5B RID: 12123 RVA: 0x00118BF4 File Offset: 0x00116DF4
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					if (lord.ownedPawns[i].InMentalState)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
