using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009EF RID: 2543
	public class CompProperties_NeuralSupercharger : CompProperties_Rechargeable
	{
		// Token: 0x06003EAF RID: 16047 RVA: 0x00156F05 File Offset: 0x00155105
		public CompProperties_NeuralSupercharger()
		{
			this.compClass = typeof(CompNeuralSupercharger);
		}

		// Token: 0x06003EB0 RID: 16048 RVA: 0x00156F1D File Offset: 0x0015511D
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.effectCharged != null && parentDef.tickerType != TickerType.Normal)
			{
				yield return string.Format("CompProperties_NeuralSupercharger has effectCharged but parent {0} has tickerType!=Normal", parentDef);
			}
			yield break;
		}

		// Token: 0x04002186 RID: 8582
		[MustTranslate]
		public string jobString;

		// Token: 0x04002187 RID: 8583
		public EffecterDef effectCharged;
	}
}
