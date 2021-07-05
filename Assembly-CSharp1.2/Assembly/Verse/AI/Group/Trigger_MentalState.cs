using System;

namespace Verse.AI.Group
{
	// Token: 0x02000B09 RID: 2825
	public class Trigger_MentalState : Trigger
	{
		// Token: 0x0600422D RID: 16941 RVA: 0x001891A4 File Offset: 0x001873A4
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
