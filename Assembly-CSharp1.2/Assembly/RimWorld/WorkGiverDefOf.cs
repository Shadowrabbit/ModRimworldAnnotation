using System;

namespace RimWorld
{
	// Token: 0x02001C98 RID: 7320
	[DefOf]
	public static class WorkGiverDefOf
	{
		// Token: 0x06009F9B RID: 40859 RVA: 0x0006A6B8 File Offset: 0x000688B8
		static WorkGiverDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WorkGiverDefOf));
		}

		// Token: 0x04006C31 RID: 27697
		public static WorkGiverDef Refuel;

		// Token: 0x04006C32 RID: 27698
		public static WorkGiverDef Repair;

		// Token: 0x04006C33 RID: 27699
		public static WorkGiverDef DoBillsMedicalHumanOperation;
	}
}
