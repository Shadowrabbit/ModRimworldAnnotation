using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200179C RID: 6044
	public class CompBiocodableApparel : CompBiocodable
	{
		// Token: 0x0600858A RID: 34186 RVA: 0x000598B2 File Offset: 0x00057AB2
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (this.biocoded)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Apparel, "Stat_Thing_Biocoded_Name".Translate(), this.codedPawnLabel, "Stat_Thing_Biocoded_Desc".Translate(), 2753, null, null, false);
			}
			yield break;
		}
	}
}
