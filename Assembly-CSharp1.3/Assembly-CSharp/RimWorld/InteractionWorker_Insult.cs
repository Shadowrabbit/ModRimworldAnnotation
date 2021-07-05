using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DF6 RID: 3574
	public class InteractionWorker_Insult : InteractionWorker
	{
		// Token: 0x060052CB RID: 21195 RVA: 0x001BF2C1 File Offset: 0x001BD4C1
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return 0.007f * NegativeInteractionUtility.NegativeInteractionChanceFactor(initiator, recipient);
		}

		// Token: 0x040030D7 RID: 12503
		private const float BaseSelectionWeight = 0.007f;
	}
}
