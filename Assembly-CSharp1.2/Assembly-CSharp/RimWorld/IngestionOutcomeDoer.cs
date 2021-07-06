using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F3D RID: 3901
	public abstract class IngestionOutcomeDoer
	{
		// Token: 0x060055C8 RID: 21960 RVA: 0x0003B984 File Offset: 0x00039B84
		public void DoIngestionOutcome(Pawn pawn, Thing ingested)
		{
			if (Rand.Value < this.chance)
			{
				this.DoIngestionOutcomeSpecial(pawn, ingested);
			}
		}

		// Token: 0x060055C9 RID: 21961
		protected abstract void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested);

		// Token: 0x060055CA RID: 21962 RVA: 0x0003B99B File Offset: 0x00039B9B
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			yield break;
		}

		// Token: 0x04003738 RID: 14136
		public float chance = 1f;

		// Token: 0x04003739 RID: 14137
		public bool doToGeneratedPawnIfAddicted;
	}
}
