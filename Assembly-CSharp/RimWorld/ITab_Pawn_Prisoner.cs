using System;

namespace RimWorld
{
	// Token: 0x02001B0E RID: 6926
	public class ITab_Pawn_Prisoner : ITab_Pawn_Visitor
	{
		// Token: 0x17001803 RID: 6147
		// (get) Token: 0x06009871 RID: 39025 RVA: 0x000658EE File Offset: 0x00063AEE
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.IsPrisonerOfColony;
			}
		}

		// Token: 0x06009872 RID: 39026 RVA: 0x000658FB File Offset: 0x00063AFB
		public ITab_Pawn_Prisoner()
		{
			this.labelKey = "TabPrisoner";
			this.tutorTag = "Prisoner";
		}
	}
}
