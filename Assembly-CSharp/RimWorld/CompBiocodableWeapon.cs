using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200179F RID: 6047
	public class CompBiocodableWeapon : CompBiocodable
	{
		// Token: 0x06008595 RID: 34197 RVA: 0x0005990C File Offset: 0x00057B0C
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (this.biocoded)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Weapon, "Stat_Thing_Biocoded_Name".Translate(), this.codedPawnLabel, "Stat_Thing_Biocoded_Desc".Translate(), 5404, null, null, false);
			}
			yield break;
		}
	}
}
