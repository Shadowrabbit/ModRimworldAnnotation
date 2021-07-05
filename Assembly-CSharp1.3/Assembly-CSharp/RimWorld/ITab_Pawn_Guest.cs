using System;

namespace RimWorld
{
	// Token: 0x02001346 RID: 4934
	public class ITab_Pawn_Guest : ITab_Pawn_Visitor
	{
		// Token: 0x170014FE RID: 5374
		// (get) Token: 0x0600778A RID: 30602 RVA: 0x002A1BA8 File Offset: 0x0029FDA8
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.HostFaction == Faction.OfPlayer && !base.SelPawn.IsPrisoner && !base.SelPawn.IsSlaveOfColony;
			}
		}

		// Token: 0x0600778B RID: 30603 RVA: 0x002A1BD9 File Offset: 0x0029FDD9
		public ITab_Pawn_Guest()
		{
			this.labelKey = "TabGuest";
			this.tutorTag = "Guest";
		}
	}
}
