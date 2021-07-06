using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C8B RID: 7307
	[DefOf]
	public static class TrainabilityDefOf
	{
		// Token: 0x06009F8E RID: 40846 RVA: 0x0006A5DB File Offset: 0x000687DB
		static TrainabilityDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TrainabilityDefOf));
		}

		// Token: 0x04006BD8 RID: 27608
		public static TrainabilityDef None;

		// Token: 0x04006BD9 RID: 27609
		public static TrainabilityDef Simple;

		// Token: 0x04006BDA RID: 27610
		public static TrainabilityDef Intermediate;

		// Token: 0x04006BDB RID: 27611
		public static TrainabilityDef Advanced;
	}
}
