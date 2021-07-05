using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A2D RID: 2605
	public class IngestionOutcomeDoer_OffsetPsyfocus : IngestionOutcomeDoer
	{
		// Token: 0x06003F35 RID: 16181 RVA: 0x00158791 File Offset: 0x00156991
		protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested)
		{
			Pawn_PsychicEntropyTracker psychicEntropy = pawn.psychicEntropy;
			if (psychicEntropy == null)
			{
				return;
			}
			psychicEntropy.OffsetPsyfocusDirectly(this.offset);
		}

		// Token: 0x06003F36 RID: 16182 RVA: 0x001587A9 File Offset: 0x001569A9
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			if (ModsConfig.RoyaltyActive)
			{
				string str = (this.offset >= 0f) ? "+" : string.Empty;
				yield return new StatDrawEntry(StatCategoryDefOf.Drug, "Psyfocus".Translate(), str + this.offset.ToStringPercent(), "PsyfocusDesc".Translate() + ".", 1000, null, null, false);
			}
			yield break;
		}

		// Token: 0x0400229A RID: 8858
		public float offset;
	}
}
