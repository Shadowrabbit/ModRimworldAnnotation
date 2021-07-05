using System;

namespace RimWorld
{
	// Token: 0x02001475 RID: 5237
	[DefOf]
	public static class RitualOutcomeEffectDefOf
	{
		// Token: 0x06007D67 RID: 32103 RVA: 0x002C4CF3 File Offset: 0x002C2EF3
		static RitualOutcomeEffectDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RitualOutcomeEffectDefOf));
		}

		// Token: 0x04004E2D RID: 20013
		[MayRequireRoyalty]
		public static RitualOutcomeEffectDef BestowingCeremony;
	}
}
