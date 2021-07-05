using System;

namespace RimWorld
{
	// Token: 0x02001348 RID: 4936
	public class ITab_Pawn_Slave : ITab_Pawn_Visitor
	{
		// Token: 0x17001500 RID: 5376
		// (get) Token: 0x0600778E RID: 30606 RVA: 0x002A1C22 File Offset: 0x0029FE22
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.IsSlaveOfColony;
			}
		}

		// Token: 0x0600778F RID: 30607 RVA: 0x002A1C2F File Offset: 0x0029FE2F
		public ITab_Pawn_Slave()
		{
			this.labelKey = "TabSlave";
			this.tutorTag = "Slave";
		}
	}
}
