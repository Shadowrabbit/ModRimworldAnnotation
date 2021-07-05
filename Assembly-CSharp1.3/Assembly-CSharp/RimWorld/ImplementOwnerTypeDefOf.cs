using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001454 RID: 5204
	[DefOf]
	public static class ImplementOwnerTypeDefOf
	{
		// Token: 0x06007D47 RID: 32071 RVA: 0x002C4AD3 File Offset: 0x002C2CD3
		static ImplementOwnerTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ImplementOwnerTypeDefOf));
		}

		// Token: 0x04004D00 RID: 19712
		public static ImplementOwnerTypeDef Weapon;

		// Token: 0x04004D01 RID: 19713
		public static ImplementOwnerTypeDef Bodypart;

		// Token: 0x04004D02 RID: 19714
		public static ImplementOwnerTypeDef Hediff;

		// Token: 0x04004D03 RID: 19715
		public static ImplementOwnerTypeDef Terrain;

		// Token: 0x04004D04 RID: 19716
		public static ImplementOwnerTypeDef NativeVerb;
	}
}
