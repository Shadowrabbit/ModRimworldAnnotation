using System;

namespace Verse.AI.Group
{
	// Token: 0x02000B0A RID: 2826
	public class Trigger_NoMentalState : Trigger
	{
		// Token: 0x0600422F RID: 16943 RVA: 0x001891E8 File Offset: 0x001873E8
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
