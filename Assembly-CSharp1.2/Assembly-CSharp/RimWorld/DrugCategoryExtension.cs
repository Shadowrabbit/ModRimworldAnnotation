using System;

namespace RimWorld
{
	// Token: 0x02000F35 RID: 3893
	public static class DrugCategoryExtension
	{
		// Token: 0x060055A5 RID: 21925 RVA: 0x0003B826 File Offset: 0x00039A26
		public static bool IncludedIn(this DrugCategory lhs, DrugCategory rhs)
		{
			return lhs <= rhs;
		}
	}
}
