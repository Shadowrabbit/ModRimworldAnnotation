using System;

namespace RimWorld
{
	// Token: 0x02001CA9 RID: 7337
	[DefOf]
	public static class WeaponTraitDefOf
	{
		// Token: 0x06009FAC RID: 40876 RVA: 0x0006A7D9 File Offset: 0x000689D9
		static WeaponTraitDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WeaponTraitDefOf));
		}

		// Token: 0x04006C78 RID: 27768
		[MayRequireRoyalty]
		public static WeaponTraitDef NeedKill;
	}
}
