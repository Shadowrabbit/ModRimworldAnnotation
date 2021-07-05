using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020006A9 RID: 1705
	public class Trigger_UrgentlyHungry : Trigger
	{
		// Token: 0x06002F5F RID: 12127 RVA: 0x00118C7C File Offset: 0x00116E7C
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					if (lord.ownedPawns[i].needs.food.CurCategory >= HungerCategory.UrgentlyHungry)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
