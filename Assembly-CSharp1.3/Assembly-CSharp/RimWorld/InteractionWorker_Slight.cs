using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DFC RID: 3580
	public class InteractionWorker_Slight : InteractionWorker
	{
		// Token: 0x060052E3 RID: 21219 RVA: 0x001C0DB5 File Offset: 0x001BEFB5
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return 0.02f * NegativeInteractionUtility.NegativeInteractionChanceFactor(initiator, recipient);
		}

		// Token: 0x040030EF RID: 12527
		private const float BaseSelectionWeight = 0.02f;
	}
}
