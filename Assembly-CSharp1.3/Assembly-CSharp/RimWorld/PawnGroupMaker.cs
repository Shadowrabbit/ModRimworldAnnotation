using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DC4 RID: 3524
	public class PawnGroupMaker
	{
		// Token: 0x06005190 RID: 20880 RVA: 0x001B7F00 File Offset: 0x001B6100
		public float MinPointsToGenerateAnything(PawnGroupMakerParms parms = null)
		{
			return this.kindDef.Worker.MinPointsToGenerateAnything(this, parms);
		}

		// Token: 0x06005191 RID: 20881 RVA: 0x001B7F14 File Offset: 0x001B6114
		public IEnumerable<Pawn> GeneratePawns(PawnGroupMakerParms parms, bool errorOnZeroResults = true)
		{
			return this.kindDef.Worker.GeneratePawns(parms, this, errorOnZeroResults);
		}

		// Token: 0x06005192 RID: 20882 RVA: 0x001B7F29 File Offset: 0x001B6129
		public IEnumerable<PawnKindDef> GeneratePawnKindsExample(PawnGroupMakerParms parms)
		{
			return this.kindDef.Worker.GeneratePawnKindsExample(parms, this);
		}

		// Token: 0x06005193 RID: 20883 RVA: 0x001B7F40 File Offset: 0x001B6140
		public bool CanGenerateFrom(PawnGroupMakerParms parms)
		{
			return parms.points <= this.maxTotalPoints && (this.disallowedStrategies == null || !this.disallowedStrategies.Contains(parms.raidStrategy)) && this.kindDef.Worker.CanGenerateFrom(parms, this);
		}

		// Token: 0x0400304D RID: 12365
		public PawnGroupKindDef kindDef;

		// Token: 0x0400304E RID: 12366
		public float commonality = 100f;

		// Token: 0x0400304F RID: 12367
		public List<RaidStrategyDef> disallowedStrategies;

		// Token: 0x04003050 RID: 12368
		public float maxTotalPoints = 9999999f;

		// Token: 0x04003051 RID: 12369
		public List<PawnGenOption> options = new List<PawnGenOption>();

		// Token: 0x04003052 RID: 12370
		public List<PawnGenOption> traders = new List<PawnGenOption>();

		// Token: 0x04003053 RID: 12371
		public List<PawnGenOption> carriers = new List<PawnGenOption>();

		// Token: 0x04003054 RID: 12372
		public List<PawnGenOption> guards = new List<PawnGenOption>();
	}
}
