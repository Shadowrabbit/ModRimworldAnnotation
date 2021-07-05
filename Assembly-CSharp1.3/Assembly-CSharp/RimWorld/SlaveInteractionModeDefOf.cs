using System;

namespace RimWorld
{
	// Token: 0x02001447 RID: 5191
	[DefOf]
	public static class SlaveInteractionModeDefOf
	{
		// Token: 0x06007D3A RID: 32058 RVA: 0x002C49F6 File Offset: 0x002C2BF6
		static SlaveInteractionModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SlaveInteractionModeDefOf));
		}

		// Token: 0x04004CC2 RID: 19650
		[MayRequireIdeology]
		public static SlaveInteractionModeDef NoInteraction;

		// Token: 0x04004CC3 RID: 19651
		[MayRequireIdeology]
		public static SlaveInteractionModeDef Imprison;

		// Token: 0x04004CC4 RID: 19652
		[MayRequireIdeology]
		public static SlaveInteractionModeDef Suppress;

		// Token: 0x04004CC5 RID: 19653
		[MayRequireIdeology]
		public static SlaveInteractionModeDef Emancipate;

		// Token: 0x04004CC6 RID: 19654
		[MayRequireIdeology]
		public static SlaveInteractionModeDef Execute;
	}
}
