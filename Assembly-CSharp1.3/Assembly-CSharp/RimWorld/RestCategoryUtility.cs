using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E53 RID: 3667
	public static class RestCategoryUtility
	{
		// Token: 0x060054E6 RID: 21734 RVA: 0x001CC27C File Offset: 0x001CA47C
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
