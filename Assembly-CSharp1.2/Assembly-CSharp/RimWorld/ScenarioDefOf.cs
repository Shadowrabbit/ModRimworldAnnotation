using System;

namespace RimWorld
{
	// Token: 0x02001C65 RID: 7269
	[DefOf]
	public static class ScenarioDefOf
	{
		// Token: 0x06009F68 RID: 40808 RVA: 0x0006A355 File Offset: 0x00068555
		static ScenarioDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ScenarioDefOf));
		}

		// Token: 0x0400692D RID: 26925
		public static ScenarioDef Crashlanded;

		// Token: 0x0400692E RID: 26926
		public static ScenarioDef Tutorial;
	}
}
