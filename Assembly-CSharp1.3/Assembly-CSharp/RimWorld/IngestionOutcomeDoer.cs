using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A2A RID: 2602
	public abstract class IngestionOutcomeDoer
	{
		// Token: 0x06003F2B RID: 16171 RVA: 0x0015863E File Offset: 0x0015683E
		public void DoIngestionOutcome(Pawn pawn, Thing ingested)
		{
			if (Rand.Value < this.chance)
			{
				this.DoIngestionOutcomeSpecial(pawn, ingested);
			}
		}

		// Token: 0x06003F2C RID: 16172
		protected abstract void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested);

		// Token: 0x06003F2D RID: 16173 RVA: 0x00158655 File Offset: 0x00156855
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			yield break;
		}

		// Token: 0x04002290 RID: 8848
		public float chance = 1f;

		// Token: 0x04002291 RID: 8849
		public bool doToGeneratedPawnIfAddicted;
	}
}
