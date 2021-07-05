using System;

namespace Verse.AI.Group
{
	// Token: 0x020006A8 RID: 1704
	public class Trigger_NoMentalState : Trigger
	{
		// Token: 0x06002F5D RID: 12125 RVA: 0x00118C38 File Offset: 0x00116E38
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					if (lord.ownedPawns[i].InMentalState)
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
