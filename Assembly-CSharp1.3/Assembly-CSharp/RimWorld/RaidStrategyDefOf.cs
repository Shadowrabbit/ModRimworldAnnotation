using System;

namespace RimWorld
{
	// Token: 0x02001426 RID: 5158
	[DefOf]
	public static class RaidStrategyDefOf
	{
		// Token: 0x06007D19 RID: 32025 RVA: 0x002C47C5 File Offset: 0x002C29C5
		static RaidStrategyDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RaidStrategyDefOf));
		}

		// Token: 0x040049AD RID: 18861
		public static RaidStrategyDef ImmediateAttack;

		// Token: 0x040049AE RID: 18862
		public static RaidStrategyDef ImmediateAttackFriendly;
	}
}
