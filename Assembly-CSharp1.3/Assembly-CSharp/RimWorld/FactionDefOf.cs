using System;

namespace RimWorld
{
	// Token: 0x0200140C RID: 5132
	[DefOf]
	public static class FactionDefOf
	{
		// Token: 0x06007CFF RID: 31999 RVA: 0x002C460B File Offset: 0x002C280B
		static FactionDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(FactionDefOf));
		}

		// Token: 0x0400478C RID: 18316
		public static FactionDef PlayerColony;

		// Token: 0x0400478D RID: 18317
		public static FactionDef PlayerTribe;

		// Token: 0x0400478E RID: 18318
		public static FactionDef Ancients;

		// Token: 0x0400478F RID: 18319
		public static FactionDef AncientsHostile;

		// Token: 0x04004790 RID: 18320
		public static FactionDef Mechanoid;

		// Token: 0x04004791 RID: 18321
		public static FactionDef Insect;

		// Token: 0x04004792 RID: 18322
		public static FactionDef Pirate;

		// Token: 0x04004793 RID: 18323
		public static FactionDef OutlanderCivil;

		// Token: 0x04004794 RID: 18324
		public static FactionDef TribeCivil;

		// Token: 0x04004795 RID: 18325
		public static FactionDef OutlanderRough;

		// Token: 0x04004796 RID: 18326
		public static FactionDef TribeRough;

		// Token: 0x04004797 RID: 18327
		[MayRequireRoyalty]
		public static FactionDef Empire;

		// Token: 0x04004798 RID: 18328
		[MayRequireRoyalty]
		public static FactionDef OutlanderRefugee;

		// Token: 0x04004799 RID: 18329
		[MayRequireIdeology]
		public static FactionDef Beggars;

		// Token: 0x0400479A RID: 18330
		[MayRequireIdeology]
		public static FactionDef Pilgrims;
	}
}
