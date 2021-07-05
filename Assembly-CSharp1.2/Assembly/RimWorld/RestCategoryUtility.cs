using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014F0 RID: 5360
	public static class RestCategoryUtility
	{
		// Token: 0x0600737B RID: 29563 RVA: 0x00234094 File Offset: 0x00232294
		public static string GetLabel(this RestCategory fatigue)
		{
			switch (fatigue)
			{
			case RestCategory.Rested:
				return "HungerLevel_Rested".Translate();
			case RestCategory.Tired:
				return "HungerLevel_Tired".Translate();
			case RestCategory.VeryTired:
				return "HungerLevel_VeryTired".Translate();
			case RestCategory.Exhausted:
				return "HungerLevel_Exhausted".Translate();
			default:
				throw new InvalidOperationException();
			}
		}
	}
}
