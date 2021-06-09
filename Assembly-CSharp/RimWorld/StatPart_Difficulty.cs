using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D32 RID: 7474
	[Obsolete]
	public class StatPart_Difficulty : StatPart
	{
		// Token: 0x0600A274 RID: 41588 RVA: 0x0006BEB8 File Offset: 0x0006A0B8
		public override void TransformValue(StatRequest req, ref float val)
		{
			val *= this.Multiplier(Find.Storyteller.difficulty);
		}

		// Token: 0x0600A275 RID: 41589 RVA: 0x0006BECF File Offset: 0x0006A0CF
		public override string ExplanationPart(StatRequest req)
		{
			return "StatsReport_DifficultyMultiplier".Translate() + ": x" + this.Multiplier(Find.Storyteller.difficulty).ToStringPercent();
		}

		// Token: 0x0600A276 RID: 41590 RVA: 0x0000CE6C File Offset: 0x0000B06C
		private float Multiplier(DifficultyDef d)
		{
			return 1f;
		}

		// Token: 0x04006E6C RID: 28268
		private List<float> factorsPerDifficulty = new List<float>();
	}
}
