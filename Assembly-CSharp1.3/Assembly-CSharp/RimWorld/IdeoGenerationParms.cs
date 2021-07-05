using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02000ED3 RID: 3795
	public struct IdeoGenerationParms
	{
		// Token: 0x060059F0 RID: 23024 RVA: 0x001ED64F File Offset: 0x001EB84F
		public IdeoGenerationParms(FactionDef forFaction, bool forceNoExpansionIdeo = false, List<PreceptDef> disallowedPrecepts = null, List<MemeDef> disallowedMemes = null)
		{
			this.forFaction = forFaction;
			this.forceNoExpansionIdeo = forceNoExpansionIdeo;
			this.disallowedPrecepts = disallowedPrecepts;
			this.disallowedMemes = disallowedMemes;
		}

		// Token: 0x040034AB RID: 13483
		public FactionDef forFaction;

		// Token: 0x040034AC RID: 13484
		public bool forceNoExpansionIdeo;

		// Token: 0x040034AD RID: 13485
		public List<PreceptDef> disallowedPrecepts;

		// Token: 0x040034AE RID: 13486
		public List<MemeDef> disallowedMemes;
	}
}
