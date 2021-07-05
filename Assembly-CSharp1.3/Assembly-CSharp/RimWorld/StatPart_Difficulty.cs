using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014CB RID: 5323
	[Obsolete]
	public class StatPart_Difficulty : StatPart
	{
		// Token: 0x06007F04 RID: 32516 RVA: 0x002CF371 File Offset: 0x002CD571
		public override void TransformValue(StatRequest req, ref float val)
		{
			val *= this.Multiplier(Find.Storyteller.difficultyDef);
		}

		// Token: 0x06007F05 RID: 32517 RVA: 0x002CF388 File Offset: 0x002CD588
		public override string ExplanationPart(StatRequest req)
		{
			return "StatsReport_DifficultyMultiplier".Translate() + ": x" + this.Multiplier(Find.Storyteller.difficultyDef).ToStringPercent();
		}

		// Token: 0x06007F06 RID: 32518 RVA: 0x0001F15E File Offset: 0x0001D35E
		private float Multiplier(DifficultyDef d)
		{
			return 1f;
		}

		// Token: 0x04004F68 RID: 20328
		private List<float> factorsPerDifficulty = new List<float>();
	}
}
