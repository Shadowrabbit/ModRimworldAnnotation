using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001488 RID: 5256
	public class InteractionWorker_Slight : InteractionWorker
	{
		// Token: 0x0600715F RID: 29023 RVA: 0x0004C4BD File Offset: 0x0004A6BD
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return 0.02f * NegativeInteractionUtility.NegativeInteractionChanceFactor(initiator, recipient);
		}

		// Token: 0x04004AD8 RID: 19160
		private const float BaseSelectionWeight = 0.02f;
	}
}
