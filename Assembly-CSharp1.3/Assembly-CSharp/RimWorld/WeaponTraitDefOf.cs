using System;

namespace RimWorld
{
	// Token: 0x0200146A RID: 5226
	[DefOf]
	public static class WeaponTraitDefOf
	{
		// Token: 0x06007D5C RID: 32092 RVA: 0x002C4C38 File Offset: 0x002C2E38
		static WeaponTraitDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WeaponTraitDefOf));
		}

		// Token: 0x04004D8F RID: 19855
		[MayRequireRoyalty]
		public static WeaponTraitDef NeedKill;
	}
}
