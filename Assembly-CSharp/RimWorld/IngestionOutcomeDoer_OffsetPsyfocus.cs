using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F43 RID: 3907
	public class IngestionOutcomeDoer_OffsetPsyfocus : IngestionOutcomeDoer
	{
		// Token: 0x060055EB RID: 21995 RVA: 0x0003BA93 File Offset: 0x00039C93
		protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested)
		{
			Pawn_PsychicEntropyTracker psychicEntropy = pawn.psychicEntropy;
			if (psychicEntropy == null)
			{
				return;
			}
			psychicEntropy.OffsetPsyfocusDirectly(this.offset);
		}

		// Token: 0x060055EC RID: 21996 RVA: 0x0003BAAB File Offset: 0x00039CAB
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			if (ModsConfig.RoyaltyActive)
			{
				string str = (this.offset >= 0f) ? "+" : string.Empty;
				yield return new StatDrawEntry(StatCategoryDefOf.Drug, "Psyfocus".Translate(), str + this.offset.ToStringPercent(), "PsyfocusDesc".Translate() + ".", 1000, null, null, false);
			}
			yield break;
		}

		// Token: 0x0400374F RID: 14159
		public float offset;
	}
}
