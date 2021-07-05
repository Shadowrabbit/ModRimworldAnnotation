using System;

namespace RimWorld
{
	// Token: 0x02001398 RID: 5016
	public class CompProperties_AbilityTransferEntropy : CompProperties_AbilityEffect
	{
		// Token: 0x06006CD3 RID: 27859 RVA: 0x0004A000 File Offset: 0x00048200
		public CompProperties_AbilityTransferEntropy()
		{
			this.compClass = typeof(CompAbilityEffect_TransferEntropy);
		}

		// Token: 0x04004815 RID: 18453
		public bool targetReceivesEntropy = true;
	}
}
