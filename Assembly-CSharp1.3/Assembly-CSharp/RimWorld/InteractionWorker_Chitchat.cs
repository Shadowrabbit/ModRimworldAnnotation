using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DF1 RID: 3569
	public class InteractionWorker_Chitchat : InteractionWorker
	{
		// Token: 0x060052BD RID: 21181 RVA: 0x0001F15E File Offset: 0x0001D35E
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return 1f;
		}
	}
}
