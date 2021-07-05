using System;

namespace RimWorld
{
	// Token: 0x02000EB9 RID: 3769
	public struct FactionGeneratorParms
	{
		// Token: 0x060058C9 RID: 22729 RVA: 0x001E4919 File Offset: 0x001E2B19
		public FactionGeneratorParms(FactionDef factionDef, IdeoGenerationParms ideoGenerationParms = default(IdeoGenerationParms), bool? hidden = null)
		{
			this.factionDef = factionDef;
			this.ideoGenerationParms = ideoGenerationParms;
			this.hidden = hidden;
		}

		// Token: 0x0400343F RID: 13375
		public FactionDef factionDef;

		// Token: 0x04003440 RID: 13376
		public IdeoGenerationParms ideoGenerationParms;

		// Token: 0x04003441 RID: 13377
		public bool? hidden;
	}
}
