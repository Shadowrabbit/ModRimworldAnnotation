using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A18 RID: 2584
	public class CompProperties_Ingredients : CompProperties
	{
		// Token: 0x06003F11 RID: 16145 RVA: 0x0015807B File Offset: 0x0015627B
		public CompProperties_Ingredients()
		{
			this.compClass = typeof(CompIngredients);
		}

		// Token: 0x04002223 RID: 8739
		public bool performMergeCompatibilityChecks = true;
	}
}
