using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001480 RID: 5248
	public class InteractionWorker_Insult : InteractionWorker
	{
		// Token: 0x06007142 RID: 28994 RVA: 0x0004C3EE File Offset: 0x0004A5EE
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return 0.007f * NegativeInteractionUtility.NegativeInteractionChanceFactor(initiator, recipient);
		}

		// Token: 0x04004ABE RID: 19134
		private const float BaseSelectionWeight = 0.007f;
	}
}
