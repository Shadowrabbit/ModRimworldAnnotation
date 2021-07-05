using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E48 RID: 3656
	public static class HungerLevelUtility
	{
		// Token: 0x060054AC RID: 21676 RVA: 0x001CB0F8 File Offset: 0x001C92F8
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
