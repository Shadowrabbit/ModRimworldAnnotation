using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200142D RID: 5165
	public class PawnGroupMaker
	{
		// Token: 0x17001119 RID: 4377
		// (get) Token: 0x06006F7D RID: 28541 RVA: 0x0004B609 File Offset: 0x00049809
		public float MinPointsToGenerateAnything
		{
			get
			{
				return this.kindDef.Worker.MinPointsToGenerateAnything(this);
			}
		}

		// Token: 0x06006F7E RID: 28542 RVA: 0x0004B61C File Offset: 0x0004981C
		public IEnumerable<Pawn> GeneratePawns(PawnGroupMakerParms parms, bool errorOnZeroResults = true)
		{
			return this.kindDef.Worker.GeneratePawns(parms, this, errorOnZeroResults);
		}

		// Token: 0x06006F7F RID: 28543 RVA: 0x0004B631 File Offset: 0x00049831
		public IEnumerable<PawnKindDef> GeneratePawnKindsExample(PawnGroupMakerParms parms)
		{
			return this.kindDef.Worker.GeneratePawnKindsExample(parms, this);
		}

		// Token: 0x06006F80 RID: 28544 RVA: 0x00222494 File Offset: 0x00220694
		public bool CanGenerateFrom(PawnGroupMakerParms parms)
		{
			return parms.points <= this.maxTotalPoints && (this.disallowedStrategies == null || !this.disallowedStrategies.Contains(parms.raidStrategy)) && this.kindDef.Worker.CanGenerateFrom(parms, this);
		}

		// Token: 0x040049A1 RID: 18849
		public PawnGroupKindDef kindDef;

		// Token: 0x040049A2 RID: 18850
		public float commonality = 100f;

		// Token: 0x040049A3 RID: 18851
		public List<RaidStrategyDef> disallowedStrategies;

		// Token: 0x040049A4 RID: 18852
		public float maxTotalPoints = 9999999f;

		// Token: 0x040049A5 RID: 18853
		public List<PawnGenOption> options = new List<PawnGenOption>();

		// Token: 0x040049A6 RID: 18854
		public List<PawnGenOption> traders = new List<PawnGenOption>();

		// Token: 0x040049A7 RID: 18855
		public List<PawnGenOption> carriers = new List<PawnGenOption>();

		// Token: 0x040049A8 RID: 18856
		public List<PawnGenOption> guards = new List<PawnGenOption>();
	}
}
