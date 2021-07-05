using System;

namespace RimWorld
{
	// Token: 0x02001C66 RID: 7270
	[DefOf]
	public static class RaidStrategyDefOf
	{
		// Token: 0x06009F69 RID: 40809 RVA: 0x0006A366 File Offset: 0x00068566
		static RaidStrategyDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RaidStrategyDefOf));
		}

		// Token: 0x0400692F RID: 26927
		public static RaidStrategyDef ImmediateAttack;

		// Token: 0x04006930 RID: 26928
		public static RaidStrategyDef ImmediateAttackFriendly;
	}
}
