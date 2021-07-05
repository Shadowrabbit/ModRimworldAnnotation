using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001189 RID: 4489
	public static class RottableUtility
	{
		// Token: 0x06006BF5 RID: 27637 RVA: 0x0024357C File Offset: 0x0024177C
		public static bool IsNotFresh(this Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			return compRottable != null && compRottable.Stage > RotStage.Fresh;
		}

		// Token: 0x06006BF6 RID: 27638 RVA: 0x002435A0 File Offset: 0x002417A0
		public static bool IsDessicated(this Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			return compRottable != null && compRottable.Stage == RotStage.Dessicated;
		}

		// Token: 0x06006BF7 RID: 27639 RVA: 0x002435C4 File Offset: 0x002417C4
		public static RotStage GetRotStage(this Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			if (compRottable == null)
			{
				return RotStage.Fresh;
			}
			return compRottable.Stage;
		}
	}
}
