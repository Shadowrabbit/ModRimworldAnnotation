using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200182C RID: 6188
	public static class RottableUtility
	{
		// Token: 0x06008927 RID: 35111 RVA: 0x00281614 File Offset: 0x0027F814
		public static bool IsNotFresh(this Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			return compRottable != null && compRottable.Stage > RotStage.Fresh;
		}

		// Token: 0x06008928 RID: 35112 RVA: 0x00281638 File Offset: 0x0027F838
		public static bool IsDessicated(this Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			return compRottable != null && compRottable.Stage == RotStage.Dessicated;
		}

		// Token: 0x06008929 RID: 35113 RVA: 0x0028165C File Offset: 0x0027F85C
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
