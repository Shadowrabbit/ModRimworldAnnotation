using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001967 RID: 6503
	public class Alert_BrawlerHasRangedWeapon : Alert
	{
		// Token: 0x170016B8 RID: 5816
		// (get) Token: 0x06008FDF RID: 36831 RVA: 0x00296A24 File Offset: 0x00294C24
		private List<Pawn> BrawlersWithRangedWeapon
		{
			get
			{
				this.brawlersWithRangedWeaponResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
				{
					if (pawn.story.traits.HasTrait(TraitDefOf.Brawler) && pawn.equipment.Primary != null && pawn.equipment.Primary.def.IsRangedWeapon)
					{
						this.brawlersWithRangedWeaponResult.Add(pawn);
					}
				}
				return this.brawlersWithRangedWeaponResult;
			}
		}

		// Token: 0x06008FE0 RID: 36832 RVA: 0x0006074E File Offset: 0x0005E94E
		public Alert_BrawlerHasRangedWeapon()
		{
			this.defaultLabel = "BrawlerHasRangedWeapon".Translate();
			this.defaultExplanation = "BrawlerHasRangedWeaponDesc".Translate();
		}

		// Token: 0x06008FE1 RID: 36833 RVA: 0x0006078B File Offset: 0x0005E98B
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BrawlersWithRangedWeapon);
		}

		// Token: 0x04005B88 RID: 23432
		private List<Pawn> brawlersWithRangedWeaponResult = new List<Pawn>();
	}
}
