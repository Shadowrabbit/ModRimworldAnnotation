using System;

namespace RimWorld
{
	// Token: 0x02001478 RID: 5240
	[DefOf]
	public static class ComplexDefOf
	{
		// Token: 0x06007D6A RID: 32106 RVA: 0x002C4D26 File Offset: 0x002C2F26
		static ComplexDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ComplexDefOf));
		}

		// Token: 0x04004E30 RID: 20016
		[MayRequireIdeology]
		public static ComplexDef AncientComplex;

		// Token: 0x04004E31 RID: 20017
		[MayRequireIdeology]
		public static ComplexDef AncientComplex_Loot;
	}
}
