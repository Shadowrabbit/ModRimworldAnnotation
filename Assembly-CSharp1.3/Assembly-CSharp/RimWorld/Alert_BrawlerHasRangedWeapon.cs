using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001277 RID: 4727
	public class Alert_BrawlerHasRangedWeapon : Alert
	{
		// Token: 0x170013B7 RID: 5047
		// (get) Token: 0x06007111 RID: 28945 RVA: 0x0025AC38 File Offset: 0x00258E38
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

		// Token: 0x06007112 RID: 28946 RVA: 0x0025ACDC File Offset: 0x00258EDC
		public Alert_BrawlerHasRangedWeapon()
		{
			this.defaultLabel = "BrawlerHasRangedWeapon".Translate();
			this.defaultExplanation = "BrawlerHasRangedWeaponDesc".Translate();
		}

		// Token: 0x06007113 RID: 28947 RVA: 0x0025AD19 File Offset: 0x00258F19
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BrawlersWithRangedWeapon);
		}

		// Token: 0x04003E35 RID: 15925
		private List<Pawn> brawlersWithRangedWeaponResult = new List<Pawn>();
	}
}
