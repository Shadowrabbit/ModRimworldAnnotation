using System;

namespace RimWorld
{
	// Token: 0x02000E8C RID: 3724
	public class Thought_Banished : Thought_Memory
	{
		// Token: 0x17000CC1 RID: 3265
		// (get) Token: 0x06005366 RID: 21350 RVA: 0x0003A2D9 File Offset: 0x000384D9
		public override bool ShouldDiscard
		{
			get
			{
				return base.ShouldDiscard || this.otherPawn.Faction == this.pawn.Faction;
			}
		}
	}
}
