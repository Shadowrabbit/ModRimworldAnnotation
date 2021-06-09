using System;

namespace RimWorld
{
	// Token: 0x02001B0D RID: 6925
	public class ITab_Pawn_Guest : ITab_Pawn_Visitor
	{
		// Token: 0x17001802 RID: 6146
		// (get) Token: 0x0600986F RID: 39023 RVA: 0x000658AC File Offset: 0x00063AAC
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.HostFaction == Faction.OfPlayer && !base.SelPawn.IsPrisoner;
			}
		}

		// Token: 0x06009870 RID: 39024 RVA: 0x000658D0 File Offset: 0x00063AD0
		public ITab_Pawn_Guest()
		{
			this.labelKey = "TabGuest";
			this.tutorTag = "Guest";
		}
	}
}
