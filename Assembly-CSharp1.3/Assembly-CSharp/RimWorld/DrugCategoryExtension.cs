using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A25 RID: 2597
	public static class DrugCategoryExtension
	{
		// Token: 0x06003F1C RID: 16156 RVA: 0x0015819D File Offset: 0x0015639D
		public static bool IncludedIn(this DrugCategory lhs, DrugCategory rhs)
		{
			return lhs <= rhs;
		}

		// Token: 0x06003F1D RID: 16157 RVA: 0x001581A8 File Offset: 0x001563A8
		public static string GetLabel(this DrugCategory category)
		{
			switch (category)
			{
			case DrugCategory.None:
				return "DrugCategory_None".Translate();
			case DrugCategory.Medical:
				return "DrugCategory_Medical".Translate();
			case DrugCategory.Social:
				return "DrugCategory_Social".Translate();
			case DrugCategory.Hard:
				return "DrugCategory_Hard".Translate();
			}
			return "DrugCategory_Any".Translate();
		}
	}
}
