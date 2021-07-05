using System;

namespace RimWorld
{
	// Token: 0x02001446 RID: 5190
	[DefOf]
	public static class PrisonerInteractionModeDefOf
	{
		// Token: 0x06007D39 RID: 32057 RVA: 0x002C49E5 File Offset: 0x002C2BE5
		static PrisonerInteractionModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PrisonerInteractionModeDefOf));
		}

		// Token: 0x04004CBA RID: 19642
		public static PrisonerInteractionModeDef NoInteraction;

		// Token: 0x04004CBB RID: 19643
		public static PrisonerInteractionModeDef AttemptRecruit;

		// Token: 0x04004CBC RID: 19644
		public static PrisonerInteractionModeDef ReduceResistance;

		// Token: 0x04004CBD RID: 19645
		public static PrisonerInteractionModeDef Release;

		// Token: 0x04004CBE RID: 19646
		public static PrisonerInteractionModeDef Execution;

		// Token: 0x04004CBF RID: 19647
		[MayRequireIdeology]
		public static PrisonerInteractionModeDef Enslave;

		// Token: 0x04004CC0 RID: 19648
		[MayRequireIdeology]
		public static PrisonerInteractionModeDef ReduceWill;

		// Token: 0x04004CC1 RID: 19649
		[MayRequireIdeology]
		public static PrisonerInteractionModeDef Convert;
	}
}
