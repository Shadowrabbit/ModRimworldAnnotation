using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200144C RID: 5196
	[DefOf]
	public static class TrainabilityDefOf
	{
		// Token: 0x06007D3F RID: 32063 RVA: 0x002C4A4B File Offset: 0x002C2C4B
		static TrainabilityDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TrainabilityDefOf));
		}

		// Token: 0x04004CD0 RID: 19664
		public static TrainabilityDef None;

		// Token: 0x04004CD1 RID: 19665
		public static TrainabilityDef Intermediate;

		// Token: 0x04004CD2 RID: 19666
		public static TrainabilityDef Advanced;
	}
}
