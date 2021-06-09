using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200147D RID: 5245
	public class InteractionWorker_Chitchat : InteractionWorker
	{
		// Token: 0x0600713C RID: 28988 RVA: 0x0000CE6C File Offset: 0x0000B06C
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return 1f;
		}
	}
}
