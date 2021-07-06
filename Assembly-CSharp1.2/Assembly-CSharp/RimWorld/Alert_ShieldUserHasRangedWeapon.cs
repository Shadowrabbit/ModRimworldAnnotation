using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001968 RID: 6504
	public class Alert_ShieldUserHasRangedWeapon : Alert
	{
		// Token: 0x170016B9 RID: 5817
		// (get) Token: 0x06008FE2 RID: 36834 RVA: 0x00296AC8 File Offset: 0x00294CC8
		private List<Pawn> ShieldUsersWithRangedWeapon
		{
			get
			{
				this.shieldUsersWithRangedWeaponResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
				{
					if (pawn.equipment.Primary != null && pawn.equipment.Primary.def.IsRangedWeapon)
					{
						List<Apparel> wornApparel = pawn.apparel.WornApparel;
						for (int i = 0; i < wornApparel.Count; i++)
						{
							if (wornApparel[i] is ShieldBelt)
							{
								this.shieldUsersWithRangedWeaponResult.Add(pawn);
								break;
							}
						}
					}
				}
				return this.shieldUsersWithRangedWeaponResult;
			}
		}

		// Token: 0x06008FE3 RID: 36835 RVA: 0x00060798 File Offset: 0x0005E998
		public Alert_ShieldUserHasRangedWeapon()
		{
			this.defaultLabel = "ShieldUserHasRangedWeapon".Translate();
			this.defaultExplanation = "ShieldUserHasRangedWeaponDesc".Translate();
		}

		// Token: 0x06008FE4 RID: 36836 RVA: 0x000607D5 File Offset: 0x0005E9D5
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ShieldUsersWithRangedWeapon);
		}

		// Token: 0x04005B89 RID: 23433
		private List<Pawn> shieldUsersWithRangedWeaponResult = new List<Pawn>();
	}
}
