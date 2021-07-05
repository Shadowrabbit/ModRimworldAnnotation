using System;

namespace RimWorld
{
	// Token: 0x02000982 RID: 2434
	public class Thought_Banished : Thought_Memory
	{
		// Token: 0x17000AD9 RID: 2777
		// (get) Token: 0x06003D8C RID: 15756 RVA: 0x001527A1 File Offset: 0x001509A1
		public override bool ShouldDiscard
		{
			get
			{
				return base.ShouldDiscard || this.otherPawn.Faction == this.pawn.Faction;
			}
		}
	}
}
