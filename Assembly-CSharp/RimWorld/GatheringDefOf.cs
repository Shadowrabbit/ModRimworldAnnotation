using System;

namespace RimWorld
{
	// Token: 0x02001CA4 RID: 7332
	[DefOf]
	public static class GatheringDefOf
	{
		// Token: 0x06009FA7 RID: 40871 RVA: 0x0006A784 File Offset: 0x00068984
		static GatheringDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(GatheringDefOf));
		}

		// Token: 0x04006C64 RID: 27748
		public static GatheringDef Party;

		// Token: 0x04006C65 RID: 27749
		public static GatheringDef MarriageCeremony;

		// Token: 0x04006C66 RID: 27750
		[MayRequireRoyalty]
		public static GatheringDef ThroneSpeech;
	}
}
