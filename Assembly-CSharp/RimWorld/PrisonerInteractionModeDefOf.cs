using System;

namespace RimWorld
{
	// Token: 0x02001C86 RID: 7302
	[DefOf]
	public static class PrisonerInteractionModeDefOf
	{
		// Token: 0x06009F89 RID: 40841 RVA: 0x0006A586 File Offset: 0x00068786
		static PrisonerInteractionModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PrisonerInteractionModeDefOf));
		}

		// Token: 0x04006BCA RID: 27594
		public static PrisonerInteractionModeDef NoInteraction;

		// Token: 0x04006BCB RID: 27595
		public static PrisonerInteractionModeDef AttemptRecruit;

		// Token: 0x04006BCC RID: 27596
		public static PrisonerInteractionModeDef ReduceResistance;

		// Token: 0x04006BCD RID: 27597
		public static PrisonerInteractionModeDef Release;

		// Token: 0x04006BCE RID: 27598
		public static PrisonerInteractionModeDef Execution;
	}
}
