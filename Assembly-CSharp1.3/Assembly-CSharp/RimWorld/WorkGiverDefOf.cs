using System;

namespace RimWorld
{
	// Token: 0x02001458 RID: 5208
	[DefOf]
	public static class WorkGiverDefOf
	{
		// Token: 0x06007D4B RID: 32075 RVA: 0x002C4B17 File Offset: 0x002C2D17
		static WorkGiverDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WorkGiverDefOf));
		}

		// Token: 0x04004D25 RID: 19749
		public static WorkGiverDef Refuel;

		// Token: 0x04004D26 RID: 19750
		public static WorkGiverDef Repair;

		// Token: 0x04004D27 RID: 19751
		public static WorkGiverDef DoBillsMedicalHumanOperation;
	}
}
