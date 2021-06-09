using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014E7 RID: 5351
	public static class HungerLevelUtility
	{
		// Token: 0x0600734C RID: 29516 RVA: 0x002334A4 File Offset: 0x002316A4
		public static string GetLabel(this HungerCategory hunger)
		{
			switch (hunger)
			{
			case HungerCategory.Fed:
				return "HungerLevel_Fed".Translate();
			case HungerCategory.Hungry:
				return "HungerLevel_Hungry".Translate();
			case HungerCategory.UrgentlyHungry:
				return "HungerLevel_UrgentlyHungry".Translate();
			case HungerCategory.Starving:
				return "HungerLevel_Starving".Translate();
			default:
				throw new InvalidOperationException();
			}
		}
	}
}
