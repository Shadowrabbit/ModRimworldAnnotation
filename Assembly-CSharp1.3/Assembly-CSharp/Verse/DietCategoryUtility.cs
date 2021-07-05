using System;

namespace Verse
{
	// Token: 0x020000AB RID: 171
	public static class DietCategoryUtility
	{
		// Token: 0x06000559 RID: 1369 RVA: 0x0001BBF0 File Offset: 0x00019DF0
		public static string ToStringHuman(this DietCategory diet)
		{
			switch (diet)
			{
			case DietCategory.NeverEats:
				return "DietCategory_NeverEats".Translate();
			case DietCategory.Herbivorous:
				return "DietCategory_Herbivorous".Translate();
			case DietCategory.Dendrovorous:
				return "DietCategory_Dendrovorous".Translate();
			case DietCategory.Ovivorous:
				return "DietCategory_Ovivorous".Translate();
			case DietCategory.Omnivorous:
				return "DietCategory_Omnivorous".Translate();
			case DietCategory.Carnivorous:
				return "DietCategory_Carnivorous".Translate();
			default:
				return "error";
			}
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0001BC84 File Offset: 0x00019E84
		public static string ToStringHumanShort(this DietCategory diet)
		{
			switch (diet)
			{
			case DietCategory.NeverEats:
				return "DietCategory_NeverEats_Short".Translate();
			case DietCategory.Herbivorous:
				return "DietCategory_Herbivorous_Short".Translate();
			case DietCategory.Dendrovorous:
				return "DietCategory_Dendrovorous_Short".Translate();
			case DietCategory.Ovivorous:
				return "DietCategory_Ovivorous_Short".Translate();
			case DietCategory.Omnivorous:
				return "DietCategory_Omnivorous_Short".Translate();
			case DietCategory.Carnivorous:
				return "DietCategory_Carnivorous_Short".Translate();
			default:
				return "error";
			}
		}
	}
}
