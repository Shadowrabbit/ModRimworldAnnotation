using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C94 RID: 7316
	[DefOf]
	public static class ImplementOwnerTypeDefOf
	{
		// Token: 0x06009F97 RID: 40855 RVA: 0x0006A674 File Offset: 0x00068874
		static ImplementOwnerTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ImplementOwnerTypeDefOf));
		}

		// Token: 0x04006C0C RID: 27660
		public static ImplementOwnerTypeDef Weapon;

		// Token: 0x04006C0D RID: 27661
		public static ImplementOwnerTypeDef Bodypart;

		// Token: 0x04006C0E RID: 27662
		public static ImplementOwnerTypeDef Hediff;

		// Token: 0x04006C0F RID: 27663
		public static ImplementOwnerTypeDef Terrain;

		// Token: 0x04006C10 RID: 27664
		public static ImplementOwnerTypeDef NativeVerb;
	}
}
