using System;

namespace RimWorld
{
	// Token: 0x02001425 RID: 5157
	[DefOf]
	public static class ScenarioDefOf
	{
		// Token: 0x06007D18 RID: 32024 RVA: 0x002C47B4 File Offset: 0x002C29B4
		static ScenarioDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ScenarioDefOf));
		}

		// Token: 0x040049AB RID: 18859
		public static ScenarioDef Crashlanded;

		// Token: 0x040049AC RID: 18860
		public static ScenarioDef Tutorial;
	}
}
