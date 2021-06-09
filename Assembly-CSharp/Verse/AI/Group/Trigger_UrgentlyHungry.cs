using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000B0B RID: 2827
	public class Trigger_UrgentlyHungry : Trigger
	{
		// Token: 0x06004231 RID: 16945 RVA: 0x0018922C File Offset: 0x0018742C
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
