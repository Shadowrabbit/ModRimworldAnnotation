using System;

namespace RimWorld
{
	// Token: 0x02000D77 RID: 3447
	public class CompProperties_AbilityTransferEntropy : CompProperties_AbilityEffect
	{
		// Token: 0x06004FE7 RID: 20455 RVA: 0x001ABAE6 File Offset: 0x001A9CE6
		public CompProperties_AbilityTransferEntropy()
		{
			this.compClass = typeof(CompAbilityEffect_TransferEntropy);
		}

		// Token: 0x04002FCF RID: 12239
		public bool targetReceivesEntropy = true;
	}
}
