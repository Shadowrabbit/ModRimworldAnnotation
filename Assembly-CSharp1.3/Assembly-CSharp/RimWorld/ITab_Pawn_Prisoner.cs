using System;

namespace RimWorld
{
	// Token: 0x02001347 RID: 4935
	public class ITab_Pawn_Prisoner : ITab_Pawn_Visitor
	{
		// Token: 0x170014FF RID: 5375
		// (get) Token: 0x0600778C RID: 30604 RVA: 0x002A1BF7 File Offset: 0x0029FDF7
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.IsPrisonerOfColony;
			}
		}

		// Token: 0x0600778D RID: 30605 RVA: 0x002A1C04 File Offset: 0x0029FE04
		public ITab_Pawn_Prisoner()
		{
			this.labelKey = "TabPrisoner";
			this.tutorTag = "Prisoner";
		}
	}
}
